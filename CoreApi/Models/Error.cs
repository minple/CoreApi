namespace CoreApi.Models {
    public class Error {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }

        public Error() {
            this.Id = 0;
            this.Message = "None";
            this.Source = "None";
        }
    }
}