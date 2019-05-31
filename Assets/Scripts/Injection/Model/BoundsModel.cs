using UnityEngine;

namespace Injection.Model {

    public class BoundsModel : MonoBehaviour, BoundsModel.IGetter
    {

        [SerializeField] private Collider m_colliderMovement;

        public interface IGetter {
            Vector2 ClampPositionToBounds(Vector3 position);

            Vector2 GetRandomPositionV2();
            Vector3 GetRandomPositionV3();
        }

        private void Awake() {
            if(m_colliderMovement == null) {
                LogUtil.PrintError(this, this.GetType(), "Cannot have NULL collider.");
                Destroy(this);
            }
        }

        public Vector2 GetRandomPositionV2() {
            Bounds bounds = m_colliderMovement.bounds;
            return new Vector2(Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y));
        }

        public Vector3 GetRandomPositionV3() {
            Vector2 v2 = GetRandomPositionV2();
            return new Vector3(v2.x, v2.y, 0);
        }

        public Vector2 ClampPositionToBounds(Vector3 position) {
            Bounds bounds = m_colliderMovement.bounds;
            return new Vector2(
                Mathf.Clamp(position.x, bounds.min.x, bounds.max.x), 
                Mathf.Clamp(position.y, bounds.min.y, bounds.max.y));
        }
    }

}