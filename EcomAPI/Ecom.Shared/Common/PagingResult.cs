namespace Ecom.Shared.Common
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        /// <summary>
        /// Constructor
        /// </summary>
        public PagingResult() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PagingResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalItems = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
