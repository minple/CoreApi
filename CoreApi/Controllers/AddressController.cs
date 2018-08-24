using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreApi.Models;
using CoreApi.DTO;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly CoreApiContext _context;
        public AddressesController(CoreApiContext context)
        {
            _context = context;
        }

        [HttpGet("page")]
        public ActionResult GetUsersAddresses([FromQuery(Name = "size")] int PageSize, [FromQuery(Name = "current")] int CurrentPage)
        {

            ActionResult Response = Unauthorized();

            var result = (from a in _context.Address
                          from u in _context.Users
                          where (
                              a.Id == u.Address1 ||
                              a.Id == u.Address2
                          )
                          from c in _context.Country
                          from ci in _context.City
                          from d in _context.District
                          from at in _context.AddressType
                          where a.CountryId == c.Id &&
                                  a.CityId == ci.Id &&
                                  a.DistrictId == d.Id &&
                                  a.AddressTypeId == at.Id
                          orderby u.Id
                          select new
                          {
                              UserId = u.Id,
                              UserName = u.Name,
                              Street = a.Street,
                              AddressType = at.Name,
                              District = d.Name,
                              City = ci.Name,
                              Country = c.Name,
                          });
            var resultPagination = result.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            
            Pagination Pagination = new Pagination();
            Error Error = new Error();
            try
            {
                
                if (PageSize > 20)
                    PageSize = 20;
                Pagination.PageSize = PageSize;
                Pagination.PageNumber = CurrentPage;
                Pagination.TotalItems = result.Count();

                if (result.Count() > 0)
                {
                    Error.Id = 100;
                    Error.Message = "Success!";
                    Response = Ok(new {
                        UserAddress = resultPagination,
                        Error = Error,
                        Pagination = Pagination
                    });
                }
                else
                {
                    Error.Id = 500;
                    Error.Message = "No Success! Please check your infomation that you sent!";
                    Pagination = null;
                }
            }
            catch (Exception e)
            {
                Error.Id = 1000;
                Error.Message = "The problems happen!";
                Error.Source = e.Message;
                Response = Ok(new
                {
                    Error = Error
                });
            }

            return Response;
        }



        [HttpGet("user/{id}")]
        public IQueryable GetUserAddresses(int id)
        {
            // IQueryable result = from a in _context.Address
            //                 join c in _context.Country on a.CountryId equals c.Id
            //                 join ci in _context.City on a.CityId equals ci.Id
            //                 join d in _context.District on a.DistrictId equals d.Id
            //                 join us in _context.Users on a.Id equals us.Address1 or (bao loi bao loi bao loi)

            //                 select new {
            //                     Username = us.Name,
            //                     District = d.Name,
            //                     City = ci.Name,
            //                     Country = c.Name
            //                 };
            IQueryable result = from a in _context.Address
                                from u in _context.Users
                                where (
                                    a.Id == u.Address1 ||
                                    a.Id == u.Address2
                                )
                                from c in _context.Country
                                from ci in _context.City
                                from d in _context.District
                                from at in _context.AddressType
                                where a.CountryId == c.Id &&
                                        a.CityId == ci.Id &&
                                        a.DistrictId == d.Id &&
                                        a.AddressTypeId == at.Id &&

                                        u.Id == id
                                select new
                                {
                                    id = a.Id,
                                    Street = a.Street,
                                    addressType = at.Name,
                                    district = d.Name,
                                    city = ci.Name,
                                    country = c.Name,

                                };
            IQueryable userAddresses;
            userAddresses = from u in _context.Users
                            where u.Id == id
                            select new
                            {
                                User = u,
                                Addresses = result
                            };

            return userAddresses;
        }

        [HttpGet("{id}")]
        public ActionResult GetAddressFromId(int id)
        {
            ActionResult Response = Unauthorized();
            Error Error = new Error();
            List<Address> AddressItem = new List<Address>();

            try
            {
                AddressItem = _context.Address.Where(ad => ad.Id == id).ToList();

                if (AddressItem.Any())
                {
                    Error.Id = 100;
                    Error.Message = "Success!";
                }
                else
                {
                    Error.Id = 500;
                    Error.Message = "No Success! Please check your infomation that you sent!";
                }
                Response = Ok(new
                {
                    Address = AddressItem.First(),
                    Error = Error
                });
            }
            catch (Exception e)
            {
                Error.Id = 1000;
                Error.Message = "Cannot add user! The problems happen!";
                Error.Source = e.Message;
                Response = Ok(new
                {
                    Error = Error
                });
            }
            return Response;

        }

        [HttpPost]
        public ActionResult PostUserAddresses([FromBody] UserAddresses _userAddresses)
        {
            ActionResult Response = Unauthorized();
            Error Error = new Error();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                List<int> AddressCurrentID = new List<int>();
                List<Address> Addresses = _userAddresses.Addresses;
                foreach (var info in Addresses)
                {
                    _context.Address.Add(info);
                    _context.SaveChanges();

                    AddressCurrentID.Add(info.Id);
                }

                User User = _userAddresses.User;
                if (AddressCurrentID.Count() == 1)
                {
                    _userAddresses.User.Address1 = AddressCurrentID[0];
                    _context.Users.Add(User);
                    _context.SaveChanges();
                }
                else if (AddressCurrentID.Count() == 2)
                {
                    _userAddresses.User.Address1 = AddressCurrentID[0];
                    _userAddresses.User.Address2 = AddressCurrentID[1];
                    _context.Users.Add(User);
                    _context.SaveChanges();
                }

                Error.Id = 100;
                Error.Message = "Success!";
                Response = Ok(new
                {
                    Error = Error
                });
            }
            catch (Exception e)
            {
                Error.Id = 1000;
                Error.Message = "Cannot add user! The problems happen!";
                Error.Source = e.Message;
                Response = Ok(new
                {
                    Error = Error
                });
            }
            return Response;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAddressFromId(int id, Address address)
        {
            Error Error = new Error();
            try
            {
                var result = _context.Address.Find(id);
                if (result != null)
                {
                    Error.Id = 100;
                    Error.Message = "Success!";
                }
                else
                {
                    Error.Id = 500;
                    Error.Message = "No Success! Please check your infomation that you sent!";
                }
                result.CountryId = address.CountryId;
                result.CityId = address.CityId;
                result.DistrictId = address.DistrictId;
                result.Street = address.Street;

                _context.Address.Update(result);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Error.Id = 1000;
                Error.Message = "The problems happen!";
                Error.Source = e.Message;
            }
            return Ok();
            //return Ok(Error);
        }
    }
}