using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CoreApi.Models;
using CoreApi.DTO;
//used for Exception in try catch
using System;
//
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CoreApiContext _context;
        
        public UsersController(CoreApiContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetUsers")]
        public UsersData GetAll()
        {
            UsersData UsersData = new UsersData();
            try {
                var permissionCount = Int32.Parse(HttpContext.User.FindFirst("permissionCount").Value);
                var userId = Int32.Parse(HttpContext.User.FindFirst("userId").Value);

                if( permissionCount == 3  ){
                    UsersData.Users = _context.Users.OrderBy(x=>x.Id).Take(UsersData.Pagination.PageSize).ToList();
                }
                else {
                    User user = _context.Users.Find(userId);
                    UsersData.Users.Add(user);
                }
                UsersData.Error.Id = 100;
                UsersData.Error.Message = "Success!";
            }
            catch(Exception e) {
                UsersData.Error.Id = 1000;
                UsersData.Error.Message = "Some problems happen!";
                UsersData.Error.Source = e.Message;
            }
            
            return UsersData;
        }

        [HttpPost]
        public IActionResult AddUser([FromBody]User user) {
            IActionResult Response = Unauthorized();
            Error Error = new Error();

            try {
                var permissionCount = Int32.Parse(HttpContext.User.FindFirst("permissionCount").Value);
                if( permissionCount == 3 && user != null ) {
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    Error.Id = 100;
                    Error.Message = "Success!";
                    Response = Ok( new {
                        Error = Error
                    });
                }
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

        [HttpGet("page")]
        public ActionResult<UsersData> GetUserPage([FromQuery(Name = "size")] int PageSize, [FromQuery(Name = "current")] int PageNumber) {

            ActionResult Response = Unauthorized();
            UsersData UsersData = new UsersData();
            try {
                var permissionCount = Int32.Parse(HttpContext.User.FindFirst("permissionCount").Value);

                if(permissionCount == 3) {
                    if(PageSize > 20)
                        PageSize = 20;
                    UsersData.Pagination.PageSize = PageSize;
                    UsersData.Pagination.PageNumber = PageNumber;
                    UsersData.Pagination.TotalItems = _context.Users.Count();

                    var result =  (from X in _context.Users orderby X.Id
                    select X).Skip((PageNumber-1)*PageSize).Take(PageSize);

                    UsersData.Users = result.ToList();
                    Response = Ok(UsersData);

                    if(UsersData.Users.Any()) {
                        UsersData.Error.Id = 100;
                        UsersData.Error.Message = "Success!";
                    }
                    else {
                        UsersData.Error.Id = 500;
                        UsersData.Error.Message = "No Success! Please check your infomation that you sent!";
                        UsersData.Pagination = null;
                    }
                }
            }
            catch(Exception e) {
                UsersData.Error.Id = 1000;
                UsersData.Error.Message = "The problems happen!";
                UsersData.Error.Source = e.Message;
                Response = Ok(new {
                    Error = UsersData.Error
                });
            }
            return Response;
        }
        
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public IActionResult GetUserFromId(int id) {
            UsersData UsersData = new UsersData();

            try {
                var result = _context.Users.Where(
                                    us => 
                                        us.Id == id);
                foreach (var us in result) 
                { 
                    UsersData.Users.Add(us);
                }
                UsersData.Pagination = null;
                if(UsersData.Users.Any()) {
                    UsersData.Error.Id = 100;
                    UsersData.Error.Message = "Success!";
                }
                else {
                    UsersData.Error.Id = 500;
                    UsersData.Error.Message = "No Success! Please check your infomation that you sent!";
                }
            }
            catch(Exception e) {
                UsersData.Error.Id = 1000;
                UsersData.Error.Message = "The problems happen!";
                UsersData.Error.Source = e.Message;
            }
            return CreatedAtRoute("GetUsers", UsersData);
        } 
        
        [HttpGet("logout")]
        public IActionResult GetById(string token)
        {
           string headers = HttpContext.Request.Headers["Authorization"];
           TokenBlackList Token = new TokenBlackList();
           Token.Token = headers;
           _context.TokenBlackList.Add(Token);
           _context.SaveChanges();
           return Ok();
        }      

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UsersData Usersdata = new UsersData();
            try {
                var result = _context.Users.Where(
                                    us =>
                                        us.Id == id);
                foreach (var us in result)
                {
                    Usersdata.Users.Add(us);
                }

                _context.Users.Remove(Usersdata.Users.First());
                _context.SaveChanges();

                if(Usersdata.Users.Any()) {
                    Usersdata.Error.Id = 100;
                    Usersdata.Error.Message = "Success!";
                }
                else {
                    Usersdata.Error.Id = 500;
                    Usersdata.Error.Message = "No Success! Please check your infomation that you sent!";
                }
                Usersdata.Pagination = null;
                Usersdata.Users = null;
            }
            catch(Exception e) {
                Usersdata.Error.Id = 1000;
                Usersdata.Error.Message = "The problems happen!";
                Usersdata.Error.Source = e.Message;
            }
            return Ok(Usersdata);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult  Update(int id, User user)
        {
            Error Error = new Error();
            try {
                var result = _context.Users.Find(id);
                if (result != null)
                {
                    Error.Id = 100;
                    Error.Message = "Success!";
                }
                else {
                    Error.Id = 500;
                    Error.Message = "No Success! Please check your infomation that you sent!";
                }
                
                result.Name = user.Name;
                result.Password = user.Password;
                result.Address1 = user.Address1;
                result.Address2 = user.Address2;

                _context.Users.Update(result);
                _context.SaveChanges();
                }
            catch(Exception e) {
                Error.Id = 1000;
                Error.Message = "The problems happen!";
                Error.Source = e.Message;
            }
            return Ok(Error);
        }  
    }
}