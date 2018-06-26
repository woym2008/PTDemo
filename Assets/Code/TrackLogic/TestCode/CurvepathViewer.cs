/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/26 11:36:25
 *	版 本：v 1.0
 *	描 述：在Screen中显示路径
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvepathViewer : MonoBehaviour {



    void OnDrawGizmos()
    {
        Transform parent = transform;
        int childCount = parent.childCount;
        for(int i =1; i< childCount; ++ i)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(parent.GetChild(i - 1).position, parent.GetChild(i).position);
        }
       
    }
}
