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
    public class DistrictsController:ControllerBase {
        private readonly CoreApiContext _context;
        public DistrictsController(CoreApiContext contex) {
            _context = contex;
        }

        [HttpGet("city/{id}")]
        public ActionResult GetDistricts(int id) {
            ActionResult Response = Unauthorized();
            Error Error = new Error();
            List<District> Districts = new List<District>();

            try {
                Districts = _context.District.Where( dt => dt.CityId == id ).ToList();

                if(Districts.Any()) {
                    Error.Id = 100;
                    Error.Message = "Success!";
                }
                else {
                    Error.Id = 500;
                    Error.Message = "No Success! Please check your infomation that you sent!";
                }
                Response = Ok( new {
                    Districts = Districts,
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