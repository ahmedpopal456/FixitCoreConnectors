using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fixit.Core.Connectors.Adapters;
using Fixit.Core.Connectors.UnitTests.Adapters;
using Fixit.Core.DataContracts.Seeders;

namespace Fixit.Core.Connectors.UnitTests
{
  public class TestBase
  {
    protected IFakeSeederFactory _fakeDtoSeederFactory;

    protected Mock<IGraphServiceClientAdapter> _graphServiceClientAdapter;

    public TestBase()
    {
      _fakeDtoSeederFactory = new FakeDtoSeederFactory();
    }

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext) { }

    [AssemblyCleanup]
    public static void AfterSuiteTests() { }
  }
}
