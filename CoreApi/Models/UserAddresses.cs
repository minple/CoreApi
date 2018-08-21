using CoreApi.DTO;
using System.Collections.Generic;

namespace CoreApi.Models {
    public class UserAddresses {
        public User User { get; set; }
        public List<Address> Addresses { get; set; }
    }
}