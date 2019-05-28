using UniRx;
using UniRx.Triggers;

namespace Character.Movement {

    public class ProjectileMovement : MovementBehaviour
    {

        private bool m_isObservingCollisions = true;

        private void Start() {
            SetBoundsCollisionObserver();
        }

        private void SetBoundsCollisionObserver() {
            this.OnCollisionEnter2DAsObservable()
                .Where(otherCollision2D => m_isObservingCollisions &&
                    TagLayerUtil.IsEqual(otherCollision2D.gameObject.tag, GameTags.Bounds))
                .Subscribe(_ => {
                    Destroy(this.gameObject);
                })
            .AddTo(this);
        }

        protected override void SafelyStopMovementComponents() {
            m_isObservingCollisions = false;
        }

    }

}

