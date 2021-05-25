namespace WebApp.MapServices.Request
{
    public static class GeoRequestExtensions
    {
        public static QueryStringGeoRequest SetParameter(this QueryStringGeoRequest request, string parameterName, string parameterValue)
        {
            request[parameterName] = parameterValue;
            return request;
        }

        public static QueryStringGeoRequest SetVersion(this QueryStringGeoRequest request, string version)
        {
            return request.SetParameter(GeoRequestConstants.VERSION_PARAMETER_NAME, version);
        }

        public static QueryStringGeoRequest SetDefaultWmsVersion(this QueryStringGeoRequest request)
        {
            return request.SetVersion(GeoRequestConstants.DEFAULT_WMS_VERSION);
        }

        public static QueryStringGeoRequest SetDefaultWfsVersion(this QueryStringGeoRequest request)
        {
            return request.SetVersion(GeoRequestConstants.DEFAULT_WFS_VERSION);
        }

        public static QueryStringGeoRequest SetService(this QueryStringGeoRequest request, string service)
        {
            return request.SetParameter(GeoRequestConstants.SERVICE_PARAMETER_NAME, service);
        }

        public static QueryStringGeoRequest SetWms(this QueryStringGeoRequest request)
        {
            return request.SetService(GeoRequestConstants.WMS_NAME)
                .SetDefaultWfsVersion();
        }

        public static QueryStringGeoRequest SetWfs(this QueryStringGeoRequest request)
        {
            return request.SetService(GeoRequestConstants.WFS_NAME)
                .SetDefaultWfsVersion();
        }

        public static QueryStringGeoRequest SetRequest(this QueryStringGeoRequest request, string requestName)
        {
            return request.SetParameter(GeoRequestConstants.REQUEST_PARAMETER_NAME, requestName);
        }

        public static QueryStringGeoRequest SetWmts(this QueryStringGeoRequest request)
        {
            return request.SetService(GeoRequestConstants.WMTS_NAME)
                .SetDefaultWmsVersion();
        }
    }
}
