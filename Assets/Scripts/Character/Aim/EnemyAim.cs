using Injection.Model;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.Aim {

    public class EnemyAim : AimBehaviour
    {

        [SerializeField] private TargetDetector m_targetDetector;

        [Space]
        [Tooltip("The number of calls before random aiming is refreshed.")]
        [Range(100, 1000)]
        [SerializeField] private int m_randomAimRefresh = 200;
        [Range(0f, 100f)]
        [SerializeField] private float m_aimSpeed = 1f;

        [Inject] readonly BoundsModel.IGetter m_boundsModel;

        private GameObject m_cachedTarget;
        private Vector3 m_cachedRandomAimPosition;
        private int m_tempRandomAimCalls = 0;

        private void Awake() {
            if(m_targetDetector == null) {
                LogUtil.PrintError(this, this.GetType(), "Cannot have NULL target detector.");
                Destroy(this);
            }
        }

        private void Start() {
            m_targetDetector.m_isTargetDetected
                .Subscribe(hasTarget => {
                    m_cachedTarget = hasTarget ? 
                        m_targetDetector.m_targets[0].gameObject : null;
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Select(_ => m_cachedTarget)
                .Subscribe(target => {
                    if(target == null) {
                        RandomAim();
                    }
                    else {
                        LockOn();
                    }
                })
                .AddTo(this);
        }

        private void LockOn() {
            //LogUtil.PrintInfo(this, this.GetType(), "Locking on...");

            transform.rotation = new Quaternion() { eulerAngles = new Vector3(0, 0, 
                GetRotationAngle(m_cachedTarget.transform.position) - 90) };
        }

        private void RandomAim() {
            //LogUtil.PrintInfo(this, this.GetType(), "Randomly aiming...");

            if (m_tempRandomAimCalls >= m_randomAimRefresh) {
                m_tempRandomAimCalls = 0;
                m_cachedRandomAimPosition = m_boundsModel.GetRandomPositionV3();
            }
            else {
                m_tempRandomAimCalls++;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.Euler(0f, 0f, GetRotationAngle(m_cachedRandomAimPosition)), Time.deltaTime * m_aimSpeed);
        }

        private float GetRotationAngle(Vector3 targetPosition) {
            Vector3 newPosition = (targetPosition - transform.position);
            return Mathf.Atan2(newPosition.y, newPosition.x) * Mathf.Rad2Deg;
        }

    }

}