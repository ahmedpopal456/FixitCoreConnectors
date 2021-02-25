using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.Connectors.Adapters;
using Fixit.Core.Connectors.Mediators;
using Fixit.Core.Connectors.Mediators.MicrosoftGraph.Internal;
using Fixit.Core.Connectors.DataContracts;
using Fixit.Core.DataContracts.Users.Account;
using Fixit.Core.Connectors.UnitTests.Adapters;
using Microsoft.Graph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using OperationStatus = Fixit.Core.DataContracts.OperationStatus;

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

      // Create fake Data objects
      _fakeUsers = _fakeDtoSeederFactory.CreateSeederFactory(new FakeMSGraphUserSeeder());
    }

    [TestMethod]
    [DataRow(null, false, DisplayName = "Null_UserId")]
    public async Task UpdateAccountSignInStatusAsync_UserIdNullOrWhiteSpace_ThrowsArgumentNullException(string userId, bool blockSignIn)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userId, blockSignIn, cancellationToken));
    }

    [TestMethod]
    [DataRow("something@somewhere.com", false, DisplayName = "Any_UserId")]
    public async Task UpdateAccountSignInStatusAsync_UpdateAccountSignInStatusAsyncSuccess_ReturnsSuccess(string userId, bool blockSignIn)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userId, blockSignIn, cancellationToken))
                           .Returns(Task.CompletedTask);

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.GetUserAsync(userId, cancellationToken))
                           .ReturnsAsync(fakeUser);

      // Act
      ConnectorDto<UserAccountStateDto> actionResult = await _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userId, blockSignIn, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.Result);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("fake@email.com", true, DisplayName = "NonExisting_UserId")]
    public async Task UpdateAccountSignInStatusAsync_UpdateAccountSignInStatusAsyncException_ReturnsOperationExceptio(string userId, bool blockSignIn)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userId, blockSignIn, cancellationToken))
                           .Throws(new Exception());

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.GetUserAsync(userId, cancellationToken))
                           .ReturnsAsync(fakeUser);

      // Act
      ConnectorDto<UserAccountStateDto> actionResult = await _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userId, blockSignIn, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNull(actionResult.Result);
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("fake@email.com", true, DisplayName = "NonExisting_UserId")]
    public async Task UpdateAccountSignInStatusAsync_GetUserAsyncException_ReturnsOperationExceptio(string userId, bool blockSignIn)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateAccountSignInStatusAsync(userId, blockSignIn, cancellationToken))
                           .Returns(Task.CompletedTask);

      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.GetUserAsync(userId, cancellationToken))
                           .Throws(new Exception());


      // Act
      ConnectorDto<UserAccountStateDto> actionResult = await _microsoftGraphMediator.UpdateAccountSignInStatusAsync(userId, blockSignIn, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNull(actionResult.Result);
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow(null, DisplayName = "Any_UserId")]
    public async Task DeleteUserAsync_UserIdNullOrWhiteSpace_ThrowsArgumentNullException(string userId)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _microsoftGraphMediator.DeleteAccountAsync(userId, cancellationToken));
    }

    [TestMethod]
    [DataRow("something@somewhere.com", DisplayName = "Any_UserId")]
    public async Task DeleteUserAsync_DeleteUserAsyncSuccess_ReturnsSuccess(string userId)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.DeleteUserAsync(userId, cancellationToken))
                           .Returns(Task.CompletedTask);

      // Act
      OperationStatus actionResult = await _microsoftGraphMediator.DeleteAccountAsync(userId, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("fake@email.com", DisplayName = "NonExisting_UserId")]
    public async Task DeleteUserAsync_DeleteUserAsyncException_ReturnsOperationException(string userId)
    {
      // Arrange
      var fakeUser = _fakeUsers.First();
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.DeleteUserAsync(userId, cancellationToken))
                           .Returns(Task.FromException(new Exception()));

      // Act
      OperationStatus actionResult = await _microsoftGraphMediator.DeleteAccountAsync(userId, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNotNull(actionResult.OperationException);
      Assert.IsFalse(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow(null, "Fakepwd4291", DisplayName = "Any_UserIdAndNewPassword")]
    public async Task UpdateAccountPasswordAsync_UserIdNullOrWhiteSpace_ThrowsArgumentNullException(string userId, string newPassword)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _microsoftGraphMediator.UpdateAccountPasswordAsync(userId, newPassword, cancellationToken));
    }

    [TestMethod]
    [DataRow("fake@email.com", null, DisplayName = "Any_UserIdAndNewPassword")]
    public async Task UpdateAccountPasswordAsync_NewPasswordNullOrWhiteSpace_ThrowsArgumentNullException(string userId, string newPassword)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;

      // Act
      // Assert
      await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _microsoftGraphMediator.UpdateAccountPasswordAsync(userId, newPassword, cancellationToken));
    }

    [TestMethod]
    [DataRow("fake@email.com", "Fakepwd4291", DisplayName = "Any_UserIdAndNewPassword")]
    public async Task UpdateAccountPasswordAsync_UpdateAccountPasswordAsyncSuccess_ReturnsSuccess(string userId, string newPassword)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateUserPasswordAsync(userId, newPassword, cancellationToken))
                           .Returns(Task.CompletedTask);

      // Act
      OperationStatus actionResult = await _microsoftGraphMediator.UpdateAccountPasswordAsync(userId, newPassword, cancellationToken);

      // Assert
      Assert.IsNotNull(actionResult);
      Assert.IsNull(actionResult.OperationException);
      Assert.IsTrue(actionResult.IsOperationSuccessful);
    }

    [TestMethod]
    [DataRow("fake@email.com", "Fakepwd4291", DisplayName = "Any_UserIdAndNewPassword")]
    public async Task UpdateAccountPasswordAsync_UpdateAccountPasswordAsyncException_ReturnsOperationException(string userId, string newPassword)
    {
      // Arrange
      CancellationToken cancellationToken = CancellationToken.None;
      _graphServiceClientAdapter.Setup(graphServiceClientAdapter => graphServiceClientAdapter.UpdateUserPasswordAsync(userId, newPassword, cancellationToken))
                           .Returns(Task.FromException(new Exception()));

      // Act
      OperationStatus actionResult = await _microsoftGraphMediator.UpdateAccountPasswordAsync(userId, newPassword, cancellationToken);

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
