/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/30 19:48:00
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using UnityEngine;
using UnityEditor;
using System.Collections;

public class GenerateStaticCubemap : ScriptableWizard
{
    public Transform renderPosition;
    public Cubemap cubemap;

    void OnWizardUpdate()
    {
        helpString = "Select transform to render" +
       " from and cubemap to render into";
        if (renderPosition != null && cubemap != null)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
    }
    void OnWizardCreate()
    {
        GameObject go = new GameObject("CubemapCamera");
        go.AddComponent<Camera>();

        go.transform.position = renderPosition.position;
        go.transform.rotation = Quaternion.identity;

        go.GetComponent<Camera>().RenderToCubemap(cubemap);

        DestroyImmediate(go);
    }

    [MenuItem("CookBook/Render Cubemap")]
    static void RenderCubemap()
    {
        ScriptableWizard.DisplayWizard("Render CubeMap", typeof(GenerateStaticCubemap), "Render!");
    }

}
