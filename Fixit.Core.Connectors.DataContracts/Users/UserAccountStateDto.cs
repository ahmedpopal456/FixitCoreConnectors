using System.Runtime.Serialization;
using Fixit.Core.Connectors.DataContracts.Users.Enums;

namespace Fixit.Core.Connectors.DataContracts.Users
{
  [DataContract, KnownType(typeof(OperationStatus))]
  public class UserAccountStateDto : OperationStatus
  {
    [DataMember]
    public UserState State { get; set; }
  }
}
