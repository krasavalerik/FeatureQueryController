using System.Collections.Generic;
using WebApp.MapServices.Request;
using WebApp.MapServices.Response;

namespace WebApp.MapServices
{
    public class GeoServerClearWfsService : GeoserverWfsService
    {
        public GeoServerClearWfsService(IWfsResponseParser responseParser, IGeoProxy geoProxy/*,
            NetworkCredential credentials = null*/) : base(responseParser, geoProxy/*, credentials*/)
        {
        }

        public override GetFeatureResponseItem[] GetFeature(string serverUrl, string layerLink, IEnumerable<string> typeNames, 
            BoundingBox bBox, string srs, string geoAttributeName = "")
        {
            var request = CreateSearchByCoordRequest(serverUrl + "/wfs", layerLink, typeNames, bBox, srs, geoAttributeName);
            var response = GeoProxy.ProxyRequest(request);
            var result = ResponseParser.ParseFeatures(response);
            return result;
        }

        public override WfsCapabilities[] GetCapabilities(string url, string layerLink)
        {
            var request = new QueryStringGeoRequest(url + "/wfs", RequestTypes.Get)
                .SetWfs()
                .SetRequest("GetCapabilities")
                .SetParameter("outputFormat", "GML2")
                .SetVersion("2.0.0");

            var response = GeoProxy.ProxyRequest(request);
            var layers = ResponseParser.ParseCapabilities(response);
            return layers;
        }
    }
}
