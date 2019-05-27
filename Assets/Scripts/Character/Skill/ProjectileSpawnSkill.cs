using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Character.Skill {

	public class ProjectileSpawnSkill : SkillBehaviour {

		[Tooltip("If !isAuto, spawning is set to await input.")]
		[SerializeField] private bool m_isAuto;
		[Range(0f, 5000f)]
		[SerializeField] private float m_force = 1f;
		[SerializeField] private Transform m_childSpawnPoint;
		[SerializeField] private GameObject m_prefabProjectile;

		private void Awake() {
			if((m_prefabProjectile == null) || (m_childSpawnPoint == null)) {
				LogUtil.PrintError(this, this.GetType(), 
					"Cannot spawn anything if Prefab for Projectile or" +
					"Child Spawn Point is NULL.");	
				Destroy(this);
			}
		}

		public override void UseSkill() {
//			LogUtil.PrintInfo(this, this.GetType(), "Spawning projectile...");
			GameObject projectile = Instantiate(m_prefabProjectile, m_childSpawnPoint.gameObject.transform.position,
				m_childSpawnPoint.gameObject.transform.rotation);
			
			Rigidbody2D projectileBody = projectile.GetComponent<Rigidbody2D>();
			projectileBody.AddForce(projectileBody.transform.up * m_force);
		}

	}

}
