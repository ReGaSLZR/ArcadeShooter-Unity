using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Injection.Model {

    public class LevelModel : MonoBehaviour, LevelModel.ISetter
    {

        #region Interface

        public interface ISetter {
            void LoadMainMenu();
            void LoadGamePlay();
            void ReloadCurrentLevel();
        }

        #endregion

        /*
            NOTE: Reflect here the indexes defined in Build Settings 
        */
        private const int INDEX_MAIN_MENU = 0;
        private const int INDEX_GAMEPLAY = 1;
        
        [SerializeField] private Image m_panelLoading;
        [SerializeField] private Button[] m_buttonsToDisable;

        private void Start() {
            m_panelLoading.gameObject.SetActive(false);
        }

        private int GetCurrentSceneIndex() {
            return SceneManager.GetActiveScene().buildIndex;
        }

        private void LoadScene(int index) {
            SceneManager.UnloadSceneAsync(GetCurrentSceneIndex());

            LogUtil.PrintInfo(this, GetType(), "Loading scene index " + index);
            Time.timeScale = 1;
            m_panelLoading.gameObject.SetActive(true);
            DisableAllButtons();

            SceneManager.LoadScene(index);
        }

        private void DisableAllButtons() {
            foreach (Button button in m_buttonsToDisable) {
                button.interactable = false;
            }
        }

        public void LoadMainMenu() {
            LoadScene(INDEX_MAIN_MENU);
        }

        public void LoadGamePlay() {
            LoadScene(INDEX_GAMEPLAY);
        }

        public void ReloadCurrentLevel() {
            LoadScene(GetCurrentSceneIndex());
        }
    }


}