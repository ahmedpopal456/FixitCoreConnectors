using System;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.Connectors.Adapters;
using Fixit.Core.Connectors.Mediators;
using Fixit.Core.Connectors.Mediators.MicrosoftGraph.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;

using OperationStatus = Fixit.Core.DataContracts.OperationStatus;
using Fixit.Core.Connectors.DataContracts;
using Fixit.Core.DataContracts.Users.Account;

namespace Fixit.Core.Connectors.UnitTests.Mediators.MicrosoftGraph
{

  [TestClass]
  public class MicrosoftGraphConnectorMediatorTests : TestBase
  {

    private IMicrosoftGraphMediator _microsoftGraphMediator;
    private IEnumerable<User> _fakeUsers;

    [TestInitialize]
    public void TestInitialize()
    {
      _graphServiceClientAdapter = new Mock<IGraphServiceClientAdapter>();
      _microsoftGraphMediator = new MicrosoftGraphMediator(_graphServiceClientAdapter.Object);

      // Create Seeders
      var fakeUserSeeder = fakeDtoSeederFactory.CreateFakeSeeder<User>();

      // Create fake Data objects
      _fakeUsers = fakeUserSeeder.SeedFakeDtos();

    }

    [TestMethod]
    [DataRow(null, false, DisplayName = "Null_UserPrincipalName")]
    public async Task UpdateAccountSignInStatusAsync_UserPrincipalNameNullOrWhiteSpace_ThrowsArgumentNullException(string userPrincipalName, bool blockSignIn)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignIn, cancellationToken));
    }

    [TestMethod]
    [DataRow("something@somewhere.com", false, DisplayName = "Any_UserPrincipalName")]
    public async Task UpdateAccountSignInStatusAsync_UpdateAccountSignInStatusAsyncSuccess_ReturnsSuccess(string userPrincipalName, bool blockSignIn)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignIn, cancellationToken))
                           .Returns(Task.CompletedTask);

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.GetUserAsync(userPrincipalName, cancellationToken))
                           .ReturnsAsync(fakeUser);

      // Act
      ConnectorDto<UserAccountStateDto> actionResult = await _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignIn, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.Result);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("fake@email.com", true, DisplayName = "NonExisting_UserPrincipalName")]
    public async Task UpdateAccountSignInStatusAsync_UpdateAccountSignInStatusAsyncException_ReturnsOperationExceptio(string userPrincipalName, bool blockSignIn)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignIn, cancellationToken))
                           .Throws(new Exception());

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.GetUserAsync(userPrincipalName, cancellationToken))
                           .ReturnsAsync(fakeUser);

      // Act
      ConnectorDto<UserAccountStateDto> actionResult = await _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignIn, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNull(actionResult.Result);
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("fake@email.com", true, DisplayName = "NonExisting_UserPrincipalName")]
    public async Task UpdateAccountSignInStatusAsync_GetUserAsyncException_ReturnsOperationExceptio(string userPrincipalName, bool blockSignIn)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignIn, cancellationToken))
                           .Returns(Task.CompletedTask);

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.GetUserAsync(userPrincipalName, cancellationToken))
                           .Throws(new Exception());


      // Act
      ConnectorDto<UserAccountStateDto> actionResult = await _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userPrincipalName, blockSignIn, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNull(actionResult.Result);
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow(null, DisplayName = "Any_UserPrincipalName")]
    public async Task DeleteUserAsync_UserPrincipalNameNullOrWhiteSpace_ThrowsArgumentNullException(string userPrincipalName)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _microsoftGraphMediator.DeleteAccountAsync(userPrincipalName, cancellationToken));
    }

    [TestMethod]
    [DataRow("something@somewhere.com", DisplayName = "Any_UserPrincipalName")]
    public async Task DeleteUserAsync_DeleteUserAsyncSuccess_ReturnsSuccess(string userPrincipalName)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.DeleteUserAsync(userPrincipalName, cancellationToken))
                           .Returns(Task.CompletedTask);

      // Act
      OperationStatus actionResult = await _microsoftGraphMediator.DeleteAccountAsync(userPrincipalName, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("fake@email.com", DisplayName = "NonExisting_UserPrincipalName")]
    public async Task DeleteUserAsync_DeleteUserAsyncException_ReturnsOperationException(string userPrincipalName)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.DeleteUserAsync(userPrincipalName, cancellationToken))
                           .Returns(Task.FromException(new Exception()));

      // Act
      OperationStatus actionResult = await _microsoftGraphMediator.DeleteAccountAsync(userPrincipalName, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestCleanup]
    public void TestCleanup()
    {
      // Clean-up mock objects
      _graphServiceClientAdapter.Reset();

      // Clean-up data objects
      _fakeUsers = null;
    }

  }

}
