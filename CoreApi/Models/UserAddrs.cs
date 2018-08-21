using CoreApi.DTO;
using System.Collections.Generic;

namespace CoreApi.Models {
    public class UserAddress {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Street { get; set; }
        public string AddressType { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}