﻿using System.Collections.Generic;
using Fixit.Core.DataContracts.Seeders;
using Microsoft.Graph;

namespace Fixit.Core.Connectors.UnitTests.Adapters
{
  /// <summary>
  /// A seeder for the <see cref="User"/> model from Microsoft.Graph
  /// </summary>
  public class FakeMSGraphUserSeeder : IFakeSeederAdapter<User>
  {
    public IList<User> SeedFakeDtos()
    {
      var firstUser = new User()
      {
        Id = "someid",
        UserPrincipalName = "something@somewhere.com",
        GivenName = "Jane",
        Surname = "Doe",
        AccountEnabled = true,
      };

      var secondUser = new User()
      {
        Id = "someid",
        UserPrincipalName = "somethingelse@somewhere.com",
        GivenName = "John",
        Surname = "Smith",
        AccountEnabled = false,
      };

      return new List<User>
      {
        firstUser,
        secondUser
      };
    }
  }
}
