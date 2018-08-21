
//used for List<>
using System.Collections.Generic;
using CoreApi.DTO;

namespace CoreApi.Models
{
    public class UserAddrsData {
        public List<UserAddress> UserAddress { get; set; }
        public Error Error { get; set; }
        public Pagination Pagination { get; set; }

        public UserAddrsData() {
            this.UserAddress = new List<UserAddress>();
            this.Error = new Error();
            this.Pagination = new Pagination();
        }
    }
}