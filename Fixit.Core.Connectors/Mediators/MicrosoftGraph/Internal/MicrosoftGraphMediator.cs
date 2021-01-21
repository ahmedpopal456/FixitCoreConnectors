using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AutoMapper;
using Fixit.Core.Connectors.Adapters;
using Fixit.Core.Connectors.DataContracts;
using Fixit.Core.Connectors.Mappers;
using Microsoft.Graph;
using OperationStatus = Fixit.Core.Connectors.DataContracts.OperationStatus;
using Fixit.Core.Connectors.DataContracts.Users;

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

    public async Task<OperationStatus> DeleteAccountAsync(string userPrincipalName, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus resultStatus = new OperationStatus();

      if (string.IsNullOrWhiteSpace(userPrincipalName))
      {
        throw new ArgumentNullException($"{nameof(MicrosoftGraphMediator)} expects a value for {nameof(userPrincipalName)}... null argument was provided");
      }

      try
      {
        await _graphServiceClientAdapter.DeleteUserAsync(userPrincipalName, cancellationToken);
        resultStatus.IsOperationSuccessful = true;
      }
      catch (Exception exception)
      {

        resultStatus.OperationException = exception;
        resultStatus.IsOperationSuccessful = false;
      }

      return resultStatus;
    }

    public async Task<ConnectorDto<UserAccountStateDto>> UpdateAccountSignInStatusAsync(string userPrincipalName, bool blockSignin, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      ConnectorDto<UserAccountStateDto> result = new ConnectorDto<UserAccountStateDto>() { IsOperationSuccessful = true };

      if (string.IsNullOrWhiteSpace(userPrincipalName))
      {
        throw new ArgumentNullException($"{nameof(MicrosoftGraphMediator)} expects a value for {nameof(userPrincipalName)}... null argument was provided");
      }

      try
      {
        await _graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignin, cancellationToken);
        var user = await _graphServiceClientAdapter.GetUserAsync(userPrincipalName, cancellationToken);
        result.Result = _mapper.Map<User, UserAccountStateDto>(user);
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }
  }
}
