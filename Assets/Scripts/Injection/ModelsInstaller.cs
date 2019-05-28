using Injection.Model;
using UnityEngine;
using Zenject;

namespace Injection {

    public class ModelsInstaller : MonoInstaller<ModelsInstaller> {

        [SerializeField] private BoundsModel m_boundsModel;

        public override void InstallBindings() {
            Container.Bind<BoundsModel.IGetter>().FromInstance(m_boundsModel);
        }

    }

}