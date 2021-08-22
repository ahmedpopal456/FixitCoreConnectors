using System;
using System.Linq;
using AutoMapper;
using Fixit.Core.DataContracts.Users.Address;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Details.Response;

namespace Empower.Mdm.Data.Contexts.User.Dals.Mappers.Converters.Agents
{
  public class AddressDetailsToAddressDto : ITypeConverter<DetailsResult, AddressDto>
  {
    public AddressDto Convert(DetailsResult source, AddressDto destination, ResolutionContext context)
    {
      destination ??= new AddressDto();

      if (source != null)
      {
        destination.FormattedAddress = source?.FormattedAddress;
        destination.AddressComponents = source?.AddressComponents.Select(ac => context.Mapper.Map<AddressComponent, AddressComponentDto>(ac));
      }

      return destination;
    }
  }
}
