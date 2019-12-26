using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace ZipCodeApi.Extensions
{
    public class XmlResult: ActionResult
    {
        public XmlResult(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            Data = data;
        }
        public object Data { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "text/xml";

            var serializer = new XmlSerializer(Data.GetType());
            serializer.Serialize(response.OutputStream, Data);
        }
    }
}