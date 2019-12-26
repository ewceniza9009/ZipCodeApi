using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ZipCodeApi.Controllers
{
    public class HomeController : Controller
    {
        //private Data.ZipCodeDataContext zipData;

        public ActionResult Index()
        {
            ViewBag.Title = "AccountMate - Geo location API services";

            ViewBag.City = "NA";
            ViewBag.State = "NA";

            var dbPath = Server.MapPath("~/Data/ZipCode.xml");
            var zipDB = new DataSet();

            zipDB.ReadXml(dbPath);

            string address = Request.Form.Get("zipCode");

            //zipData = new Data.ZipCodeDataContext();

            if (address != null)
            {
                if (address.Trim() != "")
                {
                    var zipTbl = zipDB.Tables["ZipCode"]
                    .Select("PostalCode=" + address)
                    .AsEnumerable();

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

                    //Models.AddressModel addressData = addData
                    //    .Where(x => x.PostalCode == address)
                    //    .OrderByDescending(o => o.City)
                    //    .Select(y => new Models.AddressModel()
                    //    {
                    //        postal_code = y.PostalCode,
                    //        locality = y.City,
                    //        administrative_area_level_1 = y.State,
                    //        administrative_area_level_2 = "NA",
                    //        country = y.Country,
                    //        abbreviation = y.Abbreviation
                    //    })
                    //    .FirstOrDefault();

                    ViewBag.ZipCode = address;

                    if(addressData != null && addressData.Count != 0)
                    {
                        ViewBag.City = addressData
                        .OrderByDescending(o => o.locality)
                        .FirstOrDefault().locality;

                        ViewBag.State = addressData
                            .OrderByDescending(o => o.locality)
                            .FirstOrDefault().administrative_area_level_1;
                    }
                }
            }

            return View();
        }
    }
}