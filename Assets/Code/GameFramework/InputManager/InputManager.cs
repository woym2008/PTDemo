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
                        //Vector3 mouseworld = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                        Ray cam_ray = m_Camera.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
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

#else
            if (Input.GetMouseButtonDown(0))
            {
                //Vector3 mouseworld = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                Ray cam_ray = m_Camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(cam_ray, out hitInfo, 100.0f, layerMask))
                {
                    TouchTileBase pBTC = hitInfo.collider.gameObject.GetComponent<TouchTileBase>();
                    
                    if (pBTC != null)
                    {
                        pBTC.OnTouchBeat(hitInfo.point);
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

#endif
        }

        #endregion

    }
}

