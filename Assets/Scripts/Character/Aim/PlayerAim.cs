using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;
using Injection.Model;

namespace Character.Aim
{

    public class PlayerAim : AimBehaviour
    {

        [Inject] private readonly RoundModel.IGetter m_roundGetter;

        private void Start() {
            this.UpdateAsObservable()
                .Select(_ => m_roundGetter.GetTimer().Value)
                .Where(timer => (timer > 0))
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