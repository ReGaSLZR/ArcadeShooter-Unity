using Character.Skill;
using Injection.Model;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Character.AI {

	public class PlayerAI : AIBehaviour {

		[SerializeField] private SkillBehaviour m_skillSpecialLimitedUse;
		[SerializeField] private SkillBehaviour m_skillInvocableRechargeable;

		[Range(0f, 10f)]
		[SerializeField] private float m_skillInterval = 1f;

        [Inject] private readonly PlayerStatsModel.IStatGetter m_statsGetter;
        [Inject] private readonly PlayerStatsModel.IStatSetter m_statsSetter;

        [Inject] private readonly RoundModel.IGetter m_roundGetter;
        [Inject] private readonly RoundModel.ISetter m_roundSetter;

        private DateTimeOffset m_lastFired;

		private void Awake() {
			if((m_skillDefault == null) || (m_skillSpecialLimitedUse == null) || 
                (m_skillInvocableRechargeable == null)) {
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

            m_skillSpecialLimitedUse.StopSkill();
			Destroy(m_skillSpecialLimitedUse);	
		}

		private void SetSkillObservers() {
            //Normal/Default skill
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

            //Tickable skill: Left-click mouse
			this.UpdateAsObservable()
				.Select(_ => Input.GetMouseButtonDown(1))
				.Where(hasClickedMouse1 => hasClickedMouse1 && 
                    (m_statsGetter.GetLimitedSkill().Value > 0) && (m_roundGetter.GetTimer().Value > 0))
				.Timestamp()
				.Where(timestamp => 
					(timestamp.Timestamp > m_lastFired.AddSeconds(m_skillInterval)))
				.Subscribe(timestamp => {
					m_skillSpecialLimitedUse.UseSkill();
                    m_statsSetter.UseLimitedSkill();
					m_lastFired = timestamp.Timestamp;
				})
				.AddTo(this);

            //Draining Skill: Jump/Spacebar
            this.UpdateAsObservable()
                .Select(_ => Input.GetButtonDown("Jump"))
                .Where(hasPressed => hasPressed &&
                    (m_roundGetter.GetTimer().Value > 0))
                .Subscribe(timestamp => {
                    bool isToggled = m_statsSetter.UseRechargeableSkill(!m_skillInvocableRechargeable.m_isActive);

                    if(isToggled) {
                        m_skillInvocableRechargeable.UseSkill();
                    } else {
                        LogUtil.PrintWarning(this, GetType(), "Cannot use Shield skill.");
                    }

                })
                .AddTo(this);

            //Force-stop Skill when:
            // [1] the rechargeable skill from playerStats has been drained, OR
            // [2] when the skill has been auto-recharged and not in use
            m_statsGetter.GetRechargeableSkill()
                .Where(charge => (charge == 0) || 
                    (m_statsGetter.IsRechargeableSkillOnFull() && !m_statsGetter.IsRechargeableSkillInUse()))
                .Subscribe(_ => m_skillInvocableRechargeable.StopSkill())
                .AddTo(this);
        }
	}


}