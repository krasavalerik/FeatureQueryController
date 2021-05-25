using System.Collections.Generic;

namespace WebApp.MapServices
{
    public class WfsCapabilities
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string DisplayFieldName { get; set; }

        public string GeometryFieldName { get; set; }

        public List<string> HiddenFields { get; set; }
    }
}
