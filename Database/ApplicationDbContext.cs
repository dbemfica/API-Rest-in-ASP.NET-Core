
using System;
using System.Collections.Generic;
using System.Text;
using startapidotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace startapidotnet.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public DbSet<UserModel> users {get; set;}

    }
}
