using System;
using UnityEngine;

namespace Misc {

    [Serializable]
    public class SpawnableObject
    {

        private const int MIN_CHANCE = 0;
        private const int MAX_CHANCE = 10;

        [SerializeField] private GameObject m_prefabSpawnable;
        [Range(MIN_CHANCE, MAX_CHANCE)]
        [SerializeField] private int m_spawnChance;

        public void TrySpawn(Injection.Instantiator instantiator, GameObject spawnParent) {
            if(m_prefabSpawnable == null) {
                LogUtil.PrintError(spawnParent, GetType(), "Cannot TrySpawn() if prefab is NULL.");
                return;
            }

            bool shouldSpawn = (UnityEngine.Random.Range(MIN_CHANCE, MAX_CHANCE) <= m_spawnChance);
            if(shouldSpawn) {
                instantiator.InstantiateInjectPrefab(m_prefabSpawnable, spawnParent);
            }
        }

    }

}