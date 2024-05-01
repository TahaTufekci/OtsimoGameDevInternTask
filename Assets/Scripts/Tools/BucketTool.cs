using GameData;
using Helpers;
using Managers;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace Tools
{
    public class BucketTool : BaseTool
    {
        [SerializeField] private SpriteRenderer background;
        private Color _currentColor = Color.white;
        private Color _lastUsedColor = Color.white;
        
        public override ToolType ToolType
        {
            get => ToolType.Bucket;
            set { }
        }

        protected override void Awake()
        {
            base.Awake();
            ToolType = ToolType.Bucket;
            IsColorDependent = true;
            if (PlayerPrefs.HasKey("SavedBucketData"))
            {
                _currentColor = GameManager.Instance.SaveManager.LoadBucketData().colorData;
            }
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
            GameManager.Instance.EmptyPools();
            GameManager.Instance.DestroySavedDrawings();
            
            background.color = _currentColor;
            _lastUsedColor = _currentColor;
            PlayParticleEffect(MousePosition, _currentColor);
            PlaySound();
            PlayerPrefs.SetString("LastUsedTool", ToolType.Bucket.ToString());
        }
        public override void PlaySound()
        {
            AudioManager.Instance.PlaySound("Bucket");
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
        public override void EmptyPool()
        {
            return;
        }
        public override void SaveData()
        {
            BucketData savedBucketData;
            if(_lastUsedColor == Color.white && PlayerPrefs.HasKey("SavedBucketData"))
            {
                savedBucketData = new BucketData()
                {
                    colorData = GameManager.Instance.SaveManager.LoadBucketData().colorData
                };
            }
            else
            {
                savedBucketData = new BucketData()
                {
                    colorData = _lastUsedColor
                };
            }
            GameManager.Instance.SaveManager.SaveBucketData(savedBucketData);
        }

    }
}