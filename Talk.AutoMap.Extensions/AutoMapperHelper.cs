using AutoMapper;
using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Talk.Extensions;

namespace Talk.AutoMap.Extensions
{
    internal static class AutoMapperHelper
    {
        internal static void CreateMap(IEnumerable<Type> types, Type[] AttributeTypes)
        {
            Mapper.Initialize(c =>
            {
                foreach (var type in types)
                {
                    if (type.GetCustomAttributes<AutoMapProfileAttribute>().Any())
                    {
                        c.AddProfile(type);
                        continue;
                    }
                    foreach (Type TAttribute in AttributeTypes)
                    {

                        foreach (AutoMapAttribute autoMapToAttribute in type.GetCustomAttributes(TAttribute))
                        {
                            if (autoMapToAttribute.TargetTypes.IsNullOrEmpty())
                            {
                                continue;
                            }

                            foreach (var targetType in autoMapToAttribute.TargetTypes)
                            {
                                if (autoMapToAttribute.Direction.HasFlag(AutoMapDirection.To))
                                {
                                    c.CreateMap(type, targetType);
                                }

                                if (autoMapToAttribute.Direction.HasFlag(AutoMapDirection.From))
                                {
                                    c.CreateMap(targetType, type);
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
