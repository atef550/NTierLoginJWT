using NTierLoginJWT.Core.Models;
using NTierLoginJWT.DAL;
using NTierLoginJWT.DAL.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace NTierLoginJWT.BLL.Services
{
    public interface IUserService
    {
        string Authenticate(AuthenticateRequestModel model);
        void DeleteById(int id);
        int Register(RegisteredModel model);
        int EditRegister(int id,RegisteredModel model);

    }

    public class UserService : IUserService
    {
        private readonly CRUDContext _context;
        private readonly IConfiguration _config;
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        /* private List<User> _users = new List<User>
         {
             new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" },
             new User { Id = 2, FirstName = "Ahmed", LastName = "Atef", Username = "ahmed", Password = "123" }
         };
        */


        //private readonly AppSettings _appSettings;

        public UserService(IConfiguration config, CRUDContext context)
        {
            _config = config;
            _context = context;
        }

        public string Authenticate(AuthenticateRequestModel model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(model);

            return token;
        }


        public void DeleteById(int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }




        /* public IEnumerable<User> GetAll()
         {
             return _users;
         }*/


        public int Register(RegisteredModel model)
        {
            User user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password
            };
            _context.Users.Add(user);
            return _context.SaveChanges();
        }

        public int EditRegister(int id,RegisteredModel model)
        {

            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Username = model.Username;
            user.Password = model.Password;
           
            _context.Users.Update(user);
            return _context.SaveChanges();
        }

        private string GenerateJwtToken(AuthenticateRequestModel user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["AppSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}