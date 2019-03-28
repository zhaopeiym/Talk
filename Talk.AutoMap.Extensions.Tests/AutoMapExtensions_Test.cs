using AutoMapper;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Talk.AutoMap.Extensions.Tests
{
    public class AutoMapExtensions_Test
    {
        Entity obj1 = null;
        public AutoMapExtensions_Test()
        {
            var entityOrder1 = new EntityOrder()
            {
                Id = 1,
                Code = "0001",
            };
            var entityOrder2 = new EntityOrder()
            {
                Id = 2,
                Code = "0002",
            };

            obj1 = new Entity()
            {
                Id = 1,
                Name = "张三",
                EntityOrders = new List<EntityOrder>() {
                            entityOrder1,
                            entityOrder1,
                            entityOrder1,
                            entityOrder2 ,
                }
            };
        }

        [Fact]
        public void 特性自动映射测试()
        {
            AutoMapperModule.Initialize(new Type[] { typeof(EntityBaseDto) });

            var obj2 = obj1.MapTo<EntityBaseDto>();
            Assert.Equal(obj2.Name, "张三");

            var obj3 = obj2.MapTo<Entity>();
            obj3.Name.ShouldBe("张三");
        }

        [Fact]
        public void Profile配置映射_覆盖_Attribute映射()
        {
            AutoMapperModule.Initialize(new Type[] { typeof(EntityBaseDto), typeof(EntityProfile) });

            var obj1 = new Entity() { Id = 1, Name = "张三" };
            var obj2 = obj1.MapTo<EntityBaseDto>();
            Assert.Equal(obj2.Name, "张三");

            var obj3 = obj2.MapTo<Entity>();
            obj3.Name.ShouldBe("张三Test");
        }

        [Fact]
        public void 配置映射_条件过滤()
        {
            AutoMapperModule.Initialize(new Type[] { typeof(EntityBaseDto), typeof(EntityOrderProfile) });
            var obj2 = obj1.MapTo<EntityBaseDto>();
            Assert.Equal(obj2.EntityOrders.Count(), 3);
        }
    }

    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EntityOrder> EntityOrders { get; set; }
    }

    public class EntityOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }

    [AutoMap(typeof(Entity))]
    public class EntityBaseDto
    {
        public string Name { get; set; }
        public string Name2 { get; set; }
        public List<EntityOrder> EntityOrders { get; set; }
    }

    [AutoMapProfile]
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<EntityBaseDto, Entity>().ForMember(t => t.Name, f => f.MapFrom(w => w.Name + "Test")); ;
            CreateMap<Entity, EntityBaseDto>();
        }
    }

    [AutoMapProfile]
    public class EntityOrderProfile : Profile
    {
        public EntityOrderProfile()
        {
            CreateMap<Entity, EntityBaseDto>().ForMember(t => t.EntityOrders, f => f.MapFrom(w => w.EntityOrders.Where(e => e.Id == 1)));
        }
    }

}
