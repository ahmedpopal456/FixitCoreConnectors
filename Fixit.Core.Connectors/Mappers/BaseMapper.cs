using AutoMapper;

namespace Fixit.Core.Connectors.Mappers
{
  public class BaseMapper
  {
    private static IMapper _mapper;

    private BaseMapper()
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
        cfg.AddProfile<ConnectorMapper>();
      });

      _mapper = config.CreateMapper();
    }

    public static IMapper getMapper()
    {
      if (_mapper == null)
      {
        new BaseMapper();
      }

      return _mapper;
    }
  }
}
