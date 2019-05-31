using UnityEngine;

namespace Misc.Randomizer {

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRandomizer : MonoBehaviour
    {

        [SerializeField] private Sprite[] m_sprites;

        private void Awake() {
            if(m_sprites.Length == 0) {
                LogUtil.PrintError(this, GetType(), "Cannot have 0 textures.");
                Destroy(this);
            }
		}
			
		private void OnEnable() {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = m_sprites[Random.Range(0, m_sprites.Length)];

            Destroy(this);
        }

    }

}

