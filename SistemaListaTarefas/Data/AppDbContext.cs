using Microsoft.EntityFrameworkCore;
using SistemaListaTarefas.Models;

namespace SistemaListaTarefas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}