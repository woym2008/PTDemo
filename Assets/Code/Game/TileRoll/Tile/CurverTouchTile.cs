using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class CurverTouchTile : TouchTile<TouchTileEvent>
    {
        //--------------------------------------
        public static string m_TileName = "CurverTile";
        //--------------------------------------
        public MorphCurve RenderModel;

        public Material m_SelfMaterial;

        bool m_EnablePressed;
        float m_PressedTime = 0.0f;
        public float m_Height = 0.1f;
        float m_PressedHeight = 0;
        float m_speed = 2f;

        public override void InitTile(
            MidiTile data, 
            float scale, 
            float musictime, float delaytime,
            float startpresstime
            )
        {
            base.InitTile(data, scale, musictime, delaytime,startpresstime);

            RenderModel = this.GetComponent<MorphCurve>();

            m_SelfMaterial = this.GetComponent<MeshRenderer>().material;

            RenderModel.SetLength(m_Height);
            RenderModel.SetWidth(0.2f);
            RenderModel.SetDepth(scale);

            m_EnablePressed = false;
            m_PressedHeight = 0;

            m_PressedTime = 0.0f;
        }

        public override void CreateMesh(Vector3[] points, Vector3[] normals = null)
        {
            RenderModel.CreateMesh(points, normals);

            m_SelfMaterial = RenderModel.GetMeshMaterial();
        }

        public override void setPosition(Vector3 pos)
        {
            base.setPosition(pos);

            //this.transform.position += this.transform.rotation * (new Vector3(0, -m_PressedHeight, 0));

            this.transform.position += this.transform.rotation * (new Vector3(0, -m_PressedHeight, 0));
        }

        public override void setRotation(Quaternion rot)
        {
            //base.setRotation(rot);
            //this.transform.rotation = rot;

        }

        public override void onUpdate()
        {
            base.onUpdate();

            switch(m_TouchState)
            {
                case TouchState.NotTouch:
                    break;
                case TouchState.Touching:
                    if (!m_EnablePressed)
                    {
                        if (m_PressedHeight < 0)
                        {
                            m_TouchState = TouchState.Touched;
                            return;
                        }
                        m_PressedHeight -= Time.deltaTime * m_speed;
                    }
                    else
                    {
                        m_PressedTime += Time.deltaTime;

                        if (m_PressedTime > (m_TileData.EndTime - m_TileData.StartTime))
                        {
                            //m_TileData.StopTile();
                            m_TouchState = TouchState.Touched;
                        }

                        if (m_PressedHeight < 2 * m_Height)
                        {
                            m_PressedHeight += Time.deltaTime * m_speed;
                        }
                        
                    }
                        
                    break;

            }            

        }

        public override void OnTouchBeat(Vector3 touchpos)
        {
            if (m_TouchState == TouchState.CanTouch)
            {
                m_TouchState = TouchState.Touching;
                m_EnablePressed = true;
                m_TileData.PlayTile();
            }
        }

        public override void OnEndTouch()
        {
            if(m_TouchState == TouchState.Touching)
            {
                m_TouchState = TouchState.Touched;

                m_TileData.StopTile();
            }            
        }

        public override void OnPress<TouchTileEvent>(TouchTileEvent e)
        {
            GameObject touchobj = e.m_TouchObj;
            if (touchobj == this.gameObject)
            {
                OnTouchBeat(e.m_TouchPos);
            }
        }

        public override void OnEnd<TouchTileEvent>(TouchTileEvent e)
        {
            OnEndTouch();
        }
    }
}
