using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailsTGBot.Models;

namespace CocktailsTGBot
{
    public class Client
    {
        private HttpClient httpClient;
        private static string token;
        private static string address;
        public Client()
        {
            address = Constants.Address;
            token = Constants.Token;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(address);
        }
        public async Task<Cocktail> GetCocktailsByNameAsync(string drink)
        {
            var responce = await httpClient.GetAsync($"/api/Cocktails/cocktail_by_name?drink={drink}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Cocktail>(content);
            return result;
        }

        public async Task<Cocktail> GetCocktailsByIngredienteAsync(string ingrediente)
        {
            var responce = await httpClient.GetAsync($"/api/Cocktails/cocktail_by_ingrediente?ingrediente={ingrediente}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Cocktail>(content);
            return result;
        }

        public async Task<Cocktail> GetCocktailRandomAsync()
        {
            var responce = await httpClient.GetAsync($"/api/Cocktails/cocktail_random");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Cocktail>(content);
            return result;
        }

    }
}
