namespace Talk
{
    public interface IReturn
    {

    }

    /// <summary>
    /// 契约响应接口, 用以标识Request对应的Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReturn<T> : IReturn
    {
    }
}
