using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    /// <summary>
    /// A singleton base class, generally inherited by managers, which is used for ensuring only one instance exists in
    /// a scene and provides a global/static reference (e.g. SomeManager.Instance).
    /// </summary>
    /// <typeparam name="T">Type of the inheriting singleton (e.g. class MySingleton : Singleton<MySingleton></typeparam>
    [DefaultExecutionOrder(-100)]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                }
                return _instance;
            }
        }

        /// <summary>
        /// Sets this instance as the static instance if none already exists, otherwise destroys this instance.
        /// NOTE: Must call base.Awake() when overriding in derived classes.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning($"[Singleton] Singleton of type {typeof(T)} already exists, destroying this copy.", gameObject);
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }
    }
}
