/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/25 10:13:17
 *	版 本：v 1.0
 *	描 述：用于注册游戏系统注册，通过 Attribute实例化和获取对应实例
* ========================================================*/

using System;

namespace Demo.FrameWork
{
    // 属性类的父类
    public class AutoRegisterAttribute : Attribute
    {
        // TODO additional  self      
    }

    // 游戏状态类型属性
    public class GameStateAttribute:AutoRegisterAttribute
    {

    }
}


