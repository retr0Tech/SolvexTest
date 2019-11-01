using System;
using Microsoft.EntityFrameworkCore;

namespace SolvexTest.Api.Models
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        public DbSet<Client> clients { get; set; }
    }
}