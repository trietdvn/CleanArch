namespace CleanArch.Core.Interfaces
{
    public interface IQueryStringParameters
    {
        string Search { get; set; }
        int? PageIndex { get; set; }
        int? PageSize { get; set; }
        string OrderBy { get; set; }
    }
}