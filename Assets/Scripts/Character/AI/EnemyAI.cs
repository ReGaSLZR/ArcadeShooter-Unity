using System.Collections;
using UnityEngine;

namespace Character.AI {

	public class EnemyAI : AIBehaviour {



		private void Awake() {
			if(m_movement == null) {
				LogUtil.PrintError(this, this.GetType(), "Cannot have NULL movement.");
				Destroy(this);
			}
		}

		private void Start() {
			
		}

		protected override void SafelyStopExtraComponents() {
			
		}
	}

}