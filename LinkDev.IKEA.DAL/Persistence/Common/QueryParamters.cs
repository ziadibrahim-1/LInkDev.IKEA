namespace LinkDev.IKEA.DAL.Persistence.Common
{
    public class QueryParamters
    {
        private const int MaxPageSize = 20;
        public int PageIndex { get; set; } = 1;

        private string? searchTerm;

        public string? SearchTerm
        {
            get => searchTerm;
            set { searchTerm = value?.ToLower(); }
        }


        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? SortedBy { get; set; }
        public bool SortAscynding { get; set; }
    }
}
