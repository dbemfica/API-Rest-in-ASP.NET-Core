using System.Linq;
using startapidotnet.Database;
using Microsoft.Extensions.Configuration;

namespace startapidotnet.Services
{
    public class ListUsers
    {
        private readonly ApplicationDbContext database;
        private IConfiguration config;

        public ListUsers(ApplicationDbContext database, IConfiguration config){
            this.database = database;
            this.config = config;
        }

        public dynamic run()
        {
            return this.database.users.ToList();
        }
    }
}
