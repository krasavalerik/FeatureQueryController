using Microsoft.EntityFrameworkCore;
using WebApp.Entitys;

namespace WebApp.Repositorys
{
    public class EfContext : DbContext
    {
        public DbSet<Layer> Layers { get; set; }
    }
}
