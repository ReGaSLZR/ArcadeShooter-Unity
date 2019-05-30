using UnityEngine;

namespace Injection.Model {

    public class SpawnParentModel : MonoBehaviour, 
        SpawnParentModel.ILocation, SpawnParentModel.IParent
    {
        #region Interfaces
        public interface ILocation {
            Transform GetRandomSpawnLocation();
        }

        public interface IParent {
            void ParentThisChild(GameObject child);
            void DestroyAllSpawns();
        }
        #endregion

        [Tooltip("The empty gameObject designated to hold all objects " +
            "spawned for management (i.e. mass destruction).")]
        [SerializeField] private Transform m_parent;
        [SerializeField] private Transform[] m_spawnableLocations;

        private int m_lastRandomLocationIndex;

        private void Awake() {
            if((m_parent == null) || (m_spawnableLocations.Length == 0)) {
                LogUtil.PrintError(this, GetType(), "Cannot have " +
                    "NULL or EMPTY values on the variables.");
                Destroy(this);
            }
        }

        public void ParentThisChild(GameObject child) {
            child.transform.SetParent(m_parent);
        }

        public void DestroyAllSpawns() {
            if(m_parent.childCount > 0) {
                for (int x=(m_parent.childCount - 1); x >= 0; x--) {
                    Destroy(m_parent.GetChild(x).gameObject);
                }
            }
        }

        public Transform GetRandomSpawnLocation() {
            int tempLocation = m_lastRandomLocationIndex;

            while (tempLocation == m_lastRandomLocationIndex) {
                tempLocation = Random.Range(0, m_spawnableLocations.Length - 1);
            }

            m_lastRandomLocationIndex = tempLocation;
            return m_spawnableLocations[tempLocation];
        }
    }

}