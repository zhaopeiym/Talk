namespace Talk.Application.Dto
{
    public abstract class PagedRequestDto : IPagedRequest
    {
        //自然数包括 零和正整数
        //[RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "SkipCount必须为自然数")]
        public virtual int SkipCount { get; set; }

        //[RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "SkipCount必须为正整数")]
        public virtual int MaxResultCount { get; set; }
    }
}
