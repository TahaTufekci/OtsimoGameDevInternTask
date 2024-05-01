using System;
using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools
{
    public abstract class BaseTool : MonoBehaviour
    {
        #region Protected Variables
        protected Vector2 MousePosition;
        protected static Camera MainCamera;
        protected bool IsColorDependent;
        protected ObjectGameObjectPool Pool;
        protected bool TouchOnUIButton = false;
        protected float UiTouchCooldown = 0.5f; 
        protected float LastUITouchTime;
        [SerializeField]protected ParticleSystem[] particleEffectPrefab; // Reference to particle system
        #endregion

        #region Private Variables
        private RaycastHit2D[] _hit;
        private int _hitCount;
        #endregion

        #region Abstract
        public abstract ToolType ToolType { get; set; }
        public abstract void UseTool();
        public abstract void SaveData();
        public abstract void EmptyPool();
        public abstract void PlaySound();
        #endregion
     
        protected virtual void Awake()
        {
            if (MainCamera != null) return;
            MainCamera = Camera.main;
            _hit = new RaycastHit2D[1];
        }
        
        protected void PlayParticleEffect(Vector3 position, Color color)
        {
            if (particleEffectPrefab != null)
            {
                var particleInstance = Instantiate(particleEffectPrefab[0], position, Quaternion.identity);
                
                // Optionally parent it to the obstacle's parent
                particleInstance.transform.parent = transform.parent;
                var particleMain = particleInstance.main;
                particleMain.startColor = color;
                
                // Play the particle system
                particleInstance.Play();

                // Subscribe to the particle system's finished event to destroy it after playing
                Destroy(particleInstance.gameObject, particleInstance.main.duration);
            }
            else
            {
                Debug.Log("Particle effect prefab is not assigned to the tool: " + name);
            }
        }
        
        protected bool TouchOnButton ()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Check if finger is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
