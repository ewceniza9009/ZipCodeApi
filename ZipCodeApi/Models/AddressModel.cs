namespace ZipCodeApi.Models
{
    public class AddressModel
    {
        public string postal_code { get; set; }
        public string locality { get; set; }
        public string administrative_area_level_1 { get; set; }
        public string administrative_area_level_2 { get; set; }
        public string abbreviation { get; set; }
        public string country { get; set; }
    }
}