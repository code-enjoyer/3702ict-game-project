using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
        [Tooltip("Whether the singleton should persist throughout scenes.")]
        [SerializeField] private bool _isPersistant = false;
        [ShowIf("_isPersistant")]
        [Tooltip("Whether the existing instance should override this instance (i.e. should the previous scene's instance " +
            "be used if there is another instance in the new scene?)")]
        [SerializeField] private bool _preferExisting = true;

        private static T _instance;
        private static bool _isInitialized = false;

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
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Sets this instance as the static instance if none already exists, otherwise destroys this instance.
        /// NOTE: Must call base.Awake() when overriding in derived classes.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance != null)
            {
                if (_preferExisting)
                {
                    Debug.Log($"[Singleton] Singleton of type {typeof(T)} already exists and preferExisting is set to true, destroying the new copy.", _instance);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log($"[Singleton] Singleton of type {typeof(T)} already exists but preferExisting is set to false, destroying the old copy.", gameObject);
                    _instance = this as T;
                }
            }
            else
            {
                _instance = this as T;
            }

            if (_isPersistant)
            {
                DontDestroyOnLoad(gameObject);
            }

            _isInitialized = true;
        }
    }
}
