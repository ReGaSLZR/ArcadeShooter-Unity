using Injection.Model;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.Movement {

    public class PlayerMovement : MovementBehaviour
    {

        [Inject] readonly BoundsModel.IGetter m_boundsModel;

        private void Start() {
            SetInputObservers();
        }

        protected override void SafelyStopMovementComponents() {
            LogUtil.PrintInfo(this, GetType(), "SafelyStopExtraComponents()...");
            //TODO code stoppage of extras here
        }

        private void SetInputObservers() {
            this.FixedUpdateAsObservable()
                .Select(_ => Input.GetAxis("Horizontal"))
                .Subscribe(x => {
                    Vector2 movement = Vector2.zero;
                    movement.x = x;
                    MoveToPosition(movement);
                })
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Select(_ => Input.GetAxis("Vertical"))
                .Subscribe(y => {
                    Vector2 movement = Vector2.zero;
                    movement.y = y;

                    MoveToPosition(movement);
                })
                .AddTo(this);
        }

        private void MoveToPosition(Vector2 position) {
            m_rigidBody2D.position += (position * m_movementSpeed * Time.fixedDeltaTime);
            m_rigidBody2D.MovePosition(m_boundsModel.ClampPositionToScreenBounds(m_rigidBody2D.position));
        }

    }

}