using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo.UIKeyboard
{
    public class KeyboradPressPoint : MonoBehaviour
    {        
        public Vector3 Pos
        {
            get;
            set;
        }

        public void Press()
        {
            //Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = new Ray(this.transform.position, Camera.main.transform.position);

            Debug.DrawRay(this.transform.position, Camera.main.transform.position);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100.0f))
            {
                if (hitInfo.collider != null)
                {
                    TouchTileBase pBTC = hitInfo.collider.gameObject.GetComponent<TouchTileBase>();
                    if (pBTC != null)
                    {
                        pBTC.OnTouchBeat(hitInfo.point);
                    }
                }
            }

            
        }
    }
}
