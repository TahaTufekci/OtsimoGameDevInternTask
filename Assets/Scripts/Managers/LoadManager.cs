using System;
using System.Collections.Generic;
using GameData;
using Helpers;
using Interfaces;
using Tools;
using UnityEngine;

namespace Managers
{
    public class LoadManager : MonoBehaviour,IDraw,IStamp
    {
        #region StampVariables
        [SerializeField] private Sprite stampSprite; // Sprite to be stamped
        #endregion
        
        #region SplashVariables
        [SerializeField] private Sprite[] splashSprites; // Sprite to be splashed
        #endregion

        #region PenVariables
        [SerializeField] private GameObject linePrefab; 
        private GameObject _currentLine;
        private LineRenderer _lineRenderer;
        #endregion

        #region BucketVariables
        [SerializeField] private SpriteRenderer background;
        #endregion

        #region EraserVariables
        private GameObject _eraserLine;
        private LineRenderer _eraserLineRenderer;
        #endregion

        public void LoadSavedStampPositions(StampData loadedStampData)
        {
            // Update the tool positions from the loaded data
            foreach (var stampPositions in loadedStampData.StampPositionsData)
            {
                CreateStamp(stampPositions);
            }
        }
        public void LoadSavedSplashPositions(SplashData loadedSplashData)
        {
            var colorType = loadedSplashData.colorData;
            var colorIndex = 0;
            // Update the tool positions from the loaded data
            foreach (var splashPositions in loadedSplashData.SplashPositionsData)
            {
                CreateSplash(splashPositions,colorType[colorIndex++]);
            }
        }
        private void CreateSplash(Vector3 position,ColorType colorType)
        {
            var splashObject = new GameObject("Splash")
            {
                transform =
                {
                    position = position
                }
            };
            var spriteRenderer = splashObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = splashSprites[(int)colorType];
            spriteRenderer.sortingOrder = 0;
            splashObject.transform.localScale = new Vector3(0.5f,0.5f, 1f);
            GameManager.Instance.savedDrawings.Add(splashObject);
        }

        public void LoadSavedDrawPositions(DrawData loadedDrawData)
        {
            // Update the tool positions from the loaded data
        foreach (var kvp in loadedDrawData.DrawPositionsData)
        {
            var savedDrawLineId = kvp.Key;
            var positions = kvp.Value;

            if (!loadedDrawData.DrawPositionsData.ContainsKey(savedDrawLineId))
            {
                loadedDrawData.DrawPositionsData[savedDrawLineId] = new List<Vector3>();
            }
            // Add the positions to the dictionary
            InstantiateNewLine(loadedDrawData.colorData[savedDrawLineId]);
            _lineRenderer.positionCount = loadedDrawData.DrawPositionsData[savedDrawLineId].Count;
            for (var i = 0; i <  _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, positions[i]);
            }
        }
        }
        public void LoadSavedEraserPositions(EraserData loadedEraserData)
        {
            // Update the tool positions from the loaded data
            foreach (var kvp in loadedEraserData.ErasePositionsData)
            {
                var savedEraserLineId = kvp.Key;
                var positions = kvp.Value;

                if (!loadedEraserData.ErasePositionsData.ContainsKey(savedEraserLineId))
                {
                    loadedEraserData.ErasePositionsData[savedEraserLineId] = new List<Vector3>();
                }
                InstantiateEraserLine(Color.white);
                _eraserLineRenderer.positionCount = loadedEraserData.ErasePositionsData[savedEraserLineId].Count;
                for (var i = 0; i <  _eraserLineRenderer.positionCount; i++)
                {
                    _eraserLineRenderer.SetPosition(i, positions[i]);
                }
            }
        }
        public void LoadSavedBucketColor(BucketData loadedBucketData)
        {
            background.color = loadedBucketData.colorData;
        }
        public void CreateStamp(Vector3 position)
        {
            var stampObject = new GameObject("Stamp")
            {
                transform =
                {
                    position = position
                }
            };
            var spriteRenderer = stampObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = stampSprite;
            stampObject.transform.localScale = new Vector3(0.2f,0.2f, 1f);
            spriteRenderer.sortingOrder = 0;
            GameManager.Instance.savedDrawings.Add(stampObject);
        }

        private void InstantiateEraserLine(Color color)
        {
            _eraserLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            _eraserLineRenderer = _eraserLine.GetComponent<LineRenderer>();
            _eraserLineRenderer.sortingOrder = 1;
            _eraserLineRenderer.startColor = color;
            _eraserLineRenderer.endColor = color;
             GameManager.Instance.savedDrawings.Add(_eraserLine);
        }
        public void InstantiateNewLine(Color color)
        {
            _currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            _lineRenderer = _currentLine.GetComponent<LineRenderer>();
            _lineRenderer.sortingOrder = 0;
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
            GameManager.Instance.savedDrawings.Add(_currentLine);
        }
    }
}


