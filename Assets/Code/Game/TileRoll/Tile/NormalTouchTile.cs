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

        public override void InitTile(MidiTile data, float scale)
        {
            base.InitTile(data, scale);

            RenderModel.SetLength(scale);
            RenderModel.SetWidth(0.1f);
            RenderModel.SetDepth(0.1f);

            RenderModel.CreateMesh();

            m_SelfMaterial = RenderModel.GetMeshMaterial();
        }

        public override string GetRecycleType()
        {
            return m_TileName;
        }

    }
}
