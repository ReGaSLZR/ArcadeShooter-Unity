using UnityEngine;
using UnityEngine.UI;

namespace UI.GamePlay {

    public class HealthPanel : MonoBehaviour
    {

        [Tooltip("Make sure the values inside this array are of ascending order.")]
        [SerializeField] private RawImage[] m_imageValues;

        private void Awake() {
            if(m_imageValues.Length == 0) {
                LogUtil.PrintError(this, this.GetType(), "Cannot have ZERO health Image values.");
                Destroy(this);
            }
        }

        private void ChangeImageAlpha(RawImage image, bool isFaded) {
            Color color = image.color;
            color.a = isFaded ? 0 : 255;
            image.color = color;
        }

        public void SetHealthValue(int health) {
            if(health > m_imageValues.Length) {
                LogUtil.PrintWarning(this, this.GetType(), "SetHealthValue() " +
                    "cannot go higher than MAX of " + m_imageValues.Length);
                return;
            }

            for(int x=0; x<m_imageValues.Length; x++) {
                ChangeImageAlpha(m_imageValues[x], (x >= health));
            }
        }


    }

}