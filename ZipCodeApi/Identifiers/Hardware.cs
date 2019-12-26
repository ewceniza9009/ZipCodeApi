using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management;

namespace ZipCodeApi.Identifiers
{
    public class Hardware
    {
        public Hardware() {
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");

            mbsList = mbs.Get();

            string id = "";

            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorID"].ToString();
            }

            ProcessorId = id;
        }
        public string ProcessorId { get; set; }

    }
}