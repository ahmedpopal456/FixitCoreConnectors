using System;
using Fixit.Core.Connectors.Adapters.MicrosoftGraph.Internal;
using Fixit.Core.Connectors.Mediators;
using Fixit.Core.Connectors.Mediators.GoogleApis;
using Fixit.Core.Connectors.Mediators.MicrosoftGraph.Internal;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace Fixit.Core.Connectors
{
  public static class ConnectorFactory
  {
    public static IMicrosoftGraphMediator CreateMicrosoftGraphClient(string appId, string tenantId, string clientSecret)
    {
      if (string.IsNullOrWhiteSpace(appId))
      {
        throw new ArgumentNullException($"{nameof(ConnectorFactory)} expects a valid value for {nameof(appId)} within the configuration file");
      }

      if (string.IsNullOrWhiteSpace(tenantId))
      {
        throw new ArgumentNullException($"{nameof(ConnectorFactory)} expects a valid value for {nameof(tenantId)} within the configuration file");
      }

      if (string.IsNullOrWhiteSpace(clientSecret))
      {
        throw new ArgumentNullException($"{nameof(ConnectorFactory)} expects a valid value for {nameof(clientSecret)} within the configuration file");
      }

      IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
        .Create(appId)
        .WithTenantId(tenantId)
        .WithClientSecret(clientSecret)
        .Build();

      ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
      GraphServiceClient graphClient = new GraphServiceClient(authProvider);
      GraphServiceClientAdapter msGraphAdapter = new GraphServiceClientAdapter(graphClient);
      return new MicrosoftGraphMediator(msGraphAdapter);
    }

    public static IGoogleApiMediator CreateGoogleApiClient(string key)
    {
      var googleApiMediator  = new GoogleApiMediator(key);
      return  googleApiMediator;
    }
  }
}
