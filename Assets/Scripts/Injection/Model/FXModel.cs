using UnityEngine;

namespace Injection.Model {

    public class FXModel : MonoBehaviour, FXModel.IGetter {

        public enum FXDeath {
            Ship, Asteroid, Projectile
        }

        public interface IGetter {
            GameObject GetRandomFXDeath(FXDeath deathType);
        }

        [SerializeField] private GameObject[] m_fxDeathShip;
        [SerializeField] private GameObject[] m_fxDeathProjectile;
        [SerializeField] private GameObject[] m_fxDeathAsteroid;

        public GameObject GetRandomFXDeath(FXDeath deathType)
        {
            switch (deathType) {
                case FXDeath.Asteroid: {
                        return GetRandomItemFromArray(m_fxDeathAsteroid);
                }
                case FXDeath.Projectile: {
                    return GetRandomItemFromArray(m_fxDeathProjectile);
                }
                case FXDeath.Ship:
                default: {
                    return GetRandomItemFromArray(m_fxDeathShip);
                }
            }
        }

        private GameObject GetRandomItemFromArray(GameObject[] arrayObject) {
            return arrayObject[Random.Range(0, arrayObject.Length - 1)];
        }

    }

}