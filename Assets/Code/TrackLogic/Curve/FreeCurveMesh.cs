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

    private LinkedList<CurveNode> nodelist = new LinkedList<CurveNode>();

    void OnEnable()
    {

    }
	   
    


    ///////////////////////////////////////
    #region CurveNode Data
    /// <summary>
    /// 曲线上的路径点信息
    /// </summary>
    public class CurveNode
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public GameObject nodeObject { get; set; }      // 节点对象
    }
    #endregion
}
