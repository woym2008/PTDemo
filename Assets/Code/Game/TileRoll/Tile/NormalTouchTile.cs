using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class NormalTouchTile : TouchTileBase
    {
        //--------------------------------------
        public static string m_TileName = "NormalTile";
        //--------------------------------------
        public bool m_bCanDestory;
        Material m_SelfMaterial;
        //--------------------------------------
        public MorphCube RenderModel;
        //--------------------------------------
        public float m_Height = 0.1f;
        float m_PressedHeight = 0;
        float m_speed = 2f;

        bool m_EnablePressed;
        //--------------------------------------

        public override void InitTile(MidiTile data, float scale, float startprocess, float endprocess, float delaytime)
        {
            base.InitTile(data, scale, startprocess, endprocess, delaytime);

            RenderModel = this.GetComponent<MorphCube>();

            m_SelfMaterial = this.GetComponent<MeshRenderer>().material;

            RenderModel.SetLength(m_Height);
            RenderModel.SetWidth(0.2f);
            RenderModel.SetDepth(scale);

            if(scale > 2)
            {
                RenderModel.segment_z = (int)scale + 1;
            }

            RenderModel.collidescale = 1.5f;

            m_EnablePressed = false;

            m_PressedHeight = 0;
        }

        public override void CreateMesh(Vector3[] points)
        {
            RenderModel.CreateMesh();

            m_SelfMaterial = RenderModel.GetMeshMaterial();
        }

        public override string GetRecycleType()
        {
            return m_TileName;
        }

        public override void ShowPress()
        {
            m_EnablePressed = true;
        }

        public override void FrameUpdate(float dt)
        {
            base.FrameUpdate(dt);

            if(m_EnablePressed)
            {
                if(m_PressedHeight > 2*m_Height)
                {
                    m_TouchState = TouchState.Touched;
                    return;
                }
                m_PressedHeight += dt * m_speed;
            }
            
        }

        public override void setPosition(Vector3 pos)
        {
            this.transform.position = 
                new Vector3(pos.x, pos.y - m_PressedHeight, pos.z);
        }
    }
}
