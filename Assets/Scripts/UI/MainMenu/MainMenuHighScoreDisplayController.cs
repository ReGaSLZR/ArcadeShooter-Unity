using Injection.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu
{

    public class MainMenuHighScoreDisplayController : MonoBehaviour
    {

        [SerializeField] private Image m_panelNoHighScore;

        [Space]

        [SerializeField] private Image m_panelHighScore;
        [SerializeField] private TextMeshProUGUI m_textHighScore;
        [SerializeField] private TextMeshProUGUI m_textHighScorePlayer;

        [Space]

        [SerializeField] private TextMeshProUGUI m_textPlayButton;
        [SerializeField] private string m_buttonTextDefault = "Play Now";
        [SerializeField] private string m_buttonTextChallenge = "Challenge Accepted";

        [Inject] private readonly PlayerPrefsModel.IGetter m_playerPrefsGetter;

        private void Start() {
            bool hasHighScore = (m_playerPrefsGetter.GetHighScore() > 0);

            m_textPlayButton.text = hasHighScore ? 
                m_buttonTextChallenge : m_buttonTextDefault;

            m_panelHighScore.gameObject.SetActive(hasHighScore);
            m_panelNoHighScore.gameObject.SetActive(!hasHighScore);

            if (hasHighScore) {
                m_textHighScore.text = m_playerPrefsGetter.GetHighScore().ToString();
                m_textHighScorePlayer.text = m_playerPrefsGetter.GetHighScorePlayerName();
            }
            else {
                LogUtil.PrintInfo(this, GetType(), "No high score to display.");
            }

            Destroy(this);
        }

    }

}