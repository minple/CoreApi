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
    public class CountriesController:ControllerBase {
        private readonly CoreApiContext _context;
        public CountriesController(CoreApiContext contex) {
            _context = contex;
        }

        [HttpGet]
        public ActionResult GetCountries() {
            ActionResult Response = Unauthorized();
            Error Error = new Error();
            List<Country> Countries = new List<Country>();

            try {
                Countries = _context.Country.ToList();

                if(Countries.Any()) {
                    Error.Id = 100;
                    Error.Message = "Success!";
                }
                else {
                    Error.Id = 500;
                    Error.Message = "No Success! Please check your infomation that you sent!";
                }
                Response = Ok( new {
                    Countries = Countries,
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