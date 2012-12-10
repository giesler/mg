using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Collections.Generic;

[WebServiceBinding(ConformanceClaims=WsiClaims.BP10,EmitConformanceClaims = true)]
public class PictureWebService : System.Web.Services.WebService {

    [WebMethod]
    public bool AddPicture(byte[] pictureArray, AddPictureData pictureData)
    {
        string saveFilename = @"c:\picdrop\" + pictureData.PictureId.ToString();

        FileStream fs = new FileStream(saveFilename, FileMode.Create, FileAccess.ReadWrite);
        fs.Write(pictureArray, 0, pictureArray.Length);
        fs.Close();

        return true;
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    
}

public struct AddPictureData
{
    public Guid PictureId;
    public DateTime PictureDate;
    public string Title;
    public int SortOrder;
    public int PictureByPersonId;
    public List<int> CategoryIds;
    public List<int> GroupIds;
    public List<int> PersonIds;
}         