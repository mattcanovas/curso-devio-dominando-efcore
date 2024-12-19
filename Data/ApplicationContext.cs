using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data {
    public class ApplicationContext : DbContext {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            const string strConnection = "Server=127.0.0.1; Port=5432; User ID=postgres; Password=Y7QXzeybI0VChLw; Database=curso-desenvolvedor-io; Pooling=true; MinPoolSize=0; MaxPoolSize=5; Connection Lifetime=3000; Timeout=15";
            optionsBuilder
                .UseNpgsql(strConnection)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
