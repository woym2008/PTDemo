/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/16 10:10:53
 *	版 本：v 1.0
 *	描 述：单例模板
* ========================================================*/
using System;

public abstract class XSingleton<T> where T : class, new()
{

    //public static T instance
    //{
    //    get { return XSingletonCreator.instance; }
    //}

    //class XSingletonCreator
    //{
    //    internal static readonly T instance = new T();
    //}

    private static T s_instance;

    protected XSingleton()
    { }

    public static void CreateInstance()
    {
        if (XSingleton<T>.s_instance == null)
        {
            XSingleton<T>.s_instance = Activator.CreateInstance<T>();

            (XSingleton<T>.s_instance as XSingleton<T>).Init();
        }
    }

    public static void DestroyInstance()
    {
        if (XSingleton<T>.s_instance != null)
        {
            (XSingleton<T>.s_instance as XSingleton<T>).UnInit();
            XSingleton<T>.s_instance = null;
        }
    }

    public static T GetInstance()
    {
        if (XSingleton<T>.s_instance == null)
        {
            XSingleton<T>.CreateInstance();
        }
        return XSingleton<T>.s_instance;
    }

    public static bool HasInstance()
    {
        return (XSingleton<T>.s_instance != null);
    }

    public virtual void Init()
    { }
    public virtual void UnInit()
    { }
    public static T instance
    {
        get
        {
            if (XSingleton<T>.s_instance == null)
            {
                XSingleton<T>.CreateInstance();
            }
            return XSingleton<T>.s_instance;
        }
    }
}
