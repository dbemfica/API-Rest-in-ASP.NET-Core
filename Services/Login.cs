using System;
using System.Text;
using System.Linq;
using startapidotnet.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using startapidotnet.Models;

namespace startapidotnet.Services
{
    public class Login
    {
        private readonly ApplicationDbContext database;
        private IConfiguration config;
        private  dynamic parameters;

        public Login(ApplicationDbContext database, IConfiguration config, dynamic parameters){
            this.database = database;
            this.config = config;
            this.parameters = parameters;
        }

        public dynamic run()
        {
            string login = this.parameters.login;
            string password = this.parameters.password;
            UserModel user = null;

            try {
                user = this.database.users.First(u => u.login == login);
            } catch(Exception) {
                throw new ServiceException("User not found", 404);
            }

            bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.password);
            if (validPassword == false) {
                throw new ServiceException("User not found", 404);
            }

            var symetrictKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["JwtSecretKey"]));
            var signingCredentials = new SigningCredentials(symetrictKey,SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim("id",user.id.ToString()));
            claims.Add(new Claim("email",user.email));
            claims.Add(new Claim("login",user.login));

            var JWT = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims
            ); 

            return new { token = new JwtSecurityTokenHandler().WriteToken(JWT)};
        }
    }
}
