using Injection.Model;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Misc {

    [RequireComponent(typeof(Collider2D))]
    public class StatItem : MonoBehaviour
    {

        public enum ItemType {
            Coin
            //Health, //TODO nice-to-have
            //Rocket,
            //ShieldRecharge,
            //ExtraScore
        }

        [SerializeField] private ItemType m_type;
        [Range(1, 10)]
        [SerializeField] private int m_itemValue;

        [Inject] private readonly PlayerStatsModel.ICoinSetter m_coinSetter;

        private void Start() {
            this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => TagLayerUtil.IsTagPlayer(otherCollider2D.gameObject.tag))
                .Subscribe(otherCollider2D => {
                    //LogUtil.PrintInfo(this, GetType(), "Trigger tag is: " + otherCollider2D.gameObject.tag);
                    GiveItemToPlayer();
                 })
                .AddTo(this);
        }

        private void GiveItemToPlayer() {
            switch(m_type) {
                case ItemType.Coin: {
                        m_coinSetter.IncreaseCoinsBy(m_itemValue);
                        break;
                }
                //TODO code here the case blocks for other item types (nice-to-have)
                default: {
                        LogUtil.PrintInfo(this, this.GetType(), "GiveItemToPlayer() item type unknown.");
                        break;
                }
            }

            Destroy(this.gameObject);
        }

    }

}