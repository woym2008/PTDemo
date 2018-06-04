using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindObj : EditorWindow
{
    [MenuItem("Window/Objects")]
    // Use this for initialization
    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 640, 480);
        FindObj window = (FindObj)EditorWindow.GetWindowWithRect(typeof(FindObj), wr, true, "FindObj");
        window.Show();

    }

    void OnGUI()
    {
        if (GUILayout.Button("FindObjs", GUILayout.Width(200)))
        {
            FindObjs();
        }
    }

    void FindObjs()
    {
        if(Selection.gameObjects != null && Selection.gameObjects.Length > 0)
        {
            List<GameObject> selectobjs = new List<GameObject>();
            for(int j=0; j < Selection.gameObjects.Length; ++j)
            {
                for (int i = 0; i < Selection.gameObjects[j].transform.childCount; ++i)
                {
                    Transform trans = Selection.gameObjects[j].transform.GetChild(i);
                    if (trans != null)
                    {
                        BoxCollider c = trans.gameObject.GetComponent<BoxCollider>();
                        if (c != null)
                        {
                            ////trans.gameObject.SetActive(false);
                            c.enabled = false;
                            //selectobjs.Add(trans.gameObject);
                        }
                    }
                }
            }

            //Selection.gameObjects = selectobjs.ToArray();
        }
    }
}
