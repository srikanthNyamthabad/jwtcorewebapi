using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using jwtcorewebapi.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace jwtcorewebapi.Services
{
    public class UserService:IUserService
    {
        private readonly Appsettings _appsettings;

        
        public UserService(IOptions<Appsettings> appSettings)
        {
            _appsettings = appSettings.Value;
        }
        private List<User> users1 = new List<User>()
        {
            new User{UserId=22,FirstName="srikanth",LastName="world", UserName="srikanth",Password="Srikanth" }
        };
        public  User Authenticate(string userName, string password)
        {
            var userdata = users1.SingleOrDefault(x => x.UserName == userName && x.Password == password);

            if (userdata == null)
                return null;
            //if token not found
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name ,userdata.UserId.ToString()),

                    new Claim(ClaimTypes.Role, "Admin")

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userdata.Token = tokenHandler.WriteToken(token);
            userdata.Password=null;
            return userdata;

        }
    }
}
