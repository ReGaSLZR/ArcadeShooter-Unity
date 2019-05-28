using UniRx;
using UniRx.Triggers;

namespace Character.Movement {

    public class ProjectileMovement : MovementBehaviour
    {

        private void Start() {
            SetBoundsCollisionObserver();
        }

        private void SetBoundsCollisionObserver() {
            this.OnCollisionEnter2DAsObservable()
                .Where(otherCollision2D =>
                    TagLayerUtil.IsEqual(otherCollision2D.gameObject.tag, GameTags.Bounds))
                .Subscribe(_ => {
                    Destroy(this.gameObject);
                })
            .AddTo(this);
        }

    }

}

