/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/30 15:41:20
 *	版 本：v 1.0
 *	描 述：Superlines 的菜单栏扩展
* ========================================================*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


static public class SuperLinesMenue
{
    [MenuItem("Tools/SuperSplines/Create/SplineGameObject")]
    static public void CreateSplineGameobject()
    {
        GameObject go = new GameObject("SplineObject");
        Transform _trans = go.transform;
        _trans.localPosition = Vector3.zero;
        _trans.localRotation = Quaternion.identity;

        Spline sp = go.AddComponent<Spline>();

        sp.interpolationMode = Spline.InterpolationMode.Hermite;        
        sp.rotationMode = Spline.RotationMode.Tangent;
        sp.tangentMode = Spline.TangentMode.UseNodeForwardVector;
        sp.perNodeTension = false;
        sp.tension = 3.0f;
        sp.updateMode = Spline.UpdateMode.DontUpdate;


        List<SplineNode> nodeList = new List<SplineNode>();
        for(int i = 0;i < 7; ++ i)
        {
            GameObject node = new GameObject();
            node.transform.parent = _trans;            
            node.transform.localRotation = Quaternion.identity;
            node.name = i.ToString("D4");

            SplineNode spn = node.AddComponent<SplineNode>();

            nodeList.Add(spn);
        }

        for (int index = 0; index < nodeList.Count; ++index)
        {
            nodeList[index].transform.localPosition = new Vector3(0, 0, index * 2);
        }

        sp.splineNodesArray = nodeList;


        SplineMesh sm = go.AddComponent<SplineMesh>();
        sm.spline = sp;
        sm.updateMode = SplineMesh.UpdateMode.WhenSplineChanged;
        sm.uvMode = SplineMesh.UVMode.InterpolateV;
        sm.uvScale = Vector2.one;
        sm.xyScale = Vector2.one;
        sm.segmentCount = 100;
        sm.splitMode = SplineMesh.SplitMode.DontSplit;       

        MeshRenderer mr = go.AddComponent<MeshRenderer>();

    }

}
