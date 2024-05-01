using System.Collections.Generic;
using GameData;
using Helpers;
using Interfaces;
using Managers;
using UnityEngine;

namespace Tools
{
    public class StampTool : BaseTool, IStamp
    {
        [SerializeField] private Sprite stampSprite; // Sprite to be stamped
        private GameObject _stampGameObject;
        private List<Vector3> _stampPositions = new List<Vector3>();
        private List<GameObject> _allStampObjects = new List<GameObject>();

        
        public override ToolType ToolType
        {
            get => ToolType.Stamp;
            set { }
        }
        
        protected override void Awake()
        {
            base.Awake();
            _stampGameObject = new GameObject("StampGameObject");
            if (Pool == null || Pool.GetPoolSize() < 1)
            {
                Pool = new ObjectGameObjectPool(_stampGameObject, 3);
            }
            ToolType = ToolType.Stamp;
            IsColorDependent = false;
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
            if (Input.GetMouseButtonDown(0))
            {
                UseTool();
            }
        }

        public override void UseTool()
        {
            // Get the mouse position in world coordinates
            MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Instantiate the stamp sprite at the mouse position
            CreateStamp(new Vector3(MousePosition.x, MousePosition.y, 0f));
            PlayParticleEffect(MousePosition, Color.white);
            PlaySound();
            PlayerPrefs.SetString("LastUsedTool", ToolType.Stamp.ToString());
        }
        public override void PlaySound()
        {
            AudioManager.Instance.PlaySound("Stamp");
        }


        public void CreateStamp(Vector3 position)
        {
            var stampObject = Pool.Get();
            stampObject.transform.position = position; // Ensure z position is 0
            
            if (stampObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.sprite = stampSprite;
                spriteRenderer.sortingOrder = 0;
            }
            else
            {
                var newSpriteRenderer = stampObject.AddComponent<SpriteRenderer>();
                newSpriteRenderer.sprite = stampSprite; 
                newSpriteRenderer.sortingOrder = 0;
            }
            stampObject.transform.localScale = new Vector3(0.2f,0.2f, 1f);
            stampObject.gameObject.SetActive(true);
            _stampPositions.Add(stampObject.transform.position);
            _allStampObjects.Add(stampObject);
        }
        public override void EmptyPool()
        {
            _stampPositions.Clear();
            foreach (var line in _allStampObjects)
            {
                Pool.Return(line);
            }
            _allStampObjects.Clear();
        }

        public override void SaveData()
        {
            var savedToolData = new StampData()
            {
                StampPositionsData = _stampPositions
            };
            GameManager.Instance.SaveManager.SaveStampData(savedToolData);
        }
    }
}