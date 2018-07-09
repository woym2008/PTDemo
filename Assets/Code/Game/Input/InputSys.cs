using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class InputSys : MonoBehaviour
    {
        public Camera m_MainCamera;
        int m_InputLayerMask;
        private void Start()
        {
            int i = 100;
            m_InputLayerMask = 1 << LayerMask.NameToLayer("Tile");

        }
        void Update()
        {
            if(!DemoGame.m_SwitchInput)
            {
                //return;
            }

            if (m_MainCamera == null)
            {
                return;
            }

            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; ++i)
                {
                    Touch oneTouch = Input.touches[i];

                    if (oneTouch.phase == TouchPhase.Began)
                    {
                        Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitInfo;
                        if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, m_InputLayerMask))
                        {
                            if (hitInfo.collider != null)
                            {
                                EmitPressEvent(hitInfo.collider.gameObject,hitInfo.point);                                
                            }
                        }
                        
                        //Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        //Debug.Log("world :" + mouseworld.x + "," + mouseworld.y + "," + mouseworld.z);
                        //Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        //Debug.DrawLine(cam_ray.origin, cam_ray.direction);

                        //RaycastHit hitInfo;
                        //if (Physics.Raycast(cam_ray, out hitInfo, 100.0f))
                        //{
                        //    if (hitInfo.collider != null)
                        //    {
                        //        EventCenter.getInstance<TouchTileEvent>.OnPress();
                        //        if (hitInfo.collider.gameObject.tag == "Cloud")
                        //        {
                        //            //Debug.LogError("Cloud");
                        //            TouchCloud tc = hitInfo.collider.gameObject.GetComponent<TouchCloud>();
                        //            tc.MoveCloud(m_MainCamera);
                        //        }
                        //        else
                        //        {
                        //            TouchTileBase pBTC = hitInfo.collider.gameObject.GetComponent<TouchTileBase>();
                        //            if (pBTC != null)
                        //            {
                        //                pBTC.OnTouchBeat(hitInfo.point);

                        //                //m_CurClickPoint = hitInfo.point;

                        //                //tempTouchBeat = pBTC;
                        //            }
                        //        }

                        //    }
                        //}
                        return;
                    }
                    else if(oneTouch.phase == TouchPhase.Moved ||
                        oneTouch.phase == TouchPhase.Stationary) {
                        Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Debug.Log("world :" + mouseworld.x + "," + mouseworld.y + "," + mouseworld.z);
                        Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        RaycastHit hitInfo;
                        Vector3 hitpos = Vector3.zero;
                        if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, m_InputLayerMask))
                        {
                            hitpos = hitInfo.point;
                            if(hitInfo.collider == null)
                            {
                                EmitTouchingEvent(null, hitInfo.point);
                            }
                            else
                            {
                                EmitTouchingEvent(hitInfo.collider.gameObject, hitInfo.point);
                            }
                            
                        }                        
                        return;
                    }
                    
                    else if (oneTouch.phase == TouchPhase.Ended)
                    {
                        //if (tempTouchBeat != null)
                        //{
                        //    tempTouchBeat.OnEndTouch();
                        //    tempTouchBeat = null;
                        //}
                        Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitInfo;
                        Vector3 hitpos = Vector3.zero;
                        if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, m_InputLayerMask))
                        {
                            hitpos = hitInfo.point;
                        }
                        EmitEndEvent(hitInfo.point);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, m_InputLayerMask))
                {
                    if (hitInfo.collider != null)
                    {
                        EmitPressEvent(hitInfo.collider.gameObject, hitInfo.point);
                    }
                }

                //Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ////Debug.Log("world :" + mouseworld.x + "," + mouseworld.y + "," + mouseworld.z);
                //Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Debug.DrawLine(cam_ray.origin, cam_ray.direction);

                //RaycastHit hitInfo;
                //if (Physics.Raycast(cam_ray, out hitInfo, 100.0f))
                //{
                //    if (hitInfo.collider != null)
                //    {
                //        if (hitInfo.collider.gameObject.tag == "Cloud")
                //        {
                //            //Debug.LogError("Cloud");
                //            TouchCloud tc = hitInfo.collider.gameObject.GetComponent<TouchCloud>();
                //            tc.MoveCloud(m_MainCamera);
                //        }
                //        else
                //        {
                //            TouchTileBase pBTC = hitInfo.collider.gameObject.GetComponent<TouchTileBase>();
                //            if (pBTC != null)
                //            {
                //                pBTC.OnTouchBeat(hitInfo.point);

                //                //m_CurClickPoint = hitInfo.point;

                //                //tempTouchBeat = pBTC;
                //            }
                //        }

                //    }
                //}
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //if (tempTouchBeat != null)
                //{
                //    tempTouchBeat.OnEndTouch();
                //}
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                Vector3 hitpos = Vector3.zero;
                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, m_InputLayerMask))
                {
                    hitpos = hitInfo.point;
                }
                EmitEndEvent(hitInfo.point);
            }
            else if(Input.GetMouseButton(0))
            {
                Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("world :" + mouseworld.x + "," + mouseworld.y + "," + mouseworld.z);
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                Vector3 hitpos = Vector3.zero;
                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, m_InputLayerMask))
                {
                    hitpos = hitInfo.point;
                    if (hitInfo.collider == null)
                    {
                        EmitTouchingEvent(null, hitInfo.point);
                    }
                    else
                    {
                        EmitTouchingEvent(hitInfo.collider.gameObject, hitInfo.point);
                    }

                }
            }
        }

        void EmitPressEvent(GameObject touchobj, Vector3 touchpos)
        {
            TouchTileEvent pevent = new TouchTileEvent();
            pevent.reset();
            pevent.Sender = this.gameObject;
            pevent.m_TouchPos = touchpos;
            pevent.m_TouchObj = touchobj;
            //pevent.SetParam(m_PanelID, bstart);
            EventCenter.getInstance.OnPress<TouchTileEvent>(pevent);
        }

        void EmitEndEvent(Vector3 touchpos)
        {
            TouchTileEvent pevent = new TouchTileEvent();
            pevent.reset();
            pevent.Sender = this.gameObject;
            pevent.m_TouchPos = touchpos;
            pevent.m_TouchObj = null;
            EventCenter.getInstance.OnEnd<TouchTileEvent>(pevent);
        }

        void EmitTouchingEvent(GameObject touchobj, Vector3 touchpos)
        {
            TouchTileEvent pevent = new TouchTileEvent();
            pevent.reset();
            pevent.Sender = this.gameObject;
            pevent.m_TouchPos = touchpos;
            pevent.m_TouchObj = touchobj;
            EventCenter.getInstance.OnPressing<TouchTileEvent>(pevent);
        }
    }
}
