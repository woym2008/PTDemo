/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/24 19:30:40
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Demo.FrameWork
{
    public class ClassEnumerator
    {

        private System.Type AttributeType;
        private System.Type InterfaceType;
        protected List<Type> Results = new List<Type>();

        public ClassEnumerator(
            Type InAttributeType, 
            Type InInterfaceType, 
            Assembly InAssembly, 
            bool bIgnoreAbstract = true, 
            bool bInheritAttribute = false, 
            bool bShouldCrossAssembly = false)
        {
            this.AttributeType = InAttributeType;
            this.InterfaceType = InInterfaceType;
            try
            {
                if (bShouldCrossAssembly)
                {
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    if (assemblies != null)
                    {
                        for (int i = 0; i < assemblies.Length; i++)
                        {
                            Assembly inAssembly = assemblies[i];
                            this.CheckInAssembly(inAssembly, bIgnoreAbstract, bInheritAttribute);
                        }
                    }
                }
                else
                {
                    this.CheckInAssembly(InAssembly, bIgnoreAbstract, bInheritAttribute);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in enumerate classes :" + ex.Message);
            }
        }

        protected void CheckInAssembly(Assembly InAssembly, bool bInIgnoreAbstract, bool bInInheritAttribute)
        {
            Type[] types = InAssembly.GetTypes();
            if (types != null)
            {
                for (int i = 0; i < types.Length; i++)
                {
                    Type c = types[i];

                    if ((this.InterfaceType == null || this.InterfaceType.IsAssignableFrom(c))
                        && (!bInIgnoreAbstract || (bInIgnoreAbstract && !c.IsAbstract )) 
                        && c.GetCustomAttributes(this.AttributeType, bInInheritAttribute).Length > 0)
                    {
                        this.Results.Add(c);
                    }
                }
            }            

        }

        public List<Type> results
        {
            get { return this.Results; }
        }
    }
}

