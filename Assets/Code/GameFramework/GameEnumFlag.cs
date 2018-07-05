/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/29 16:24:58
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System;

// 资源类型
public enum enResourceType
{
    BattleScene,
    Numeric,
    Sound,
    UIForm,
    UIPrefab,
    UI3DImage,
    UISprite
}

// 资源当前的在内存中的状态
public enum enResourceState
{
    Unload,
    Loading,
    Loaded
}