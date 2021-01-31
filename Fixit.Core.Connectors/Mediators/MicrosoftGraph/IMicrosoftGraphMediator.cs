using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.Connectors.DataContracts;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Users.Account;

namespace Fixit.Core.Connectors.Mediators
{
  public interface IMicrosoftGraphMediator
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="blockSignIn"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ConnectorDto<UserAccountStateDto>> UpdateAccountSignInStatusAsync(string userId, bool blockSignIn, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteAccountAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newPassword"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> UpdateAccountPasswordAsync(string userId, string newPassword, CancellationToken cancellationToken);
  }
}
