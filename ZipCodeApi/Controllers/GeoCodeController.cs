using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ZipCodeApi.Controllers
{
    [Attributes.APIAuthorize]
    [RoutePrefix("api")]
    public class GeocodeController : Controller
    {
        //private Data.ZipCodeDataContext zipData;

        // GET: geocode string format
        [Route("geocode/jsonstr/address/{address}/{token}")]
        public ContentResult GetAddressJSONString(string address)
        {
            var dbPath = Server.MapPath("~/Data/ZipCode.xml");
            var zipDB = new DataSet();

            zipDB.ReadXml(dbPath);

            var zipTbl = zipDB.Tables["ZipCode"]
                .Select("PostalCode=" + address).AsEnumerable();

            //zipData = new Data.ZipCodeDataContext();

            //IQueryable<Models.AddressModel> addressData = zipData
            //    .ZipCodes
            //    .Where(x => x.PostalCode == address)
            //    .Select(y => new Models.AddressModel()
            //    {
            //        postal_code = y.PostalCode,
            //        locality = y.City,
            //        administrative_area_level_1 = y.State,
            //        administrative_area_level_2 = "NA",
            //        country = y.Country,
            //        abbreviation = y.Abbreviation
            //    });

            var addressData = new List<Models.AddressModel>();

            foreach (var row in zipTbl)
            {
                addressData.Add(new Models.AddressModel
                {
                    postal_code = row.Field<string>("PostalCode"),
                    locality = row.Field<string>("City"),
                    administrative_area_level_1 = row.Field<string>("State"),
                    administrative_area_level_2 = "NA",
                    country = row.Field<string>("Country"),
                    abbreviation = row.Field<string>("Abbreviation")
                });
            }

            string result = "";

            string newLine = "\n";
            string oneTab = "  ";
            string twoTab = oneTab + oneTab;
            string threeTab = twoTab + oneTab;
            string fourTab = twoTab + twoTab;
            string fiveTab = fourTab + oneTab;
            string openCB = "{";
            string closeCB = "}";
            string openSB = "[";
            string closeSB = "]";

            result += openCB + newLine + oneTab;
            result += "\"results\" : " + openSB + newLine + twoTab;

            int ctr = 0;

            foreach (Models.AddressModel addressLine in addressData)
            {
                result += openCB + newLine + threeTab;
                result += "\"address_components\" : " + openSB + newLine + fourTab;

                result += openCB + newLine + fiveTab;

                result += "\"long_name\" :  \"" + addressLine.postal_code + "\", " + newLine + fiveTab;
                result += "\"short_name\" :  \"" + addressLine.postal_code + "\", " + newLine + fiveTab;
                result += "\"types\" :  [\"postal_code\"]" + newLine + fourTab;

                result += closeCB + "," + newLine + fourTab;

                result += openCB + newLine + fiveTab;

                result += "\"long_name\" :  \"" + addressLine.locality + "\", " + newLine + fiveTab;
                result += "\"short_name\" :  \"" + addressLine.locality + "\", " + newLine + fiveTab;
                result += "\"types\" :  [\"locality\", \"political\"]" + newLine + fourTab;

                result += closeCB + "," + newLine + fourTab;

                result += openCB + newLine + fiveTab;

                result += "\"long_name\" :  \"" + addressLine.administrative_area_level_1 + "\", " + newLine + fiveTab;
                result += "\"short_name\" :  \"" + addressLine.abbreviation + "\", " + newLine + fiveTab;
                result += "\"types\" :  [\"administrative_area_level_1\", \"political\"]" + newLine + fourTab;

                result += closeCB + "," + newLine + fourTab;

                result += openCB + newLine + fiveTab;

                result += "\"long_name\" :  \"" + addressLine.administrative_area_level_2 + "\", " + newLine + fiveTab;
                result += "\"short_name\" :  \"" + addressLine.administrative_area_level_2 + "\", " + newLine + fiveTab;
                result += "\"types\" :  [\"administrative_area_level_2\", \"political\"]" + newLine + fourTab;

                result += closeCB + "," + newLine + fourTab;

                result += openCB + newLine + fiveTab;

                result += "\"long_name\" :  \"" + addressLine.country + "\", " + newLine + fiveTab;
                result += "\"short_name\" :  \"" + addressLine.country + "\", " + newLine + fiveTab;
                result += "\"types\" :  [\"country\", \"political\"]" + newLine + fourTab;

                result += closeCB + newLine + threeTab;

                result += closeSB + newLine + twoTab;
                result += closeCB;

                if (addressData.Count() > 1 && ctr < addressData.Count() - 1)
                {
                    result += "," + newLine + twoTab;
                }

                ctr++;
            }

            result += newLine + oneTab + closeSB + ",";
            result += newLine + oneTab + "\"status\" : \"OK\"";
            result += newLine + closeCB;

            return Content(result, "application/json");
        }

        [Route("geocode/json/address/{address}/{token}")]
        public JsonResult GetAddressJSON(string address)
        {
            var dbPath = Server.MapPath("~/Data/ZipCode.xml");
            var zipDB = new DataSet();

            zipDB.ReadXml(dbPath);

            var zipTbl = zipDB.Tables["ZipCode"]
                .Select("PostalCode=" + address).AsEnumerable();

            Models.JSON.GeocodeResponse response = new Models.JSON.GeocodeResponse();
            List<Models.JSON.result> results = new List<Models.JSON.result>();

            try
            {
                var addressData = new List<Models.AddressModel>();

                foreach (var row in zipTbl)
                {
                    addressData.Add(new Models.AddressModel
                    {
                        postal_code = row.Field<string>("PostalCode"),
                        locality = row.Field<string>("City"),
                        administrative_area_level_1 = row.Field<string>("State"),
                        administrative_area_level_2 = "NA",
                        country = row.Field<string>("Country"),
                        abbreviation = row.Field<string>("Abbreviation")
                    });
                }

                foreach (Models.AddressModel add in addressData)
                {
                    results.Add(new Models.JSON.result()
                    {
                        address_component = new List<Models.JSON.address_component>()
                        {
                            new Models.JSON.address_component {
                                long_name = add.postal_code,
                                short_name = add.postal_code,

                                types = new List<string> {"postal_code"}
                            },
                            new Models.JSON.address_component {
                                long_name = add.locality,
                                short_name = add.locality,

                                types = new List<string> {"locality", "political"}
                            },
                            new Models.JSON.address_component {
                                long_name = add.administrative_area_level_1,
                                short_name = add.abbreviation,

                                types = new List<string> {"administrative_area_level_1", "political"}
                            },
                            new Models.JSON.address_component {
                                long_name = add.administrative_area_level_2,
                                short_name = add.administrative_area_level_2,

                                types = new List<string> {"administrative_area_level_2", "political"}
                            },
                            new Models.JSON.address_component {
                                long_name = add.country,
                                short_name = add.country,

                                types = new List<string> {"country", "political"}
                            }
                        }
                    });
                }

                response.results = results;
                response.status = "OK";

                if (response.results.Count < 1)
                {
                    response.results = null;
                    response.status = "No Result";
                }
            }
            catch (Exception ex)
            {
                response.results = null;
                response.status = ex.Message.ToString();
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Route("geocode/xml/address/{address}/{token}")]
        public Extensions.XmlResult GetAddressXML(string address, string token)
        {
            var dbPath = Server.MapPath("~/Data/ZipCode.xml");
            var zipDB = new DataSet();

            zipDB.ReadXml(dbPath);

            var zipTbl = zipDB.Tables["ZipCode"]
                .Select("PostalCode=" + address).AsEnumerable();

            Models.XML.GeocodeResponse response = new Models.XML.GeocodeResponse();
            List<Models.XML.result> result = new List<Models.XML.result>();

            try
            {
                var addressData = new List<Models.AddressModel>();

                foreach (var row in zipTbl)
                {
                    addressData.Add(new Models.AddressModel
                    {
                        postal_code = row.Field<string>("PostalCode"),
                        locality = row.Field<string>("City"),
                        administrative_area_level_1 = row.Field<string>("State"),
                        administrative_area_level_2 = "NA",
                        country = row.Field<string>("Country"),
                        abbreviation = row.Field<string>("Abbreviation")
                    });
                }

                foreach (Models.AddressModel add in addressData)
                {
                    result.Add(new Models.XML.result()
                    {
                        type = new List<string> { "zip_code" },
                        formatted_address = string.Format("{0}, {1} {2}, {3}", add.locality, add.abbreviation, add.postal_code, add.country),
                        address_component = new List<Models.XML.address_component>
                        {
                            new Models.XML.address_component {
                                long_name = add.postal_code,
                                short_name = add.postal_code,

                                type = new List<string> {"postal_code"}
                            },
                            new Models.XML.address_component {
                                long_name = add.locality,
                                short_name = add.locality,

                                type = new List<string> {"locality", "political"}
                            },
                            new Models.XML.address_component {
                                long_name = add.administrative_area_level_1,
                                short_name = add.abbreviation,

                                type = new List<string> {"administrative_area_level_1", "political"}
                            },
                            new Models.XML.address_component {
                                long_name = add.administrative_area_level_2,
                                short_name = add.administrative_area_level_2,

                                type = new List<string> {"administrative_area_level_2", "political"}
                            },
                            new Models.XML.address_component {
                                long_name = add.country,
                                short_name = add.country,

                                type = new List<string> {"country", "political"}
                            }
                        }
                    });
                }

                response = new Models.XML.GeocodeResponse()
                {
                    status = "OK",
                    result = result
                };

                if (response.result.Count < 1)
                {
                    response = new Models.XML.GeocodeResponse()
                    {
                        status = "No Result",
                        result = null
                    };
                }
            }
            catch (Exception ex)
            {
                response = new Models.XML.GeocodeResponse()
                {
                    status = ex.Message.ToString(),
                    result = null
                };
            }

            return new Extensions.XmlResult(response);
        }
    }
}