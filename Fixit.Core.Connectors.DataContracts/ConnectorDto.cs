using System.Runtime.Serialization;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Connectors.DataContracts
{
  [DataContract]
  public class ConnectorDto<T> : OperationStatus
  {
    [DataMember]
    public T Result { get; set; }
  }
}
