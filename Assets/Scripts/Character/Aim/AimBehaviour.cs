using UnityEngine;

namespace Character.Aim {
	
	public abstract class AimBehaviour : MonoBehaviour {

		public void StopAiming() {
			//LogUtil.PrintInfo(this, this.GetType(), "StopAiming() called.");
            Destroy(this);
		}

	}

}