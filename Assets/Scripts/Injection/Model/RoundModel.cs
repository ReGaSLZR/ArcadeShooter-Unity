using UnityEngine;
using UniRx;

namespace Injection.Model {

    public class RoundModel : MonoBehaviour, RoundModel.IGetter, RoundModel.ISetter
    {

        #region Interfaces
        public interface IGetter {
            ReactiveProperty<int> GetTimer();
        }

        public interface ISetter {
            void StartTimer();
        }
        #endregion

        [Tooltip("Changes to this value will only be applied on game restart.")]
        [Range(10, 180)]
        [SerializeField] private int m_countdownInSeconds = 60;

        private const int DEFAULT_INTERVAL_TIMER_TICK = 1;

        private ReactiveProperty<int> m_reactiveCountdown;

        private void Awake() {
            m_reactiveCountdown = new ReactiveProperty<int>(-1);
        }

        private void Start() {
            Observable.Interval(System.TimeSpan.FromSeconds(DEFAULT_INTERVAL_TIMER_TICK))
                .Where(_ => m_reactiveCountdown.Value > 0)
                .Subscribe(x => m_reactiveCountdown.Value--)
                .AddTo(this);

        }

        public ReactiveProperty<int> GetTimer() {
            return m_reactiveCountdown;
        }

        public void StartTimer() {
            m_reactiveCountdown.Value = m_countdownInSeconds;
        }

    }

}