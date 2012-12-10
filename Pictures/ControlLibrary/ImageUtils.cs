using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace Utilities
{
    public enum PictureOrientation
    {
        Normal,
        Rotate90,
        Rotate180,
        Roteate270,
        Unknown
    }
    
    public class ExifMetadata
    {
		private Regex dateRegEx;

		public ExifMetadata()
		{
			dateRegEx = new Regex(@"(?<year>\d{4}):(?<month>\d{2}):(?<day>\d{2}) (?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2})");
		}

		public struct MetadataDetail
		{
			public string Hex;
			public string RawValueAsString;
			public string DisplayValue;
		}

        public struct OrientationDetail
        {
            public PictureOrientation Orientation;
            public string RawValueAsString;
        }


        public struct Metadata
		{
			public MetadataDetail EquipmentMake;
			public MetadataDetail CameraModel;
            public MetadataDetail ExposureTime;
			public MetadataDetail Fstop;
			public MetadataDetail DatePictureTaken;
			public MetadataDetail ShutterSpeed;
			public MetadataDetail ExposureCompensation;
			public MetadataDetail MeteringMode;
			public MetadataDetail Flash;
			public MetadataDetail XResolution;
			public MetadataDetail YResolution;
			public MetadataDetail ImageWidth;
			public MetadataDetail ImageHeight;
            public OrientationDetail Orientation;
        }

        public string LookupExifValue(string Description, string Value)
		{
			string DescriptionValue = null;

			if (Description == "MeteringMode")
			{
				switch(Value) 
				{
					case "0":
						DescriptionValue = "Unknown";
						break;
					case "1":
						DescriptionValue = "Average";
						break;
					case "2":
						DescriptionValue = "Center Weighted Average";
						break;
					case "3":
						DescriptionValue = "Spot";
						break;
					case "4":
						DescriptionValue = "Multi-spot";
						break;
					case "5":
						DescriptionValue = "Multi-segment";
						break;
					case "6":
						DescriptionValue = "Partial";
						break;
					case "255":
						DescriptionValue = "Other";
						break;
				}
			}

			if (Description == "ResolutionUnit")
			{
				switch(Value) 
				{
					case "1":
						DescriptionValue = "No Units";
						break;
					case "2":
						DescriptionValue = "Inch";
						break;
					case "3":
						DescriptionValue = "Centimeter";
						break;
				}
			}

			if (Description == "Flash")
			{
				switch(Value) 
				{
					case "0":
						DescriptionValue = "Flash did not fire";
						break;
					case "1":
						DescriptionValue = "Flash fired";
						break;
					case "5":
						DescriptionValue = "Flash fired but strobe return light not detected";
						break;
					case "7":
						DescriptionValue = "Flash fired and strobe return light detected";
						break;
				}
			} 
			
			return DescriptionValue;
		}

        public PictureOrientation GetOrientation(string value)
        {
            string temp = value;
            if (temp.IndexOf("-") > 0)
            {
                temp = temp.Substring(0, temp.IndexOf("-"));
            }

            int result = 0;
            if (int.TryParse(temp, out result))
            {
                switch (result)
                {
                    case 1:
                    case 5:
                        return PictureOrientation.Normal;
                    case 2:
                    case 6:
                        return PictureOrientation.Rotate90;
                    case 3:
                    case 7:
                        return PictureOrientation.Rotate180;
                    case 4:
                    case 8:
                        return PictureOrientation.Roteate270;
                    default:
                        return PictureOrientation.Unknown;
                }
            }

            return PictureOrientation.Unknown;
        }

        public Metadata GetExifMetadata(string PhotoName)
		{
			// Create an instance of the image to gather metadata from 
			Image image = Image.FromFile(PhotoName);

			// Create an integer array to hold the property id list,
			// and populate it with the list from my image.
			/* Note: this only generates a list of integers, one for for each PropertyID.  
			 * We will populate the PropertyItem values later. */
			int[] propertyIdList = image.PropertyIdList; 

			// Create an array of PropertyItems, but don't populate it yet.
			/* Note: there is a bug in .net framework v1.0 SP2 and also in 1.1 beta:
			 * If any particular PropertyItem has a length of 0, you will get an unhandled error
			 * when you populate the array directly from the image.
			 * So, rather than create an array of PropertyItems and then populate it directly
			 * from the image, we will create an empty one of the appropriate length, and then 
			 * test each of the PropertyItems ourselves, one at a time, and not add any that 
			 * would cause an error. */
			PropertyItem[] propertyItemList = new PropertyItem[propertyIdList.Length];

			// Create an instance of Metadata and populate Hex codes (values populated later)
			Metadata data					= new Metadata();
			data.EquipmentMake.Hex			= "10f";
			data.CameraModel.Hex			= "110";
			data.DatePictureTaken.Hex 		= "9003";
			data.ExposureTime.Hex 			= "829a";
			data.Fstop.Hex					= "829d";
			data.ShutterSpeed.Hex			= "9201";
			data.ExposureCompensation.Hex	= "9204";
			data.MeteringMode.Hex 			= "9207";
			data.Flash.Hex					= "9209";

			// Declare an ASCIIEncoding to use for returning string values from bytes
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
   
			// Populate propertyItemList.  
			// For each propertyID... 
			int index = 0;
			foreach (int propertyId in propertyIdList)
			{
				// ... try to call GetPropertyItem (it crashes if PropertyItem has length 0, so use Try/Catch)
				try 
				{
					// Assign the image's PropertyItem to the PropertyItem array
					propertyItemList[index] = image.GetPropertyItem(propertyId);

                    
					// Troublshooting
					/*    
					textBox1.AppendText("\r\n\t" + 
					BitConverter.ToString(image.GetPropertyItem
					(propertyId).Value));

					textBox1.AppendText("\r\n\thex location: " + 
					image.GetPropertyItem(propertyId).Id.ToString("x"));
						*/

					// Assign each element of data
					if (image.GetPropertyItem(propertyId).Id.ToString("x") == "10f") // EquipmentMake
					{
     					data.EquipmentMake.RawValueAsString		= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
						data.EquipmentMake.DisplayValue			= encoding.GetString(propertyItemList[index].Value);
					}

					if 	(image.GetPropertyItem(propertyId).Id.ToString("x") == "110") // CameraModel
					{
						data.CameraModel.RawValueAsString		= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
						data.CameraModel.DisplayValue			= encoding.GetString(propertyItemList[index].Value);
					}

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "112") // Orientation
                    {
                        data.Orientation.RawValueAsString = BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
                        data.Orientation.Orientation = GetOrientation(data.Orientation.RawValueAsString);
                    }

                    if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9003") // DatePictureTaken
					{
						data.DatePictureTaken.RawValueAsString	= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
     					data.DatePictureTaken.DisplayValue		= encoding.GetString(propertyItemList[index].Value);

						// Try parsing into valid date/time
						Match match = dateRegEx.Match(data.DatePictureTaken.DisplayValue);
						if (match.Groups["year"] != null && match.Groups["month"] != null && match.Groups["day"] != null)
						{
							try
							{
								string temp = string.Format(@"{0}/{1}/{2} {3}:{4}:{5}", 
									match.Groups["month"].Value,	match.Groups["day"].Value,		match.Groups["year"].Value,
									match.Groups["hour"].Value,		match.Groups["minute"].Value,	match.Groups["second"].Value);
								DateTime date = DateTime.Parse(temp);

								data.DatePictureTaken.DisplayValue	= date.ToString();
							}
							catch (Exception ex)
							{
								System.Diagnostics.Trace.WriteLine("Error converting to date/time: " + ex.Message);
							}
						}
						
						// Match 00:00:00
					}

					if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9207") // MeteringMode
					{
						data.MeteringMode.RawValueAsString		= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
						data.MeteringMode.DisplayValue			= LookupExifValue("MeteringMode", BitConverter.ToInt16(image.GetPropertyItem(propertyId).Value,0).ToString());
					}

					if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9209") // Flash
					{
     					data.Flash.RawValueAsString				= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
     					data.Flash.DisplayValue					= LookupExifValue("Flash", BitConverter.ToInt16(image.GetPropertyItem(propertyId).Value,0).ToString());
					}

					if (image.GetPropertyItem(propertyId).Id.ToString("x") == "829a") // ExposureTime
					{
						data.ExposureTime.RawValueAsString		= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);

						string temp = "";
						for (int Offset = 0; Offset < image.GetPropertyItem(propertyId).Len; Offset = Offset + 4)
						{      
							temp += BitConverter.ToInt32(image.GetPropertyItem(propertyId).Value,Offset).ToString() + "/";
						}
     
						data.ExposureTime.DisplayValue			= temp.Substring(0, temp.Length-1);
					}

					if (image.GetPropertyItem(propertyId).Id.ToString("x") == "829d") // F-stop
					{
     					data.Fstop.RawValueAsString				= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
						int int1								= BitConverter.ToInt32(image.GetPropertyItem(propertyId).Value,0);
						int int2								= BitConverter.ToInt32(image.GetPropertyItem(propertyId).Value,4);
						data.Fstop.DisplayValue					= "F/" + (int1/int2);
					}

					if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9201") // ShutterSpeed
					{
						data.ShutterSpeed.RawValueAsString		= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);

						string temp								= BitConverter.ToString(image.GetPropertyItem(propertyId).Value).Substring(0,2);
						data.ShutterSpeed.DisplayValue			= "1/" + temp;
					}
       
					if (image.GetPropertyItem(propertyId).Id.ToString("x") == "9204") // ExposureCompensation
					{
						data.ExposureCompensation.RawValueAsString	= BitConverter.ToString(image.GetPropertyItem(propertyId).Value);
						string temp									= BitConverter.ToString(image.GetPropertyItem(propertyId).Value).Substring(0,1);
						data.ExposureCompensation.DisplayValue		= temp + " (Needs work to confirm accuracy)";
					}

				}
				catch (Exception exc)
				{
					// if it is the expected error, do nothing
					if (exc.GetType().ToString() != "System.ArgumentNullException")
					{
					}
				}
				finally
				{
					index++;
				}
			}

			data.XResolution.DisplayValue	= image.HorizontalResolution.ToString();
  			data.YResolution.DisplayValue	= image.VerticalResolution.ToString();
  			data.ImageHeight.DisplayValue	= image.Height.ToString();
  			data.ImageWidth.DisplayValue	= image.Width.ToString();

			return data;
		}
	}
}
