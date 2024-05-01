using System;
using System.Collections.Generic;
using Helpers;
using Tools;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        #region Lists
        [SerializeField]private List<BaseTool> tools = new List<BaseTool>();
        public List<BaseTool> Tools => tools;

        public List<GameObject> savedDrawings = new List<GameObject>();
        #endregion
        
        #region Actions
        public Action<Color> OnColorChanged;
        public Action<ToolType> OnToolChanged;
        #endregion
        
        [SerializeField] private SaveManager saveManager;
        public SaveManager SaveManager => saveManager;
        [SerializeField] private LoadManager loadManager;
        
        private BaseTool _activeTool;
        public ToolType CurrentToolType { get; set; }
        private Color _currentColor;
        public Color CurrentColor => _currentColor;

        private void Start()
        {
            ChangeTool(tools[0].ToolType);
            LoadSavedData();
        }

        private void LoadSavedData()
        {
            if(PlayerPrefs.GetString("LastUsedTool") == ToolType.Bucket.ToString())
            {
                if (PlayerPrefs.HasKey("SavedBucketData"))
                {
                    loadManager.LoadSavedBucketColor(saveManager.LoadBucketData());
                }
            }
            else
            {
                if (PlayerPrefs.HasKey("SavedBucketData"))
                {
                    loadManager.LoadSavedBucketColor(saveManager.LoadBucketData());
                }
                if (PlayerPrefs.HasKey("SavedDrawData"))
                {
                    loadManager.LoadSavedDrawPositions(saveManager.LoadDrawData());
                }

                if (PlayerPrefs.HasKey("SavedStampData"))
                {
                    loadManager.LoadSavedStampPositions(saveManager.LoadStampData());
                }

                if (PlayerPrefs.HasKey("SavedSplashData"))
                {
                    loadManager.LoadSavedSplashPositions(saveManager.LoadSplashData());
                }

                if (PlayerPrefs.HasKey("SavedEraserData"))
                {
                    loadManager.LoadSavedEraserPositions(saveManager.LoadEraserData());
                }
            }

        }
        private void ChangeTool(ToolType changedTool)
        {
            var oldTool = tools.Find(tool => tool.ToolType == CurrentToolType);
            if (oldTool != null)
            {
                oldTool.gameObject.SetActive(false);
            }
            var selectedTool = tools.Find(tool => tool.ToolType == changedTool);
            selectedTool.gameObject.SetActive(true);
            CurrentToolType = changedTool;
        }
        
        private void SaveToolData()
        {
            tools.ForEach(tool => tool.SaveData());
        }
        
        public void EmptyPools()
        {
            tools.ForEach(tool => tool.EmptyPool());
        }
        
        private void UpdateColor(Color color)
        {
            _currentColor = color;
        }
        
        public void DestroySavedDrawings()
        {
            if (savedDrawings.Count <= 0) return;
            for (int i = savedDrawings.Count - 1; i >= 0; i--)
            {
                Destroy(savedDrawings[i]);
                savedDrawings.RemoveAt(i);
            }
        }

        private void OnApplicationQuit()
        {
            SaveToolData();
        }

        private void OnEnable()
        {
            OnToolChanged += ChangeTool;
            OnColorChanged += UpdateColor;
        }
        private void OnDisable()
        {
            OnToolChanged -= ChangeTool;
            OnColorChanged -= UpdateColor;
        }

    }
}
