using System;
using Talk.Common;

namespace Talk.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RpcRouteAttribute : Attribute
    {
        public RpcRouteAttribute(string path)
        {
            Path = path;
        }
        public RpcRouteAttribute(string path, string notes)
        {
            Path = path;
            Notes = notes;
        }
        public RpcRouteAttribute(string path, string notes, RequestTypeEnum verbs)
        {
            Path = path;
            Notes = notes;
            Verbs = verbs;
        }
        public string Path { get; set; }
        public string Notes { get; set; }
        public RequestTypeEnum Verbs { get; set; } = RequestTypeEnum.Post;
    }
}
