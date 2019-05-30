using Injection.Model;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.Movement {

    public class PlayerMovement : MovementBehaviour
    {

        [SerializeField] private Vector2 m_startingPosition;

        [Inject] readonly BoundsModel.IGetter m_boundsModel;
        [Inject] readonly RoundModel.IGetter m_roundModel;

        private void Start() {
            SetInputObservers();

            m_roundModel.GetTimer()
                .Where(countdown => (countdown == 0))
                .Subscribe(_ => m_rigidBody2D.MovePosition(m_startingPosition))
                .AddTo(this);
        }

        private void SetInputObservers() {
            this.FixedUpdateAsObservable()
                .Where(_ => m_roundModel.GetTimer().Value > 0)
                .Select(_ => Input.GetAxis("Horizontal"))
                .Subscribe(x => {
                    Vector2 movement = Vector2.zero;
                    movement.x = x;
                    MoveToPosition(movement);
                })
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_roundModel.GetTimer().Value > 0)
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