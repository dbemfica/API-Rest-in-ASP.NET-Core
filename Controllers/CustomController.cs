using System;
using Microsoft.AspNetCore.Mvc;
using startapidotnet.Database;
using Microsoft.Extensions.Configuration;
using startapidotnet.Services;

namespace startapidotnet.Controllers
{
    public abstract class CustomController : ControllerBase
    {
        protected readonly ApplicationDbContext database;
        protected IConfiguration config;

        public CustomController(ApplicationDbContext database, IConfiguration config){
            this.database = database;
            this.config = config;
        }

        protected dynamic service(string serviceName)
        {
            dynamic service = null;
            try {
                Type type = Type.GetType(serviceName);
                service = Activator.CreateInstance(type, this.database, this.config);
            } catch (Exception) {
                throw new ServiceException("Service not found", 500);
            }

            try {
                return service.run();
            } catch (ServiceException e) {
                throw e;
            }
        }

        protected dynamic service(string serviceName, dynamic parameter)
        {
            dynamic service = null;
            try {
                Type type = Type.GetType(serviceName);
                service = Activator.CreateInstance(type, this.database, this.config, parameter);
            } catch (Exception) {
                throw new ServiceException("Service not found", 500);
            }
            
            try {
                return service.run();
            } catch (ServiceException e) {
                throw e;
            }
        }
    }
}
