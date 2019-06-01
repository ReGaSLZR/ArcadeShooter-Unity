using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu {

    public class MainMenuPanelSwitcher : MonoBehaviour
    {

        [SerializeField] private Image m_panelMainContent;
        [SerializeField] private Button m_buttonMainContent;
		[Space]
        [SerializeField] private Image m_panelInstructions;
        [SerializeField] private Button m_buttonInstructions;

        private void Awake() {
            if((m_panelMainContent == null) || (m_panelInstructions == null)) {
                LogUtil.PrintError(this, GetType(), "Cannot have NULL panels.");
                Destroy(this);
            }
        }

        private void Start() {
            SetObservables();
            ActivateOnePanel(m_panelMainContent);
        }

        private void ActivateOnePanel(Image panel) {
            DeActivateAllPanels();
            panel.gameObject.SetActive(true);
        }

        private void DeActivateAllPanels() {
            m_panelMainContent.gameObject.SetActive(false);
            m_panelInstructions.gameObject.SetActive(false);
        }

        private void SetObservables() {
            m_buttonMainContent.OnClickAsObservable()
                .Subscribe(_ => ActivateOnePanel(m_panelMainContent))
                .AddTo(this);

            m_buttonInstructions.OnClickAsObservable()
                .Subscribe(_ => ActivateOnePanel(m_panelInstructions))
                .AddTo(this);
        }

    }


}