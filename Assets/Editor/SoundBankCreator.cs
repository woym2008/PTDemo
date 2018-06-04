using PTAudio.Audio.SoundBank;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SoundBankCreator : EditorWindow
{
    [MenuItem("Window/SoundBankCreator")]
    // Use this for initialization
    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 640, 480);
        SoundBankCreator window = (SoundBankCreator)EditorWindow.GetWindowWithRect(typeof(SoundBankCreator), wr, true, "Create Soundbank");
        window.Show();

    }

    // Update is called once per frame
    void OnGUI()
    {        
        m_CurBankName = GUILayout.TextArea(m_CurBankName, GUILayout.Width(200));
        if (GUILayout.Button("FindDirectry", GUILayout.Width(200)))
        {
            FindDirectory();
        }
        if (GUILayout.Button("Create", GUILayout.Width(200)))
        {
            //CreateSoundbank();

            CreateSoundbankTemp();
        }
    }

    string m_CurBankName = "";
    string m_SelectDir = "";
    void FindDirectory()
    {
#if UNITY_STANDALONE_WIN
        m_SelectDir = "";
        FileOperation.OpenFilesPath(out m_SelectDir);
#endif
    }

    void CreateSoundbank()
    {
        if (m_SelectDir == "")
        {
            return;
        }

        if (m_CurBankName == "")
        {
            return;
        }

        string[] files = Directory.GetFiles(m_SelectDir, "*.mp3", SearchOption.TopDirectoryOnly);

        FileInfo info;
        FileStream stream;
        try
        {
            SoundBankConfig sbc = CreateInstance<SoundBankConfig>();

            sbc.Syms = new string[files.Length];
            for (int i = 0; i < files.Length; ++i)
            {
                sbc.Syms[i] = files[i];
            }

            sbc.Path = m_SelectDir;

            string bankassetpath = "Assets/" + m_CurBankName + ".asset";
            UnityEditor.AssetDatabase.CreateAsset(sbc, bankassetpath);
        }
        catch (IndexOutOfRangeException exp)
        {
            Debug.LogError(exp.StackTrace);
        }
    }

    void CreateSoundbankTemp()
    {
        string[] s_music_syms = new string[]{
     "C-2" ,
     "#C-2" ,
     "D-2" ,
     "#D-2",
     "E-2" ,
     "F-2" ,
     "#F-2",
     "G-2" ,
     "#G-2",
     "A-2" ,
     "#A-2",
     "B-2" ,
     "C-1" ,
     "#C-1" ,
     "D-1" ,
     "#D-1" ,
     "E-1" ,
     "F-1" ,
     "#F-1" ,
     "G-1" ,
     "#G-1" ,
     "A-1" ,
     "#A-1" ,
     "B-1" ,
     "c" ,
     "#c" ,
     "d" ,
     "#d" ,
     "e" ,
     "f" ,
     "#f" ,
     "g" ,
     "#g" ,
     "a" ,
     "#a" ,
     "b" ,
     "c1" ,
     "#c1" ,
     "d1" ,
     "#d1" ,
     "e1" ,
     "f1" ,
     "#f1" ,
     "g1" ,
     "#g1" ,
     "a1" ,
     "#a1" ,
     "b1" ,
     "c2" ,
     "#c2" ,
     "d2" ,
     "#d2" ,
     "e2" ,
     "f2" ,
     "#f2" ,
     "g2" ,
     "#g2" ,
     "a2" ,
     "#a2" ,
     "b2" ,
     "c3" ,
     "#c3" ,
     "d3" ,
     "#d3" ,
     "e3" ,
     "f3" ,
     "#f3" ,
     "g3" ,
     "#g3" ,
     "a3" ,
     "#a3" ,
     "b3" ,
     "c4" ,
     "#c4" ,
     "d4" ,
     "#d4" ,
     "e4" ,
     "f4" ,
     "#f4" ,
     "g4" ,
     "#g4" ,
     "a4" ,
     "#a4" ,
     "b4" ,
        "c5" ,
     "#c5" ,
     "d5" ,
     "e5" ,
     "#e5" ,
     "f5" ,
     "#f5" ,
     "g5" ,
     "#g5" ,
     "a5" ,
     "#a5" ,
     "b5" ,
     "c6" ,
     "#c6" ,
     "d6" ,
     "e6" ,
     "#e6" ,
     "f6" ,
     "#f6" ,
     "g6" ,
     "#g6" ,
     "a6" ,
     "#a6" ,
     "b6" ,
     "c7" ,
     "#c7" ,
     "d7" ,
     "e7" ,
     "#e7" ,
     "f7" ,
     "#f7" ,
     "g7" ,
     "#g7" ,
     "a7" ,
     "#a7" ,
     "b7" ,
     "c8" ,
     "#c8" ,
     "d8" ,
     "e8" ,
     "#e8" ,
     "f8" ,
     "#f8" ,
     "g8" ,
     "mute" ,
    };

        if (m_SelectDir == "")
        {
            return;
        }

        if (m_CurBankName == "")
        {
            return;
        }

        string[] files = Directory.GetFiles(m_SelectDir, "*.mp3", SearchOption.TopDirectoryOnly);

        FileInfo info;
        FileStream stream;
        try
        {
            SoundBankConfig sbc = CreateInstance<SoundBankConfig>();

            //sbc.Syms = new string[files.Length];
            //for (int i = 0; i < files.Length; ++i)
            //{
            //    sbc.Syms[i] = files[i];
            //}

            sbc.Syms = new string[s_music_syms.Length];
            for (int i = 0; i < s_music_syms.Length; ++i)
            {
                sbc.Syms[i] = s_music_syms[i];
            }

            sbc.Path = m_SelectDir;

            string bankassetpath = "Assets/" + m_CurBankName + ".asset";
            UnityEditor.AssetDatabase.CreateAsset(sbc, bankassetpath);
        }
        catch (IndexOutOfRangeException exp)
        {
            Debug.LogError(exp.StackTrace);
        }
    }
}
