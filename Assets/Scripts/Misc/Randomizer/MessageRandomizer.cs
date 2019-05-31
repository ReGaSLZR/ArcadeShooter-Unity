using TMPro;
using UnityEngine;

namespace Misc.Randomizer {

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MessageRandomizer : MonoBehaviour
    {
		[TextArea]
        [SerializeField] private string[] m_messages;

        private void OnEnable() {
            if(m_messages.Length == 0) {
                LogUtil.PrintError(this, GetType(), "Cannot have 0 messages.");
                Destroy(this);
            }

            TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
            tmp.text = m_messages[Random.Range(0, m_messages.Length)];
        }

    }

}

