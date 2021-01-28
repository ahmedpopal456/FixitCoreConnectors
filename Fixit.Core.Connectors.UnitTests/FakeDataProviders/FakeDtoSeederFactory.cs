using Fixit.Core.DataContracts;
using Microsoft.Graph;

namespace Fixit.Core.Connectors.UnitTests.Adapters
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IFakeSeederAdapter<T> CreateFakeSeeder<T>() where T : class
    {
      string type = typeof(T).Name;

      switch (type)
      {
        case nameof(User):
          return (IFakeSeederAdapter<T>)new FakeMSGraphUserSeeder();
        default:
          return null;
      }
    }
  }
}
