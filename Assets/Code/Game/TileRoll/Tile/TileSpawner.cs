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

        public float getLength
        {
            get
            {
                return m_Length;
            }
        }

        MidiTile m_SelfData;
        float m_Length;

        public TileSpawner(
            MidiTile data, float l)
        {
            m_SelfData = data;

            m_Length = l;
        }

        public TouchTileBase CreateTile(float startprocess, string name, Vector3[] points = null)
        {
            TouchTileBase pBeat = TouchTileFactory.CreateTile(name);
            if (pBeat != null)
            {
                pBeat.InitTile(
                    m_SelfData, m_Length, startprocess);

                pBeat.CreateMesh(points);
            }

            return pBeat;
        }

    }
}
