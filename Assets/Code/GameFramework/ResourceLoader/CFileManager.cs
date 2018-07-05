/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/29 16:30:39
 *	版 本：v 1.0
 *	描 述：文件管理系统
* ========================================================*/

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public enum enFileOperation
{
    ReadFile,
    WriteFile,
    DeleteFile,
    CreateDirectory,
    DeleteDirectory
}

public class CFileManager
{
    public delegate void DelegateOnOperateFileFail(string fullPath, enFileOperation fileOperation);
   
    public static DelegateOnOperateFileFail s_delegateOnOperateFileFail = delegate{};

    private static MD5CryptoServiceProvider s_md5Provider = new MD5CryptoServiceProvider();

    // 当期在编辑器下
    public static string s_ifsExtractFolder = "Resources";

    private static string s_ifsExtractPath = null;

    private static string s_cachePath = null;
      
    
    // 本地目录的前端标识
    private static string GetLocalPathHeader()
    {
        return "file://";
    }

    // 清空目录下的所有内容
    public static bool ClearDirectory(string fullPath)
    {
        try
        {
            string[] files = Directory.GetFiles(fullPath);
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
            string[] directories = Directory.GetDirectories(fullPath);
            for (int j = 0; j < directories.Length; j++)
            {
                Directory.Delete(directories[j], true);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ClearDirectory(string fullPath, string[] fileExtensionFilter, string[] folderFilter)
    {
        try
        {
            if (fileExtensionFilter != null)
            {
                string[] files = Directory.GetFiles(fullPath);
                for (int i = 0; i < files.Length; i++)
                {
                    if ((fileExtensionFilter != null) && (fileExtensionFilter.Length > 0))
                    {
                        for (int j = 0; j < fileExtensionFilter.Length; j++)
                        {
                            if (files[i].Contains(fileExtensionFilter[j]))
                            {
                                DeleteFile(files[i]);
                                break;
                            }
                        }
                    }
                }
            }
            if (folderFilter != null)
            {
                string[] directories = Directory.GetDirectories(fullPath);
                for (int k = 0; k < directories.Length; k++)
                {
                    if ((folderFilter != null) && (folderFilter.Length > 0))
                    {
                        for (int m = 0; m < folderFilter.Length; m++)
                        {
                            if (directories[k].Contains(folderFilter[m]))
                            {
                                DeleteDirectory(directories[k]);
                                break;
                            }
                        }
                    }
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string CombinePath(string path1, string path2)
    {
        if (path1.LastIndexOf('/') != (path1.Length - 1))
        {
            path1 = path1 + "/";
        }
        if (path2.IndexOf('/') == 0)
        {
            path2 = path2.Substring(1);
        }
        return (path1 + path2);
    }

    public static string CombinePaths(params string[] values)
    {
        if (values.Length <= 0)
        {
            return string.Empty;
        }
        if (values.Length == 1)
        {
            return CombinePath(values[0], string.Empty);
        }
        if (values.Length <= 1)
        {
            return string.Empty;
        }
        string str = CombinePath(values[0], values[1]);
        for (int i = 2; i < values.Length; i++)
        {
            str = CombinePath(str, values[i]);
        }
        return str;
    }

    public static void CopyFile(string srcFile, string dstFile)
    {
        File.Copy(srcFile, dstFile, true);
    }

    public static bool CreateDirectory(string directory)
    {
        if (IsDirectoryExist(directory))
        {
            return true;
        }
        int num = 0;
        while (true)
        {
            try
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            catch (Exception exception)
            {
                num++;
                if (num >= 3)
                {
                    Debug.Log("Create Directory " + directory + " Error! Exception = " + exception.ToString());
                    s_delegateOnOperateFileFail(directory, enFileOperation.CreateDirectory);
                    return false;
                }
            }
        }
    }

    public static bool DeleteDirectory(string directory)
    {
        if (!IsDirectoryExist(directory))
        {
            return true;
        }
        int num = 0;
        while (true)
        {
            try
            {
                Directory.Delete(directory, true);
                return true;
            }
            catch (Exception exception)
            {
                num++;
                if (num >= 3)
                {
                    Debug.Log("Delete Directory " + directory + " Error! Exception = " + exception.ToString());
                    s_delegateOnOperateFileFail(directory, enFileOperation.DeleteDirectory);
                    return false;
                }
            }
        }
    }

    public static bool DeleteFile(string filePath)
    {
        if (!IsFileExist(filePath))
        {
            return true;
        }
        int num = 0;
        while (true)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception exception)
            {
                num++;
                if (num >= 3)
                {
                    Debug.Log("Delete File " + filePath + " Error! Exception = " + exception.ToString());
                    s_delegateOnOperateFileFail(filePath, enFileOperation.DeleteFile);
                    return false;
                }
            }
        }
    }

    public static string EraseExtension(string fullName)
    {
        if (fullName == null)
        {
            return null;
        }
        int length = fullName.LastIndexOf('.');
        if (length > 0)
        {
            return fullName.Substring(0, length);
        }
        return fullName;
    }

    public static string GetCachePath()
    {
        if (s_cachePath == null)
        {
            s_cachePath = Application.persistentDataPath;
        }
        return s_cachePath;
    }

    public static string GetCachePath(string fileName)
    {
        return CombinePath(GetCachePath(), fileName);
    }

    public static string GetCachePathWithHeader(string fileName)
    {
        return (GetLocalPathHeader() + GetCachePath(fileName));
    }

    public static string GetExtension(string fullName)
    {
        int startIndex = fullName.LastIndexOf('.');
        if ((startIndex > 0) && ((startIndex + 1) < fullName.Length))
        {
            return fullName.Substring(startIndex);
        }
        return string.Empty;
    }

    public static int GetFileLength(string filePath)
    {
        if (!IsFileExist(filePath))
        {
            return 0;
        }
        int num = 0;
        while (true)
        {
            try
            {
                FileInfo info = new FileInfo(filePath);
                return (int) info.Length;
            }
            catch (Exception exception)
            {
                num++;
                if (num >= 3)
                {
                    Debug.Log("Get FileLength of " + filePath + " Error! Exception = " + exception.ToString());
                    return 0;
                }
            }
        }
    }

    public static string GetFileMd5(string filePath)
    {
        if (!IsFileExist(filePath))
        {
            return string.Empty;
        }
        return BitConverter.ToString(s_md5Provider.ComputeHash(ReadFile(filePath))).Replace("-", string.Empty);
    }

    public static string GetFullDirectory(string fullPath)
    {
        return Path.GetDirectoryName(fullPath);
    }

    public static string GetFullName(string fullPath)
    {
        if (fullPath == null)
        {
            return null;
        }
        int num = fullPath.LastIndexOf("/");
        if (num > 0)
        {
            return fullPath.Substring(num + 1, (fullPath.Length - num) - 1);
        }
        return fullPath;
    }

    public static string GetIFSExtractPath()
    {
        if (s_ifsExtractPath == null)
        {
            s_ifsExtractPath = CombinePath(GetCachePath(), s_ifsExtractFolder);
        }
        return s_ifsExtractPath;
    }

    public static string GetMd5(byte[] data)
    {
        return BitConverter.ToString(s_md5Provider.ComputeHash(data)).Replace("-", string.Empty);
    }

    public static string GetMd5(string str)
    {
        return BitConverter.ToString(s_md5Provider.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty);
    }

    public static string GetStreamingAssetsPathWithHeader(string fileName)
    {
        return Path.Combine(Application.streamingAssetsPath, fileName);
    }

    public static bool IsDirectoryExist(string directory)
    {
        return Directory.Exists(directory);
    }

    public static bool IsFileExist(string filePath)
    {
        return File.Exists(filePath);
    }

    public static byte[] ReadFile(string filePath)
    {
        if (IsFileExist(filePath))
        {
            byte[] buffer = null;
            int num = 0;
            do
            {
                try
                {
                    buffer = File.ReadAllBytes(filePath);
                }
                catch (Exception exception)
                {
                    Debug.Log(string.Concat(new object[] { "Read File ", filePath, " Error! Exception = ", exception.ToString(), ", TryCount = ", num }));
                    buffer = null;
                }
                if ((buffer != null) && (buffer.Length > 0))
                {
                    return buffer;
                }
                num++;
            }
            while (num < 3);
            Debug.Log(string.Concat(new object[] { "Read File ", filePath, " Fail!, TryCount = ", num }));
            s_delegateOnOperateFileFail(filePath, enFileOperation.ReadFile);
        }
        return null;
    }

    public static bool WriteFile(string filePath, byte[] data)
    {
        int num = 0;
        while (true)
        {
            try
            {
                File.WriteAllBytes(filePath, data);
                return true;
            }
            catch (Exception exception)
            {
                num++;
                if (num >= 3)
                {
                    Debug.Log("Write File " + filePath + " Error! Exception = " + exception.ToString());
                    DeleteFile(filePath);
                    s_delegateOnOperateFileFail(filePath, enFileOperation.WriteFile);
                    return false;
                }
            }
        }
    }

    public static bool WriteFile(string filePath, byte[] data, int offset, int length)
    {
        FileStream stream = null;
        int num = 0;
        while (true)
        {
            try
            {
                stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                stream.Write(data, offset, length);
                stream.Close();
                return true;
            }
            catch (Exception exception)
            {
                if (stream != null)
                {
                    stream.Close();
                }
                num++;
                if (num >= 3)
                {
                    Debug.Log("Write File " + filePath + " Error! Exception = " + exception.ToString());
                    DeleteFile(filePath);
                    s_delegateOnOperateFileFail(filePath, enFileOperation.WriteFile);
                    return false;
                }
            }
        }
    }

    
}


