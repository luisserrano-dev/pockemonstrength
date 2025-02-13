using Newtonsoft.Json.Linq;
using System.Text;

namespace PokemonStrength
{
    public class PokemonStrengthService
    {
        private readonly HttpClient _httpClient;
        public PokemonStrengthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPokemon(string name)
        {
            using HttpClient client = new();
            string url = $"https://pokeapi.co/api/v2/pokemon/{name.ToLower()}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public string GetPokemonTypeName(string pokemonData)
        {
            var pokemon = JObject.Parse(pokemonData);
            var type = pokemon?["types"]?[0]?["type"]?["name"]?.ToString();
            return type;
        }

        public async Task<string> GetTypeData(string typeName)
        {
            string url = $"https://pokeapi.co/api/v2/type/{typeName.ToLower()}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public void PrintStrengthsAndWeaknesses(string typeData)
        {
            var type = JObject.Parse(typeData);
            var damageRelations = type?["damage_relations"];
            StringBuilder weakAgainst = new();
            StringBuilder strongAgainst = new();

            // Strengths
            var doubleDamageTo = damageRelations?["double_damage_to"];
            var noDamageFrom = damageRelations?["no_damage_from"];
            var halfDamageFrom = damageRelations?["half_damage_from"];

            // Weaknesses
            var noDamageTo = damageRelations?["no_damage_to"];
            var halfDamageTo = damageRelations?["half_damage_to"];
            var doubleDamageFrom = damageRelations?["double_damage_from"];

            if (doubleDamageTo != null)
            {
                foreach (var relation in doubleDamageTo)
                {
                    strongAgainst.Append($" {relation["name"]}");
                }
            }

            if (noDamageFrom != null)
            {
                foreach (var relation in noDamageFrom)
                {
                    strongAgainst.Append($" {relation["name"]}");
                }
            }

            if (halfDamageFrom != null)
            {
                foreach (var relation in halfDamageFrom)
                {
                    strongAgainst.Append($" {relation["name"]}");
                }
            }

            if (noDamageTo != null)
            {
                foreach (var relation in noDamageTo)
                {
                    weakAgainst.Append($" {relation["name"]}");
                }
            }

            if (halfDamageTo != null)
            {
                foreach (var relation in halfDamageTo)
                {
                    weakAgainst.Append($" {relation["name"]}");
                }
            }

            if (doubleDamageFrom != null)
            {
                foreach (var relation in doubleDamageFrom)
                {
                    weakAgainst.Append($" {relation["name"]}");
                }
            }

            Console.WriteLine("Strong against: " + strongAgainst.ToString());
            Console.WriteLine("Weak against: " + weakAgainst.ToString());

        }
    }
}
