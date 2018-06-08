/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/08 16:47:22
 *	版 本：v 1.0
 *	描 述：cubemap 文件处理
* ========================================================*/

using UnityEditor;
using UnityEngine;
using System.Collections;


#region Create Cubemap
public class CreateRenderCubemap : ScriptableWizard
{
    [Tooltip("Target render camera")]
    public Camera renderCamera;

    void OnWizardUpdate()
    {
        if(renderCamera != null)
        {
            base.isValid = true;
        }
        else
        {
            base.isValid = false;
        }
    }
    void OnWizardCreate()
    {
        var cubemap = new Cubemap(64, TextureFormat.ARGB32, false);
        renderCamera.RenderToCubemap(cubemap);

        var path = EditorUtility.SaveFilePanelInProject("Save Render Cubemap", "NewRenderCubemap", "cubemap",
                "Enter a file name to save the new render cubemap.");
        if(string.IsNullOrEmpty(path))
        {
            return;
        }

        AssetDatabase.CreateAsset(cubemap, path);
        AssetDatabase.Refresh();
    }
}

#endregion

#region Render Cubemap Imgs
//继承ScriptableWizard 脚本向导化
public class RenderCubemapImg : ScriptableWizard
{
    
    public Transform renderFromPosition; //目标位置 就是以谁为中心做CubeMap    
    public int screensize = 1024;
    public bool alpha = false;

    private string folderPaht = null;
    private string[] cubeMapImage = new string[6] { "frontImage", "leftImage", "backImage", "rightImage", "upImage", "downImage" };
    private Vector3[] cameraEuler = new Vector3[6] { new Vector3(0,0,0)
                                                    ,new Vector3(0,-90,0)
                                                    ,new Vector3(0,180,0)
                                                    ,new Vector3(0,90,0)
                                                    ,new Vector3(-90,0,0)
                                                    ,new Vector3(90,0,0)};

    void OnWizardUpdate()
    {
        helpString = "CubeMap生成插件";

        //设置启用和禁止创建按钮的组件，以便用户不能点击
        isValid = (renderFromPosition != null);
    }

    void OnWizardCreate()
    {
        folderPaht = EditorUtility.OpenFolderPanel("保存路径", "Assets", "");
        if(string.IsNullOrEmpty(folderPaht))
        {
            EditorUtility.DisplayDialog("警告", "未选择有效的文件夹路径", "确定");
            return;
        }

        GameObject go = new GameObject("CubemapCamera",typeof(Camera));
        go.transform.position = renderFromPosition.position;
        if(renderFromPosition.GetComponent<Renderer>() != null)
        {
            go.transform.position = renderFromPosition.GetComponent<Renderer>().bounds.center;
        }
        go.transform.rotation = Quaternion.identity;

        Camera camera = go.GetComponent<Camera>();
        camera.backgroundColor = Color.black;
        // 背景填充天空盒颜色
        camera.clearFlags = CameraClearFlags.Skybox;     
        //设置摄像机的视野是90° 
        camera.fieldOfView = 90f;        
        //设置长宽比为1.0f（宽度除以高度）
        camera.aspect = 1.0f;
              

        //一次渲染6张图
        for (int index = 0; index < cameraEuler.Length; index++)
        {
            renderCubeMapImage(index, go);
        }

        //渲染结束立即销毁物体
        DestroyImmediate(go);
        
        AssetDatabase.Refresh();
    }

    // 将天空盒渲染成单独的图片
    void renderCubeMapImage(int rotationIndex,GameObject go)
    {
        go.transform.eulerAngles = cameraEuler[rotationIndex];
        Camera camera = go.GetComponent<Camera>();

        //以设置的比例创建RT贴图并设置其深度
        RenderTexture rt = RenderTexture.GetTemporary(screensize, screensize, 24);
        camera.targetTexture = rt;

        //设置贴图格式不包含透明通道
        Texture2D screenShot = alpha ? new Texture2D(screensize,screensize,TextureFormat.ARGB32,false) : new Texture2D(screensize, screensize, TextureFormat.RGB24, false);
        //渲染RT贴图
        camera.Render();
        //激活渲染纹理
        RenderTexture.active = rt;

        //读取屏幕纹理信息并储存为纹理数据
        screenShot.ReadPixels(new Rect(0, 0, screensize, screensize), 0, 0);
        RenderTexture.active = null;

        //立即销毁RT贴图
        RenderTexture.ReleaseTemporary(rt);
        //DestroyImmediate(rt);

        //将此纹理编码为PNG格式，返回的字节数组是PNG“文件”。仅在纹理格式为APGB32和RGA24的情况下工作
        byte[] bytes = screenShot.EncodeToPNG();
        ////文件保存的目录
        //string directory = "Assets/CubeMaps/"+folderPaht;
        string directory = folderPaht;
        //判断目录是否存在
        if (!System.IO.Directory.Exists(directory))
        {
            //创建一个设定的路径
            System.IO.Directory.CreateDirectory(directory);
        }
        //创建一个新的文件，将规定的字节数组写入到文件中后关闭文件，如果该目标文件已经存在，将会覆盖文件
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(directory, cubeMapImage[rotationIndex]+".png"),bytes);
    }

}

#endregion
