using DominandoEFCore.Data;

class Program {
    private static void Main(string[] args) {
        EnsureCreatedAndDeleted();
    }

    static void EnsureCreatedAndDeleted() {
        using var db = new ApplicationContext();
        //db.Database.EnsureCreated();
        db.Database.EnsureDeleted();
    }
}