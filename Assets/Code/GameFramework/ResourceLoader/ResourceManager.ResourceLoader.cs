/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/02 17:32:26
 *	版 本：v 1.0
 *	描 述：资源加载类
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo.FrameWork
{
    internal partial class ResourceManager
    {
        private partial class ResourceLoader
        {
            private AsyncOperation m_AsyncOperation = null;
            private LoadSceneCallbacks m_loadSceneCallbacks = null;
            private object m_userData;
            private float m_duration = 0f;
            private string m_loadingSceneName;

            //private static int load

            public ResourceLoader()
            {
                m_AsyncOperation = null;

                m_duration = 0f;
            }

            /// <summary>
            /// 资源管理器轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                
            }

            /// <summary>
            /// 使用同步方式加载场景
            /// </summary>
            /// <param name="sceneName">场景名称</param>
            /// <param name="priority">加载优先级,数值越低，加载优先级越高</param>
            /// <param name="loadSceneCallbacks">加载回调函数类</param>
            /// <param name="userData">用户数据</param>
            public void LoadScene(string sceneName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                if (loadSceneCallbacks.LoadSceneSuccessCallback != null)
                {
                    loadSceneCallbacks.LoadSceneSuccessCallback(sceneName, 0, userData);
                }
            }

            public void LoadSceneAsync(string sceneName, int priority, LoadSceneCallbacks loadSceneCallbacks, object userData)
            {
                m_loadSceneCallbacks = loadSceneCallbacks;
                m_userData = userData;
                m_duration = 0f;

                GameEntry.GetInstance().StartCoroutine(BeginLoadSceneAsync(sceneName,priority));
            }

            private IEnumerator BeginLoadSceneAsync(string sceneName, int priority)
            {
                yield return new WaitForSeconds(0.01f);
                m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
                m_loadingSceneName = sceneName;

                while (!m_AsyncOperation.isDone)
                {
                    m_duration += Time.deltaTime;

                    if (m_loadSceneCallbacks.LoadSceneUpdateCallback != null)
                    {
                        m_loadSceneCallbacks.LoadSceneUpdateCallback(m_loadingSceneName, m_AsyncOperation.progress, m_userData);
                    }
                    Debug.LogWarning(m_AsyncOperation.progress);
                    yield return null;
                }

                m_duration += Time.deltaTime;
                if (m_loadSceneCallbacks.LoadSceneSuccessCallback != null)
                {
                    m_loadSceneCallbacks.LoadSceneSuccessCallback(m_loadingSceneName, m_duration, m_userData);
                }
                m_AsyncOperation = null;
                m_userData = null;
                m_duration = 0;

                yield break;
            }

        }
    }
}

