using UnityEngine;
using Zenject;
using Injection.Model;

namespace UI.GamePlay {

    public class GamePlayShopPresenter : MonoBehaviour
    {
        [Inject] private readonly RoundModel.IGetter m_timerGetter;
        [Inject] private readonly RoundModel.ISetter m_timerSetter;

        private void Start() {
            //m_timerGetter.GetTimer()
                //.Where(timer => (timer <= 0))
                //.Subscribe(_ => {
                        //Configure shop items
                //})
                //.AddTo(this);

            //TODO just a test; remove this line later
            m_timerSetter.StartTimer();
        }

    }


}