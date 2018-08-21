using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoreApi.Models;
using CoreApi.DTO;

//used for sql vd where
using System.Linq;


namespace CoreApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController: ControllerBase {
        private IConfiguration _config;
        private CoreApiContext _context;

        public TokenController(IConfiguration config, CoreApiContext context) {
            _config = config;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]User user) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            IActionResult Response = Unauthorized();
            User _user = null;
            Error Error = new Error();
            try {
                var result = _context.Users.Where(
                            us => 
                                us.Name == user.Name && 
                                us.Password == user.Password);
                _user = result.First();
                Error.Id = 100;
                Error.Message = "Success!";
            }
            catch(Exception e) {
                Error.Id = 1000;
                Error.Message = "The problems happen!";
                Error.Source = e.Message;
            }
            
            if( _user != null ) {
                var tokenString = BuildToken(_user);
                Response = Ok(new {
                        token = tokenString,
                        Error = Error
                    });
            }
            else {
                Error.Id = 500;
                Error.Message = "User not exist!";
                Response = Ok(new {
                    token = "",
                    Error = Error
                });
            }
            return Response;
        }

        private string BuildToken(User user)
        {
            var userRole = from r in _context.Roles
                            join a in _context.Access on r.Id equals a.RoleId where a.UserId == user.Id
                            select new {
                                name = r.Name
                            };
            
            Int32 permissionCount = 0;
            foreach (var count in userRole)
            {
                permissionCount++;
            }
            string nameRole;
            if(permissionCount == 3)
                nameRole = "admin";
            else
                nameRole = "normal";
            
            var claims = new[] {
                new Claim(ClaimTypes.Role, nameRole),
                new Claim("permissionCount", permissionCount.ToString()),
                new Claim("userId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(1000),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}