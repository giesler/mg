using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Diagnostics;
using System.Collections;
using System.Text;

namespace msn2.net.Pictures
{

	/// <summary>
	/// Common variables and such for this app
	/// </summary>
	public struct PictureConfig 
	{
		public string ConnectionString;
		public string SmtpServer;
		public string PictureDirectory;
		public string CacheDirectory;
	}

	public class Msn2Config
	{
		public static PictureConfig Load()
		{
			PictureConfig config;
			config.ConnectionString = "data source=barbrady;initial catalog=picdb;Integrated Security=SSPI;persist security info=False";
			config.PictureDirectory	= @"\\sp\Data\Pictures\pics.msn2.net\";
			config.SmtpServer		= "192.168.1.5";
			config.CacheDirectory	= @"\\ike\picCache\";
			return config;
		}
	}

	public class Msn2Mail
	{
		public static string BuildMessage(string messageHtml)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);
			sb.Append("<html><body topmargin=\"0\" leftmargin=\"0\">");
			sb.Append("<table width=\"100%\" style=\"");
			sb.Append("filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#007300', EndColorStr='#000000'):");
			sb.Append("\"><tr height=\"5\"><td></td></tr></table>");	
			sb.Append("<blockquote>");
			sb.Append(messageHtml);
			sb.Append("</blockquote></body></html>");

			return sb.ToString();
		}
	}

	/// <summary>
	/// Retreives user information
	/// </summary>
	[Serializable()]
	public class PersonInfo 
	{
		#region Declares
		protected int _PersonID = 0;
		protected string _Name;
		protected string _Email;
		#endregion

		#region Constructors/Loaders

		/// <summary>
		/// Constructor for serialization
		/// </summary>
		public PersonInfo()
		{
		}

		public PersonInfo(int id, string name, string email) 
		{
			this._PersonID	= id;
			this._Name		= name;
			this._Email		= email;
		}

		#endregion

		#region Peron properties
		public int Id 
		{
			get { return _PersonID; }
		}

		public string Name 
		{
			get { return _Name; }
		}
		public string Email
		{
			get { return _Email; }
		}

		#endregion

	}


	public class PersonInfoCollection: ReadOnlyCollectionBase
	{
		internal void Add(PersonInfo person)
		{
			InnerList.Add(person);
		}

		public PersonInfo this[int index]
		{
			get
			{
				return (PersonInfo) InnerList[index];
			}
		}

		public PersonInfo FindById(int id)
		{
			foreach (PersonInfo person in InnerList)
			{
				if (id == person.Id)
				{
					return person;
				}
			}

			return null;
		}
	}

	#region PictureIdCollection

	public class PictureIdCollection: CollectionBase
	{
		public void Add(int pictureId)
		{
			if (!InnerList.Contains(pictureId))
			{
				InnerList.Add(pictureId);

				if (ItemAddedEvent != null)
				{
					ItemAddedEvent(this, new PictureIdEventArgs(pictureId));
				}
			}
		}

		public void Remove(int pictureId)
		{
			InnerList.Remove(pictureId);
		}

		public bool Contains(int pictureId)
		{
			foreach (int i in InnerList)
			{
				if (i == pictureId)
				{
					return true;
				}
			}
			return false;
		}

		public string GetListAsString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (int i in InnerList)
			{
				if (sb.Length > 0) sb.Append(",");
				sb.Append(i.ToString());
			}			
			return sb.ToString();
		}

		public event ItemAddedEventHandler ItemAddedEvent;
	}
	
	public delegate void ItemAddedEventHandler(object sender, PictureIdEventArgs e);

	public class PictureIdEventArgs: EventArgs
	{
		private int pictureId;

		public PictureIdEventArgs(int pictureId)
		{
			this.pictureId = pictureId;			
		}

		public int PictureId
		{
			get
			{
				return pictureId;
			}
		}
	}

	#endregion

}
