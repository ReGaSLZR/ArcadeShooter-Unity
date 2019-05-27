using System.Collections;
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

		private void Awake() {
			m_isTargetDetected = new ReactiveProperty<bool>(false);
			m_targets = new List<Collider2D>();
		}

		private void Start() {
			this.OnCollisionEnter2DAsObservable()
				.Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
				.Subscribe(otherCollision2D => {
					if(m_isTargetDetected.Value == false) {
						RefreshTargets(otherCollision2D.collider);
					}
				})
				.AddTo(this);
		}

		private void RefreshTargets(Collider2D detectedCollider) {
			m_targets.Clear();
			bool hasNewTargets = false;

			if(m_isLockedToOne && IsMatchingTag(detectedCollider.tag)) {				
				m_targets.Add(detectedCollider);
				hasNewTargets = true;
			}
			else {
				Collider2D[] tempTargets = Physics2D.OverlapCircleAll(transform.position, m_range);	

				//filter targets by tags
				foreach(Collider2D collider2D in tempTargets) {
					if(IsMatchingTag(collider2D.tag)) {
						m_targets.Add(collider2D);
						hasNewTargets = true;
					}	
				}
			}
				
			m_isTargetDetected.Value = hasNewTargets;
		}

		private bool IsMatchingTag(string tag) {
			for(int x=0; x<m_targetTags.Length; x++) {
				if(tag.Equals(m_targetTags[x].ToString())) {
					return true;
				}
			}

			return false;
		}

	}

}
	