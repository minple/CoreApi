
//used for List<>
using System.Collections.Generic;
using CoreApi.DTO;

namespace CoreApi.Models
{
    public class UsersData {
        public List<User> Users { get; set; }
        public Error Error { get; set; }
        public Pagination Pagination { get; set; }

        public UsersData() {
            this.Users = new List<User>();
            this.Error = new Error();
            this.Pagination = new Pagination();
        }
    }
}