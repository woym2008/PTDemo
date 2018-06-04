using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class TouchTileFactory
    {
        static Recycler m_Recycler;

        static public string m_BasePath = "TileRes/";

        static public void Init()
        {
            m_Recycler = new Recycler();
        }

        static public TouchTileBase CreateTile()
        {
            Debug.LogWarning("CreateTile");
            string respath = m_BasePath + NormalTouchTile.m_TileName;
            TouchTileBase tile = m_Recycler.Pop(NormalTouchTile.m_TileName) as TouchTileBase;
            if(tile == null)
            {
                tile = InstanceTilePrefab(respath);
            }
            tile.gameObject.SetActive(true);

            return tile;
        }

        static public void ReleaseTile(TouchTileBase tile)
        {
            Debug.LogWarning("ReleaseTile");
            if(tile != null)
            {
                tile.gameObject.SetActive(false);
                m_Recycler.Push(tile);
            }
        }

        private static TouchTileBase InstanceTilePrefab(string prefabName)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            GameObject go = GameObject.Instantiate(prefab);
            TouchTileBase instance = go.GetComponent<TouchTileBase>();

            if (instance == null)
            {
                Debug.LogError("null tile obj" + prefabName);
            }

            return instance;
        }
    }
}
