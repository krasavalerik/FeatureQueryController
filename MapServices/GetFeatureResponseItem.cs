using System.Collections.Generic;

namespace WebApp.MapServices
{
    public class GetFeatureResponseItem
    {
        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        //public string Geometry { get; set; }

        public string Fid { get; set; }

        public string TypeName { get; set; }
    }
}
