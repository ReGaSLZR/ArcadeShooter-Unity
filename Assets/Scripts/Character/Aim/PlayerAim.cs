using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.Aim
{

    public class PlayerAim : AimBehaviour
    {

        private bool m_isAiming = true;

        private void Start() {
            this.UpdateAsObservable()
                .Select(_ => m_isAiming)
                .Where(isAiming => isAiming)
                .Subscribe(_ => {
                    transform.up = GetMouseDirection();
                })
                .AddTo(this);
        }

        private Vector2 GetMouseDirection() {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
                Input.mousePosition);
            return new Vector2(
                mousePosition.x - transform.position.x,
                mousePosition.y - transform.position.y
            );
        }

        protected override void SafelyStopAimingComponents() {
            m_isAiming = false;
        }
    }

}