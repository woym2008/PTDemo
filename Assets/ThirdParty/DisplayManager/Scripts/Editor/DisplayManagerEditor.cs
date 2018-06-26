/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/13 16:09:08
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Sunshine.DisplayManager;

[CustomEditor(typeof (DisplayManager))]
public class DisplayManagerEditor : Editor {

    private DisplayManager model;
    private string dispName = "";
    private string transStartName = "";
    private string transEndName = "";
    private float transTime = 2.0f;
    private bool bFoldOut = false;
    private bool bEditeEnv = false;
    private bool bEnvInfo = false;
    private bool bCCCurves = false;
    private float fTransTime = 0f;
    private float fDelayTime = 0f;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        model = target as DisplayManager;

        if (GUILayout.Button("Import All Settings"))
        {
            string foldPath = EditorUtility.OpenFolderPanel("Import All", "Assets/DisplayManager", "");
            int index = foldPath.IndexOf("Assets");
            foldPath = foldPath.Remove(0, index);

            var guids = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { foldPath });
            foreach (string guid in guids)
            {
                string strpath = AssetDatabase.GUIDToAssetPath(guid);
                DisplayEffectSetting info = AssetDatabase.LoadAssetAtPath(strpath, typeof(DisplayEffectSetting)) as DisplayEffectSetting;
                if(info != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(strpath);
                    model.AddEffect(filename, info);
                }
            }
        }
         
        if(GUILayout.Button("ImportAsset"))
        {
            string fullPath = EditorUtility.OpenFilePanel("Import EffectInfo", "Assets/DisplayManager", "asset");
            if (string.IsNullOrEmpty(fullPath))
            {
                Debug.LogWarning("Please select legal file");
                return;
            }
            int index = fullPath.IndexOf("Assets");
            fullPath = fullPath.Remove(0, index);
            DisplayEffectSetting objInfo = AssetDatabase.LoadAssetAtPath(fullPath, typeof(DisplayEffectSetting)) as DisplayEffectSetting;
            if(objInfo == null)
            {
                return;
            }

            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            model.AddEffect(fileName, objInfo);

            model.SetCurrentEffect(fileName, objInfo);
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New", GUILayout.Width(100)))
        {
            if (string.IsNullOrEmpty(dispName))
            {
                Debug.LogWarning("DisplayManger name can't be null..");
                return;
            }
            if (model.GetEffectSetting(dispName) != null)
            {
                Debug.LogWarning("Add DisplayEffectInfo failed! Already have DisplayEffectInfo named:" + dispName);
                return;
            }           

            string filePath = EditorUtility.SaveFilePanel("Saving DisplaySetting", "Assets/DisplayManager", dispName, "asset");
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogWarning("Illegal file path " + filePath);
                return;
            }
            int index = filePath.IndexOf("Assets");
            filePath = filePath.Remove(0 ,index);
            Debug.LogWarning(filePath);         
            
            DisplayEffectSetting objInfo = ScriptableObject.CreateInstance<DisplayEffectSetting>();
            AssetDatabase.CreateAsset(objInfo, filePath);
            AssetDatabase.Refresh();

            objInfo.GenerateEnvInfo();          

            model.AddEffect(dispName, objInfo);
        }

        GUILayout.Space(20);
        dispName = GUILayout.TextField(dispName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Transition", GUILayout.Width(80)))
        {
            if (string.IsNullOrEmpty(transEndName))
            {
                Debug.LogWarning("DisplayManger Transition name can't be null..");
                return;
            }
            if (model.GetEffectSetting(transEndName) == null)
            {
                Debug.LogWarning("Transition DisplayEffectInfo failed! don't have DisplayEffectInfo named:" + transEndName);
                return;
            }
            if (string.IsNullOrEmpty(transStartName))
            {
                model.Transition(transEndName, fTransTime, fDelayTime);
            }
            else
            {
                if (model.GetEffectSetting(transStartName) == null)
                {
                    Debug.LogWarning("Transition DisplayEffectInfo failed! don't have DisplayEffectInfo named:" + transStartName);
                    return;
                }
                DisplayEffectSetting startEffectInfo = model.GetEffectSetting(transStartName);
                DisplayEffectSetting EndEffectInfo = model.GetEffectSetting(transEndName);
                model.Transition(startEffectInfo, EndEffectInfo, fTransTime, fDelayTime);
            }
        }
        GUILayout.Label("From");
        transStartName = EditorGUILayout.TextField(transStartName);
        GUILayout.Label("End");
        transEndName = EditorGUILayout.TextField(transEndName);  
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Time");
        fTransTime = EditorGUILayout.FloatField(fTransTime);
        GUILayout.Label("Delay");
        fDelayTime = EditorGUILayout.FloatField(fDelayTime);
        GUILayout.EndHorizontal();

        // 编辑界面
        bEditeEnv = EditorGUILayout.Foldout(bEditeEnv, "编辑场景环境属性");
        if(bEditeEnv)
        {
            bEnvInfo = EditorGUILayout.Foldout(bEnvInfo, "RenderSettings:");
            if (bEnvInfo == true)            {
                
                RenderSettings.fog = GUILayout.Toggle(RenderSettings.fog, "Fog");
                RenderSettings.fogColor = EditorGUILayout.ColorField("Fog Color", RenderSettings.fogColor);
                RenderSettings.fogMode = (FogMode)EditorGUILayout.EnumPopup("Fog Mode", RenderSettings.fogMode);
                RenderSettings.fogDensity = EditorGUILayout.FloatField("Fog Density", RenderSettings.fogDensity);
                RenderSettings.fogStartDistance = EditorGUILayout.FloatField("Linear Fog Start", RenderSettings.fogStartDistance);
                RenderSettings.fogEndDistance = EditorGUILayout.FloatField("Linear Fog End", RenderSettings.fogEndDistance);
                RenderSettings.ambientLight = EditorGUILayout.ColorField("Ambient Light", RenderSettings.ambientLight);
            }        
        }
        
        
        bFoldOut = EditorGUILayout.Foldout(bFoldOut, "All Display Manager Settings:" + model.m_effects.Count);
        if (bFoldOut == true)
        {
            string bDelString = "";
            foreach (var objItem in model.m_effects)
            {
                string key = objItem.Key;
                DisplayEffectSetting objInfo = objItem.Value;
                GUILayout.BeginHorizontal();
                GUILayout.Label(key);
                if (GUILayout.Button("Transition"))
                {
                    model.Transition(key, fTransTime,fDelayTime);
                }
                if (GUILayout.Button("Exe&Save"))
                {
                    objInfo.GenerateEnvInfo();

                    // 保存当前的编辑数据
                    model.SetCurrentEffect(key,objInfo);                 
                }
                if (GUILayout.Button("Delete"))
                {
                    bDelString = key;
                }
                GUILayout.EndHorizontal();
            }
            if (!string.IsNullOrEmpty(bDelString))
            {
                model.m_effects.Remove(bDelString);
            }
        }
    }
    
}
