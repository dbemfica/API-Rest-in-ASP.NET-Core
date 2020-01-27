using System.Linq;
using startapidotnet.Database;
using Microsoft.Extensions.Configuration;
using startapidotnet.Models;

namespace startapidotnet.Services
{
    public class UpdateUser
    {
        private readonly ApplicationDbContext database;
        private IConfiguration config;
        private  dynamic parameters;

        public UpdateUser(ApplicationDbContext database, IConfiguration config, dynamic parameters){
            this.database = database;
            this.config = config;
            this.parameters = parameters;
        }

        public dynamic run()
        {
            int id = this.parameters.id;
            UserModel user = this.database.users.First(u => u.id == id);
            user.nome = this.parameters.nome;
            user.email = this.parameters.email;
            user.login = this.parameters.login;
            user.password = BCrypt.Net.BCrypt.HashPassword(this.parameters.password);
        
            this.database.users.Update(user);
            this.database.SaveChanges();
            return user;
        }
    }
}
