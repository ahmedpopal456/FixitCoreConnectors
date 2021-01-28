using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fixit.Core.Connectors.Adapters;
using Fixit.Core.DataContracts;
using Fixit.Core.Connectors.UnitTests.Adapters;

namespace Fixit.Core.Connectors.UnitTests
{
  public class TestBase
  {
    public IFakeSeederFactory fakeDtoSeederFactory;

    protected Mock<IGraphServiceClientAdapter> _graphServiceClientAdapter;

    public TestBase()
    {
      fakeDtoSeederFactory = new FakeDtoSeederFactory();
    }

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext) { }

    [AssemblyCleanup]
    public static void AfterSuiteTests() { }
  }
}
