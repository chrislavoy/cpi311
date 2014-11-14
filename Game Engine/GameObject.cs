using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class GameObject
    {
        public Transform Transform { get; protected set; }
        // Common components
        public Camera Camera { get { return Get<Camera>(); } }
        public Rigidbody Rigidbody { get { return Get<Rigidbody>(); } }
        public Renderer Renderer { get { return Get<Renderer>(); } }
        public Light Light { get { return Get<Light>(); } }
        // All Components
        private Dictionary<Type,Component> Components {get; set;}

        public GameObject()
        {
            Transform = new Transform();
        }

        public T Add<T>() where T:Component, new()
        {
            Remove<T>();
            T component = new T();
            component.GameObject = this;
            component.Transform = Transform;
            Components.Add(typeof(T), component);
            return component;
        }

        public T Get<T>() where T:Component
        {
            if (Components.ContainsKey(typeof(T)))
                return Components[typeof(T)] as T;
            else
                return null;
        }

        public void Remove<T>() where T:Component
        {
            if (Components.ContainsKey(typeof(T)))
                Components.Remove(typeof(T));
        }
    }
}
