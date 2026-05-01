using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Homework2;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

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

    // Универсальный метод для получения данных (используется в ReportBuilder)
    public List<string[]> GetTable(string sql)
    {
        var results = new List<string[]>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = new SqliteCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string[] row = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++) row[i] = reader[i].ToString();
            results.Add(row);
        }
        return results;
    }

    public void AddMovie(string title, int studioId, int budget)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = new SqliteCommand("INSERT INTO movie (title, studio_id, budget_mln) VALUES (@t, @s, @b)", conn);
        cmd.Parameters.AddWithValue("@t", title);
        cmd.Parameters.AddWithValue("@s", studioId);
        cmd.Parameters.AddWithValue("@b", budget);
        cmd.ExecuteNonQuery();
    }

    public void UpdateMovie(int id, string title, int studioId, int budget)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = new SqliteCommand("UPDATE movie SET title=@t, studio_id=@s, budget_mln=@b WHERE movie_id=@id", conn);
        cmd.Parameters.AddWithValue("@t", title);
        cmd.Parameters.AddWithValue("@s", studioId);
        cmd.Parameters.AddWithValue("@b", budget);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public void DeleteMovie(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = new SqliteCommand("DELETE FROM movie WHERE movie_id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}