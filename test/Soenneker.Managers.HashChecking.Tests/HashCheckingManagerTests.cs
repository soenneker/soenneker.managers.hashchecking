using Soenneker.Managers.HashChecking.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Managers.HashChecking.Tests;

[Collection("Collection")]
public class HashCheckingManagerTests : FixturedUnitTest
{
    private readonly IHashCheckingManager _util;

    public HashCheckingManagerTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IHashCheckingManager>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
