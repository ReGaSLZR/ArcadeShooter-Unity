using Injection.Model;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.Movement {

    public class EnemyMovement : MovementBehaviour
    {

        [Range(-0.1f, -5f)]
        [SerializeField] private float m_speedVariationDecrement;
        [Range(0.1f, 5f)]
        [SerializeField] private float m_speedVariationIncrement;

        [Header("Destination Change")]

        [Tooltip("The MINIMUM time to wait before changing destination.")]
        [Range(0.1f, 15f)]
        [SerializeField] private float m_changeIntervalMin = 3f;
        [Tooltip("The MAXIMUM time to wait before changing destination.")]
        [Range(10f, 30f)]
        [SerializeField] private float m_changeIntervalMax = 10f;

        [Inject] readonly BoundsModel.IGetter m_boundsMovement;

        private float m_currentTime;

        private float m_tempSpeed;
        private float m_tempInterval;
        private Vector2 m_tempDestination;

        private void Start() {
            this.FixedUpdateAsObservable()
                .Subscribe(_ => {
                    if (m_currentTime >= m_tempInterval) {
                        RefreshMovement();
                    }
                    else {
                        m_currentTime += Time.fixedDeltaTime;
                        m_rigidBody2D.MovePosition(Vector2.MoveTowards(
                            transform.position, m_tempDestination, m_tempSpeed * Time.fixedDeltaTime));
                    }
                })
                .AddTo(this);

            SetCollisionRefreshObserver();
        }

        private void SetCollisionRefreshObserver() {
            this.OnCollisionEnter2DAsObservable()
                .Where(otherCollision2D =>
                    TagLayerUtil.IsEqual(otherCollision2D.gameObject.tag, GameTags.Bounds) ||
                    TagLayerUtil.IsEqual(otherCollision2D.gameObject.tag, GameTags.NPC))
                .Subscribe(_ => RefreshMovement())
                .AddTo(this);
        }

        private void RefreshMovement() {
            m_tempDestination = m_boundsMovement.GetRandomPositionV2();
            m_tempInterval = Random.Range(m_changeIntervalMin, m_changeIntervalMax);
            m_tempSpeed = (m_movementSpeed + Random.Range(m_speedVariationDecrement, m_speedVariationIncrement));

            m_currentTime = 0f;
        }

    }

}
