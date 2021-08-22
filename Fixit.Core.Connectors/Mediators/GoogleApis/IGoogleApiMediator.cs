using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Users.Address;
using Fixit.Core.DataContracts.Users.Address.Query;

namespace Fixit.Core.Connectors.Mediators.GoogleApis
{
  public interface IGoogleApiMediator
  {
    /// <summary>
    /// Get multiple predicted addresses by passing an input search text
    /// </summary>
    /// <param name="search"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<OperationStatusWithObject<IEnumerable<AddressQueryItem>>> GetAddressesBySearchAsync(string search, CancellationToken cancellationToken);

    /// <summary>
    /// Get an address' detail based on its id
    /// </summary>
    /// <param name="addressId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<OperationStatusWithObject<AddressDto>> GetAddressByIdAsync(string addressId, CancellationToken cancellationToken);
  }
}
