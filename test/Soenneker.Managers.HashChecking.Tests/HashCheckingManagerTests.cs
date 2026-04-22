using Soenneker.Managers.HashChecking.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Managers.HashChecking.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class HashCheckingManagerTests : HostedUnitTest
{
    private readonly IHashCheckingManager _util;

    public HashCheckingManagerTests(Host host) : base(host)
    {
        _util = Resolve<IHashCheckingManager>(true);
    }

    [Test]
    public void Default()
    {

    }
}
