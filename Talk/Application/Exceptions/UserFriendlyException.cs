using System;
using Talk.Enums;

namespace Talk.Application.Exceptions
{
    /// <summary>
    /// 自定义友好异常
    /// </summary>
    public class UserFriendlyException : Exception
    {
        /// <summary>
        /// 异常的附加信息
        /// </summary>
        public string Details { get; private set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public ExceptionCodeEnum Code { get; set; } = ExceptionCodeEnum.C500;
         
        public UserFriendlyException()
        {
        }

        public UserFriendlyException(string message)
          : base(message)
        {

        }

        public UserFriendlyException(ExceptionCodeEnum code, string message)
          : this(message)
        {
            Code = code; 
        }

        public UserFriendlyException(string message, string details)
           : this(message)
        {
            Details = details; 
        }

        public UserFriendlyException(ExceptionCodeEnum code, string message, string details)
          : this(message, details)
        {
            Code = code; 
        }

        public UserFriendlyException(string message, Exception innerException)
           : base(message, innerException)
        {

        }

        public UserFriendlyException(string message, string details, Exception innerException)
            : this(message, innerException)
        {
            Details = details; 
        }
    }
}
