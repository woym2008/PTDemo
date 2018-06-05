/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/05 17:46:33
 *	版 本：v 1.0
 *	描 述：自由曲线 Mesh
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class FreeCurveMesh : MonoBehaviour {

    public Spline spline;

    public SplineMesh splinemesh;

    private List<SplineNode> nodecachelist = new List<SplineNode>();

    void OnEnable()
    {
       
        if(spline  == null )
        {
            spline = base.GetComponent<Spline>();
            if(spline == null)
            {
                Debug.LogError("Not Contain Componet Spline");
                base.enabled = false;
                return;
            }
        }
        if(splinemesh == null)
        {
            splinemesh = base.GetComponent<SplineMesh>();
            if(splinemesh == null)
            {
                base.enabled = false;
                Debug.LogError("Not Contain Componet SplineMesh");
                return;
            }
        }

        spline.updateMode = Spline.UpdateMode.DontUpdate;
        splinemesh.updateMode = SplineMesh.UpdateMode.DontUpdate;

        SetupNodeList();
        
    }

    private void SetupNodeList()
    {
        nodecachelist.Clear();
        int count = transform.childCount;

        for(int num = 0; num < count; ++ num)
        {
            Transform child = transform.GetChild(num);
            if(child.GetComponent<SplineNode>() != null )
            {
                nodecachelist.Add(child.GetComponent<SplineNode>());
            }
        }

        SplineNode[] nodeArray = spline.SplineNodes;
        for(int i = 0; i< nodeArray.Length; ++ i)
        {
            if(nodecachelist.Contains(nodeArray[i]))
            {
                nodecachelist.Remove(nodeArray[i]);
            }
        }
    }


    public int GetNodeCount()
    {
        return (spline.SplineNodes.Length + nodecachelist.Count);
    }

    public void AdjustCurveNodes(CurveNodeData[] arrNode)
    {
        int len = arrNode.Length;
        int maxlen = GetNodeCount();
        if(len > maxlen)
        {
            len = maxlen;
        }

        SplineNode[] internalNodes = spline.SplineNodes;
        int internalCount = internalNodes.Length;

        if(len < internalCount)
        {
            for(int num = len; num < internalCount; ++ num)
            {                
                spline.RemoveSplineNode(internalNodes[num]);
                nodecachelist.Add(internalNodes[num]);
            }
        }

        for(int i = 0; i< len; ++ i)
        {

        }     
                
        spline.RemoveSplineNode()
    }


    ///////////////////////////////////
#region Curve Node Data
    public class CurveNodeData
    {
        public Vector3 position{get;set;}
        public Quaternion rotation { get; set; }
    }

#endregion
}
