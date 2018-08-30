
namespace CoreApi.Models {
    public class Pagination {
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public Pagination() {
            this.PageSize = 3;
            this.CurrentPage = 1;
        }
    }
}