using Injection.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GamePlay {

    public class GamePlayLevelSwitcher : MonoBehaviour
    {

        [SerializeField] private Button[] m_buttonsMainMenu;
        [SerializeField] private Button[] m_buttonsReloadLevel;

        [Inject] private readonly LevelModel.ISetter m_levelModel;

        private void Awake() {
            if((m_buttonsMainMenu.Length == 0) || (m_buttonsReloadLevel.Length == 0)) {
                LogUtil.PrintError(this, GetType(), "Cannot have 0 buttons at all.");
                Destroy(this);
            }
        }

        private void Start() {
            foreach(Button button in m_buttonsMainMenu) {
                button.OnClickAsObservable()
                    .Subscribe(_ => m_levelModel.LoadMainMenu())
                    .AddTo(this);
            }

            foreach (Button button in m_buttonsReloadLevel) {
                button.OnClickAsObservable()
                    .Subscribe(_ => m_levelModel.ReloadCurrentLevel())
                    .AddTo(this);
            }
        }
    }

}
