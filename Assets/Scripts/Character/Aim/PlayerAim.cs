using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.Aim
{

    public class PlayerAim : AimBehaviour
    {

        private void Start() {
            this.UpdateAsObservable()
                .Select(_ => Time.timeScale)
                .Where(timeScale => (timeScale > 0))
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

    }

}