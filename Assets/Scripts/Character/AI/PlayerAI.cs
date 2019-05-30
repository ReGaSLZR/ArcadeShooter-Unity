using Character.Skill;
using Injection.Model;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.AI {

	public class PlayerAI : AIBehaviour {

		[SerializeField] private SkillBehaviour m_skillSpecial;

		[Range(0f, 10f)]
		[SerializeField] private float m_skillInterval = 1f;

        [Inject] private readonly PlayerStatsModel.IStatGetter m_statsGetter;
        [Inject] private readonly PlayerStatsModel.IStatSetter m_statsSetter;

        [Inject] private readonly RoundModel.IGetter m_roundGetter;
        [Inject] private readonly RoundModel.ISetter m_roundSetter;

        private DateTimeOffset m_lastFired;

		private void Awake() {
			if((m_skillDefault == null) || (m_skillSpecial == null)) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot have a NULL skills.");
				Destroy(this);
			}
		}

        protected override void Start() {
            base.Start();
            SetSkillObservers();
        }

        protected override void SafelyStopExtraComponents() {
            m_roundSetter.StopTimer();

            m_skillSpecial.StopSkill();
			Destroy(m_skillSpecial);	
		}

		private void SetSkillObservers() {
			this.UpdateAsObservable()
				.Select(_ => Input.GetMouseButtonDown(0))
				.Where(hasClickedMouse0 => hasClickedMouse0 && (m_roundGetter.GetTimer().Value > 0))
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
				.Where(hasClickedMouse1 => hasClickedMouse1 && 
                    (m_statsGetter.GetRockets().Value > 0) && (m_roundGetter.GetTimer().Value > 0))
				.Timestamp()
				.Where(timestamp => 
					(timestamp.Timestamp > m_lastFired.AddSeconds(m_skillInterval)))
				.Subscribe(timestamp => {
					m_skillSpecial.UseSkill();
                    m_statsSetter.UseRocket();
					m_lastFired = timestamp.Timestamp;
				})
				.AddTo(this);
		}
	}


}