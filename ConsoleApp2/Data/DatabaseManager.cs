using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Homework2;

/// <summary> Класс для управления базой данных SQLite. </summary>
public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    /// <summary> Инициализация БД. Метод public для исправления ошибки CS0122. </summary>
    public void InitializeDatabase()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS studio (studio_id INTEGER PRIMARY KEY, name TEXT);
            CREATE TABLE IF NOT EXISTS movie (
                movie_id INTEGER PRIMARY KEY AUTOINCREMENT, 
                studio_id INTEGER, title TEXT, budget_mln INTEGER);";
        cmd.ExecuteNonQuery();
    }

    /// <summary> Импорт данных с очисткой. </summary>
    public void ImportFromCsv(string sPath, string mPath)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var clear = conn.CreateCommand();
        clear.CommandText = "DELETE FROM movie; DELETE FROM studio;";
        clear.ExecuteNonQuery();

        if (File.Exists(sPath))
            foreach (var line in File.ReadAllLines(sPath).Skip(1))
            {
                var p = line.Split(';');
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO studio VALUES (@id, @n)";
                cmd.Parameters.AddWithValue("@id", p[0]);
                cmd.Parameters.AddWithValue("@n", p[1]);
                cmd.ExecuteNonQuery();
            }

        if (File.Exists(mPath))
            foreach (var line in File.ReadAllLines(mPath).Skip(1))
            {
                var p = line.Split(';');
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO movie (studio_id, title, budget_mln) VALUES (@s, @t, @b)";
                cmd.Parameters.AddWithValue("@s", p[1]);
                cmd.Parameters.AddWithValue("@t", p[2]);
                cmd.Parameters.AddWithValue("@b", p[3]);
                cmd.ExecuteNonQuery();
            }
    }

    /// <summary> Метод вставки для исправления ошибки CS1061. </summary>
    public void AddMovie(Movie m)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO movie (studio_id, title, budget_mln) VALUES (@s, @t, @b)";
        cmd.Parameters.AddWithValue("@s", m.StudioId);
        cmd.Parameters.AddWithValue("@t", m.Title);
        cmd.Parameters.AddWithValue("@b", m.BudgetMln);
        cmd.ExecuteNonQuery();
    }

    /// <summary> Метод удаления для исправления ошибки CS1061. </summary>
    public void DeleteMovie(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM movie WHERE movie_id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public List<Movie> GetAllMovies() =>
        ExecuteQuery("SELECT m.movie_id, m.studio_id, m.title, m.budget_mln, s.name FROM movie m JOIN studio s ON m.studio_id = s.studio_id");

    public List<Movie> ExecuteQuery(string sql)
    {
        var list = new List<Movie>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        while (r.Read())
            // Исправлена ошибка CS1729: передаем ровно 5 аргументов
            list.Add(new Movie(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetInt32(3), r.FieldCount > 4 ? r.GetString(4) : ""));
        return list;
    }
}