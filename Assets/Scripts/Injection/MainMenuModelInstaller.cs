using Injection.Model;
using UnityEngine;
using Zenject;

namespace Injection {

    public class MainMenuModelInstaller : MonoInstaller<MainMenuModelInstaller>
    {

        [SerializeField] private PlayerPrefsModel m_playerPrefsModel;
        [SerializeField] private LevelModel m_levelModel;

        public override void InstallBindings() {
            Container.Bind<PlayerPrefsModel.IGetter>().FromInstance(m_playerPrefsModel);
            Container.Bind<PlayerPrefsModel.ISetter>().FromInstance(m_playerPrefsModel);

            Container.Bind<LevelModel.ISetter>().FromInstance(m_levelModel);
        }

    }

}
