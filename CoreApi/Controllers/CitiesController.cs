using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreApi.Models;
using CoreApi.DTO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CoreApi.Controllers {
    [Authorize(Roles="admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController:ControllerBase {
        private readonly CoreApiContext _context;
        public CitiesController(CoreApiContext contex) {
            _context = contex;
        }

        [HttpGet("country")]
        public ActionResult GetCities([FromQuery(Name = "id")] int CountryId) {
            ActionResult Response = Unauthorized();
            Error Error = new Error();
            List<City> Cities = new List<City>();

            try {
                Cities = _context.City.Where( ct => ct.CountryId == CountryId ).ToList();

                if(Cities.Any()) {
                    Error.Id = 100;
                    Error.Message = "Success!";
                }
                else {
                    Error.Id = 500;
                    Error.Message = "No Success! Please check your infomation that you sent!";
                }
                Response = Ok( new {
                    Cities = Cities,
                    Error = Error
                });
                Response = Ok( new {
                    Cities = Cities,
                    Error = Error
                });
            }
            catch(Exception e) {
                Error.Id = 1000;
                Error.Message = "Cannot add user! The problems happen!";
                Error.Source = e.Message;
                Response = Ok(new {
                    Error = Error
                });
            }
            return Response;

        }
    }
}