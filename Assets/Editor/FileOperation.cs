using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

public class OpenDialogFile
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenDialogDir
{
    public IntPtr hwndOwner = IntPtr.Zero;
    public IntPtr pidlRoot = IntPtr.Zero;
    public String pszDisplayName = null;
    public String lpszTitle = null;
    public UInt32 ulFlags = 0;
    public IntPtr lpfn = IntPtr.Zero;
    public IntPtr lParam = IntPtr.Zero;
    public int iImage = 0;
}

public class DllOpenFileDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenDialogFile ofn);
    //public static bool GetOpenFileName1([In, Out] OpenDialogFile ofn)

    //{
    //    return GetOpenFileName(ofn);
    //}

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenDialogFile ofn);

    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

}


public class FileOperation
{
    public static void OpenFilesPath(out string exportPath)
    {
        OpenDialogDir ofn2 = new OpenDialogDir();

        ofn2.pszDisplayName = new string(new char[2000]); ;     // 存放目录路径缓冲区    
        ofn2.lpszTitle = "Open Project";// 标题    
        //ofn2.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // 新的样式,带编辑框    
        IntPtr pidlPtr = DllOpenFileDialog.SHBrowseForFolder(ofn2);

        char[] charArray = new char[2000];
        for (int i = 0; i < 2000; i++)
            charArray[i] = '\0';

        DllOpenFileDialog.SHGetPathFromIDList(pidlPtr, charArray);
        string fullDirPath = new String(charArray);

        fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));

        Debug.Log(fullDirPath);//这个就是选择的目录路径。 

        exportPath = fullDirPath;
    }

    public static string[] GetFileNames(string path, string extension = "")
    {
        string[] files = Directory.GetFiles(path);

        List<string> findnames = new List<string>();

        foreach (var name in files)
        {
			#if UNITY_STANDALONE_WIN
            string filename = name.Substring(name.LastIndexOf("\\") + 1);
            string[] names = filename.Split('.');
            
            if(names.Length > 1 && names[1] == extension)
            {
                findnames.Add(filename);
            }
			#elif UNITY_STANDALONE_OSX
			string filename = name.Substring(name.LastIndexOf("/") + 1);
			string[] names = filename.Split('.');

			if(names.Length > 1 && names[1] == extension)
			{
				findnames.Add(filename);
			}
			#endif
        }

        return findnames.ToArray();
    }
}
