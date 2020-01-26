using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using startapidotnet.Database;
using startapidotnet.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace startapidotnet.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext database;
        private IConfiguration config;

        public UsersController(ApplicationDbContext database, IConfiguration config){
            this.database = database;
            this.config = config;
        }

        [HttpGet]
        [Authorize]
        public IActionResult index()
        {
            var users = this.database.users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult index(int id)
        {
            try {
                var user = this.database.users.First(u => u.id == id);
                Response.StatusCode = 200;
                return Ok(user);
            } catch(Exception) {
                Response.StatusCode = 404;
                return new ObjectResult(new {mensagem = "User not found"});
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult create([FromBody] UserModel userModel)
        {
            UserModel u = new UserModel();
            u.nome = userModel.nome;
            u.email = userModel.email;
            u.login = userModel.login;
            u.password = BCrypt.Net.BCrypt.HashPassword(userModel.password);
        
            database.users.Add(u);
            database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult(u);
        }

        [HttpPut]
        [Authorize]
        public IActionResult update([FromBody] UserModel userModel)
        {
            UserModel user = this.database.users.First(u => u.id == userModel.id);
            user.nome = userModel.nome;
            user.email = userModel.email;
            user.login = userModel.login;
            user.password = BCrypt.Net.BCrypt.HashPassword(userModel.password);
        
            database.users.Update(user);
            database.SaveChanges();

            Response.StatusCode = 200;
            return Ok(userModel);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult delete(int id)
        {
            try {
                UserModel user = this.database.users.First(u => u.id == id);
                this.database.users.Remove(user);
                database.SaveChanges();
                return Ok("ok");
            } catch(Exception) {
                Response.StatusCode = 400;
                return new ObjectResult("");
            }
        }

        [HttpPost("login")]
        public IActionResult login([FromBody] UserModel credentials)
        {
            UserModel user = null;
            try {
                user = this.database.users.First(u => u.login == credentials.login);
            } catch(Exception) {
                Response.StatusCode = 404;
                return new ObjectResult(new {mensagem = "User not found"});
            }

            bool validPassword = BCrypt.Net.BCrypt.Verify(credentials.password, user.password);
            if (validPassword == false) {
                Response.StatusCode = 404;
                return new ObjectResult(new {mensagem = "User not found"});
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

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(JWT)});
        }
    }
}
