using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo
{
    public class InputPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        int m_PanelID = -1;

        void initInputPanel(int id)
        {
            m_PanelID = id;
        }

        void postEvent(bool bstart)
        {
            PressTrack pevent = new PressTrack();
            pevent.reset();
            pevent.Sender = this.gameObject;
            pevent.SetParam(m_PanelID, bstart);

            //EventCenter.getInstance.OnEvent<PressTrack>(pevent);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            postEvent(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            postEvent(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            postEvent(false);
        }
    }
}
