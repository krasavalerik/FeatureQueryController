using WebApp.MapServices.Abstract;
using WebApp.MapServices.Request;
using WebApp.MapServices.Response;

namespace WebApp.MapServices
{
    public class GeoserverWfsService : AbstractWfsService
    {
        public GeoserverWfsService(IWfsResponseParser responseParser, IGeoProxy geoProxy/*, NetworkCredential credentials = null*/) : base(responseParser, geoProxy/*, credentials*/)
        {
        }

        public override WfsCapabilities[] GetCapabilities(string url, string layerLink)
        {
            var request = new QueryStringGeoRequest(url, RequestTypes.Get)
                .SetWfs()
                .SetRequest("GetCapabilities")
                .SetParameter("namespace", layerLink)
                .SetParameter("outputFormat", "GML2")
                .SetVersion("2.0.0");

            var response = GeoProxy.ProxyRequest(request); //TODO: реализовать ProxyRequest
            var layers = ResponseParser.ParseCapabilities(response);
            return layers;
        }
    }
}
