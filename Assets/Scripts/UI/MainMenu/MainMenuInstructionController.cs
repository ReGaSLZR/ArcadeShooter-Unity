using Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu {

    public class MainMenuInstructionController : MonoBehaviour
    {

        [SerializeField] private RawImage m_display;
        [Space]
        [SerializeField] private InstructionImage[] m_instructions;

        private void Awake() {
            if((m_display == null) || (m_instructions.Length == 0)) {
                LogUtil.PrintError(this, GetType(), "Cannot have NULL display or EMPTY instructions.");
                Destroy(this);
            }
        }

        private void Start() {
            foreach(InstructionImage instruction in m_instructions) {
                instruction.m_button.OnClickAsObservable()
                    .Subscribe(_ => m_display.texture = instruction.m_instructionTexture)
                    .AddTo(this);
            }
        }


    }

}