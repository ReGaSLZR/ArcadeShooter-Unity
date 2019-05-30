using UnityEngine;

namespace Character.Skill {

	public class ProjectileSpawnSkill : SkillBehaviour {

		[Range(0f, 5000f)]
		[SerializeField] private float m_force = 1f;
        [SerializeField] private Collider2D m_spawnOwnerCollider2D;
		[SerializeField] private Transform m_childSpawnPoint;

        [Tooltip("Make sure the prefab has a Rigidbody2D attached to it.")]
		[SerializeField] private GameObject m_prefabProjectile;

        private void Awake() {
			if((m_prefabProjectile == null) || (m_childSpawnPoint == null) 
                || (m_spawnOwnerCollider2D == null)) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot spawn anything if Prefab for Projectile, Spawn Owner Collider " +
					"or the Child Spawn Point is NULL.");	
				Destroy(this);
			}
		}

        protected override void ExecuteUseSkill() {
//			LogUtil.PrintInfo(this, this.GetType(), "Spawning projectile...");
			GameObject projectile = m_instantiator.InstantiateInjectPrefab(
                m_prefabProjectile, m_childSpawnPoint.gameObject);
            m_spawnParent.ParentThisChild(projectile);

            AddForceToProjectile(projectile);
            AddOwnershipToProjectile(projectile);
        }

        private void AddForceToProjectile(GameObject projectile) {
            Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
            projectileBody.AddForce(projectileBody.transform.up * m_force);
        }

        private void AddOwnershipToProjectile(GameObject projectile) {
            if(projectile.tag.Equals(GameTags.Projectile.ToString())) {
                TargetDetector detector = projectile.GetComponent<TargetDetector>();
                detector.SetOwnerId(m_spawnOwnerCollider2D.GetInstanceID());
            }
        }

    }

}
