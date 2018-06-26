/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/15 15:45:05
 *	版 本：v 1.0
 *	描 述：气象现象接口
* ========================================================*/

using System;

namespace Sunshine.Weather
{
    public interface IPhenomena
    {
        void OnUpdate(float dt);
        void OnApplicationQuit();
    }
}

