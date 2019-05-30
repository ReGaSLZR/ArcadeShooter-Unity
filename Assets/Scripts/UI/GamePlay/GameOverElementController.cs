using Injection.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GamePlay
{

    public class GameOverElementController : MonoBehaviour
    {
        [Header("Texts for Player Score")]
        [SerializeField] private TextMeshProUGUI m_textCurrentScore;
        [SerializeField] private TextMeshProUGUI m_textExtraCoins;
        [SerializeField] private TextMeshProUGUI m_textTotalScore;

        [Header("Old Score")]
        [SerializeField] private Image m_panelOldHighScore;
        [SerializeField] private TextMeshProUGUI m_textOldHighScore;
        [SerializeField] private TextMeshProUGUI m_textOldHighScorePlayer;

        [Header("New High Score")]
        [SerializeField] private Image m_panelNewHighScore;
        [SerializeField] private TMP_InputField m_inputPlayerName;
        [SerializeField] private Button m_buttonSave;
        [SerializeField] private TextMeshProUGUI m_textButtonSave;

        [Inject] private readonly PlayerPrefsModel.IGetter m_playerPrefsGetter;
        [Inject] private readonly PlayerPrefsModel.ISetter m_playerPrefsSetter;

        [Inject] private readonly PlayerStatsModel.IStatGetter m_statGetter;

        private const int COIN_MULTIPLIER = 25;
        private const string BUTTON_TEXT_SAVED = "SAVED";

        private void Start() {
            m_textOldHighScore.text = m_playerPrefsGetter.GetHighScore().ToString();
            m_textOldHighScorePlayer.text = m_playerPrefsGetter.GetHighScorePlayerName();
            ActivatePanelOldHighScore();

            SetObservables();
        }

        private void ActivatePanelNewHighScore() {
            m_panelNewHighScore.gameObject.SetActive(true);
            m_panelOldHighScore.gameObject.SetActive(false);
        }

        private void ActivatePanelOldHighScore() {
            m_panelNewHighScore.gameObject.SetActive(false);
            m_panelOldHighScore.gameObject.SetActive(true);
        }

        private int CalculateTotalScore() {
            return (m_statGetter.GetScore().Value + (m_statGetter.GetCoins().Value * COIN_MULTIPLIER));
        }

        private void ConfigurePlayerScorePanel() {
            m_textCurrentScore.text = m_statGetter.GetScore().Value.ToString();
            m_textExtraCoins.text = m_statGetter.GetCoins().Value.ToString();
            m_textTotalScore.text = CalculateTotalScore().ToString();
        }

        private void SetObservables() {
            m_statGetter.GetHealth()
                .Where(health => (health == 0))
                .Subscribe(_ => {
                    ConfigurePlayerScorePanel();

                    if (CalculateTotalScore() > m_playerPrefsGetter.GetHighScore()) {
                        ActivatePanelNewHighScore();
                    }
                })
                .AddTo(this);

            m_buttonSave.OnClickAsObservable()
                .Subscribe(_ => {
                    m_playerPrefsSetter.SetHighScore(CalculateTotalScore(), m_inputPlayerName.text);
                    m_buttonSave.interactable = false;
                    m_textButtonSave.text = BUTTON_TEXT_SAVED;
                })
                .AddTo(this);
        }

    }

}