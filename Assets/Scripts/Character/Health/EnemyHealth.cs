namespace Character.Health {

	public class EnemyHealth : HealthBehaviour {

		protected override void ApplyDamageTick() {
			m_health--;
        }

	}

}
