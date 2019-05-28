using Character.Skill;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.AI {

	public class PlayerAI : AIBehaviour {

		[SerializeField] private SkillBehaviour m_skillSpecial;

		[Range(0f, 10f)]
		[SerializeField] private float m_skillInterval = 1f;

		private DateTimeOffset m_lastFired;

		private void Awake() {
			if((m_skillDefault == null) || ((m_skillSpecial == null))) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot have a NULL skills.");
				Destroy(this);
			}
		}

		private void Start() {
			SetSkillObservers();
		}

		protected override void SafelyStopExtraComponents() {
            m_skillSpecial.StopSkill();
			Destroy(m_skillSpecial);	
		}

		private void SetSkillObservers() {
			this.UpdateAsObservable()
				.Select(_ => Input.GetMouseButtonDown(0))
				.Where(hasClickedMouse0 => hasClickedMouse0)
				.Timestamp()
				.Where(timestamp => 
					(timestamp.Timestamp > m_lastFired.AddSeconds(m_skillInterval)))
				.Subscribe(timestamp => {
					m_skillDefault.UseSkill();
					m_lastFired = timestamp.Timestamp;
				})
				.AddTo(this);

			this.UpdateAsObservable()
				.Select(_ => Input.GetMouseButtonDown(1))
				.Where(hasClickedMouse1 => hasClickedMouse1)
				.Timestamp()
				.Where(timestamp => 
					(timestamp.Timestamp > m_lastFired.AddSeconds(m_skillInterval)))
				.Subscribe(timestamp => {
					m_skillSpecial.UseSkill();
					m_lastFired = timestamp.Timestamp;
				})
				.AddTo(this);
		}
	}


}