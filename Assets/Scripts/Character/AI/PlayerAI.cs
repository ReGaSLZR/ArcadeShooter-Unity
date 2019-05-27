using Character.Skill;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.AI {

	public class PlayerAI : MonoBehaviour {

		[Range(0f, 10f)]
		[SerializeField] private float m_firingInterval = 1f;

		[Header("Player Skills")]
		[SerializeField] private SkillBehaviour m_skillNormal;
		[SerializeField] private SkillBehaviour m_skillSpecial;

		private DateTimeOffset m_lastFired;

		private void Awake() {
			if(m_skillNormal == null) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot go on with a NULL skill.");
			}
		}

		private void Start() {
			SetSkillObservers();
		}

		private void SetSkillObservers() {
			this.UpdateAsObservable()
				.Select(_ => Input.GetMouseButtonDown(0))
				.Where(hasClickedMouse0 => hasClickedMouse0)
				.Timestamp()
				.Where(timestamp => 
					(timestamp.Timestamp > m_lastFired.AddSeconds(m_firingInterval)))
				.Subscribe(timestamp => {
					m_skillNormal.UseSkill();
					m_lastFired = timestamp.Timestamp;
				})
				.AddTo(this);
		}


	}


}