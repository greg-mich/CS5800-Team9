using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using backend.Models;
using backend.Data.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using backend.Infrastructure.PasswordSecurity;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private ILogger<TokenController> _logger;
        private readonly ApplicationDbContext _context;

        public TokenController(
            IConfiguration config,
            ILogger<TokenController> logger,
            ApplicationDbContext context)
        {
            _config = config;
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]LoginModel login)
        {
            var user = await Authenticate(login);

            if (user.IsAuthenticated)
            {
                var tokenString = BuildToken(user, user.Roles);
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }

        private string BuildToken(UserModel user, ICollection<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            // TODO: assign the proper load based on interpreting the db
            token.Payload["roles"] = roles;
            token.Payload["email"] = user.Email;
            token.Payload["uId"] = user.Id;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserModel> Authenticate(LoginModel login)
        {
            UserModel user = new UserModel
            {
                Name = "Test",
                Email = login.Email,
                IsAuthenticated = false,
                Roles = new List<string>()
            };

            // we query the database for the following "claims", which just means
            // that we are querying to see what kind of user the email pertains to
            // which is important for distuinguishing their role and what kind of
            // table relationships can exist for them.
            var studentClaim = await 
                _context
                .Students
                .Where(_ => _.Email == login.Email)
                .FirstOrDefaultAsync();

            var instructorClaim = await
                _context
                .Instructors
                .Where(_ => _.Email == login.Email)
                .FirstOrDefaultAsync();

            if (studentClaim != null)
            {
                if (studentClaim.Password == login.Password)
                //if (PasswordSecurity.CompareHashedPasswords(login.Password, studentClaim.Password))
                {
                    user.Roles.Add("Student");
                    user.IsAuthenticated = true;
                    user.Id = studentClaim.StudentId;
                }
            }
            else if (instructorClaim != null)
            {
                if (instructorClaim.Password == login.Password)
                {
                    user.Roles.Add("Instructor");
                    user.IsAuthenticated = true;
                    user.Id = instructorClaim.InstructorId;
                }
            }
            return user;
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
            public ICollection<string> Roles { get; set; }
            public bool IsAuthenticated { get; set; }

            public int Id { get; set; }
        }
    }
}