namespace Games.Bingo
{
    using System;
using UnityEngine;

namespace BrilliantBingo.Code.Infrastructure.Views
{
    public class ReadySteadyGoView : MonoBehaviour
    {
        #region Events

        public event EventHandler Go;
        public void OnGo()
        {
        }

        #endregion

        #region Methods

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {

            gameObject.SetActive(true);
        }

        public void Ready_Sounds(int i)
        {
            if (i == 0)
            {
                SoundManager.instance._Readysteadyfx();
                UIManager.instance.Empty_Panel.SetActive(true);

            }
            else if (i == 1)
            {
            }
            else if (i == 2)
            {
               SoundManager.instance.OneRing_Fx();
                Invoke("Em_panel_Off", 0.3f);
            }
        }

        public void Em_panel_Off()
        {
                UIManager.instance.Empty_Panel.SetActive(false);
            Timer.Instance.isTime = true;
        }

        #endregion
    }
}
}