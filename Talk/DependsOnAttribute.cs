using System;

namespace Talk
{
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// 类型的依赖模块
        /// </summary>
        public Type[] DependedModuleTypes { get; private set; }

        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }
    }
}
