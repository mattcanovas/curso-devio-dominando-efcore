using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

class Program {
    private static void Main(string[] args) {
        //EnsureCreatedAndDeleted();
        GapDoEnsureCreated();
    }

    static void EnsureCreatedAndDeleted() {
        using var db = new ApplicationContext();
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();
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