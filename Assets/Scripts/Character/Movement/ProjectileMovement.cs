using UniRx;
using UniRx.Triggers;


namespace Character.Movement {

    public class ProjectileMovement : MovementBehaviour
    {

        private bool m_isObservingCollisions = true;

        private void Start() {
            this.OnCollisionEnter2DAsObservable()
            .Where(otherCollision2D => m_isObservingCollisions && TagLayerUtil.IsUntagged(otherCollision2D.gameObject.tag))
            .Subscribe(_ => {
                Destroy(this.gameObject);
            })
            .AddTo(this);
        }

        protected override void SafelyStopExtraComponents() {
            m_isObservingCollisions = false;
        }

    }

}

