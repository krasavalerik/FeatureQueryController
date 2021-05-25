using System.Threading.Tasks;
using System.Threading;
using WebApp.MapServices.Request;

namespace WebApp.MapServices.Response
{
    public interface IGeoProxy
    {
        GeoResponse ProxyRequest(IGeoRequest request);

        //Task<GeoResponse> ProxyRequestAsync(IGeoRequest request);

        //Task<GeoResponse> ProxyRequestAsync(IGeoRequest request, CancellationToken token);
    }
}
