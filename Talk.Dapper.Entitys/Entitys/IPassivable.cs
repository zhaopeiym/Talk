namespace Talk.Dapper.Entitys.Entitys
{
    /// <summary>
    /// 是否有效
    /// </summary>
    public interface IPassivable
    {
        /// <summary>        
        /// true：启用
        /// false：禁用
        /// </summary>
        bool IsActive { get; set; }
    }
}
