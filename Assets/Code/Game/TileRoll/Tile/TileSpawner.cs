using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        MidiTile m_SelfData;
        float m_Scale;

        public TileSpawner(
            MidiTile data, float scale)
        {
            m_SelfData = data;

            m_Scale = scale;
        }

        public TouchTileBase CreateTile()
        {
            TouchTileBase pBeat = TouchTileFactory.CreateTile();
            if (pBeat != null)
            {
                pBeat.InitTile(
                    m_SelfData, m_Scale);
            }

            return pBeat;
        }
    }
}
