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
    /// <param name="userPrincipalName"></param>
    /// <param name="blockSignIn"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ConnectorDto<UserAccountStateDto>> UpdateAccountSignInStatusAsync(string userPrincipalName, bool blockSignIn, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteAccountAsync(string userPrincipalName, CancellationToken cancellationToken);
  }
}
