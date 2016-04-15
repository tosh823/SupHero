using UnityEngine;
using System.Collections.Generic;

namespace SupHero {

    public class Pool<T> : MonoBehaviour where T : MonoBehaviour {

        protected List<T> items;
        public int capacity;

        public virtual void Start() {
            
        }

        public virtual void Update() {

        }

        public virtual void Init(int maxCapacity) {
            capacity = maxCapacity;
            items = new List<T>(capacity);
        }

        protected virtual bool canPush() {
            return (items.Count < capacity);
        }

        protected virtual bool containsItems() {
            return (items.Count > 0);
        }

        public T popOrCreate(T prefab) {
            return popOrCreate(prefab, Vector3.zero, Quaternion.identity);
        }

        public T popOrCreate(T prefab, Vector3 position, Quaternion rotation) {
            if (containsItems()) {
                return pop(position, rotation);
            }
            else {
                T instance = (T)Instantiate(prefab, position, rotation);
                push(instance);
                return pop(position, rotation);
            }
        }

        protected T pop(Vector3 position, Quaternion rotation) {
            int index = items.Count - 1;
            T item = items[index];
            items.RemoveAt(index);
            item.transform.position = position;
            item.transform.rotation = rotation;
            return item;
        }

        public bool push(T item) {
            if (canPush()) {
                items.Add(item);
                return true;
            }
            else return false;
        }
        
    }
}
