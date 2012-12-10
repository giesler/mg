using System;
using System.Collections;

namespace msn2.net.Common
{
	/// <summary>
	/// Common functionality
	/// </summary>
	public class Utilities
	{

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
