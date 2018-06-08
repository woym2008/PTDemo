using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class TileSpawner
    {
        public double getStartTime
        {
            get
            {
                return m_SelfData.StartTime;
            }
        }

        public double getEndTime
        {
            get
            {
                return m_SelfData.EndTime;
            }
        }

        public float getScale
        {
            get
            {
                return m_Scale;
            }
        }

        MidiTile m_SelfData;
        float m_Scale;

        public TileSpawner(
            MidiTile data, float scale)
        {
            m_SelfData = data;

            m_Scale = scale;
        }

        public TouchTileBase CreateTile(float startprocess, string name, Vector3[] points = null)
        {
            TouchTileBase pBeat = TouchTileFactory.CreateTile(name);
            if (pBeat != null)
            {
                pBeat.InitTile(
                    m_SelfData, m_Scale, startprocess);

                pBeat.CreateMesh(points);
            }

            return pBeat;
        }

    }
}
