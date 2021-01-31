using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AutoMapper;
using Fixit.Core.Connectors.Adapters;
using Fixit.Core.Connectors.DataContracts;
using Fixit.Core.Connectors.Mappers;
using Fixit.Core.DataContracts.Users.Account;
using Microsoft.Graph;
// Resolution to:  OperationStatus' is an ambiguous reference between 'Fixit.Core.DataContracts.OperationStatus' and 'Microsoft.Graph.OperationStatus'
using OperationStatus = Fixit.Core.DataContracts.OperationStatus;

[assembly: InternalsVisibleTo("Fixit.Core.Connectors.UnitTests")]
namespace Fixit.Core.Connectors.Mediators.MicrosoftGraph.Internal
{
  internal class MicrosoftGraphMediator : IMicrosoftGraphMediator
  {
    private IGraphServiceClientAdapter _graphServiceClientAdapter;
    private IMapper _mapper;

    public MicrosoftGraphMediator(IGraphServiceClientAdapter graphServiceClientAdapter)
    {
      _graphServiceClientAdapter = graphServiceClientAdapter ?? throw new ArgumentNullException($"{nameof(MicrosoftGraphMediator)} expects a value for {nameof(graphServiceClientAdapter)}... null argument was provided");
      _mapper = BaseMapper.getMapper();
    }

    public async Task<OperationStatus> DeleteAccountAsync(string userId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus resultStatus = new OperationStatus();

      if (string.IsNullOrWhiteSpace(userId))
      {
        throw new ArgumentNullException($"{nameof(MicrosoftGraphMediator)} expects a value for {nameof(userId)}... null argument was provided");
      }

      try
      {
        await _graphServiceClientAdapter.DeleteUserAsync(userId, cancellationToken);
        resultStatus.IsOperationSuccessful = true;
      }
      catch (Exception exception)
      {

        resultStatus.OperationException = exception;
        resultStatus.IsOperationSuccessful = false;
      }

      return resultStatus;
    }

    public async Task<ConnectorDto<UserAccountStateDto>> UpdateAccountSignInStatusAsync(string userId, bool blockSignin, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      ConnectorDto<UserAccountStateDto> result = new ConnectorDto<UserAccountStateDto>() { IsOperationSuccessful = true };

      if (string.IsNullOrWhiteSpace(userId))
      {
        throw new ArgumentNullException($"{nameof(MicrosoftGraphMediator)} expects a value for {nameof(userId)}... null argument was provided");
      }

      try
      {
        await _graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userId, blockSignin, cancellationToken);
        var user = await _graphServiceClientAdapter.GetUserAsync(userId, cancellationToken);
        result.Result = _mapper.Map<User, UserAccountStateDto>(user);
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }



    public async Task<OperationStatus> UpdateAccountPasswordAsync(string userId, string newPassword, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus resultStatus = new OperationStatus();

      if (string.IsNullOrWhiteSpace(userId))
      {
        throw new ArgumentNullException($"{nameof(MicrosoftGraphMediator)} expects a value for {nameof(userId)}... null argument was provided");
      }
      
      if (string.IsNullOrWhiteSpace(newPassword))
      {
        throw new ArgumentNullException($"{nameof(MicrosoftGraphMediator)} expects a value for {nameof(newPassword)}... null argument was provided");
      }

      try
      {
        await _graphServiceClientAdapter.UpdateUserPasswordAsync(userId, newPassword, cancellationToken);
        resultStatus.IsOperationSuccessful = true;
      }
      catch (Exception exception)
      {

        resultStatus.OperationException = exception;
        resultStatus.IsOperationSuccessful = false;
      }

      return resultStatus;
    }
  }
}
