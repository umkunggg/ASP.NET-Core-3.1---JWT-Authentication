using JWT.Models;
using JWT.Entities;
using JWT.Helpers;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace JWT.Services
{
    public interface IUserService{
        AuthenticateResponse  Authenticate(AuthenticateRequest Model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            new User {Id = 1, FirstName = "test", LastName = "test", Username = "test", Password = "test"}
        };

        private  AppSettings _appSettings;

        public UserService (IOptions<AppSettings> appSettings){
            _appSettings = appSettings.Value;
        }

        public  AuthenticateResponse Authenticate(AuthenticateRequest Model)
        {
            var user = _users.SingleOrDefault(x => x.Username == Model.Username && x.Password == Model.Password);
            
            if (user == null) return null;
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user,token);

        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}