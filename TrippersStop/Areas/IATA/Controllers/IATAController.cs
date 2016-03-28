using System.Web.Http;

namespace TrippismApi.Areas.IATA.Controllers
{
    /// <summary>
    /// API to retrieve data for city pairs.
    /// </summary>
     
    public class IATAController : ApiController
    {
        //IDBService _dbService;
        ///// <summary>
        ///// Set db service.
        ///// </summary>
        //public IATAController(IDBService dbService)
        //{
        //    _dbService = dbService;
        //}

        //[Route("api/iata/airports")]
        ///// <summary>
        ///// Get available city pairs.
        ///// </summary>
        //[ResponseType(typeof(List<IATACode>))]
        //public HttpResponseMessage Get()
        //{
        //    return GetResponse();
        //}
        /// <summary>
        /// Get response from IATA service..
        /// </summary>
        //private HttpResponseMessage GetResponse()
        //{
        //    List<IATACode> AirPortCodes;
        //    MongoDatabase database = MongoDBProperty.MongoDB;

        //    var collection = _dbService.Get<IATACode>("IATA");
        //    AirPortCodes = collection.ToList();

        //    return Request.CreateResponse(HttpStatusCode.OK, AirPortCodes);
        //}
    }
}
