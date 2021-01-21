using System.Runtime.Serialization;

namespace Fixit.Core.Connectors.DataContracts
{
  [DataContract]
  public class ConnectorDto<T> : OperationStatus
  {
    [DataMember]
    public T Result { get; set; }
  }
}
