using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

class Program {
    private static void Main(string[] args) {
        //EnsureCreatedAndDeleted();
        //GapDoEnsureCreated();
        HealthCheckBancoDeDados();
    }

    static void HealthCheckBancoDeDados() {
        using var context = new ApplicationContext();

        // Maneira comum
        //try {
        //    //1
        //    var connection = context.Database.GetDbConnection();
        //    connection.Open();

        //    //2
        //    _ = context.Departamentos.Any();

        //    Console.WriteLine("Posso me conectar");
        //} catch (Exception) {
        //    Console.WriteLine("Houve um problema na conexão com o banco de dados");
        //}

        // Maneira EFCore
        if (context.Database.CanConnect()) {
            Console.WriteLine("Posso me conectar");
            return;
        }

        Console.WriteLine("Houve um problema na conexão com o banco de dados");
    }

    static void EnsureCreatedAndDeleted() {
        using var db = new ApplicationContext();
        db.Database.EnsureCreated();
        //db.Database.EnsureDeleted();
    }

    // Assim resolvemos o gap do métod EnsureCreated(); Utilizando o DatabaseCreator Service
    static void GapDoEnsureCreated() {
        using var db1 = new ApplicationContext();
        using var db2 = new ApplicationContextCidade();

        // Desta maneira apenas o db1 será criado, pois, já foi executado o EnsureCreated,
        // quando for a execução do segundo, não passará na validação e não precisará criar
        //db1.Database.EnsureCreated();
        //db2.Database.EnsureCreated();

        var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
        databaseCreator.CreateTables();
    }

}