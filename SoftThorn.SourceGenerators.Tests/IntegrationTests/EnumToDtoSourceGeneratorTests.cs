namespace SoftThorn.SourceGenerators.Tests
{
    public sealed class EnumToDtoSourceGeneratorTests
    {
        [Fact]
        public async Task GetDtos_Should_Return_Expected_Results_For_TestEnumService()
        {
            // Arrange
            var sut = new TestEnumEnumService();

            // Act
            var dtos = sut.GetDtos();

            // Assert
            await Verify(dtos);
        }

        [Fact]
        public async Task GetDtos_Should_Return_Expected_Results_For_ColorEnumService()
        {
            // Arrange
            var sut = new ColorEnumService();

            // Act
            var dtos = sut.GetDtos();

            // Assert
            await Verify(dtos);
        }

        [Fact]
        public async Task GetDtos_Should_Return_Expected_Results_For_EmptyEnumService()
        {
            // Arrange
            var sut = new EmptyEnumService();

            // Act
            var dtos = sut.GetDtos();

            // Assert
            await Verify(dtos);
        }
    }
}
