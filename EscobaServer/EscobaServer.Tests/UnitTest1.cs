using Xunit;
namespace EscobaServer.Tests;

public class PlayerTests
{
    [Fact]
    public void CreatePlayer()
    {
        // Arrange
        var expected = new Player(0);
        // Act
        var player = new Player(0);

    }
}