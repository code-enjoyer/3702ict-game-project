using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGD
{
    /// <summary>
    /// A singleton responsible for storing data and providing functions pertaining to the current level.
    /// One level manager should be present in each level.
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Transform _spawnPoint;

        protected override void Awake()
        {
            base.Awake();

            if (_spawnPoint == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("SpawnPoint");
                if (go != null)
                    _spawnPoint = go.transform;
            }
        }

        public Vector3 SpawnLocation
        {
            get
            {
                if (_spawnPoint == null)
                {
                    Debug.LogWarning("[LevelManger] No spawn point set, returning the manager's position.", gameObject);
                    return transform.position;
                }
                else
                    return _spawnPoint.position;
            }
        }

        public void GoToLevel(string levelName, bool playAnimation = true)
        {
            GameManager.Instance.SetState(GameManager.Instance.PausedState);

            // TODO: Animation stuff here (fade out/in)

            SceneManager.LoadScene(levelName);
        }
    }

    // TODO: Make a ScriptableObject for storing the data for each level?
}
