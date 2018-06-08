using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class CurverTouchTile : TouchTileBase
    {
        //--------------------------------------
        public static string m_TileName = "CurverTile";
        //--------------------------------------
        public MorphCurve RenderModel;

        public Material m_SelfMaterial;

        public override void InitTile(MidiTile data, float scale, float startprocess = 0)
        {
            base.InitTile(data, scale, startprocess);

            RenderModel = this.GetComponent<MorphCurve>();

            m_SelfMaterial = this.GetComponent<MeshRenderer>().material;

            RenderModel.SetLength(scale);
            RenderModel.SetWidth(0.1f);
            RenderModel.SetDepth(0.1f);            
        }

        public override void CreateMesh(Vector3[] points)
        {
            RenderModel.CreateMesh(points);

            m_SelfMaterial = RenderModel.GetMeshMaterial();
        }

        //public override void setRotation(Quaternion rot)
        //{
        //    this.transform.rotation = rot * Quaternion.Euler(-90,0,0);
        //}
    }
}
