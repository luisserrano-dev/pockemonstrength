using Moq;
using Moq.Protected;
using PokemonStrength;

namespace PockemonStrength.Tests
{
    public class PokemonStrengthTests
    {
        private readonly PokemonStrengthService _pokemonStrengthService;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public PokemonStrengthTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _pokemonStrengthService = new PokemonStrengthService(httpClient);
        }

        [Fact]
        public async Task GetPokemonReturnsPokemonDataWhenPokemonExists()
        {
            // Arrange
            var pokemonName = "ditto";
            var pokemonData = "{\"name\":\"ditto\"}";
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(pokemonData)
                });

            // Act
            var result = await _pokemonStrengthService.GetPokemon(pokemonName);

            // Assert
            Assert.Equal(pokemonData, result);
        }

        [Fact]
        public void PrintStrengthsAndWeaknessesShouldPrintStrengthsAndWeaknessesForGivenType()
        {
            // Arrange
            var typeData = @"
            {
                ""damage_relations"": {
                    ""double_damage_to"": [
                        {""name"": ""ghost""}
                    ],
                    ""half_damage_from"": [
                    ],
                    ""no_damage_from"": [
                    ],
                    ""double_damage_from"": [
                        {""name"": ""fighting""}
                    ],
                    ""half_damage_to"": [
                        {""name"": ""steel""}
                    ],
                    ""no_damage_to"": [
                        {""name"": ""ghost""},
                        {""name"": ""rock""}
                    ]
                }
            }";

            var expectedOutput = @"Strong against:  ghost
Weak against:  ghost rock steel fighting";

            var consoleOutput = new StringWriter();

            // Act
            Console.SetOut(consoleOutput);
            _pokemonStrengthService.PrintStrengthsAndWeaknesses(typeData);

            // Assert
            Assert.Equal(expectedOutput, consoleOutput.ToString().Trim());
        }

    }
}