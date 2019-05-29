using Injection.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI {

    public class UIUpdater : MonoBehaviour
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

        [Inject] readonly PlayerStatsModel.IStatGetter m_stats;

        private void Awake() {
            
        }

        private void Start() {
            m_stats.GetHealth()
                .Subscribe(health => m_healthPanel.SetHealthValue(health))
                .AddTo(this);

            m_stats.GetRockets()
                .Subscribe(rockets => {
                    m_sliderRocket.value = rockets;
                    m_sliderRocketFeedback.enabled = (rockets == 0);
                })
                .AddTo(this);

            m_stats.GetShield()
                .Subscribe(shield => {
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


        }

    }

}
