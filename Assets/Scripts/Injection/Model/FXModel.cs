using UnityEngine;

namespace Injection.Model {

    public class FXModel : MonoBehaviour, FXModel.IGetter {

        public enum FXDeath {
            Ship, Asteroid, Projectile
        }

        public interface IGetter {
            GameObject GetRandomFXDeath(FXDeath deathType);
            GameObject GetRandomFXDamage();
        }

        [Header("Damage FX")]
        [SerializeField] private GameObject[] m_fxDamage;

        [Header("Death FX")]
        [SerializeField] private GameObject[] m_fxDeathShip;
        [SerializeField] private GameObject[] m_fxDeathProjectile;
        [SerializeField] private GameObject[] m_fxDeathAsteroid;

        private GameObject GetRandomItemFromArray(GameObject[] arrayObject) {
            return arrayObject[Random.Range(0, arrayObject.Length - 1)];
        }

        public GameObject GetRandomFXDeath(FXDeath deathType) {
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

        public GameObject GetRandomFXDamage() {
            return GetRandomItemFromArray(m_fxDamage);
        }
    }

}