using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Fixit.Core.Connectors.Adapters.MicrosoftGraph.Internal
{
  internal class GraphServiceClientAdapter : IGraphServiceClientAdapter
  {
    private GraphServiceClient _graphClient;

    public GraphServiceClientAdapter(GraphServiceClient graphClient)
    {
      _graphClient = graphClient ?? throw new ArgumentNullException($"{nameof(GraphServiceClientAdapter)} expects a value for {nameof(graphClient)}... null argument was provided");
    }

    public Task UpdateAccountSignInStatusAsync(string userPrincipalName, bool blockSignIn, CancellationToken cancellationToken)
    {
      var user = new User
      {
        AccountEnabled = !blockSignIn
      };

      return _graphClient.Users[userPrincipalName].Request().UpdateAsync(user, cancellationToken);
    }

    public async Task<User> GetUserAsync(string userPrincipalName, CancellationToken cancellationToken)
    {
      var userRequest = _graphClient.Users[userPrincipalName]
      .Request()
      .Select(u => new
      {
        u.UserPrincipalName,
        u.Photo,
        u.GivenName,
        u.Surname,
        u.AccountEnabled,
        u.StreetAddress,
        u.PostalCode,
        u.Country,
        u.State,
        u.City,
        u.BusinessPhones,
        u.MobilePhone
      });
      return await userRequest.GetAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(string userPrincipalName, CancellationToken cancellationToken)
    {
      await _graphClient.Users[userPrincipalName].Request().DeleteAsync(cancellationToken);
    }
  }
}
