using System.Linq;
using startapidotnet.Database;
using Microsoft.Extensions.Configuration;

namespace startapidotnet.Services
{
    public class GetUser
    {
        private readonly ApplicationDbContext database;
        private IConfiguration config;
        private  dynamic parameters;

        public GetUser(ApplicationDbContext database, IConfiguration config, dynamic parameters){
            this.database = database;
            this.config = config;
            this.parameters = parameters;
        }

        public dynamic run()
        {
            int id = this.parameters.id;
            try {
                return this.database.users.First(u => u.id == id);
            } catch(ServiceException) {
                throw new ServiceException("User not found", 404);
            }
        }
    }
}
