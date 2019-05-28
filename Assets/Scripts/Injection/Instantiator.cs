using UnityEngine;

namespace Injection {

    public interface Instantiator
    {
        void InjectGameObject(GameObject gameObject);
        void InstantiateInjectPrefab(GameObject prefab, GameObject parent);
    }

}
