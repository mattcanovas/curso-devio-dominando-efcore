﻿using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

class Program {
    private static void Main(string[] args) {
        //EnsureCreatedAndDeleted();
        //GapDoEnsureCreated();
        //HealthCheckBancoDeDados();
        //_ = new ApplicationContext().Departamentos.AsNoTracking().Any();
        //_count = 0;
        //GerenciarEstadoDaConexa(false);
        //_count = 0;
        //GerenciarEstadoDaConexa(true);

        //ExecuteSql();
        SqlInjection();
    }

    static void SqlInjection() {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Departamentos.AddRange(
            new Departamento {
                Descricao = "Departamento 01"
            },
            new Departamento {
                Descricao = "Departamento 02"
            }
        );
        db.SaveChanges();
        var descricao = "Departamento 01";
        db.Database.ExecuteSqlRaw("update Departamentos set Descricao='DepartamentoAlterado' where Descricao = {0}", descricao);
        foreach(var departamento in db.Departamentos.AsNoTracking()) {
            Console.WriteLine($"Id: {departamento?.Id}, Descricao: {departamento?.Descricao}");
        }
    }

    static void ExecuteSql() {
        using var db = new ApplicationContext();

        // primeira opção
        using var cmd = db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SELECT 1;";
        cmd.ExecuteNonQuery();

        // segunda opção
        var descricao = "TESTE";
        db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id = 1;", descricao);

        // terceira opção
        db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao} where id = 1;");
    }

    static int _count;
    static void GerenciarEstadoDaConexa(bool gerenciarEstadoConexao) {
        using var db = new ApplicationContext();
        var time = Stopwatch.StartNew();

        var conexao = db.Database.GetDbConnection();
        conexao.StateChange += (_, __) => ++_count;
        if (true == gerenciarEstadoConexao) {
            conexao.Open();
        }
        for (int i = 0; i < 200; i++) {
            db.Departamentos.AsNoTracking().Any();
        }

        time.Stop();
        var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";

        Console.WriteLine(mensagem);
        conexao.Close();
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