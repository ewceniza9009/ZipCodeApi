using System.Collections.Generic;

namespace ZipCodeApi.Models.JSON
{
    public class GeocodeResponse
    {
        public List<result> results { get; set; }
        public string status { get; set; }
    }

    public class result
    {
        public List<address_component> address_component { get; set; }
    }

    public class address_component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }
}