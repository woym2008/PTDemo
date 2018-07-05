/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/29 16:19:30
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo.FrameWork
{
    internal sealed partial class ResourceManager :XSingleton<ResourceManager>
    {

        private ResourceLoader m_resourceLoader;

        private static Dictionary<int, CResource> m_cachedResourceMap = new Dictionary<int, CResource>();
        private static bool m_clearUnusedAssets;
        private static int m_clearUnusedAssetsExecuteFrame;
        //private CResourcePackerInfoSet m_resourcePackerInfoSet;
        private static int s_frameCounter;

        public override void Init()
        {
            m_resourceLoader = new ResourceLoader();
        }


        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="finishDelegate"></param>
        public static void LoadScene(string sceneName, LoadSceneCallbacks loadCallbacks)
        {
            ResourceManager.LoadScene(sceneName, loadCallbacks, null);
        }

        public static void LoadScene(string sceneName, LoadSceneCallbacks loadCallbacks, object userData)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                UnityEngine.Debug.LogError("Load Scene Name is a null value");
                return;
            }
            if (loadCallbacks == null)
            {
                UnityEngine.Debug.LogError("Load Scene not contain callbacks");
                return;
            }

            ResourceManager.GetInstance().m_resourceLoader.LoadSceneAsync(sceneName, 0, loadCallbacks, userData);
        }

        /// <summary>
        /// 资源管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds )
        {

            ResourceManager.GetInstance().m_resourceLoader.OnUpdate(elapseSeconds, realElapseSeconds);

            //s_frameCounter++;
            //if (m_clearUnusedAssets && (m_clearUnusedAssetsExecuteFrame == s_frameCounter))
            //{
            //    ExecuteUnloadUnusedAssets();
            //    m_clearUnusedAssets = false;
            //    s_frameCounter = 0;
            //}
        }

        private static void ExecuteUnloadUnusedAssets()
        {            
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public static string GetAssetBundleInfoString()
        {
            //if (this.m_resourcePackerInfoSet == null)
            //{
            //    return string.Empty;
            //}
            //string str = string.Empty;
            //int num = 0;
            //for (int i = 0; i < this.m_resourcePackerInfoSet.m_resourcePackerInfosAll.Count; i++)
            //{
            //    if (this.m_resourcePackerInfoSet.m_resourcePackerInfosAll[i].IsAssetBundleLoaded())
            //    {
            //        num++;
            //        str = str + CFileManager.GetFullName(this.m_resourcePackerInfoSet.m_resourcePackerInfosAll[i].m_pathInIFS);
            //    }
            //}
            //return str;
            return null;
        }

        public static Dictionary<int, CResource> GetCachedResourceMap()
        {
            return m_cachedResourceMap;
        }

        public static CResource GetResource(string fullPathInResources, Type resourceContentType, enResourceType resourceType, bool needCached = false, bool unloadBelongedAssetBundleAfterLoaded = false)
        {
            if (string.IsNullOrEmpty(fullPathInResources))
            {
                return new CResource(0, string.Empty, null, resourceType, unloadBelongedAssetBundleAfterLoaded);
            }
            string s = CFileManager.EraseExtension(fullPathInResources);

            //int key = s.JavaHashCodeIgnoreCase();
            int key = s.GetHashCode();

            CResource resource = null;
            if (m_cachedResourceMap.TryGetValue(key, out resource))
            {
                if (resource.m_resourceType != resourceType)
                {
                    resource.m_resourceType = resourceType;
                }
                return resource;
            }
            resource = new CResource(key, fullPathInResources, resourceContentType, resourceType, unloadBelongedAssetBundleAfterLoaded);
            try
            {
                LoadResource(resource);
            }
            catch (Exception exception)
            {
                object[] inParameters = new object[] { s };
                //DebugHelper.Assert(false, "Failed Load Resource {0}", inParameters);
                throw exception;
            }
            if (needCached)
            {
                m_cachedResourceMap.Add(key, resource);
            }
            return resource;
        }



        public static Type GetResourceContentType(string extension)
        {
            Type type = null;
            if (string.Equals(extension, ".prefab", StringComparison.OrdinalIgnoreCase))
            {
                return typeof(GameObject);
            }
            if (string.Equals(extension, ".bytes", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".xml", StringComparison.OrdinalIgnoreCase))
            {
                return typeof(TextAsset);
            }
            if (string.Equals(extension, ".asset", StringComparison.OrdinalIgnoreCase))
            {
                type = typeof(ScriptableObject);
            }
            return type;
        }


        public IEnumerator LoadResidentAssetBundles()
        {
            yield return null;
        }

        private static void LoadResource(CResource resource)
        {
            resource.Load();
        }

       

        public static void RemoveAllCachedResources()
        {
            RemoveCachedResources((enResourceType[]) Enum.GetValues(typeof(enResourceType)));
        }

        public void RemoveCachedResource(string fullPathInResources)
        {
            //int key = CFileManager.EraseExtension(fullPathInResources).JavaHashCodeIgnoreCase();
            int key = CFileManager.EraseExtension(fullPathInResources).GetHashCode();

            CResource resource = null;
            if (m_cachedResourceMap.TryGetValue(key, out resource))
            {
                resource.Unload();
                m_cachedResourceMap.Remove(key);
            }
        }

        public static void RemoveCachedResources(enResourceType[] resourceTypes)
        {
            for (int i = 0; i < resourceTypes.Length; i++)
            {
                RemoveCachedResources(resourceTypes[i], false);
            }

            //UnloadAllAssetBundles();
            UnloadUnusedAssets();
        }

        public static void RemoveCachedResources(enResourceType resourceType, bool clearImmediately = true)
        {
            List<int> list = new List<int>();
            Dictionary<int, CResource>.Enumerator enumerator = m_cachedResourceMap.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, CResource> current = enumerator.Current;
                CResource resource = current.Value;
                if (resource.m_resourceType == resourceType)
                {
                    resource.Unload();
                    list.Add(resource.m_key);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                m_cachedResourceMap.Remove(list[i]);
            }
            if (clearImmediately)
            {
                //this.UnloadAllAssetBundles();
                UnloadUnusedAssets();
            }
        }

        //private void UnloadAllAssetBundles()
        //{
        //    if (this.m_resourcePackerInfoSet != null)
        //    {
        //        for (int i = 0; i < this.m_resourcePackerInfoSet.m_resourcePackerInfosAll.Count; i++)
        //        {
        //            CResourcePackerInfo info = this.m_resourcePackerInfoSet.m_resourcePackerInfosAll[i];
        //            if (info.IsAssetBundleLoaded())
        //            {
        //                info.UnloadAssetBundle(false);
        //            }
        //        }
        //    }
        //}

        //public void UnloadAssetBundlesByTag(string tag)
        //{
        //    if (this.m_resourcePackerInfoSet != null)
        //    {
        //        for (int i = 0; i < this.m_resourcePackerInfoSet.m_resourcePackerInfosAll.Count; i++)
        //        {
        //            if (this.m_resourcePackerInfoSet.m_resourcePackerInfosAll[i].m_tag.Equals(tag))
        //            {
        //                this.m_resourcePackerInfoSet.m_resourcePackerInfosAll[i].UnloadAssetBundle(false);
        //            }
        //        }
        //    }
        //}

        //public void UnloadBelongedAssetbundle(string fullPathInResources)
        //{
        //    CResourcePackerInfo resourceBelongedPackerInfo = this.GetResourceBelongedPackerInfo(fullPathInResources);
        //    if ((resourceBelongedPackerInfo != null) && resourceBelongedPackerInfo.IsAssetBundleLoaded())
        //    {
        //        resourceBelongedPackerInfo.UnloadAssetBundle(false);
        //    }
        //}

        public static void UnloadUnusedAssets()
        {
            m_clearUnusedAssets = true;
            m_clearUnusedAssetsExecuteFrame = s_frameCounter + 1;
        }
       
    }
}

