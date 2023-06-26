using CocktailsTGBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Newtonsoft.Json;

namespace CocktailsTGBot
{
    public class Database
    {
        NpgsqlConnection con = new NpgsqlConnection(Constants.Con);
        public async Task InsertCocktailAsync(Cocktail cocktail, string drink)
        {
            var sql = "insert into public.\"CocktailsLog\"(\"Time\", \"Text_input\")"
                + $"values (@Time, @Text_input)";
            NpgsqlCommand comm = new NpgsqlCommand(sql, con);
            comm.Parameters.AddWithValue("Time", DateTime.Now);
            comm.Parameters.AddWithValue("Text_input", drink);
            await con.OpenAsync();
            await comm.ExecuteNonQueryAsync();
            await con.CloseAsync();
        }
        public async Task<List<Statistics>> SelectStatisticsAsync()
        {
            List<Statistics> mains = new List<Statistics>();
            await con.OpenAsync();
            var sql = "select \"Time\", \"Text_input\" from public.\"CocktailsLog\"";
            NpgsqlCommand comm = new NpgsqlCommand(sql, con);
            NpgsqlDataReader reader = await comm.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                mains.Add(new Statistics { DateTime = reader.GetDateTime(0), Drink = reader.GetString(1) });
            }
            await con.CloseAsync();
            return mains;
        }

        public async Task DeleteStatisticsAsync()
        {
            await con.OpenAsync();
            var sql = $"delete from public.\"CocktailsLog\"";
            NpgsqlCommand comm = new NpgsqlCommand(sql, con);
            NpgsqlDataReader reader = await comm.ExecuteReaderAsync();
            await con.CloseAsync();
        }
    }
}
