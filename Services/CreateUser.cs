using startapidotnet.Database;
using Microsoft.Extensions.Configuration;
using startapidotnet.Models;


namespace startapidotnet.Services
{
    public class CreateUser
    {
        private readonly ApplicationDbContext database;
        private IConfiguration config;
        private  dynamic parameters;

        public CreateUser(ApplicationDbContext database, IConfiguration config, dynamic parameters){
            this.database = database;
            this.config = config;
            this.parameters = parameters;
        }

        public dynamic run()
        {
            UserModel u = new UserModel();
            u.nome = this.parameters.nome;
            u.email = this.parameters.email;
            u.login = this.parameters.login;
            u.password = BCrypt.Net.BCrypt.HashPassword(this.parameters.password);
            this.database.users.Add(u);
            this.database.SaveChanges();
            return u;
        }
    }
}
