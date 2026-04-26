using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Homework2;

public class DatabaseManager
{
    private string _connectionString = "Data Source=cinema.db";

    public void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS studio (
                    studio_id INTEGER PRIMARY KEY,
                    name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS movie (
                    movie_id INTEGER PRIMARY KEY,
                    studio_id INTEGER NOT NULL,
                    title TEXT NOT NULL,
                    budget_mln INTEGER NOT NULL,
                    FOREIGN KEY (studio_id) REFERENCES studio(studio_id)
                );";
            cmd.ExecuteNonQuery();
        }
    }

    public void ImportFromCsv(string studiosPath, string moviesPath)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            if (File.Exists(studiosPath))
            {
                foreach (var line in File.ReadAllLines(studiosPath))
                {
                    var parts = line.Split(';');
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT OR REPLACE INTO studio (studio_id, name) VALUES (@id, @name)";
                    cmd.Parameters.AddWithValue("@id", int.Parse(parts[0]));
                    cmd.Parameters.AddWithValue("@name", parts[1]);
                    cmd.ExecuteNonQuery();
                }
            }
            if (File.Exists(moviesPath))
            {
                foreach (var line in File.ReadAllLines(moviesPath))
                {
                    var parts = line.Split(';');
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT OR REPLACE INTO movie (movie_id, studio_id, title, budget_mln) VALUES (@id, @sid, @title, @budget)";
                    cmd.Parameters.AddWithValue("@id", int.Parse(parts[0]));
                    cmd.Parameters.AddWithValue("@sid", int.Parse(parts[1]));
                    cmd.Parameters.AddWithValue("@title", parts[2]);
                    cmd.Parameters.AddWithValue("@budget", int.Parse(parts[3]));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public List<Movie> GetAllMovies()
    {
        var movies = new List<Movie>();
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT movie_id, studio_id, title, budget_mln FROM movie";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    movies.Add(new Movie(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3)));
                }
            }
        }
        return movies;
    }
}