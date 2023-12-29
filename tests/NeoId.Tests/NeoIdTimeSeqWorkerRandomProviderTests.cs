namespace NeoId.Tests;

public class NeoIdTimeSeqWorkerRandomProviderTests
{
    [Fact]
    public void CreateNewSuccess()
    {
        // Arrange
        byte[] workerId = new byte[] { 1,2,3,4 };
        NeoIdTimeSeqWorkerRandomProvider provider = new(workerId);

        // Act
        var ret = provider.CreateNew();

        // Assert
        ret.Should().NotBeEmpty();
    }

    [Fact]
    public void ParseSuccess()
    {
        // Arrange
        byte[] workerId = new byte[] { 1,2,3,4 };
        NeoIdTimeSeqWorkerRandomProvider provider = new(workerId);
        var id = provider.CreateNew();

        // Act
        var ret = NeoIdConfiguration.DefaultConfiguration.Provider.Parse(id);

        // Assert
        ret.Should().NotBeNull();
        ret.SequentialNumber.Should().Be(NeoIdBaseProvider.CurrentSequential);
        ret.WorkerId.Should().BeEquivalentTo(workerId);
    }

    [Theory]
    [InlineData("a389fe0b-3f0b-4518-875a-3ebee5495076")]
    public void ParseError(string testData)
    {
        // Arrange
        Guid test = Guid.Parse(testData);

        // Act
        var act = () => NeoIdConfiguration.DefaultConfiguration.Provider.Parse(test);

        // Assert
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void GenerateMultipleSuccess()
    {
        // Arrange
        byte[] workerId = new byte[] { 1,2,3,4 };
        NeoIdTimeSeqWorkerRandomProvider provider = new(workerId);

        // Act
        var ret = Enumerable.Range(0, 10).Select(x => provider.CreateNew());

        // Assert
        ret.Should().NotBeEmpty();
        ret.Distinct().Count().Should().Be(ret.Count());
        var endings = ret.Select(x => x.ToString()[^4..]);
        endings.Distinct().Count().Should().Be(endings.Count());
    }

}