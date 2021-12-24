using System;

namespace CleanArch.Core.Wrappers
{
    public class PagedApiResponse<T> : ApiResponse<T>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages => TotalRecords.HasValue ? (int)Math.Ceiling(TotalRecords.Value / (double)PageSize) : 0;

        public PagedApiResponse(T data, int totalRecords, int? pageIndex, int? pageSize)
        {
            TotalRecords = totalRecords;
            PageIndex = pageIndex == null ? 0 : pageIndex;
            PageSize = pageSize == null ? 1 : pageSize;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }
    }
}