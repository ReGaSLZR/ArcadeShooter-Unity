using UnityEngine;

namespace Misc {

    [System.Serializable]
    public class SpawnableCharacter
    {
        public GameObject m_prefabCharacter;
        public Transform m_spawnParent;

        [Space]

        [Tooltip("Number of characters spawned per batch.")]
        [Range(1, 100)]
        public int m_spawnPerBatch;

        [Header("Round Presence")]

        [Tooltip("The minimum Round number for this character to be spawned.")]
        [SerializeField] private int m_roundMin;

        [Tooltip("The maximum Round number for this character to be spawned.")]
        [SerializeField] private int m_roundMax;

        public bool IsEligibleForRound(int roundNumber) {
            return ((roundNumber >= m_roundMin) && (roundNumber <= m_roundMax));
        }

    }

}