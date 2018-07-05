
## UI 系统设计

### UI系统主要类功能

* UIManager UI控制管理类: 功能

>1. UI根节点的创建，UI分辨率的适配
>2. 负责UI界面的加载、显示、关闭、卸载功能
>3. 负责所有UI的显隐调度

* UIBase viewer层基类，单例，所有的UI系统都派生于此类

* UIForm UI窗口对象，挂载在每一个游戏界面根节点处。通过它实现UI系统与界面的交互操作

* UIComponent UI窗口控制脚本基类，比如界面的渐变等缓动效果都派生于此类

* UIDataDefination UI系统中的数据定义

***

### UIBase 
* 界面打开显示操作
```csharp
/// <summary>
/// 打开并显示界面
/// </summary>
public void Show(string assetName, params object[] param)
```