using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Talk.AutoMap.Extensions
{
    public static class AutoMapperModule
    {
        private static Type[] AttributeTypes
        {
            get
            {
                return new Type[] {
                    typeof(AutoMapAttribute),
                    typeof(AutoMapFromAttribute),
                    typeof(AutoMapToAttribute)
                    };
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_type"></param>
        public static void Initialize(IEnumerable<Type> _type)
        {
            var types = _type.Where(type => AttributeTypes.Any(t => type.IsDefined(t)) || type.IsDefined(typeof(AutoMapProfileAttribute)));
            AutoMapperHelper.CreateMap(types, AttributeTypes);
        }
    }
}
