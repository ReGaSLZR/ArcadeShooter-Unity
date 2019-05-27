using Character.Skill;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.AI {

	public class PlayerAI : AIBehaviour {

		[Header("-------------------")]
		[SerializeField] private SkillBehaviour m_skillSpecial;

		[Range(0f, 10f)]
		[SerializeField] private float m_firingInterval = 1f;

		private DateTimeOffset m_lastFired;

		private void Awake() {
			if(m_skillDefault == null) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot have a NULL default skill.");
				Destroy(this);
			}
		}

		private void Start() {
			SetSkillObservers();
		}

		protected override void SafelyStopExtraComponents() {
			if(m_skillSpecial != null) {
				Destroy(m_skillSpecial);	
			}
		}

		private void SetSkillObservers() {
			if (m_skillDefault == null) {
				LogUtil.PrintInfo(this, this.GetType(), "No Default Skill available.");
			} else {
				this.UpdateAsObservable()
					.Select(_ => Input.GetMouseButtonDown(0))
					.Where(hasClickedMouse0 => hasClickedMouse0)
					.Timestamp()
					.Where(timestamp => 
						(timestamp.Timestamp > m_lastFired.AddSeconds(m_firingInterval)))
					.Subscribe(timestamp => {
						m_skillDefault.UseSkill();
						m_lastFired = timestamp.Timestamp;
					})
					.AddTo(this);
			}
				
		}
	}


}