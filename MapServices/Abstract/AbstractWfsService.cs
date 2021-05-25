using System.Collections.Generic;
using System.Net;
using WebApp.MapServices.Request;
using WebApp.MapServices.Response;

namespace WebApp.MapServices.Abstract
{
    public abstract class AbstractWfsService : IWfs
    {
        protected NetworkCredential Credentials { get; }
        protected IGeoProxy GeoProxy { get; }
        protected IWfsResponseParser ResponseParser { get; }

        protected AbstractWfsService(IWfsResponseParser wfsResponseParser, IGeoProxy geoProxy/*, NetworkCredential credentials = null*/)
        {
            GeoProxy = geoProxy;
            ResponseParser = wfsResponseParser;
        }

        public abstract WfsCapabilities[] GetCapabilities(string url, string layerLink);

        /// <inheritdoc cref="IWfs.GetFeature(string,string,string[],BoundingBox,string,string)"/>
        public virtual GetFeatureResponseItem[] GetFeature(string serverUrl, string layerLink, IEnumerable<string> typeNames, 
            BoundingBox bBox, string srs, string geoAttributeName = "")
        {
            var request = CreateSearchByCoordRequest(serverUrl, layerLink, typeNames, bBox, srs, geoAttributeName);
            var response = GeoProxy.ProxyRequest(request);
            var result = ResponseParser.ParseFeatures(response);
            return result;
        }

        protected virtual IGeoRequest CreateSearchByCoordRequest(string serverUrl, string layerLink, IEnumerable<string> typeNames, 
            BoundingBox bBox, string srs, string geoAttributeName)
        {
            var request = CreateBaseRequest(serverUrl)
                .SetVersion("1.1.0")
                .SetRequest("GetFeature")
                .SetParameter("typeName", string.Join(",", typeNames))
                .SetParameter("SRSNAME", srs)

                // TODO: все это актуально только для GeoServer'а. 
                .SetParameter("CQL_FILTER", $"INTERSECTS({geoAttributeName},setCRS({bBox.ToWkt()},'{bBox.Srs}'))");

            return request;
        }

        public virtual DescribeFeatureInfo DescribeFeatureType(string serverUrl, IEnumerable<string> layerNames)
        {
            var request = CreateDescribeFeatureTypeRequest(serverUrl, string.Join(",", layerNames));
            var response = GeoProxy.ProxyRequest(request);
            var result = ResponseParser.ParseDescribeFeature(response);
            return result;
        }

        protected QueryStringGeoRequest CreateBaseRequest(string serverUrl)
        {
            return new QueryStringGeoRequest(serverUrl, RequestTypes.Get, Credentials)
                .SetWfs();
        }

        protected virtual QueryStringGeoRequest CreateDescribeFeatureTypeRequest(string serverUrl, string layerName)
        {
            return CreateBaseRequest(serverUrl)
                .SetRequest("DescribeFeatureType");
        }
    }
}
