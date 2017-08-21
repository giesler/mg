using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Drawing.Drawing2D;

namespace msn2.net.Common
{
	/// <summary>
	/// Common functionality
	/// </summary>
	public class Utilities
	{
		#region DurationToString

		public static string DurationToString(decimal duration)
		{
			return DurationToString(Convert.ToDouble(duration));
		}

		public static string DurationToString(double duration)
		{
			if (duration == 0)
				return "0:00";

			int hours   = 0;
			int minutes = (int)(duration / 60.0);
			int seconds = (int)(duration % 60.0);

			// if over 120 minutes, add hours
			if (minutes > 120)
			{
				hours   = minutes / 60;
				minutes = minutes % 60;
			}

			if (hours > 0)
			{
				return hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString("00");
			}
			else
			{
				return minutes.ToString() + ":" + seconds.ToString("00");
			}
		}

		#endregion

		#region Serialize / Deserialize

		public static string SerializeObject(object obj)
		{
			if (obj == null)
				return "";

			// Declares
			StringBuilder sb			= new StringBuilder();
			MemoryStream memStream		= new MemoryStream();
			XmlSerializer ser			= new XmlSerializer(obj.GetType());

			//System.Runtime.Serialization.Formatters.Soap.SoapFormatter ser =
			//	new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();

			// Serialize into memory stream
			ser.Serialize(memStream, obj);

			// Copy into a string
			int position = 0; int read = 0;
			byte[] buffer = new byte[1024];
			memStream.Seek(0, SeekOrigin.Begin);
			while (position < memStream.Length)
			{
				// read a block
				read = memStream.Read(buffer, 0, 1024);
                
				// write to string
				for (int i = 0; i < read; i++)
					sb.Append( (char) buffer[i] );

				// update pointer
				position += read;
			}

			memStream.Close();
            
			return sb.ToString();
		}

		public static object DeserializeObject(string xml, Type type)
		{

			// Declares
			XmlSerializer ser			= new XmlSerializer(type);
			byte[] buffer				= new byte[xml.Length];

			// Copy xml into byte array
			for (int i = 0; i < xml.Length; i++)
			{
				buffer[i] = (byte) xml[i];
			}

			// Deserialize the object
			MemoryStream memStream		= new MemoryStream(buffer);
			object obj					= ser.Deserialize(memStream);
			memStream.Close();

			return obj;
		}

		#endregion

	}

	public class Drawing
	{
		#region ShadeRegion

		public static void ShadeRegion(PaintEventArgs e, Color startColor)
		{
			if (e.ClipRectangle.Width == 0 || e.ClipRectangle.Height == 0)
				return;

			int red = 255 - ((255 - startColor.R) / 3);
			int green = 255 - ((255 - startColor.G) / 3);
			int blue = 255 - ((255 - startColor.B) / 3);

			ShadeRegion(e, startColor, Color.FromArgb(red, green, blue));
		}

		public static Color LightenColor(Color color)
		{
			int red = 255 - ((255 - color.R) / 3);
			int green = 255 - ((255 - color.G) / 3);
			int blue = 255 - ((255 - color.B) / 3);

			return Color.FromArgb(red, green, blue);
		}

		public static void ShadeRegion(PaintEventArgs e, Color startColor, Color endColor)
		{
			if (e.ClipRectangle.Width == 0 || e.ClipRectangle.Height == 0)
				return;

			using (LinearGradientBrush brush = 
					   new LinearGradientBrush(e.ClipRectangle, endColor, startColor, LinearGradientMode.Vertical))
			{
				e.Graphics.FillRectangle(brush, e.ClipRectangle);
			}

		}

		#endregion
	}

	public class IterIsolate: IEnumerable
	{
		#region IterIsolate

		internal class IterIsolateEnumerator: IEnumerator
		{
			internal ArrayList items = new ArrayList();
			internal int currentItem;

			internal IterIsolateEnumerator(IEnumerator enumerator)
			{
				while (enumerator.MoveNext() != false)
				{
					items.Add(enumerator.Current);
				}
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				currentItem = -1;
			}

			public void Reset()
			{
				currentItem = -1;
			}

			public bool MoveNext()
			{
				currentItem++;
				if (currentItem == items.Count)
					return false;

				return true;
			}

			public object Current
			{
				get
				{
					return items[currentItem];
				}
			}
		}

		public IterIsolate(IEnumerable enumerable)
		{
			this.enumerable = enumerable;
		}

		public IEnumerator GetEnumerator()
		{
			return new IterIsolateEnumerator(enumerable.GetEnumerator());
		}

		internal IEnumerable enumerable;

		#endregion
	}

	#region IterReverse

	/// <summary>
	/// Iterate a collection in the reverse order
	/// </summary>
	public class IterReverse: IterIsolate, IEnumerable
	{
		internal class IterReverseEnumerator: IterIsolateEnumerator, IEnumerator
		{
			internal IterReverseEnumerator(IEnumerator enumerator): base(enumerator)
			{
				currentItem = items.Count;
			}

			public new void Reset()
			{
				currentItem = items.Count;
			}

			public new bool MoveNext()
			{
				currentItem--;
				if (currentItem < 0)
					return false;

				return true;
			}
		}

		/// <summary>
		/// Create an instance of the IterReverse Class
		/// </summary>
		/// <param name="enumerable">A class that implements IEnumerable</param>
		public IterReverse(IEnumerable enumerable): base(enumerable)
		{
		}

		public new IEnumerator GetEnumerator()
		{
			return new IterReverseEnumerator(enumerable.GetEnumerator());
		}
	}

	#endregion
}
