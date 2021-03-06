﻿using Injection.Model;
using UnityEngine;
using Zenject;

namespace Injection {

    public class GamePlayModelInstaller : MonoInstaller<GamePlayModelInstaller>, Instantiator {

        [SerializeField] private BoundsModel m_boundsModel;
        [SerializeField] private FXModel m_fxModel;
        [SerializeField] private PlayerPrefsModel m_playerPrefsModel;
        [SerializeField] private RoundModel m_timerModel;
        [SerializeField] private LevelModel m_levelModel;
        [SerializeField] private SpawnParentModel m_spawnParentModel;

        [Space]

        [SerializeField] private PlayerStatsModel m_playerStatsModel;

        public void InjectGameObject(GameObject gameObject) {
            Container.InjectGameObject(gameObject);
        }

        public GameObject InstantiateInjectPrefab(GameObject prefab, GameObject parent) {
            if (prefab == null) {
                LogUtil.PrintWarning(this, this.GetType(), "Cannot InjectPrefab() with NULL prefab.");
                return null;
            }

            GameObject newGameObject = Instantiate(prefab, parent.transform.position, parent.transform.rotation);
            InjectGameObject(newGameObject);
            return newGameObject;
        }

        public override void InstallBindings() {
            Container.Bind<Instantiator>().FromInstance(this);

            Container.Bind<BoundsModel.IGetter>().FromInstance(m_boundsModel);
            Container.Bind<FXModel.IGetter>().FromInstance(m_fxModel);

            Container.Bind<PlayerPrefsModel.IGetter>().FromInstance(m_playerPrefsModel);
            Container.Bind<PlayerPrefsModel.ISetter>().FromInstance(m_playerPrefsModel);

            Container.Bind<LevelModel.ISetter>().FromInstance(m_levelModel);

            Container.Bind<SpawnParentModel.ILocation>().FromInstance(m_spawnParentModel);
            Container.Bind<SpawnParentModel.IParent>().FromInstance(m_spawnParentModel);

            Container.Bind<RoundModel.IGetter>().FromInstance(m_timerModel);
            Container.Bind<RoundModel.ISetter>().FromInstance(m_timerModel);

            Container.Bind<PlayerStatsModel.IStatSetter>().FromInstance(m_playerStatsModel);
            Container.Bind<PlayerStatsModel.IStatGetter>().FromInstance(m_playerStatsModel);
            Container.Bind<PlayerStatsModel.ICoinSetter>().FromInstance(m_playerStatsModel);
            Container.Bind<PlayerStatsModel.IScoreSetter>().FromInstance(m_playerStatsModel);
            Container.Bind<Shop.IShop>().FromInstance(m_playerStatsModel);

        }

    }

}