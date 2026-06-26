namespace LinkDev.IKEA.PL.Models.Employee
{
    public class EmployeeListViewModel
    {
        public required IEnumerable<EmployeeViewModel> Employees { get; set; }

        // Pagination properties
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public string? SearchTerm { get; set; }
        public string? SortedBy { get; set; }
        public bool SortAscynding { get; set; }
        public bool ApplyOrder { get; set; }


    }
}
