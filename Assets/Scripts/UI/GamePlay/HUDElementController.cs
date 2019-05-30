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

            m_stats.GetRockets()
                .Subscribe(rockets => {
                    if (rockets > m_sliderRocket.maxValue) {
                        m_sliderRocket.maxValue = rockets;
                    }

                    m_sliderRocket.value = rockets;
                    m_sliderRocketFeedback.enabled = (rockets == 0);
                })
                .AddTo(this);

            m_stats.GetShield()
                .Subscribe(shield => {
                    if (shield > m_sliderShield.maxValue) {
                        m_sliderShield.maxValue = shield;
                    }

                    m_sliderShield.value = shield;
                })
                .AddTo(this);

            m_stats.GetShieldRegen()
                .Subscribe(shieldRegen => {
                    m_sliderShieldFeedback.enabled = (shieldRegen > 0);
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
