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

        public override void InitTile(MidiTile data, float scale, float startprocess = 0)
        {
            base.InitTile(data, scale, startprocess);

            RenderModel.SetLength(0.1f);
            RenderModel.SetWidth(0.1f);
            RenderModel.SetDepth(scale);

            RenderModel.CreateMesh();

            m_SelfMaterial = RenderModel.GetMeshMaterial();
        }

        public override string GetRecycleType()
        {
            return m_TileName;
        }

    }
}
