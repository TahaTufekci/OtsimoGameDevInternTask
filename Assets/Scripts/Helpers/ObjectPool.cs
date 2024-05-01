using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Helpers
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private const int ExpandStep = 2;
    
        private readonly Queue<T> _items;

        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, int count)
        {
            _items = new Queue<T>(count);
            _prefab = prefab;
            InitializePool(prefab, count);
        }

        public ObjectPool(T prefab, int count, Transform parent)
        {
            _items = new Queue<T>(count);
            _prefab = prefab;
            _parent = parent;
            InitializePool(prefab, count, parent);
        }

        public T Get()
        {
            if (_items.Count == 0)
            {
                int c = 0;
                while (c < ExpandStep) 
                {
                    InstantiateInstance(this._prefab, this._parent);
                    c++;
                }        
            }

            return _items.Dequeue();
        }

        public void Return(T t)
        {
            _items.Enqueue(t);
        }

        private void InitializePool(T prefab, int count)
        {
            for (int i = 0; i < count; i++)
            {
                InstantiateInstance(prefab);
            }
        }
    
        private void InitializePool(T prefab, int count, Transform parent)
        {
            for (int i = 0; i < count; i++)
            {
                InstantiateInstance(prefab, parent);
            }
        }

        private void InstantiateInstance(T prefab)
        {
            T instance = Object.Instantiate(prefab);
            _items.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }
    
        private void InstantiateInstance(T prefab, Transform parent)
        {
            T instance = Object.Instantiate(prefab, parent, true);
            _items.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }
    }

    public class ObjectGameObjectPool
    {
        private const int ExpandStep = 2;
    
        private readonly Queue<GameObject> Items;

        private readonly GameObject _prefab;
        private readonly Transform _parent;

        public ObjectGameObjectPool(GameObject prefab, int count)
        {
            Items = new Queue<GameObject>(count);
            this._prefab = prefab;
            InitializePool(prefab, count);
        }

        public ObjectGameObjectPool(GameObject prefab, int count, Transform parent)
        {
            Items = new Queue<GameObject>(count);
            this._prefab = prefab;
            this._parent = parent;
            InitializePool(prefab, count, parent);
        }

        public GameObject Get()
        {
            if (Items.Count == 0)
            {
                int c = 0;
                while (c < ExpandStep) 
                {
                    InstantiateInstance(this._prefab, this._parent);
                    c++;
                }        
            }

            return Items.Dequeue();
        }

        public void Return(GameObject t)
        {
            Items.Enqueue(t);
            t.gameObject.SetActive(false);
        }

        private void InitializePool(GameObject prefab, int count)
        {
            for (int i = 0; i < count; i++)
            {
                InstantiateInstance(prefab);
            }
        }
    
        private void InitializePool(GameObject prefab, int count, Transform parent)
        {
            for (int i = 0; i < count; i++)
            {
                InstantiateInstance(prefab, parent);
            }
        }
       
        private void InstantiateInstance(GameObject prefab)
        {
            GameObject instance = Object.Instantiate(prefab);
            Items.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }
    
        private void InstantiateInstance(GameObject prefab, Transform parent)
        {
            GameObject instance = Object.Instantiate(prefab, parent, true);
            Items.Enqueue(instance);
            instance.gameObject.SetActive(false);
        }
        public int GetPoolSize()
        {
            return Items.Count;
        }
    }
}