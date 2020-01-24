using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using startapidotnet.Database;
using startapidotnet.Models;

namespace startapidotnet.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext database;

        public UsersController(ApplicationDbContext database){
            this.database = database;
        }

        [HttpGet]
        public IActionResult index()
        {
            var users = this.database.users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult index(int id)
        {
            var user = this.database.users.First(u => u.id == id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult create([FromBody] UserModel userModel)
        {
            UserModel u = new UserModel();
            u.nome = userModel.nome;
            u.email = userModel.email;
            u.login = userModel.login;
            u.password = userModel.password;
        
            database.users.Add(u);
            database.SaveChanges();

            Response.StatusCode = 201;
            return Ok(userModel);
        }

        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            UserModel user = this.database.users.First(u => u.id == id);
            this.database.users.Remove(user);
            database.SaveChanges();
            return Ok("ok");
        }
    }
}
