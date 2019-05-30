using Injection.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GamePlay {

    public class HUDElementController : MonoBehaviour
    {

        [SerializeField] HealthPanel m_healthPanel;

        [Space]
        [SerializeField] Slider m_sliderRocket;
        [SerializeField] TextMeshProUGUI m_sliderRocketFeedback;

        [Space]
        [SerializeField] Slider m_sliderShield;
        [SerializeField] TextMeshProUGUI m_sliderShieldFeedback;

        [Space]
        [SerializeField] TextMeshProUGUI m_textCoins;
        [SerializeField] TextMeshProUGUI m_textScore;

        [Space]
        [SerializeField] TextMeshProUGUI m_textTimer;

        [Inject] readonly PlayerStatsModel.IStatGetter m_stats;
        [Inject] readonly RoundModel.IGetter m_round;

        private void Start() {
            m_stats.GetHealth()
                .Subscribe(health => m_healthPanel.SetHealthValue(health))
                .AddTo(this);

            m_stats.GetSpecialLimitedSkill()
                .Subscribe(rockets => {
                    if (rockets > m_sliderRocket.maxValue) {
                        m_sliderRocket.maxValue = rockets;
                    }

                    m_sliderRocket.value = rockets;
                    m_sliderRocketFeedback.gameObject.SetActive(rockets == 0);
                })
                .AddTo(this);

            m_stats.GetInvocableRechargeableSkill()
                .Subscribe(skill => {
                    if (skill > m_sliderShield.maxValue) {
                        m_sliderShield.maxValue = skill;
                    }

                    m_sliderShield.value = skill;
                    m_sliderShieldFeedback.gameObject.SetActive(skill == 0);
                })
                .AddTo(this);

            m_stats.GetCoins()
                .Subscribe(coins => m_textCoins.text = coins.ToString())
                .AddTo(this);

            m_stats.GetScore()
                .Subscribe(score => m_textScore.text = score.ToString())
                .AddTo(this);

            m_round.GetTimer()
                .Where(countdown => (countdown >= 0))
                .Subscribe(countdown => m_textTimer.text = countdown.ToString())
                .AddTo(this);
        }

    }

}
