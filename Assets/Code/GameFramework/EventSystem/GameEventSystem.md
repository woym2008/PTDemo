<!-- 
    游戏事件通知系统
 -->
### GameEventSystem  : 游戏事件通知系统 
> 通过事件消息传递形式减少模块间的耦合度


* 消息（事件）的注册与发送

> 注册你的消息事件

```csharp
/// <typeparam name="T"> 键值类型，此处统一使用Int32 </typeparam>
/// <param name="key">键值，每一个数值对应唯一的事件 从EventIDs中获取</param>
/// <param name="func">注册函数</param>
public static bool RegisterEvent<T>(T key, OnEvent func) where T:IConvertible
``` 

> 取消你的监听事件

```csharp
/// <typeparam name="T"> 键值类型，此处统一使用Int32</typeparam>
/// <param name="key"></param>
/// <param name="func"></param>
public static void UnRegisterEvent<T>(T key, OnEvent func) where T:IConvertible
```

> 广播你的事件
```csharp
/// <typeparam name="T"> 键值类型，此处统一使用Int32</typeparam>
/// <param name="key">事件ID</param>
/// <param name="param">广播事件的参数</param>
public static bool SendEvnet<T>(T key, params object[] param) where T:IConvertible
```

> Excample
```csharp
class TestSendEvent{
    public void DoEvent()
    {
        GameEventSystem.SendEvnet(EventIDs.ShowLoginPanel,"Hello world")
    }
}

class TestGetEvent
{
    public void Init()
    {
        //初始化时注册消息事件监听
        GameEventSystem.RegisterEvent(EventIDs.ShowLoginPanel, GetEvent);
    }

    void GetEvent(int key, params object[] param)
    {
        var vale = param[0];
        Debug.Log("收到消息" + value);
    }

    void OnDestroy()
    {
        // 取消事件监听
        GameEventSystem.UnRegisterEvent(EventIDs.ShowLoginPanel, GetEvent);
    }
}
```