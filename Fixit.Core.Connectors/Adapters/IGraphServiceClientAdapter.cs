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
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="blockSignIn"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAccountSignInStatusAsync(string userId, bool blockSignIn, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> GetUserAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newPassword"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateUserPasswordAsync(string userId, string newPassword, CancellationToken cancellationToken);
  }
}
