using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Fixit.Core.Connectors.Adapters
{
  public interface IGraphServiceClientAdapter
  {
    /// <summary>
    ///
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteUserAsync(string userPrincipalName, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="blockSignIn"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAccountSignInStatusAsync(string userPrincipalName, bool blockSignIn, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> GetUserAsync(string userPrincipalName, CancellationToken cancellationToken);
  }
}
