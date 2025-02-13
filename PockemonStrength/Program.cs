using Microsoft.Extensions.DependencyInjection;

namespace PokemonStrength
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<HttpClient>()
                .AddSingleton<PokemonStrengthService>()
                .BuildServiceProvider();
            var pokemonStrengthService = serviceProvider.GetRequiredService<PokemonStrengthService>();
            Console.Write("Please enter the Pokémon's name to see its strengths and weaknesses: ");
            var pockemonName = Console.ReadLine();
            if (pockemonName == null)
            {
                Console.WriteLine("Error: Pokémon name is required.");
                return;
            }

            try
            {
                string pokemon = await pokemonStrengthService.GetPokemon(pockemonName);
                string typeName = pokemonStrengthService.GetPokemonTypeName(pokemon);
                string typeData = await pokemonStrengthService.GetTypeData(typeName);
                pokemonStrengthService.PrintStrengthsAndWeaknesses(typeData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}

