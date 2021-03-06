using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Entities;
using TL.Pokedex.Core.Services;
using Xunit;

namespace TL.Pokedex.Core.UnitTests
{
    public class Get_Pokemon
    {
        private readonly AutoMocker _mocker;
        private readonly PokemonService SUT;

        public Get_Pokemon()
        {
            _mocker = new AutoMocker();
            SUT = _mocker.CreateInstance<PokemonService>();
        }

        [Fact]
        public async Task Returns_Pokemon_When_Found()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPokemonRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new Pokemon
                {
                    Name = name,
                    Description = "description",
                    Habitat = "habitat",
                    IsLegendary = false
                });

            // Act
            var actual = await SUT.GetAsync(name);

            // Assert
            actual.Should().BeEquivalentTo(new
            {
                Name = name,
                Description = "description",
                Habitat = "habitat",
                IsLegendary = false
            });
        }

        [Fact]
        public async Task Returns_Pokemon_When_Name_Is_Cased_Differently()
        {
            // Arrange
            const string name = "MewTwo";

            _mocker.GetMock<IPokemonRepository>()
                .Setup(x => x.GetAsync("mewtwo"))
                .ReturnsAsync(new Pokemon
                {
                    Name = name,
                    Description = "description",
                    Habitat = "habitat",
                    IsLegendary = false
                });

            // Act
            var actual = await SUT.GetAsync(name);

            // Assert
            actual.Should().BeEquivalentTo(new
            {
                Name = name,
                Description = "description",
                Habitat = "habitat",
                IsLegendary = false
            });
        }

        [Fact]
        public async Task Returns_Null_When_Not_Found()
        {
            // Arrange
            const string name = "cain";

            // Act
            var actual = await SUT.GetAsync(name);

            // Assert
            actual.Should().BeNull();
        }
    }
}
