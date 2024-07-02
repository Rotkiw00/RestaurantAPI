namespace RestaurantAPI.Models
{
    public class RestaurantGetAllQuery
    {
        public string SearchPhrase { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
        public SortOrderResult SortOrderResult { get; set; }
    }
}
