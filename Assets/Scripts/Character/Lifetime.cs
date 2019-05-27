using System.Collections;
using UnityEngine;

namespace Character {
	
	public class Lifetime : MonoBehaviour {

		[Range(1f, 10f)]
		[SerializeField] private float m_lifeDuration;
		[Range(0, 5)]
		[SerializeField] private int m_poofDuration = 1;

		[SerializeField] private Transform m_childMain;
		[Tooltip("This is the FX for when the object has hit its lifetime limit.")]
		[SerializeField] private Transform m_childFXPoof;

		private static float TICK = 1f;

		private void Awake() {
			if(m_childMain == null) {
				LogUtil.PrintError(this, this.GetType(), "Cannot set lifetime" +
					" if MainChild is NULL.");
			}
		}

		private void Start() {
			StartCoroutine(CorLifetimeCountdown());		
		}

		private IEnumerator CorLifetimeCountdown() {
			while(m_lifeDuration > 0f) {
				yield return new WaitForSeconds(TICK);
				m_lifeDuration -= TICK;

				if((((int) m_lifeDuration) == m_poofDuration)
					&& m_childFXPoof != null) {
					m_childFXPoof.gameObject.SetActive(true);
					m_childMain.gameObject.SetActive(false);	
				}
			}

			Destroy(this.gameObject);
		}

	}

}