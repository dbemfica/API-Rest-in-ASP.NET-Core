using Microsoft.AspNetCore.Mvc;
using startapidotnet.Database;
using startapidotnet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using startapidotnet.Services;

namespace startapidotnet.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : CustomController
    {
        public UsersController(ApplicationDbContext database, IConfiguration config): base(database, config){}

        [HttpGet]
        [Authorize]
        public IActionResult index()
        {
            try {
                var users = this.service("startapidotnet.Services.ListUsers");
                return Ok(users);
            } catch (ServiceException e) {
                Response.StatusCode = e.code;
                return new ObjectResult(new {message = e.message});
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult index(int id)
        {
            try {
                var users = this.service("startapidotnet.Services.GetUser", new {id = id});
                return Ok(users);
            } catch (ServiceException e) {
                Response.StatusCode = e.code;
                return new ObjectResult(new {mensagem = e.message});
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult create([FromBody] UserModel userModel)
        {
            try {
                dynamic parameters = new {
                    nome = userModel.nome,
                    email = userModel.email,
                    login = userModel.login,
                    password = userModel.password
                };
                var users = this.service("startapidotnet.Services.CreateUser", parameters);
                Response.StatusCode = 201;
                return new ObjectResult(users);
            } catch (ServiceException e) {
                Response.StatusCode = e.code;
                return new ObjectResult(new {mensagem = e.message});
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult update([FromBody] UserModel userModel)
        {
            try {
                dynamic parameters = new {
                    id = userModel.id,
                    nome = userModel.nome,
                    email = userModel.email,
                    login = userModel.login,
                    password = userModel.password
                };
                var users = this.service("startapidotnet.Services.UpdateUser", parameters);
                Response.StatusCode = 200;
                return new ObjectResult(users);
            } catch (ServiceException e) {
                Response.StatusCode = e.code;
                return new ObjectResult(new {mensagem = e.message});
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult delete(int id)
        {
            try {
                dynamic parameters = new {
                    id = id
                };
                this.service("startapidotnet.Services.DeleteUser", parameters);
                return Ok();
            } catch (ServiceException e) {
                Response.StatusCode = e.code;
                return new ObjectResult(new {mensagem = e.message});
            }
        }

        [HttpPost("login")]
        public IActionResult login([FromBody] UserModel credentials)
        {
            try {
                dynamic parameters = new {
                    login = credentials.login,
                    password = credentials.password
                };
                var token = this.service("startapidotnet.Services.Login", parameters);
                Response.StatusCode = 200;
                return new ObjectResult(token);
            } catch (ServiceException e) {
                Response.StatusCode = e.code;
                return new ObjectResult(new {mensagem = e.message});
            }
        }
    }
}
