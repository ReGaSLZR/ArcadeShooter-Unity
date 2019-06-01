using Injection.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI.MainMenu {

    public class MainMenuLevelSwitcher : MonoBehaviour
    {

        [SerializeField] private Button m_buttonPlay;
        [SerializeField] private Button m_buttonExit;

        [Inject] private readonly LevelModel.ISetter m_levelSetter;

        private void Awake() {
            if((m_buttonPlay == null) || (m_buttonExit == null)) {
                LogUtil.PrintError(this, GetType(), "Cannot have a NULL buttons.");
                Destroy(this);
            }
        }

        private void Start() {
            m_buttonPlay.OnClickAsObservable()
                .Subscribe(_ => m_levelSetter.LoadGamePlay())
                .AddTo(this);

            m_buttonExit.OnClickAsObservable()
                .Subscribe(_ => m_levelSetter.ExitGame())
                .AddTo(this);
        }
    }

}