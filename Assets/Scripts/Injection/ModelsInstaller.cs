using Injection.Model;
using UnityEngine;
using Zenject;

namespace Injection {

    public class ModelsInstaller : MonoInstaller<ModelsInstaller>, Instantiator {

        [SerializeField] private BoundsModel m_boundsModel;
        [SerializeField] private FXModel m_fxModel;

        [Space]

        [SerializeField] private PlayerStatsModel m_playerStatsModel;

        public void InjectGameObject(GameObject gameObject) {
            Container.InjectGameObject(gameObject);
        }

        public void InstantiateInjectPrefab(GameObject prefab, GameObject parent) {
            if (prefab == null) {
                LogUtil.PrintWarning(this, this.GetType(), "Cannot InjectPrefab() with NULL prefab.");
                return;
            }

            InjectGameObject(Instantiate(prefab, parent.transform.position, parent.transform.rotation));
        }

        public override void InstallBindings() {
            Container.Bind<Instantiator>().FromInstance(this);

            Container.Bind<BoundsModel.IGetter>().FromInstance(m_boundsModel);
            Container.Bind<FXModel.IGetter>().FromInstance(m_fxModel);

            Container.Bind<PlayerStatsModel.IGetter>().FromInstance(m_playerStatsModel);
            Container.Bind<PlayerStatsModel.ISetter>().FromInstance(m_playerStatsModel);
        }

    }

}