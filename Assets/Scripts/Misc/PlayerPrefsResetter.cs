using UnityEngine;

namespace Misc {

    public class PlayerPrefsResetter : MonoBehaviour
    {

        [SerializeField] private bool m_shouldReset;

        private void Awake() {
            if(m_shouldReset) {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();

                LogUtil.PrintWarning(this, this.GetType(), "PlayerPrefs values reset.");
            }
        }

    }

}