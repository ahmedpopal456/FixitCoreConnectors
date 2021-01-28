using System;
using Fixit.Core.Connectors.Adapters.MicrosoftGraph.Internal;
using Fixit.Core.Connectors.Mediators;
using Fixit.Core.Connectors.Mediators.MicrosoftGraph.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace Fixit.Core.Connectors
{

  public class ConnectorFactory
  {
    private readonly string _appId;
    private readonly string _tenantId;
    private readonly string _clientSecret;
    public ConnectorFactory(IConfiguration configuration)
    {
      string appId = configuration["FIXIT-CN-MSG-APPID"];
      string tenantId = configuration["FIXIT-CN-MSG-TENANTID"];
      string clientSecret = configuration["FIXIT-CN-MSG-CLIENTSECRET"];

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

      _appId = appId;
      _tenantId = tenantId;
      _clientSecret = clientSecret;
    }

    public ConnectorFactory(string appId, string tenantId, string clientSecret)
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

      _appId = appId;
      _tenantId = tenantId;
      _clientSecret = clientSecret;
    }

    public IMicrosoftGraphMediator CreateMicrosoftGraphClient()
    {
      IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
        .Create(_appId)
        .WithTenantId(_tenantId)
        .WithClientSecret(_clientSecret)
        .Build();
      ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
      GraphServiceClient graphClient = new GraphServiceClient(authProvider);
      GraphServiceClientAdapter msGraphAdapter = new GraphServiceClientAdapter(graphClient);
      return new MicrosoftGraphMediator(msGraphAdapter);
    }

  }
}
