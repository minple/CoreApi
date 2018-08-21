using System;
using System.ComponentModel.DataAnnotations;

namespace CoreApi.DTO {
    public class User
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }
            public string Password { get; set; }
            public int? Address1 { get; set; }
            public int? Address2 { get; set; }
        }
}