namespace Backend.Architecture;

public class BasicTest
{
    [Test]
    public async Task Test()
    {
        var now = DateTime.UtcNow;

        await Assert.That(now).IsEqualTo(now);
    }
}
