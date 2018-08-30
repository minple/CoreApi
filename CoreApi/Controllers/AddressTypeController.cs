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
    public class AddressTypeController:ControllerBase {
        private readonly CoreApiContext _context;
        public AddressTypeController(CoreApiContext contex) {
            _context = contex;
        }

        [HttpGet]
        public ActionResult GetAddressType() {
            ActionResult Response = BadRequest();
            Error Error = new Error();
            List<AddressType> AddressTypes = new List<AddressType>();

            try {
                AddressTypes = _context.AddressType.ToList();

                Error.Id = 100;
                Error.Message = "Success!";

                Response = Ok( new {
                    AddressTypes = AddressTypes,
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