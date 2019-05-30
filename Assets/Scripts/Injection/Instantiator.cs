using UnityEngine;

namespace Injection {

    public interface Instantiator
    {
        void InjectGameObject(GameObject gameObject);
        GameObject InstantiateInjectPrefab(GameObject prefab, GameObject parent);
    }

}
