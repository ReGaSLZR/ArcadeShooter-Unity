using UnityEngine;
using UnityEngine.UI;

namespace UI {

    /*
        NOTE: This class was created to cater to the need
        to initially let all major panels under the Canvas
        have an active state.

        Active state is required for all UI-related MonoBehaviours
        (tied up to UI-related gameObjects) to work their magic.

        If they are not active by the time the game starts,
        and it so happens that the said MonoBehaviours are needed
        by other classes outside of the Panels, null exceptions
        show up.

        However, it is inevitable for devs to deactivate UIs on purpose,
        especially on design time. So in order to make up for that
        inevitability, this class was created.

        Deactivating of the panels will be done by the PanelSwitchers.
    */
    public class PanelWaker : MonoBehaviour
    {

        [SerializeField] private Image[] m_panels;

        private void Awake() {
            foreach (Image panel in m_panels) {
                if (panel != null) {
                    panel.gameObject.SetActive(true);
                }
            }

            Destroy(this);
        }

    }

}