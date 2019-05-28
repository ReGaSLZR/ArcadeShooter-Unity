using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character {
	
	[RequireComponent(typeof(Collider2D))]
	public class TargetDetector : MonoBehaviour {

		[SerializeField] private GameTags[] m_targetTags;

		[Tooltip("If set to FALSE, this will capture ALL targets within its range upon detection." +
			"If set to TRUE, disregard the value of Range.")]
		[SerializeField] private bool m_isLockedToOne;

		[Range(3f, 100f)]
		[SerializeField] private float m_range = 5f;

		public ReactiveProperty<bool> m_isTargetDetected { get; private set; }
		public List<Collider2D> m_targets { get; private set; }

        private Collider2D m_collider2D;
        private int m_ownerId;

		private void Awake() {
            m_collider2D = GetComponent<Collider2D>();

			m_isTargetDetected = new ReactiveProperty<bool>(false);
			m_targets = new List<Collider2D>();
		}

		private void Start() {
            if (m_collider2D.isTrigger) {
                this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => IsMatchingTag(otherCollider2D))
                .Subscribe(otherCollider2D => {
                    if (m_isTargetDetected.Value == false) {
                        RefreshTargets(otherCollider2D);
                    }
                })
                .AddTo(this);

                this.OnTriggerExit2DAsObservable()
                    .Where(otherCollider2D => IsMatchingTag(otherCollider2D))
                    .Subscribe(otherCollider2D => {
                        if (m_isTargetDetected.Value == true) {
                            m_isTargetDetected.Value = false;
                        }
                    })
                    .AddTo(this);
            }
            else {
                this.OnCollisionEnter2DAsObservable()
                .Where(otherCollision2D => IsMatchingTag(otherCollision2D.collider))
                .Subscribe(otherCollision2D => {
                    if (m_isTargetDetected.Value == false) {
                        RefreshTargets(otherCollision2D.collider);
                    }
                })
                .AddTo(this);
            }

        }

		private void RefreshTargets(Collider2D detectedCollider) {
			m_targets.Clear();
			bool hasNewTargets = false;

			if(m_isLockedToOne && IsMatchingTag(detectedCollider)) {				
				m_targets.Add(detectedCollider);
				hasNewTargets = true;
			}
			else {
				Collider2D[] tempTargets = Physics2D.OverlapCircleAll(transform.position, m_range);	

				//filter targets by tags
				foreach(Collider2D collider2D in tempTargets) {
					if(IsMatchingTag(collider2D)) {
						m_targets.Add(collider2D);
						hasNewTargets = true;
					}	
				}
			}
				
			m_isTargetDetected.Value = hasNewTargets;
		}

		private bool IsMatchingTag(Collider2D collider2D) {
            if (m_ownerId == collider2D.GetInstanceID())
            {
                return false;
            }

            for (int x=0; x<m_targetTags.Length; x++) {
				if(collider2D.gameObject.tag.Equals(m_targetTags[x].ToString())) {
					return true;
				}
			}

			return false;
		}

        public void SetOwnerId(int ownerId) {
            m_ownerId = ownerId;
        }

	}

}
	