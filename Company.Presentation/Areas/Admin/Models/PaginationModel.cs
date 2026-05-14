namespace Company.Presentation.Areas.Admin.Models
{
    public class PaginationModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
