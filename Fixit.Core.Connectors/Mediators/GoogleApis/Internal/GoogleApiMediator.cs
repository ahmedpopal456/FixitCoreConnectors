using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AutoMapper;
using Fixit.Core.Connectors.Mappers;
using Fixit.Core.DataContracts.Users.Address.Query;
using System.Collections.Generic;
using Fixit.Core.DataContracts;
using static GoogleApi.GooglePlaces;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Details.Response;
using System.Linq;
using GoogleApi.Entities.Places.Common;
using Fixit.Core.DataContracts.Users.Address;
using Fixit.Core.Connectors.Mediators.GoogleApis;

[assembly: InternalsVisibleTo("Fixit.Core.Connectors.UnitTests")]
namespace Fixit.Core.Connectors.Mediators.MicrosoftGraph.Internal
{
  internal class GoogleApiMediator : IGoogleApiMediator
  {
    private IMapper _mapper;
    private readonly string _googleApiKey; 

    public GoogleApiMediator(string key)
    {
      _googleApiKey = string.IsNullOrWhiteSpace(key) ? throw new ArgumentNullException($"{nameof(GoogleApiMediator)} expects a value for {nameof(key)}... null argument was provided") : key;
      _mapper = BaseMapper.getMapper();
    }

    public async Task<OperationStatusWithObject<IEnumerable<AddressQueryItem>>> GetAddressesBySearchAsync(string search, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      if (string.IsNullOrWhiteSpace(search))
      {
        throw new ArgumentNullException($"{nameof(GoogleApiMediator)} expects a value for {nameof(search)}... null argument was provided");
      }

      var result = new OperationStatusWithObject<IEnumerable<AddressQueryItem>>();
      try
      {
        var request = new PlacesAutoCompleteRequest()
        {
          Key = _googleApiKey,
          Input = search
        };

        var googleGetPlacesResponse = await AutoComplete.QueryAsync(request);
        if(googleGetPlacesResponse.Status == GoogleApi.Entities.Common.Enums.Status.Ok)
        {
          result.Result = googleGetPlacesResponse.Predictions.Select(prediction => _mapper.Map<Prediction, AddressQueryItem>(prediction));
          result.IsOperationSuccessful = true;
        }
      }
      catch (Exception exception)
      {

        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }

    public async Task<OperationStatusWithObject<AddressDto>> GetAddressByIdAsync(string addressId, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      if (string.IsNullOrWhiteSpace(addressId))
      {
        throw new ArgumentNullException($"{nameof(GoogleApiMediator)} expects a value for {nameof(addressId)}... null argument was provided");
      }

      var result = new OperationStatusWithObject<AddressDto>();
      try
      {
        var placesDetailsRequest = new PlacesDetailsRequest()
        {
          PlaceId = addressId,
          Key = _googleApiKey
        };

        var googleGetPlaceResponse = await Details.QueryAsync(placesDetailsRequest);
        if (googleGetPlaceResponse.Status == GoogleApi.Entities.Common.Enums.Status.Ok)
        {
          result.Result = _mapper.Map<DetailsResult, AddressDto>(googleGetPlaceResponse.Result);
          result.IsOperationSuccessful = true;
        }
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
//var key = "AIzaSyArcXMWWZs7eZjRWtBJYs_tj_OliYk8wrw";
//var request = new PlacesAutoCompleteRequest()
//{
//  Key = key,
//  Language = GoogleApi.Entities.Common.Enums.Language.English,
//  Input = "5125 Avenue Colomb"
//};


//var results = AutoComplete.Query(request);

//if (results.Status == GoogleApi.Entities.Common.Enums.Status.Ok)
//{
//  var enumerator = results.Predictions.GetEnumerator();

//  while (enumerator.MoveNext())
//  {
//    var currentPrediction = enumerator.Current;

//    var placesDetailsRequest = new PlacesDetailsRequest()
//    {
//      PlaceId = currentPrediction.PlaceId,
//      Key = key
//    };

//    PlacesDetailsResponse detail = Details.Query(placesDetailsRequest);


