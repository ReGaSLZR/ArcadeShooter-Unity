using UnityEngine;
using UniRx;

namespace Injection.Model {

    public class RoundModel : MonoBehaviour, RoundModel.IGetter, RoundModel.ISetter
    {

        #region Interfaces
        public interface IGetter {
            ReactiveProperty<int> GetTimer();
            int GetTimerMax();
            int GetRoundNumber();
        }

        public interface ISetter {
            void StartTimer();
            void StopTimer();
        }
        #endregion

        [Tooltip("Changes to this value will only be applied on game restart.")]
        [Range(10, 180)]
        [SerializeField] private int m_countdownInSeconds = 60;

        private const int DEFAULT_INTERVAL_TIMER_TICK = 1;

        private ReactiveProperty<int> m_reactiveCountdown;
        private int m_roundNumber;

        private void Awake() {
            m_reactiveCountdown = new ReactiveProperty<int>(-1);
            m_roundNumber = 1;
        }

        private void Start() {
            Observable.Interval(System.TimeSpan.FromSeconds(DEFAULT_INTERVAL_TIMER_TICK))
                .Where(_ => m_reactiveCountdown.Value > 0)
                .Subscribe(_ => {
                    m_reactiveCountdown.Value--;

                    if(m_reactiveCountdown.Value == 0) {
                        m_roundNumber++;
                    }
                })
                .AddTo(this);

        }

        #region ISetter functions

        public void StartTimer() {
            m_reactiveCountdown.Value = m_countdownInSeconds;
        }

        public void StopTimer() {
            m_reactiveCountdown.Value = -1;
        }

        #endregion

        #region IGetter functions

        public ReactiveProperty<int> GetTimer() {
            return m_reactiveCountdown;
        }

        public int GetTimerMax() {
            return m_countdownInSeconds;
        }

        public int GetRoundNumber() {
            return m_roundNumber;
        }

        #endregion

    }

}