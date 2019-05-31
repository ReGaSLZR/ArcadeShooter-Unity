using Injection.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GamePlay {

    public class GamePlayPanelSwitcher : MonoBehaviour
    {

        [Header("GamePlay Panels")]
        [SerializeField] Image m_panelGamePlay;
        [SerializeField] Image m_panelPause;
        [SerializeField] Image m_panelShop;
        [SerializeField] Image m_panelGameOver;

        [Header("Buttons for Switching Panels")]
        [SerializeField] Button[] m_buttonsPause;
        [SerializeField] Button[] m_buttonsResume;

        [Inject] private readonly RoundModel.IGetter m_roundGetter;
        [Inject] private readonly PlayerStatsModel.IStatGetter m_statGetter;

        private void Start() {
            SetButtonObservers();
            SetStatusObservers();

            ActivateOnePanel(m_panelGamePlay);
        }

        private void ActivateOnePanel(Image image) {
            DeactivateAllPanels();
            image.gameObject.SetActive(true);
        }

        private void DeactivateAllPanels() {
            m_panelGamePlay.gameObject.SetActive(false);
            m_panelGameOver.gameObject.SetActive(false);
            m_panelPause.gameObject.SetActive(false);
            m_panelShop.gameObject.SetActive(false);
        }

        private void SetButtonObservers() {
            foreach(Button pauseButton in m_buttonsPause) {
                pauseButton.OnClickAsObservable()
                    .Subscribe(_ => {
                        ActivateOnePanel(m_panelPause);
                        Time.timeScale = 0;
                    })
                    .AddTo(this);
            }

            foreach(Button resumeButton in m_buttonsResume) {
                resumeButton.OnClickAsObservable()
                    .Subscribe(_ => {
                        ActivateOnePanel(m_panelGamePlay);
                        Time.timeScale = 1;
                    })
                    .AddTo(this);
            }
        }

        private void SetStatusObservers() {
            m_statGetter.GetHealth()
               .Where(health => (health == 0))
               .Subscribe(_ => ActivateOnePanel(m_panelGameOver))
               .AddTo(this);

            m_roundGetter.GetTimer()
                .Where(timer => ((timer == 0) && !m_statGetter.IsPlayerDead()))
                .Subscribe(_ => {
                    ActivateOnePanel(m_panelShop);
                })
                .AddTo(this);
        }

    }

}