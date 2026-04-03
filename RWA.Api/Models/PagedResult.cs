namespace RWA.Api.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }

        public int ItemFrom { get; set; }
        public int ItemTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemTo = ItemFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        }
    }
}
