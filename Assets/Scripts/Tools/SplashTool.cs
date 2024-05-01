using System.Collections;
using System.Collections.Generic;
using GameData;
using Helpers;
using Managers;
using UnityEngine;

namespace Tools
{
    public class SplashTool : BaseTool
    {
        [SerializeField] private Sprite[] splashSprites; // Sprite to be splashed
        [SerializeField] private float maxParticleScale = 4.0f; // Maximum scale of the particle effect
        [SerializeField] private float minParticleScale = 0.1f; // Minimum scale of the particle effect
        private Color _currentColor;
        private GameObject _splashGameObject;
        private List<GameObject> _allSplashObjects = new List<GameObject>();
        private List<Vector3> _splashPositions = new List<Vector3>();
        private List<ColorType> _splashColors = new List<ColorType>();

        public override ToolType ToolType
        {
            get => ToolType.Splash;
            set { }
        }

        protected override void Awake()
        {
            base.Awake();
            _splashGameObject = new GameObject("SplashGameObject");
            if (Pool == null || Pool.GetPoolSize() < 1)
            {
                Pool = new ObjectGameObjectPool(_splashGameObject, 3);
            }
            ToolType = ToolType.Splash;
            IsColorDependent = true;
            _currentColor = Color.black;
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

            // Move particles towards the click position
            PlayParticles(MousePosition);
            PlayerPrefs.SetString("LastUsedTool", ToolType.Splash.ToString());
        }
        public void CreateSplashSprite(Vector2 position)
        {
            var splashObject = Pool.Get();
            splashObject.transform.position = position;

            if (splashObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                ChangeSprite(spriteRenderer);
            }
            else
            {
                var newSpriteRenderer = splashObject.AddComponent<SpriteRenderer>();
                ChangeSprite(newSpriteRenderer);
            }
            splashObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            splashObject.gameObject.SetActive(true);
            _splashPositions.Add(splashObject.transform.position);
            _allSplashObjects.Add(splashObject);
        }
        public override void EmptyPool()
        {
            _splashPositions.Clear();
            _splashColors.Clear();
            foreach (var line in _allSplashObjects)
            {
                Pool.Return(line);
            }
            _allSplashObjects.Clear();
        }
        private void ChangeSprite(SpriteRenderer spriteRenderer)
        {
            var colorType = GetColorTypeFromColor(_currentColor);
            spriteRenderer.sprite = splashSprites[(int)colorType];
            spriteRenderer.sortingOrder = 0;
            _splashColors.Add(colorType);
        }
        private ColorType GetColorTypeFromColor(Color color)
        {
            if (color == Color.black) return ColorType.Black;
            if (color == Color.blue) return ColorType.Blue;
            if (color == Color.yellow) return ColorType.Yellow;
            if (color == Color.green) return ColorType.Green;
            if (color == Color.red) return ColorType.Red;
            return ColorType.Black; // Default to black if color is not recognized
        }
        private void PlayParticles(Vector2 spawnPosition)
        {
            var colorType = GetColorTypeFromColor(_currentColor);
            var particleInstance = Instantiate(particleEffectPrefab[(int)colorType].gameObject, spawnPosition, Quaternion.identity);
            particleInstance.transform.localScale = Vector3.one * maxParticleScale;

            // Move particles towards the target position and gradually scale down
            StartCoroutine(ScaleParticlesAndCreateSplash(spawnPosition, particleInstance));
           
        }
        private IEnumerator ScaleParticlesAndCreateSplash(Vector2 targetPosition, GameObject particleObject)
        {
            // Gradually scale down the particle effect
            float scaleFactor = maxParticleScale;
            while (scaleFactor > minParticleScale)
            {
                scaleFactor -= Time.deltaTime * 2;
                particleObject.transform.localScale = Vector3.one * scaleFactor;
                yield return null;
            }
            // Destroy the scaled particle effect
            Destroy(particleObject.gameObject);
            PlaySound();
            CreateSplashSprite(targetPosition);
        }
        
        public override void PlaySound()
        {
            AudioManager.Instance.PlaySound("Splash");
        }
        
        private void ChangeColor(Color changedColor)
        {
            _currentColor = changedColor;
        }
        
        private void OnEnable()
        {
            // If tool is disabled and the current color changes it will take current color
            ChangeColor(GameManager.Instance.CurrentColor);
            // If tool becomes enabled it will subscribe to the event otherwise it takes the unchanged current color
            GameManager.Instance.OnColorChanged += ChangeColor;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnColorChanged += ChangeColor;
        }
        
        public override void SaveData()
        {
            var savedToolData = new SplashData()
            {
                SplashPositionsData = _splashPositions,
                colorData = _splashColors
            };
            GameManager.Instance.SaveManager.SaveSplashData(savedToolData);
        }
    }
}

