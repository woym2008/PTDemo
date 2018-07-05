/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/02 16:58:32
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System;

namespace Demo.FrameWork
{

#region 事件接口
    /// <summary>
    /// 加载场景成功回调函数。
    /// </summary>
    /// <param name="sceneName">要加载的场景资源名称。</param>
    /// <param name="duration">加载持续时间。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadSceneSuccessCallback(string sceneName, float duration, object userData);

    /// <summary>
    /// 加载场景更新回调函数。
    /// </summary>
    /// <param name="sceneName">要加载的场景资源名称。</param>
    /// <param name="progress">加载场景进度。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadSceneUpdateCallback(string sceneName, float progress, object userData);

    /// <summary>
    /// 加载场景时加载依赖资源回调函数。
    /// </summary>
    /// <param name="sceneAssetName">要加载的场景资源名称。</param>
    /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
    /// <param name="loadedCount">当前已加载依赖资源数量。</param>
    /// <param name="totalCount">总共加载依赖资源数量。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadSceneDependencyAssetCallback(string sceneAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData);

#endregion

    

    public sealed class LoadSceneCallbacks 
    {
        private readonly LoadSceneSuccessCallback m_loadSceneSuccessCallback;
        private readonly LoadSceneUpdateCallback m_loadSceneUpdateCallback;
        private readonly LoadSceneDependencyAssetCallback m_loadSceneDependencyCallback;


        public LoadSceneCallbacks(LoadSceneSuccessCallback successCallback,LoadSceneUpdateCallback loadUpdateCallback,
            LoadSceneDependencyAssetCallback loadDependencyCallback)
        {
            if(successCallback == null)
            {
                throw new Exception("Load Scene Success Call is a null value");
            }

            m_loadSceneSuccessCallback = successCallback;
            m_loadSceneUpdateCallback = loadUpdateCallback;
            m_loadSceneDependencyCallback = loadDependencyCallback;
        }

        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSuccessCallback)
            :this(loadSuccessCallback, null, null)
        {  }
        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSuccessCallback,LoadSceneUpdateCallback loasUpdateCallback)
            :this(loadSuccessCallback,loasUpdateCallback,null)
        { }

        public LoadSceneCallbacks(LoadSceneSuccessCallback loadSuccessCallback,LoadSceneDependencyAssetCallback loadDependencyCallback)
            :this(loadSuccessCallback, null, loadDependencyCallback)
        { }


        public LoadSceneSuccessCallback LoadSceneSuccessCallback
        {
            get {
                return m_loadSceneSuccessCallback;
            }
        }

        public LoadSceneUpdateCallback LoadSceneUpdateCallback
        {
            get
            {
                return m_loadSceneUpdateCallback;
            }
        }

        public LoadSceneDependencyAssetCallback LoadSceneDependencyCallback
        {
            get
            {
                return m_loadSceneDependencyCallback;
            }
        }
    }
}

