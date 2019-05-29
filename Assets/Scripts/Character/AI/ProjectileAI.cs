using Injection.Model;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.AI {

	public class ProjectileAI : AIBehaviour {

        [Space]
        [SerializeField] private bool m_isFromPlayer;
        [Range(1, 200)]
        [SerializeField] private int m_onHitScoreValue;

        [Inject] PlayerStatsModel.IScoreSetter m_scoreSetter;

        private void Awake() {
			if(m_targetDetector == null) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot have a NULL target detector.");
				Destroy(this);
			}

			if(m_skillDefault == null) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot have a NULL default skill.");
				Destroy(this);
			}
		}

		private void Start() {
            SetBoundsCollisionObserver();

            m_targetDetector.m_isTargetDetected
				.Where(isDetected => isDetected)
				.Subscribe(_ => {
					m_skillDefault.UseSkill();

                    if(TagLayerUtil.IsEligibleForPlayerScoring(m_isFromPlayer, 
                        m_targetDetector.m_targets[0].gameObject.tag)) {
                            m_scoreSetter.IncreaseScoreBy(m_onHitScoreValue);
                    }
                })
				.AddTo(this);
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

        protected override void SafelyStopExtraComponents() {
			LogUtil.PrintInfo(this, this.GetType(), "No extra components to stop.");
		}
			
	}


}