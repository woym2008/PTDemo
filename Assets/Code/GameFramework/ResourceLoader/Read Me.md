
### ResourceManager 资源管理器
> *管理资源的加载和卸载*

* 场景加载
```csharp
/// <summary>
/// 场景异步加载
/// </summary>
/// <param name="sceneName">场景名称，字符串类型</param>
/// <param name="loadCallbacks">加载场景的回调类</param>
public static void LoadScene(string sceneName, LoadSceneCallbacks loadCallbacks)
```


***
***
### LoadSceneCallbacks  场景加载回调类
> *加载场景时的回调函数集合*

