using UnityEngine;

namespace Character {
	
	public class Lifetime : MonoBehaviour {

		[Range(1f, 10f)]
		[SerializeField] private float m_lifeDuration;

		private void Start() {
            Destroy(this.gameObject, m_lifeDuration);	
		}

	}

}