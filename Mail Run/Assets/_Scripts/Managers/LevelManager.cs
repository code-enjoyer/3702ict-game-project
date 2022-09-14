using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGD
{
    /// <summary>
    /// A singleton responsible for storing data and providing functions pertaining to the current level.
    /// One level manager should be present in each level.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        public Vector3 SpawnLocation => _spawnPoint == null ? transform.position : _spawnPoint.position;
    }

    // TODO: Make a ScriptableObject for storing the data for each level?
}
