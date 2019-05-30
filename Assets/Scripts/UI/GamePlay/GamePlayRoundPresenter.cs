using Injection;
using Injection.Model;
using Misc;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.GamePlay {

    public class GamePlayRoundPresenter : MonoBehaviour
    {

        [SerializeField] private Button m_buttonNextRound;

        [Header("Spawning during a round")]
        [Tooltip("Spawn interval in seconds.")]
        [Range(1, 20)]
        [SerializeField] private int m_spawnInterval;
        [SerializeField] private SpawnableCharacter[] m_spawnableCharacters;

        [Header("Pre-Countdown elements")]
        [SerializeField] private Image m_panelPreRoundStart;
        [SerializeField] private TextMeshProUGUI m_textRoundValue;
        [SerializeField] private TextMeshProUGUI m_textPreRoundCountdown;

        [Range(1f, 10f)]
        [SerializeField] private float m_preRoundCountdownTimer = 3f;

        [Inject] private readonly RoundModel.IGetter m_roundGetter;
        [Inject] private readonly RoundModel.ISetter m_roundSetter;

        [Inject] private readonly SpawnParentModel.ILocation m_spawnLocations;
        [Inject] private readonly SpawnParentModel.IParent m_spawnParent;

        [Inject] private readonly PlayerStatsModel.IStatGetter m_playerStats;

        [Inject] private readonly Instantiator m_instantiator;

        private const float COUNTDOWN_TICK = 1F;

        private void Start() {
            SetDestroySpawnedCharactersObservable();

            m_buttonNextRound.OnClickAsObservable()
                .Subscribe(_ => StartCoroutine(CorPlayCutscene()))
                .AddTo(this);

            m_roundGetter.GetTimer()
                .Where(timer => !m_playerStats.IsPlayerDead() && ((timer > 0) && ((timer % m_spawnInterval) == 0)))
                .Subscribe(_ => TrySpawning())
                .AddTo(this);

            StartCoroutine(CorPlayCutscene());
        }

        private void SetDestroySpawnedCharactersObservable() {
            m_playerStats.GetHealth()
                .Where(health => (health == 0))
                .Subscribe(_ => m_spawnParent.DestroyAllSpawns())
                .AddTo(this);

            m_roundGetter.GetTimer()
                .Where(timer => (timer == 0))
                .Subscribe(_ => m_spawnParent.DestroyAllSpawns())
                .AddTo(this);
        }

        private void TrySpawning() {
            foreach(SpawnableCharacter spawnable in m_spawnableCharacters) {
                if(spawnable.IsEligibleForRound(m_roundGetter.GetRoundNumber())) {
                    for(int x=0; x<(spawnable.m_spawnPerBatch
                        * m_roundGetter.GetRoundNumber()); x++) {
                            GameObject newCharacter = m_instantiator.InstantiateInjectPrefab(
                                spawnable.m_prefabCharacter,
                                m_spawnLocations.GetRandomSpawnLocation().gameObject);

                            m_spawnParent.ParentThisChild(newCharacter);
                    }    
                }
            }
        }

        private IEnumerator CorPlayCutscene() {
            m_panelPreRoundStart.gameObject.SetActive(true);
            m_textRoundValue.text = m_roundGetter.GetRoundNumber().ToString();

            float currentCountdown = m_preRoundCountdownTimer;

            while(currentCountdown > 0f) {
                m_textPreRoundCountdown.text = currentCountdown.ToString();
                yield return new WaitForSeconds(COUNTDOWN_TICK);
                currentCountdown -= COUNTDOWN_TICK;
            }

            m_panelPreRoundStart.gameObject.SetActive(false);
            m_roundSetter.StartTimer();
        }

    }

}