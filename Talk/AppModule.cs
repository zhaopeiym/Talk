using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Talk
{
    public class AppModule
    {
        public Assembly ModuleAssembly { get; set; }

        /// <summary>
        /// 初始化前
        /// </summary>
        public virtual void PreInitialize()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// 初始化后
        /// </summary>
        public virtual void PostInitialize()
        {

        }
    }
}
