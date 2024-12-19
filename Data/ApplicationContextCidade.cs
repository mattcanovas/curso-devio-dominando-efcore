using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Data {
    public class ApplicationContextCidade : DbContext {
        public DbSet<Cidade> Cidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            const string strConnection = "Server=127.0.0.1; Port=5432; User ID=postgres; Password=Y7QXzeybI0VChLw; Database=curso-desenvolvedor-io; Pooling=true; MinPoolSize=0; MaxPoolSize=5; Connection Lifetime=3000; Timeout=15";
            optionsBuilder
                .UseNpgsql(strConnection)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
