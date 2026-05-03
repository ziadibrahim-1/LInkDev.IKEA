namespace LinkDev.IKEA.DAL.Persistence.Common
{
    public class PaginatedResult<T>
    {
        public required IEnumerable<T> date { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
