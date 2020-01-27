using System;
using System.Linq;
using startapidotnet.Database;
using Microsoft.Extensions.Configuration;
using startapidotnet.Models;

namespace startapidotnet.Services
{
    public class DeleteUser
    {
        private readonly ApplicationDbContext database;
        private IConfiguration config;
        private  dynamic parameters;

        public DeleteUser(ApplicationDbContext database, IConfiguration config, dynamic parameters){
            this.database = database;
            this.config = config;
            this.parameters = parameters;
        }

        public dynamic run()
        {
            int id = this.parameters.id;
            try {
                UserModel user = this.database.users.First(u => u.id == id);
                this.database.users.Remove(user);
                this.database.SaveChanges();
                return true;
            } catch(Exception) {
                throw new ServiceException("User not found", 404);
            }
        }
    }
}
