using UnityEngine;

namespace Injection.Model {

    public class BoundsModel : MonoBehaviour, BoundsModel.IGetter
    {

        [SerializeField] private Collider m_colliderMovement;

        public interface IGetter {
            Vector2 GetRandomPosition();
        }

        private void Awake() {
            if(m_colliderMovement == null) {
                LogUtil.PrintError(this, this.GetType(), "Cannot have NULL collider.");
                Destroy(this);
            }
        }

        public Vector2 GetRandomPosition() {
            Bounds bounds = m_colliderMovement.bounds;
            return new Vector2(Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y));
        }
    }

}