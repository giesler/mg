using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace CamAlertService
{
    public class CamVideoStorage : ICamVideoStorage
    {
        public byte[] GetFile(string fileName)
        {
            if (fileName.ToLower().Contains(@"c:\logitech alert recordings\") && Path.GetExtension(fileName).ToLower() == ".mp4")
            {
                return File.ReadAllBytes(fileName);
            }

            return null;
        }
    }
}
