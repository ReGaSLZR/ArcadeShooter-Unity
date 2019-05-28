using UnityEngine;

namespace Injection.Model {

    public class BoundsModel : MonoBehaviour, BoundsModel.IGetter
    {

        [SerializeField] private Collider m_colliderMovement;

        public interface IGetter {
            Vector2 ClampPositionToScreenBounds(Vector3 position);

            Vector2 GetRandomPositionV2();
            Vector3 GetRandomPositionV3();
        }

        private Vector3 m_screenBoundsMin;
        private Vector3 m_screenBoundsMax;

        private void Awake() {
            if(m_colliderMovement == null) {
                LogUtil.PrintError(this, this.GetType(), "Cannot have NULL collider.");
                Destroy(this);
            }

            m_screenBoundsMin = Camera.main.ScreenToWorldPoint(new Vector3(1, 1, 0));
            m_screenBoundsMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 1, Screen.height - 3, 0));
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

        public Vector2 ClampPositionToScreenBounds(Vector3 position) {
            return new Vector2(
                Mathf.Clamp(position.x, m_screenBoundsMin.x, m_screenBoundsMax.x), 
                Mathf.Clamp(position.y, m_screenBoundsMin.y, m_screenBoundsMax.y));
        }
    }

}