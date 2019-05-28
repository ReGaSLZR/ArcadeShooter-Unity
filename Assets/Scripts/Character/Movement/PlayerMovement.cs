using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.Movement {

    public class PlayerMovement : MovementBehaviour
    {

        private void Start() {
            SetInputObservers();
        }

        protected override void SafelyStopExtraComponents() {
            LogUtil.PrintInfo(this, GetType(), "SafelyStopExtraComponents()...");
            //TODO code stoppage of extras here
        }

        private void SetInputObservers() {
            this.FixedUpdateAsObservable()
                .Select(_ => Input.GetAxis("Horizontal"))
                .Subscribe(x => {
                    Vector2 movement = Vector2.zero;
                    movement.x = x;

                    m_rigidBody2D.position += (movement * m_movementSpeed * Time.fixedDeltaTime);
                })
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Select(_ => Input.GetAxis("Vertical"))
                .Subscribe(y => {
                    Vector2 movement = Vector2.zero;
                    movement.y = y;

                    m_rigidBody2D.position += (movement * m_movementSpeed * Time.fixedDeltaTime);
                })
                .AddTo(this);
        }

    }

}