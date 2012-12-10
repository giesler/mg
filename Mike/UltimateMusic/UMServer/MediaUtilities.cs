using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

namespace UMServer
{
	/// <summary>
	/// Summary description for MediaUtilities.
	/// </summary>
	public class MediaUtilities
	{

		public MediaUtilities()
		{
		}

		public static string NameMediaFile(SqlCommand cmd, string currentName)
		{
			string artist = "";
			if (cmd.Parameters["@Artist"].Value != null)
				artist = cmd.Parameters["@Artist"].Value.ToString();

			string album = "";
			if (cmd.Parameters["@Album"].Value != null)
				album = cmd.Parameters["@Album"].Value.ToString();

			int track = 0;
			if (cmd.Parameters["@Track"].Value != null)
				track = Convert.ToInt32(cmd.Parameters["@Track"].Value);

			string name = cmd.Parameters["@Name"].Value.ToString();

			return InternalNameMediaFile(name, artist, album, track, currentName);
		}

		/// <summary>
		/// Returns formatted name of existing media file
		/// </summary>
		/// <param name="row">Item to rename</param>
		/// <returns></returns>
		public static string NameMediaFile(DataSetMedia.MediaRow row)
		{
            return InternalNameMediaFile(row.Name, row.Artist, row.Album, row.Track, row.MediaFile);
		}

		/// <summary>
		/// Names a media file in the 'approved' way
		/// </summary>
		/// <param name="row">Row with media info</param>
		/// <returns>Correct filename</returns>
		public static string NameMediaFile(DataSetMedia.MediaRow row, string currentName)
		{
			string artist = "";
			if (!row.IsArtistNull())
				artist = row.Artist;

			string album = "";
			if (!row.IsAlbumNull() && row.Album.Length > 0)
				album = row.Album;
            
			int track = 0;
			if (!row.IsTrackNull() && row.Track > 0)
				track = row.Track;

			string name = row.Name;

			return InternalNameMediaFile(name, artist, album, track, currentName);
		}

		private static string InternalNameMediaFile(string name, string artist, string album,
			int track, string currentName)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(@"\\sp\dfs\Music\");

			// start with artist name
			if (artist.Length > 0)
				sb.Append(ProperCasing(artist) + @"\");

			// Add album name if we have it
			if (album.Length > 0)
				sb.Append(ProperCasing(album) + @"\");

			// FILENAME

			// Start with artist name again
			if (artist.Length > 0)
				sb.Append("[" + ProperCasing(artist) + "] ");

			// Second use album name if available
			if (album.Length > 0)
				sb.Append("[" + ProperCasing(album) + "] ");

			// Add track if available
			if (track > 0)
				sb.Append(track.ToString("00") + " ");

			// Add song name
			sb.Append(ProperCasing(name));

			// Add file extension
			sb.Append(currentName.Substring(currentName.LastIndexOf(".")));

			return sb.ToString();
		}

		public static string ProperCasing(string input)
		{
			string temp   = "";
			StringBuilder output = new StringBuilder();
			bool capChar  = true;

			// loop through each element of array
			input = input.Trim();
			for (int i = 0; i < input.Length; i++)
			{
				temp = input.Substring(i, 1);

				int val1 = String.Compare(temp, "A", true);
				int val2 = String.Compare("z", temp, true);

				if (temp.Equals("-") || temp.Equals(@"\") || temp.Equals(" "))
				{
					capChar = true;
				}
					// check if we are on a character
				else if ((String.Compare(temp, "A", true) >= 0 
					&& String.Compare("z", temp, true) >= 0 && capChar) || i ==0)
				{
                    temp = temp.ToUpper();
					capChar = false;
				}
				
				output.Append(temp);
			}

			//return output.ToString();
			return input;
		}

	}

	public class IterIsolate: IEnumerable
	{
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
	}

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
}
