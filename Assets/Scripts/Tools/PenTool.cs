using System.Collections.Generic;
using GameData;
using Helpers;
using Interfaces;
using Managers;
using UnityEngine;

namespace Tools
{
    public class PenTool : BaseTool,IDraw
    {
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private float minDistance = 0.1f;
        private GameObject _currentLine;
        private LineRenderer _lineRenderer;
        private List<Vector2> _fingerPositions;
        private List<Color> _drawingColors = new List<Color>();
        private List<GameObject> _allLines = new List<GameObject>();
        private Dictionary<int, List<Vector3>> _drawPositions = new Dictionary<int, List<Vector3>>();
        public Color currentColor;
        private int _currentLineId;
        private float _trailInterval = 0.05f;
        private float _lastTrailTime = 0f;
        private bool _lineCreated = false;
      
        
        public override ToolType ToolType
        {
            get => ToolType.Pen;
            set { }
        }

        protected override void Awake()
        {
            base.Awake();
            if (Pool == null || Pool.GetPoolSize() < 1)
            {
                Pool = new ObjectGameObjectPool(linePrefab, 3);
            }
            currentColor = Color.black;
            ToolType = ToolType.Pen;
            IsColorDependent = true;
            _fingerPositions = new List<Vector2>();
        }


        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            if (TouchOnUIButton && Time.time - LastUITouchTime < UiTouchCooldown)
            {
                // UI touch cooldown active, do not create line
                return;
            }

            if (TouchOnButton()) 
            {
                TouchOnUIButton = true;
                LastUITouchTime = Time.time;
                return;
            }
    
            if (Input.GetMouseButton(0))
            {
                if (!_lineCreated)
                {
                    CreateNewLine();
                    _lineCreated = true;
                }

                UseTool();
            }
    
            if (Input.GetMouseButtonUp(0))
            {
                TakeLinePositions();
                _lineCreated = false;
            }
        }

        private void TakeLinePositions()
        {
            List<Vector3> linePositions = new List<Vector3>();
            _lineRenderer = _currentLine.GetComponent<LineRenderer>();
            for (var i = 0; i < _lineRenderer.positionCount; i++)
            {
                linePositions.Add(_lineRenderer.GetPosition(i));
            }
            AddLinePositions(_currentLineId++, linePositions);
            _drawingColors.Add(currentColor);
            AudioManager.Instance.StopSound("Pen");
        }
    
        public override void UseTool()
        {
            MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(MousePosition, _fingerPositions[_fingerPositions.Count - 1]) > minDistance)
            {
                _lineRenderer = _currentLine.GetComponent<LineRenderer>();
                _fingerPositions.Add(MousePosition);
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, MousePosition);
                
                if (_lastTrailTime + _trailInterval < Time.time)
                {
                    _lastTrailTime = Time.time;
                    PlayParticleEffect(MousePosition, currentColor);
                }
            }
        }
        
        private void CreateNewLine()
        {
            MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            InstantiateNewLine(currentColor);
            PlayParticleEffect(MousePosition,currentColor);
            PlaySound();
            PlayerPrefs.SetString("LastUsedTool", ToolType.Pen.ToString());
        }
        public override void PlaySound()
        {
            AudioManager.Instance.PlaySound("Pen");
        }

        public void InstantiateNewLine(Color color)
        {
            _currentLine = Pool.Get();
            _lineRenderer = _currentLine.GetComponent<LineRenderer>();
            ResetCurrentLine(_lineRenderer,color);
            _currentLine.gameObject.SetActive(true);
            _allLines.Add(_currentLine);
            _fingerPositions.Clear();
            _fingerPositions.Add(MainCamera.ScreenToWorldPoint(Input.mousePosition));
            _fingerPositions.Add(MainCamera.ScreenToWorldPoint(Input.mousePosition));
            _lineRenderer.SetPosition(0, _fingerPositions[0]);
            _lineRenderer.SetPosition(1, _fingerPositions[1]);
        }

        private void ResetCurrentLine(LineRenderer lineRenderer,Color color)
        {
            lineRenderer.SetPositions(new Vector3[0]);
            lineRenderer.positionCount = 2;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.sortingOrder = 0;
        }

        public override void EmptyPool()
        {
            foreach (var line in _allLines)
            {
                Pool.Return(line);
            }
            _drawPositions.Clear();
            _drawingColors.Clear();
            _currentLineId = 0;
            _allLines.Clear();
        }
        private void ChangeColor(Color changedColor)
        {
            if (IsColorDependent)
            {
                currentColor = changedColor;
            }
        }
        private void AddLinePositions(int currentLineId, List<Vector3> positions)
        {
            _drawPositions[currentLineId] = new List<Vector3>();
            _drawPositions[currentLineId].AddRange(positions);
        }
        private void OnEnable()
        {
            GameManager.Instance.OnColorChanged += ChangeColor;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnColorChanged += ChangeColor;
        }
        public override void SaveData()
        {
            var savedDrawData = new DrawData()
            {
                colorData = _drawingColors,
                DrawPositionsData = _drawPositions
            };
            GameManager.Instance.SaveManager.SaveDrawData(savedDrawData);
        }
        public ObjectGameObjectPool Pool { get; set; }
    }
}