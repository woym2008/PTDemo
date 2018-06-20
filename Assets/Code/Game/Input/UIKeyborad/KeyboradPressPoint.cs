using Demo.TileTrack;
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

        private void Update()
        {
            
        }

        public void Press(int index)
        {
            TrackManager.instance.ActionTrack(index);

            return;

            //Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = new Ray(Camera.main.transform.position, (this.transform.position - Camera.main.transform.position).normalized);

            Debug.DrawRay(Camera.main.transform.position, (this.transform.position - Camera.main.transform.position).normalized);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100.0f))
            {
                Debug.Log("Hit ray" + hitInfo.collider.name);

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
