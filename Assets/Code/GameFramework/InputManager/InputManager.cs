/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/04 20:24:37
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo.FrameWork
{
    public class InputManager : XSingleton<InputManager>
    {

        Camera m_Camera = null;

        private Vector2 touchStartPosition = Vector2.zero;
        bool acceptInput = true;
        public Vector2 swipeDistance = new Vector2(40, 40);
        public float swipeSensitivty = 2;       // 灵敏度
        public bool swipeToChangeSlots = false;

        int layerMask = 0;
        RaycastHit hitInfo;

        public override void Init()
        {
            m_Camera = Camera.main;

            touchStartPosition = Vector2.zero;
            acceptInput = true;

            layerMask = (1 << LayerMask.NameToLayer("Tile"));

            Input.multiTouchEnabled = true;            
        }

        private void UpdateEscape()
        {

            GameLogicUpdate();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

    

        public void Update()
        {
            GameLogicUpdate();

            UpdateEscape();
        }


        # region 游戏逻辑部分，以后根据功能详细分成不同的状态模式
        void GameLogicUpdate()
        {
            TileTouchUpdate();
        }

        //判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI
        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }


        // 检测滑块点击
        void TileTouchUpdate()
        {
            if (m_Camera == null)
            {
                return;
            }
            if (IsPointerOverUIObject())
            {
                return;
            }

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_WP8)
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; ++i)
                {
                    Touch oneTouch = Input.touches[i];

                    if (oneTouch.phase == TouchPhase.Began)
                    {
                        Ray cam_ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
                        RaycastHit hitInfo;
                        if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
                        {
                            if (hitInfo.collider != null)
                            {
                                EmitPressEvent(hitInfo.collider.gameObject,hitInfo.point);                                
                            }
                        }
                        continue;
                    }
                    else if(oneTouch.phase == TouchPhase.Moved ||
                        oneTouch.phase == TouchPhase.Stationary) {
                        //Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        //Debug.Log("world :" + mouseworld.x + "," + mouseworld.y + "," + mouseworld.z);
                        Ray cam_ray = Camera.main.ScreenPointToRay(Input.touches[i].position);

                        RaycastHit hitInfo;
                        Vector3 hitpos = Vector3.zero;
                        if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
                        {
                            hitpos = hitInfo.point;
                            if(hitInfo.collider != null)
                            {
                                EmitTouchingEvent(hitInfo.collider.gameObject, hitInfo.point);
                            }
                            else
                            {
                                EmitTouchingEvent(null, hitInfo.point);
                            }
                            
                        }                        
                        continue;
                    }
                    
                    else if (oneTouch.phase == TouchPhase.Ended)
                    {
                        Ray cam_ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
                        RaycastHit hitInfo;
                        Vector3 hitpos = Vector3.zero;
                        if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
                        {
                            hitpos = hitInfo.point;
                        }
                        EmitEndEvent(hitInfo.collider.gameObject, hitInfo.point);                      
                    }
                }
            }

#else

            if (Input.GetMouseButtonDown(0))
            {
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
                {
                    if (hitInfo.collider != null)
                    {
                        EmitPressEvent(hitInfo.collider.gameObject, hitInfo.point);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                Vector3 hitpos = Vector3.zero;
                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
                {
                    hitpos = hitInfo.point;
                }
                if(hitInfo.collider != null)
                {
                    EmitEndEvent(hitInfo.collider.gameObject, hitInfo.point);
                }
                else
                {
                    EmitEndEvent(null, hitInfo.point);
                }
                
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("world :" + mouseworld.x + "," + mouseworld.y + "," + mouseworld.z);
                Ray cam_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                Vector3 hitpos = Vector3.zero;
                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
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

#endif
        }

        void EmitPressEvent(GameObject touchobj, Vector3 touchpos)
        {
            TouchTileEvent pevent = new TouchTileEvent();
            pevent.reset();
            //pevent.Sender = this.gameObject;
            pevent.m_TouchPos = touchpos;
            pevent.m_TouchObj = touchobj;
            //pevent.SetParam(m_PanelID, bstart);
            EventCenter.getInstance.OnPress<TouchTileEvent>(pevent);
        }

        void EmitEndEvent(GameObject touchobj, Vector3 touchpos)
        {
            TouchTileEvent pevent = new TouchTileEvent();
            pevent.reset();
            //pevent.Sender = this.gameObject;
            pevent.m_TouchPos = touchpos;
            pevent.m_TouchObj = touchobj;
            EventCenter.getInstance.OnEnd<TouchTileEvent>(pevent);
        }

        void EmitTouchingEvent(GameObject touchobj, Vector3 touchpos)
        {
            TouchTileEvent pevent = new TouchTileEvent();
            pevent.reset();
            //pevent.Sender = this.gameObject;
            pevent.m_TouchPos = touchpos;
            pevent.m_TouchObj = touchobj;
            EventCenter.getInstance.OnPressing<TouchTileEvent>(pevent);
        }

        #endregion


    }
}

