namespace SoftThorn.SourceGenerators.Tests
{
    public sealed class EnumToDtoSourceGeneratorTests
    {
        [Fact]
        public async Task GetDtos_Should_Return_Expected_Results()
        {
            // Arrange
            var sut = new TestEnumService();

            // Act
            var dtos = sut.GetDtos();

            // Assert
            await Verify(dtos);
        }
    }
}
