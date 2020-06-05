using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Core
{
    public class PersistentOBJSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistanceOBjPrefab;
        static bool HasSpawned = false;

       
        private void Awake()
        {
            if (HasSpawned) return;
            SpawnPersistentOBJ();
            HasSpawned = true;
        }

        private void SpawnPersistentOBJ()
        {
            GameObject presistanceOBJ = Instantiate( persistanceOBjPrefab);
            DontDestroyOnLoad(presistanceOBJ);
        }
    }
}
