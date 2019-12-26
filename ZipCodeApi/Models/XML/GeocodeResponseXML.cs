using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZipCodeApi.Models.XML
{
    [Serializable]
    [XmlRoot("GeocodeResponse")]
    public class GeocodeResponse
    {
        public string status { get; set; }
        public string pcid { get; set; }

        [XmlElement("result")]
        public List<result> result { get; set; }
    }

    public class result
    {
        [XmlElement("type")]
        public List<string> type { get; set; }

        [XmlElement("formatted_address")]
        public string formatted_address { get; set; }

        [XmlElement("address_component")]
        public List<address_component> address_component { get; set; }
    }

    public class address_component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }

        [XmlElement("type")]
        public List<string> type { get; set; }
    }
}