using System.ComponentModel.DataAnnotations;

namespace CoreApi.DTO {
    public class Address
        {
            public int Id { get; set; }
            public int AddressTypeId { get; set; }
            [Required]
            public int CountryId { get; set; }
            [Required]
            public int CityId { get; set; }
            [Required]
            public int DistrictId { get; set; }
            public string Street { get; set; }
        }
}