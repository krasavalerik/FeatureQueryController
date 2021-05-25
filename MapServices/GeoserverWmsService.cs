using WebApp.MapServices.Abstract;
using WebApp.MapServices.Response;

namespace WebApp.MapServices
{
    public class GeoserverWmsService : IWms
    {
        protected IGeoProxy GeoProxy { get; }
        protected IWmsResponseParser ResponseParser { get; }

        public GeoserverWmsService(IWmsResponseParser wmsResponseParser, IGeoProxy geoProxy/*, NetworkCredential credentials = null*/)
        {
            GeoProxy = geoProxy;
            ResponseParser = wmsResponseParser;
            //Credentials = credentials;
        }
    }
}
