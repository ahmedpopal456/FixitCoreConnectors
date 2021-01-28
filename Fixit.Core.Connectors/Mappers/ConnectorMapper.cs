using AutoMapper;
using Fixit.Core.DataContracts.Users.Account;
using Fixit.Core.DataContracts.Users.Enums;
using Microsoft.Graph;

namespace Fixit.Core.Connectors.Mappers
{
  public class ConnectorMapper : Profile
  {
    public ConnectorMapper()
    {
      CreateMap<User, UserAccountStateDto>()
        .ForMember(userState => userState.State, opts => opts.MapFrom(graphUser => graphUser != null && graphUser.AccountEnabled.Value ? UserState.Enabled : UserState.Disabled));
    }
  }
}
