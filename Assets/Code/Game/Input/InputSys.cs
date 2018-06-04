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

        void Update()
        {
            if (m_MainCamera == null)
            {
                return;
            }

            //if (Input.touchCount > 0)
            //{
            //    for (int i = 0; i < Input.touchCount; ++i)
            //    {
            //        Touch oneTouch = Input.touches[i];

            //        if (oneTouch.phase == TouchPhase.Began)
            //        {
            //            Vector3 worldPos = m_MainCamera.ScreenToWorldPoint(oneTouch.position);
            //            //bool bTouchedBeat = false;
                       
            //            //m_CurClickPoint = worldPos;

            //            Debug.LogWarning("InputTouch, oneTouch Began Screen Posx:" + oneTouch.position.x + "Posy:" + oneTouch.position.y);
            //            Debug.LogWarning("InputTouch, oneTouch Began World Posx:" + worldPos.x + "Posy:" + worldPos.y);

            //            Ray cam_ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);
            //            RaycastHit hitInfo;
            //            if (Physics.Raycast(cam_ray, out hitInfo, 100.0f))
            //            {
            //                Debug.LogWarning("InputTouch, hysics.Raycast ok");
            //                if (hitInfo.collider != null)
            //                {
            //                    if(hitInfo.collider.gameObject.tag == "Cloud")
            //                    {
            //                        TouchCloud tc = hitInfo.collider.gameObject.GetComponent<TouchCloud>();
            //                        tc.MoveCloud(m_MainCamera);
            //                    }
            //                    else
            //                    {
            //                        TouchTileBase pBTC = hitInfo.collider.gameObject.GetComponent<TouchTileBase>();
            //                        if (pBTC != null)
            //                        {
            //                            Debug.LogWarning("InputTouch, OnTouchBeat");
            //                            pBTC.OnTouchBeat(hitInfo.point);

            //                            //PressedTouchBeat = pBTC;

            //                            //bTouchedBeat = true;

            //                            //tempTouchBeat = pBTC;
            //                        }
            //                    }                                
            //                }
            //            }
            //            return;
            //        }
            //        //else if (oneTouch.phase == TouchPhase.Moved)
            //        //{
            //        //    Vector3 worldPos = m_MainCamera.ScreenToWorldPoint(oneTouch.position);
            //        //    //Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //        //    bool bTouchedBeat = false;

            //        //    //EffectManager.getInstance.PlayEffect("BlingEffect", mousePos2D, Quaternion.identity);

            //        //    m_CurClickPoint = worldPos;

            //        //    Debug.LogWarning("InputTouch, oneTouch Began Screen Posx:" + oneTouch.position.x + "Posy:" + oneTouch.position.y);
            //        //    Debug.LogWarning("InputTouch, oneTouch Began World Posx:" + worldPos.x + "Posy:" + worldPos.y);

            //        //    Ray cam_ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);
            //        //    RaycastHit hitInfo;
            //        //    if (Physics.Raycast(cam_ray, out hitInfo, 100.0f))
            //        //    {
            //        //        Debug.LogWarning("InputTouch, hysics.Raycast ok");
            //        //        if (hitInfo.collider != null)
            //        //        {
            //        //            ITouchBeat pBTC = hitInfo.collider.gameObject.GetComponent<ITouchBeat>();
            //        //            if (pBTC != null)
            //        //            {
            //        //                Debug.LogWarning("InputTouch, OnTouchBeat");
            //        //                pBTC.OnTouchBeat(hitInfo.point);

            //        //                //PressedTouchBeat = pBTC;

            //        //                bTouchedBeat = true;
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //        else if (oneTouch.phase == TouchPhase.Ended)
            //        {
            //            //if (tempTouchBeat != null)
            //            //{
            //            //    tempTouchBeat.OnEndTouch();
            //            //    tempTouchBeat = null;
            //            //}
            //        }
            //    }
            //}

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("world :" + mouseworld.x + "," + mouseworld.y + "," + mouseworld.z);
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Debug.DrawLine(cam_ray.origin, cam_ray.direction);

                RaycastHit hitInfo;
                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f))
                {
                    if (hitInfo.collider != null)
                    {
                        if (hitInfo.collider.gameObject.tag == "Cloud")
                        {
                            //Debug.LogError("Cloud");
                            TouchCloud tc = hitInfo.collider.gameObject.GetComponent<TouchCloud>();
                            tc.MoveCloud(m_MainCamera);
                        }
                        else
                        {
                            TouchTileBase pBTC = hitInfo.collider.gameObject.GetComponent<TouchTileBase>();
                            if (pBTC != null)
                            {
                                pBTC.OnTouchBeat(hitInfo.point);

                                //m_CurClickPoint = hitInfo.point;

                                //tempTouchBeat = pBTC;
                            }
                        }
                            
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //if (tempTouchBeat != null)
                //{
                //    tempTouchBeat.OnEndTouch();
                //}
            }
        }
    }
}
