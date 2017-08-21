using System;
using System.Collections;
using System.Data.SqlClient;

namespace XMAdmin
{
	/// <summary>
	/// Summary description for index.
	/// </summary>
	public class Index
	{
		public XMGuid Md5;
		public XMGuid UserId;
		public double Score;
		//public DateTime DateStamp;
		
		#region Fields Definitions
		//index fields
		public uint Cat1;
		public uint Cat2;
		public byte Setting;
		public byte Rating;
		public byte Quantity;
		public byte Content;
		public byte Build;
		public byte HairColor;
		public byte HairStyle;
		public byte Eyes;
		public byte Height;
		public byte Age;
		public byte Breasts;
		public byte Nipples;
		public byte Butt;
		public byte Race;
		public byte Quality;
		public byte Skin;
		public byte Hips;
		public byte Legs;
		public byte FemaleGen;
		public byte MaleGen;
		public byte Chest;
		public byte FacialHair;
		#endregion

		public Index(SqlDataReader rs)
		{
			FromRs(rs);
		}

		private void FromRs(SqlDataReader rs)
		{
			//read misc stuff
			Md5 = new XMGuid((byte[])rs["md5"]);
			UserId = new XMGuid((byte[])rs["userid"]);
			Score = Convert.ToDouble(rs["score"]);
			//DateStamp = Convert.ToDateTime(rs["datestamp"]);

			//read fields
			#region Read in Fields
			Age			= Convert.ToByte(rs["_Age"]);
			Breasts		= Convert.ToByte(rs["_Breasts"]);
			Build		= Convert.ToByte(rs["_Build"]);
			Butt		= Convert.ToByte(rs["_Butt"]);
			Cat1		= Convert.ToUInt32(rs["_Catagory"]);
			Cat2		= Convert.ToUInt32(rs["_Catagory2"]);
			Chest		= Convert.ToByte(rs["_Chest"]);
			Content		= Convert.ToByte(rs["_Content"]);
			Eyes		= Convert.ToByte(rs["_Eyes"]);
			FacialHair	= Convert.ToByte(rs["_FacialHair"]);
			FemaleGen	= Convert.ToByte(rs["_FemaleGen"]);
			HairColor	= Convert.ToByte(rs["_HairColor"]);
			HairStyle	= Convert.ToByte(rs["_HairStyle"]);
			Height		= Convert.ToByte(rs["_Height"]);
			Hips		= Convert.ToByte(rs["_Hips"]);
			Legs		= Convert.ToByte(rs["_Legs"]);
			MaleGen		= Convert.ToByte(rs["_MaleGen"]);
			Nipples		= Convert.ToByte(rs["_Nipples"]);
			Quality		= Convert.ToByte(rs["_Quality"]);
			Quantity	= Convert.ToByte(rs["_Quantity"]);
			Race		= Convert.ToByte(rs["_Race"]);
			Rating		= Convert.ToByte(rs["_Rating"]);
			Setting		= Convert.ToByte(rs["_Setting"]);
			Skin		= Convert.ToByte(rs["_Skin"]);
			#endregion
		}
	}

	public class Indexes : ReadOnlyCollectionBase
	{
		private Indexes(string sql)
		{
			//run query
			SqlDataReader rs = Data.Exec(sql);

			//add a record for each row
			while (rs.Read())
			{
				InnerList.Add(new Index(rs));
			}
			rs.Close();
		}

		public static Indexes FromUser(XMGuid UserId)
		{
			return new Indexes(String.Format(
				"select * from mediaindex where userid={0}", UserId.ToStringDB()));
		}

		public static Indexes FromMedia(XMGuid Md5)
		{
			return new Indexes(String.Format(
				"select * from mediaindex where md5={0}", Md5.ToStringDB()));
		}
	}



	public struct IndexField
	{
		public string Name;
		public string[] Values;
	
		public static IndexField[] Fields;	
		static IndexField()
		{
			#region Field Values

			//create field array
			Fields = new IndexField[24];
	
			//Setting
			Fields[0].Name = "Setting";
			Fields[0].Values = new string[] {
				"Outdoors",
				"Indoors",
				"Studio",
				"Water",
				"Public"
			};
	
			//Rating
			Fields[1].Name = "Rating";
			Fields[1].Values = new string[] {
				"PG",
				"PG-13",
				"R",
				"NC-17",
				"X",
				"XXX"
			};
	
			//Quantity
			Fields[2].Name = "Quantity";
			Fields[2].Values = new string[] {
				"1",
				"2",
				"3",
				"4-6",
				"7-11",
				"12+"
			};
	
			//Contents
			Fields[3].Name = "Contents";
			Fields[3].Values = new string[] {
				"Male",
				"Female",
				"Shemale",
				"Other"
			};
	
			//Build
			Fields[4].Name = "Build";
			Fields[4].Values = new string[] {
				"Slim",
				"Athletic",
				"Body Builder",
				"Normal",
				"Few Extra",
				"Plump",
				"Huge"
			};
	
			//Hair Color
			Fields[5].Name = "Hair Color";
			Fields[5].Values = new string[] {
				"Blond",
				"Brown",
				"Black",
				"Red",
				"White",
				"Other"
			};
	
			//Hair Style
			Fields[6].Name = "Hair Style";
			Fields[6].Values = new string[] {
				"Short",
				"Curly",
				"Medium",
				"Long",
				"Bunned",
				"Pony Tail"
			};
	
			//Eyes
			Fields[7].Name = "Eyes";
			Fields[7].Values = new string[] {
				"Blue",
				"Brown",
				"Hazel",
				"Green",
				"Other"
			};
	
			//Height
			Fields[8].Name = "Height";
			Fields[8].Values = new string[] {
				"Short",
				"Medium",
				"Tall"
			};
	
			//Age
			Fields[9].Name = "Age";
			Fields[9].Values = new string[] {
				"18",
				"19-21",
				"22-25",
				"26-32",
				"33-38",
				"39-49",
				"50+",
				"Ancient"
			};
	
			//Breasts
			Fields[10].Name = "Breasts";
			Fields[10].Values = new string[] {
				"Small",
				"Medium",
				"Large",
				"Firm",
				"Saggy",
				"Covered"
			};
	
			//Nipples
			Fields[11].Name = "Nipples";
			Fields[11].Values = new string[] {
				"Small",
				"Medium",
				"Large",
				"Puffy",
				"Covered"
			};
	
			//Butt
			Fields[12].Name = "Butt";
			Fields[12].Values = new string[] {
				"Small",
				"Normal",
				"Large",
				"Gargantuan",
				"Firm",
				"Flabby",
				"Hairy",
				"Covered"
			};
	
			//Catagory
			Fields[13].Name = "Catagory";
			Fields[13].Values = new string[] {
				"Belly Buttons",
				"Casts",
				"Braces (oral)",
				"Dolls & Mannequins",
				"Feet",
				"Animals",
				"Eye Glasses",
				"Hair",
				"Sleep / hypnosis / Unconsious",
				"Inflatable",
				"Robots",
				"Smoking",
				"Bondage",
				"Wet & Messy",
				"Gas Masks",
				"Tongues",
				"Piercings",
				"Fisting",
				"Pissing",
				"Cum Shot",
				"Pregnant",
				"Shitting",
				"Transvestites",
				"Cheerleaders",
				"Lingerie",
				"Masturbation",
				"Vintage Porn",
				"Strap On",
				"Food",
				"S&M",
				"Voyeur"
			};
	
			//Race
			Fields[14].Name = "Race";
			Fields[14].Values = new string[] {
				"Caucasion",
				"Asian",
				"African",
				"American-Indian",
				"East-Indian",
				"Mexican"
			};
	
			//Quality
			Fields[15].Name = "Quality";
			Fields[15].Values = new string[] {
				"Low",
				"Medium",
				"High"
			};
	
			//Skin
			Fields[16].Name = "Skin";
			Fields[16].Values = new string[] {
				"Ghost",
				"Pale",
				"Freckled",
				"Lightly Taned",
				"Darkly Tanned",
				"Sunburnt"
			};

			//Hips
			Fields[17].Name = "Hips";
			Fields[17].Values = new string[] {
				"Narrow",
				"Normal",
				"Full",
				"Covered"
			};
	
			//Legs
			Fields[18].Name = "Legs";
			Fields[18].Values = new string[] {
				"Long",
				"Athletic",
				"Short",
				"Thin",
				"Thick",
				"Stockings",
				"Flabby",
				"Covered"
			};
	
			//Female Genitals
			Fields[19].Name = "Female Genitals";
			Fields[19].Values = new string[] {
				"Shaved",
				"Trimmed",
				"Hairy",
				"Covered",
				"Normal",
				"Open"
			};
	
			//Male Genitals
			Fields[20].Name = "Male Genitals";
			Fields[20].Values = new string[] {
				"Shaved",
				"Limp",
				"Erect",
				"Small",
				"Normal",
				"Large",
				"Covered"
			};
	
			//Chest
			Fields[21].Name = "Chest";
			Fields[21].Values = new string[] {
				"Muscular",
				"Normal",
				"Hairy",
				"Flabby",
				"Covered"
			};
	
			//Facial Hair
			Fields[22].Name = "Facial Hair";
			Fields[22].Values = new string[] {
				"Clean Shaven",
				"Sideburns",
				"Goatee",
				"Beard",
				"Mustache",
				"Covered"
			};
	
			//Catagory 2
			Fields[23].Name = "Catagory 2";
			Fields[23].Values = new string[] {
				"Exhibition",
				"Upskirt",
				"Latex",
				"Anal Sex",
				"Blindfolding",
				"Douches",
				"Drag",
				"Suffocation",
				"Tattoos",
				"Leather",
				"Amputees",
				"Hairy Armpits",
				"Midgets",
				"Oral",
				"Weapons",
				"Costumes",
				"Machines & Tools",
				"Cartoons",
				"Toys",
				"Drawings / Paintings",
				"Penetration / Sex"
			};

			#endregion
		}
	}
}
