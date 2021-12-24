using CleanArch.Core.Interfaces;

namespace CleanArch.Domain.Parameters
{
    public class QueryStringParameters : IQueryStringParameters
    {
        public string Search { get; set; } = string.Empty;
        public int? PageIndex { get; set; } = null;
        public int? PageSize { get; set; } = null;
        public string OrderBy { get; set; } = string.Empty;
    }
}