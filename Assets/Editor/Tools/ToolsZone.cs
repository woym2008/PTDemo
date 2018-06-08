/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/08 16:44:28
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using UnityEngine;
using UnityEditor;

public class ToolsZone {

    [MenuItem("Tools/TextureProcessor/生成Cubemap")]
    public static void GeneriteCubemap()
    {
        ScriptableWizard.DisplayWizard("Create Cubemap", typeof(CreateRenderCubemap), "Render");
    }

    [MenuItem("Tools/TextureProcessor/将Cubemap渲染成图片")]
    public static void GeneriteCubemapRenderImg()
    {
        ScriptableWizard.DisplayWizard("Render Cubemap Img", typeof(RenderCubemapImg), "Render");
    }
}
