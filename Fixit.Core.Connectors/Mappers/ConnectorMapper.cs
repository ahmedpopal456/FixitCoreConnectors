using System.Linq;
using AutoMapper;
using Empower.Mdm.Data.Contexts.User.Dals.Mappers.Converters.Agents;
using Fixit.Core.DataContracts.Users.Account;
using Fixit.Core.DataContracts.Users.Address;
using Fixit.Core.DataContracts.Users.Address.Query;
using Fixit.Core.DataContracts.Users.Enums;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Common;
using GoogleApi.Entities.Places.Details.Response;
using Microsoft.Graph;


namespace Fixit.Core.Connectors.Mappers
{
  public class ConnectorMapper : AutoMapper.Profile
  {
    public ConnectorMapper()
    {
      CreateMap<User, UserAccountStateDto>()
        .ForMember(userState => userState.State, opts => opts.MapFrom(graphUser => graphUser != null && graphUser.AccountEnabled.Value ? UserState.Enabled : UserState.Disabled));

      CreateMap<Prediction, AddressQueryItem>()
        .ForMember(addressQueryItem => addressQueryItem.Description, opts => opts.MapFrom(prediction => prediction != null ? prediction.Description : null))
        .ForMember(addressQueryItem => addressQueryItem.PlaceId, opts => opts.MapFrom(prediction => prediction != null ? prediction.PlaceId : null));

      CreateMap<AddressComponent, AddressComponentDto>()
        .ForMember(addressDto => addressDto.LongName, opts => opts.MapFrom(detailsResult => detailsResult != null ? detailsResult.LongName : null))
        .ForMember(addressDto => addressDto.ShortName, opts => opts.MapFrom(detailsResult => detailsResult != null ? detailsResult.ShortName : null))
        .ForMember(addressDto => addressDto.Types, opts => opts.MapFrom(detailsResult => detailsResult != null ? detailsResult.Types : null));

      CreateMap<DetailsResult, AddressDto>()
        .ConvertUsing<AddressDetailsToAddressDto>();
    }
  }
}
