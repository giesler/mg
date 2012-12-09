namespace XMedia
{
    using System;
	using System.Collections;
	using System.Threading;
	using System.Xml;
	using ADODB;

	/// <summary>
	/// Simple structure, stores the host ip and
	/// network connection speed of a computer that
	/// hosts a particular file
	/// </summary>
	public struct XMMediaItemHost
	{
		public string Ip;
		public byte Speed;
	}

	/// <summary>
	/// In-memory representation of a single picture
	/// </summary>
	public class XMMediaItem
	{
		//media item data
		public string Action;
		public string Md5;
		public int Width, Height, FileSize;
		public XMIndex[] Indices;

		//helper data
		public XMMediaItem Previous, Next;
		
		//who has this file online?
		private XMMediaItemHost[] mServers = new XMMediaItemHost[5];
		private byte mServersCount = 0;
		private DateTime mServersAge = (DateTime.Now - TimeSpan.FromMinutes(11));
		/*
		public XMMediaItemHost[] GetServers(ref byte count, XMAdo ado)
		{
			//return the collection
			RebuildServers(ado);
			count = mServersCount;
			return mServers;
		}
		*/
		public byte GetServersCount(XMAdo ado)
		{
			RebuildServers(ado);
			return mServersCount;
		}

		private void RebuildServers(XMAdo ado)
		{
			//check time difference
			if (DateTime.Now.Subtract(mServersAge).Minutes > 10)
			{
				//more than 10 minutes old.. rebuild
				string sql = "exec sp_getmediastorage 0x" + Md5;
				ADODB._Recordset rs = ado.SqlExec(sql);
				mServersCount=0;
				while(!rs.EOF)
				{
					mServers[mServersCount].Ip = rs.Fields["hostip"].Value.ToString();
					mServers[mServersCount].Speed = rs.Fields["datarate"].Value.ToString().ToByte();
					mServersCount++;
					rs.MoveNext();
				}
				
				//reset the time
				mServersAge = DateTime.Now;
			}
		}

		//conversions
		public XmlElement ResultsToXml(XmlDocument xml)
		{
			//create the item element
			XmlElement e = xml.CreateElement("item");
			e.SetAttribute("md5", Md5);
			e.SetAttribute("width", Width.ToString());
			e.SetAttribute("height", Height.ToString());
			e.SetAttribute("size", FileSize.ToString());

			//create an entry for each ip
			XmlElement f;
			lock(this)
			{
				for(int i=0;i<mServersCount;i++)
				{
					f = xml.CreateElement("host");
					f.SetAttribute("ip", mServers[i].Ip);
					f.SetAttribute("speed", mServers[i].Speed.ToString());
					e.AppendChild(f);
				}
			}
			
			return e;
		}
	}
	
	/// <summary>
	/// Represents query field selections, or describes an image
	/// </summary>
	public struct XMIndex
	{
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
		public byte Gender;

		/// <summary>
		/// Returns the agregate number of bits set in all the index's fields.
		/// </summary>
		/// <param name="includeCat">If false, does not include catagories.</param>
		public int CountFields(bool includeCat)
		{
			int n = 0;
			if (includeCat)
			{
				_countInt32(Cat1, ref n);
				_countInt32(Cat2, ref n);
			}
			_countByte(Setting, ref n);
			_countByte(Rating, ref n);
			_countByte(Quantity, ref n);
			_countByte(Content, ref n);
			_countByte(Build, ref n);
			_countByte(HairColor, ref n);
			_countByte(HairStyle, ref n);
			_countByte(Eyes, ref n);
			_countByte(Height, ref n);
			_countByte(Age, ref n);
			_countByte(Breasts, ref n);
			_countByte(Nipples, ref n);
			_countByte(Butt, ref n);
			_countByte(Race, ref n);
			_countByte(Quality, ref n);
			_countByte(Skin, ref n);
			_countByte(Hips, ref n);
			_countByte(Legs, ref n);
			_countByte(FemaleGen, ref n);
			_countByte(MaleGen, ref n);
			_countByte(Chest, ref n);
			_countByte(FacialHair, ref n);
			_countByte(Gender, ref n);
			return n;
		}
		private void _countByte(Byte b, ref int n)
		{
			for(int i=0;i<8;i++)
				n += ((b&(1<<i))!=0)?1:0;
		}
		private void _countInt32(UInt32 dw, ref int n)
		{
			for(int i=0;i<32;i++)
				n += ((dw&(1<<i))!=0)?1:0;
		}

		/// <summary>
		/// Convert index to standard xml format.
		/// </summary>
		/// <param name="xml">Object factory.</param>
		public XmlElement ToXml(XmlDocument xml)
		{
			//create our base element
			XmlElement index = xml.CreateElement("index");

			//Create an element for each field, and append to index
			ToXml_AppendField(index, "Cat1", 		Cat1);
			ToXml_AppendField(index, "Cat2", 		Cat2);
			ToXml_AppendField(index, "Setting", 	Setting);
			ToXml_AppendField(index, "Rating", 		Rating);
			ToXml_AppendField(index, "Quantity", 	Quantity);
			ToXml_AppendField(index, "Content", 	Content);
			ToXml_AppendField(index, "Build", 		Build);
			ToXml_AppendField(index, "HairColor", 	HairColor);
			ToXml_AppendField(index, "HairStyle", 	HairStyle);
			ToXml_AppendField(index, "Eyes", 		Eyes);
			ToXml_AppendField(index, "Height", 		Height);
			ToXml_AppendField(index, "Age", 		Age);
			ToXml_AppendField(index, "Breasts", 	Breasts);
			ToXml_AppendField(index, "Nipples", 	Nipples);
			ToXml_AppendField(index, "Butt", 		Butt);
			ToXml_AppendField(index, "Race", 		Race);
			ToXml_AppendField(index, "Quality", 	Quality);
			ToXml_AppendField(index, "Skin", 		Skin);
			ToXml_AppendField(index, "Hips", 		Hips);
			ToXml_AppendField(index, "Legs", 		Legs);
			ToXml_AppendField(index, "FemaleGen", 	FemaleGen);
			ToXml_AppendField(index, "MaleGen", 	MaleGen);
			ToXml_AppendField(index, "Chest", 		Chest);
			ToXml_AppendField(index, "FacialHair", 	FacialHair);
			ToXml_AppendField(index, "Gender", 		Gender);

			//Done
			return index;
		}
		private void ToXml_AppendField(XmlElement index, string name, uint val)
		{
			XmlElement e = index.OwnerDocument.CreateElement(name);
			e.SetAttribute("value", val.ToString());
			index.AppendChild(e);
		}

		/// <summary>
		/// Copy the info in 'index' into our member vars.
		/// </summary>
		/// <param name="index">Source xml element.</param>
		public void FromXml(XmlElement index)
		{
			XmlElement e;
			foreach(XmlNode n in index.ChildNodes)
			{
				if (n.NodeType==XmlNodeType.Element)
				{
					e = (XmlElement)n;
					switch(e.Name)
					{
						case "Cat1":		Cat1		= e.Attributes["value"].Value.ToUInt32();	break;
						case "Cat2":		Cat2		= e.Attributes["value"].Value.ToUInt32();	break;
						case "Setting":		Setting 	= e.Attributes["value"].Value.ToByte();		break;
						case "Rating":		Rating		= e.Attributes["value"].Value.ToByte();		break;
						case "Quantity":	Quantity	= e.Attributes["value"].Value.ToByte();		break;
						case "Content":		Content		= e.Attributes["value"].Value.ToByte();		break;
						case "Build":		Build		= e.Attributes["value"].Value.ToByte();		break;
						case "HairColor":	HairColor	= e.Attributes["value"].Value.ToByte();		break;
						case "HairStyle":	HairStyle	= e.Attributes["value"].Value.ToByte();		break;
						case "Eyes":		Eyes		= e.Attributes["value"].Value.ToByte();		break;
						case "Height":		Height		= e.Attributes["value"].Value.ToByte();		break;
						case "Age":			Age			= e.Attributes["value"].Value.ToByte();		break;
						case "Breasts":		Breasts		= e.Attributes["value"].Value.ToByte();		break;
						case "Nipples":		Nipples		= e.Attributes["value"].Value.ToByte();		break;
						case "Butt":		Butt		= e.Attributes["value"].Value.ToByte();		break;
						case "Race":		Race		= e.Attributes["value"].Value.ToByte();		break;
						case "Quality":		Quality		= e.Attributes["value"].Value.ToByte();		break;
						case "Skin":		Skin 		= e.Attributes["value"].Value.ToByte();		break;
						case "Hips":		Hips 		= e.Attributes["value"].Value.ToByte();		break;
						case "Legs":		Legs 		= e.Attributes["value"].Value.ToByte();		break;
						case "FemaleGen":	FemaleGen	= e.Attributes["value"].Value.ToByte();		break;
						case "MaleGen":		MaleGen		= e.Attributes["value"].Value.ToByte();		break;
						case "Chest":		Chest		= e.Attributes["value"].Value.ToByte();		break;
						case "FacialHair":	FacialHair	= e.Attributes["value"].Value.ToByte();		break;
						case "Gender":		Gender		= e.Attributes["value"].Value.ToByte();		break;
						default:
							break;
					}
				}
			}
		}

		/// <summary>
		/// Convert all fields that are blank (0), to 0xFF.., so that
		/// fields the user didnt choose always compare true.
		/// </summary>
		public void PrepForCompareStandard()
		{
			//was catagory left blank?
			if (Cat1==0 && Cat2==0)
			{
				Cat1 = (uint)0xFFFFFFFF;
				Cat2 = (uint)0xFFFFFFFF;
			}

			//check each field
			if (Setting		== 0)	Setting 	= (byte)0xFF;
			if (Rating		== 0)	Rating		= (byte)0xFF;
			if (Quantity	== 0)	Quantity	= (byte)0xFF;
			if (Content		== 0)	Content		= (byte)0xFF;
			if (Build		== 0)	Build		= (byte)0xFF;
			if (HairColor	== 0)	HairColor	= (byte)0xFF;
			if (HairStyle	== 0)	HairStyle	= (byte)0xFF;
			if (Eyes		== 0)	Eyes		= (byte)0xFF;
			if (Height		== 0)	Height		= (byte)0xFF;
			if (Age			== 0)	Age			= (byte)0xFF;
			if (Breasts		== 0)	Breasts		= (byte)0xFF;
			if (Nipples		== 0)	Nipples		= (byte)0xFF;
			if (Butt		== 0)	Butt		= (byte)0xFF;
			if (Race		== 0)	Race		= (byte)0xFF;
			if (Quality		== 0)	Quality		= (byte)0xFF;
			if (Skin		== 0)	Skin 		= (byte)0xFF;
			if (Hips		== 0)	Hips 		= (byte)0xFF;
			if (Legs		== 0)	Legs 		= (byte)0xFF;
			if (FemaleGen	== 0)	FemaleGen	= (byte)0xFF;
			if (MaleGen		== 0)	MaleGen		= (byte)0xFF;
			if (Chest		== 0)	Chest		= (byte)0xFF;
			if (FacialHair	== 0)	FacialHair	= (byte)0xFF;
			if (Gender		== 0)	Gender		= (byte)0xFF;

		}

		/// <summary>
		/// Productions compare function
		/// </summary>
		/// <param name="a">Query Index</param>
		/// <param name="b">Media Index</param>
		public static bool CompareStandard(ref XMIndex a, ref XMIndex b)
		{
			//catagory.. all compare
			bool x;
			x = ((a.Cat1 & b.Cat1)==a.Cat1);
			x &= ((a.Cat2 & b.Cat2)==a.Cat2);
			if (!x) return false;

			//compare each standard field
			if ((a.Setting		!=0) && ((a.Setting		& b.Setting		)==0)) return false;
			if ((a.Rating		!=0) && ((a.Rating		& b.Rating		)==0)) return false;
			if ((a.Quantity		!=0) && ((a.Quantity	& b.Quantity	)==0)) return false;
			if ((a.Content		!=0) && ((a.Content		& b.Content		)==0)) return false;
			if ((a.Build		!=0) && ((a.Build		& b.Build		)==0)) return false;
			if ((a.HairColor	!=0) && ((a.HairColor	& b.HairColor	)==0)) return false;
			if ((a.HairStyle	!=0) && ((a.HairStyle	& b.HairStyle	)==0)) return false;
			if ((a.Eyes			!=0) && ((a.Eyes		& b.Eyes		)==0)) return false;
			if ((a.Height		!=0) && ((a.Height		& b.Height		)==0)) return false;
			if ((a.Age			!=0) && ((a.Age			& b.Age			)==0)) return false;
			if ((a.Breasts		!=0) && ((a.Breasts		& b.Breasts		)==0)) return false;
			if ((a.Nipples		!=0) && ((a.Nipples		& b.Nipples		)==0)) return false;
			if ((a.Butt			!=0) && ((a.Butt		& b.Butt		)==0)) return false;
			if ((a.Race			!=0) && ((a.Race		& b.Race		)==0)) return false;
			if ((a.Quality		!=0) && ((a.Quality		& b.Quality		)==0)) return false;
			if ((a.Skin			!=0) && ((a.Skin		& b.Skin		)==0)) return false;
			if ((a.Hips			!=0) && ((a.Hips		& b.Hips		)==0)) return false;
			if ((a.Legs			!=0) && ((a.Legs		& b.Legs		)==0)) return false;
			if ((a.FemaleGen	!=0) && ((a.FemaleGen	& b.FemaleGen	)==0)) return false;
			if ((a.MaleGen		!=0) && ((a.MaleGen		& b.MaleGen		)==0)) return false;
			if ((a.Chest		!=0) && ((a.Chest		& b.Chest		)==0)) return false;
			if ((a.FacialHair	!=0) && ((a.FacialHair	& b.FacialHair	)==0)) return false;
			if ((a.Gender		!=0) && ((a.Gender		& b.Gender		)==0)) return false;
			
			//everything passed
			return true;
		}

		//compare functions
		public static bool CompareExact(ref XMIndex a, ref XMIndex b)
		{
			//all bits must match, even 'off' bits
			//if (a.field1 != b.field1) return false;
			//if (a.field2 != b.field2) return false;

			//match
			return true;
		}
		public static bool CompareAny(ref XMIndex a, ref XMIndex b)
		{
			//at least one 'on' bit must match
			if ((a.Cat1			& b.Cat1		)!=0) return true;
			if ((a.Cat2			& b.Cat2		)!=0) return true;
			if ((a.Setting		& b.Setting		)!=0) return true;
			if ((a.Rating		& b.Rating		)!=0) return true;
			if ((a.Quantity		& b.Quantity	)!=0) return true;
			if ((a.Content		& b.Content		)!=0) return true;
			if ((a.Build		& b.Build		)!=0) return true;
			if ((a.HairColor	& b.HairColor	)!=0) return true;
			if ((a.HairStyle	& b.HairStyle	)!=0) return true;
			if ((a.Eyes			& b.Eyes		)!=0) return true;
			if ((a.Height		& b.Height		)!=0) return true;
			if ((a.Age			& b.Age			)!=0) return true;
			if ((a.Breasts		& b.Breasts		)!=0) return true;
			if ((a.Nipples		& b.Nipples		)!=0) return true;
			if ((a.Butt			& b.Butt		)!=0) return true;
			if ((a.Race			& b.Race		)!=0) return true;
			if ((a.Quality		& b.Quality		)!=0) return true;
			if ((a.Skin			& b.Skin		)!=0) return true;
			if ((a.Hips			& b.Hips		)!=0) return true;
			if ((a.Legs			& b.Legs		)!=0) return true;
			if ((a.FemaleGen	& b.FemaleGen	)!=0) return true;
			if ((a.MaleGen		& b.MaleGen		)!=0) return true;
			if ((a.Chest		& b.Chest		)!=0) return true;
			if ((a.FacialHair	& b.FacialHair	)!=0) return true;
			if ((a.Gender		& b.Gender		)!=0) return true;

			//no match
			return false;
		}
		/// <summary>
		/// All in a must exist in b, but not all in b must
		/// exist in a. NOTE: Parameters are NOT transative!
		/// </summary>
		/// <param name="a">User Mask</param>
		/// <param name="b">Media Index</param>
		public static bool CompareAll(ref XMIndex a, ref XMIndex b)
		{
			//all bits in a must be in b, but b may have bits
			//not in a
			//if ( (a.field1 & b.field1) != a.field1) return false;
			//if ( (a.field2 & b.field2) != a.field2) return false;

			return false;
		}
		public static bool CompareN(ref XMIndex a, ref XMIndex b, int n)
		{
			//do any match? this is so much faster than the 
			//actual n compare, its worth it to try
			//ASSUMED: n > 0
			if(!XMIndex.CompareAny(ref a, ref b))
			{
				return false;
			}
			
			//at least n 'on' bits must match
			//CompareN_Inner(a.field1, b.field1, ref n);
			//CompareN_Inner(a.field2, b.field2, ref n);

			//match if 
			return (n<1);
		}

		internal static void CompareN_Inner(uint a, uint b, ref int n)
		{
			//decrement n for each matching bit between a, b
			uint x = a & b;
			for (byte i=0;i<32;i++)
				if ((x&(1<<i))!=0) n--;
		}
	}

	/// <summary>
	/// Class representing an entire query of the
	/// media database.
	/// </summary>
	public class XMQuery
	{
		//index masks
		public XMIndex QueryMask;
		public XMIndex RejectionMask;

		//dimension restrictions
		public uint MinWidth, MaxWidth;
		public uint MinHeight, MaxHeight;
		public uint MinSize, MaxSize;

		//misc
		public bool Filter = true;			//if false, return records even
											//if no computer have them

		/// <summary>
		/// Convert an xml fragment into a query structure.
		/// </summary>
		/// <param name="query">Xml fragment.</param>
		public void FromXml(XmlElement query)
		{
			//get range checks
			foreach(XmlAttribute a in query.Attributes)
			{
				switch(a.Name.ToLower())
				{
					case "minwidth": 	MinWidth = a.Value.ToUInt32();	break;
					case "maxwidth": 	MaxWidth = a.Value.ToUInt32();	break;
					case "minheight": 	MinHeight = a.Value.ToUInt32();	break;
					case "maxheight": 	MaxHeight = a.Value.ToUInt32(); break;
					case "minsize": 	MinSize = a.Value.ToUInt32();	break;
					case "maxsize": 	MaxSize = a.Value.ToUInt32();	break;
					case "filter":
						if (a.Value.ToString()=="none")
						{
							Filter = false;
						}
						break;
					default:
						break;
				}
			}

			//get query and rejection mask
			XmlElement e;
			foreach(XmlNode n in query.ChildNodes)
			{
				if (n.NodeType==XmlNodeType.Element)
				{
					e = (XmlElement)n;
					switch(e.Name)
					{
						case "queryindex":		QueryMask.FromXml(e);		break;
						case "rejectionindex":	RejectionMask.FromXml(e);	break;
						default:
							break;
					}
				}
			}
		}

		//test against an item
		public bool Test(XMMediaItem item)
		{
			//first test dimensions
			if (item.Width < MinWidth) return false;
			if (item.Height < MinHeight) return false;
			if (item.FileSize < MinSize) return false;
			if (MaxWidth!=0) if (item.Width > MaxWidth) return false;
			if (MaxHeight!=0) if (item.Height > MaxHeight) return false;
			if (MaxSize!=0) if (item.FileSize > MaxSize) return false;

			//now test indices
			bool retval = false;
			for (uint i=0;i<item.Indices.Length;i++)
			{
				//if any index matches the rejection mask, we need 
				//to throw the whole picture out
				if (XMIndex.CompareAny(ref RejectionMask, ref item.Indices[i]))
					return false;

				//at least one index must match.. but we still need to go
				//through all indices to see if any get rejected (see above)
				if (XMIndex.CompareStandard(ref QueryMask, ref item.Indices[i]))
					retval = true;
			}

			//all indices tested
			return retval;
		}

		public static XmlElement ResultsToXml(ObjectList res, XmlDocument xml)
		{
			XmlElement e = xml.CreateElement("results");
			foreach(XMMediaItem i in res)
			{
				e.AppendChild(i.ResultsToXml(xml));
			}
			return e;
		}
	}

	/// <summary>
	/// Builds a cache of all media and indices, then
	/// pulls queries off the query engines queue, and
	/// processes them, including returning results.
	/// </summary>
	public class XMQueryProcessor
	{
		//External services
		private XMAdo mADO;
		private XMQueryEngine mEngine;
		private Thread mThread;

		//Track processor state
		private bool mAbort;
		public enum State
		{
			Stopped,
			Starting,
			Started,
			Stopping
		}
		private State mState = State.Stopped;

		//Constructor
		public XMQueryProcessor(XMQueryEngine engine)
		{
			//store engine ref
			mEngine = engine;
		}

		//Processor "Core"
		private void Alpha()
		{
			//Get ref to media list
			XMMediaItem item = mEngine.GetFullDB();
			XMMediaItem start;

			//Open a connection to db
			mADO = new XMAdo();
			if (!mADO.EnsureConnection())
				throw new Exception("Failed to open database connection.");

			//Processor has started
			lock(this)
			{
				mState = State.Started;
			}

			//Loop until we receive the abort signal
			//float ts;
			XMMessage msg, retmsg;
			ObjectList ret;
			uint c;
			while (!mAbort)
			{
				//Try to get a message
				msg = mEngine.DequeueQuery();
				if (msg!=null)
				{
					//record time
					//System.Diagnostics.Counter.GetElapsed();

					//clear the results
					ret = new ObjectList(20);
					c = 0;

					//loop until:
					//	* we find enough results
					//	* we search too many records
					//	* we search EVERY record
					start = item;
					item = item.Next;
					while ((c < 5000) && (ret.Count < 20) && (item!=start))
					{
						//keep picture?
						//System.Diagnostics.Debug.WriteLine("Testing " + item.Md5);
						if (msg.Query.Test(item))
						{
							//does anyone have this online?
							lock(item)
							{
								if ((item.GetServersCount(mADO)>0) || (!msg.Query.Filter))
								{
									//System.Diagnostics.Debug.WriteLine("Adding " + item.Md5);
									ret.Add(item);
								}
							}
						}
						
						//next record
						item = item.Next;
						c++;
					}

					//return results to client
					retmsg = msg.CreateReply();
					msg.QueryResults = ret;
					msg.Send();

					//this connection is no longer
					//processing a query
					msg.Connection.InQuery = false;

					//output time to console
					//ts = System.Diagnostics.Counter.GetElapsed();
					//System.Diagnostics.Trace.WriteLine("Query time: "+ts);
				}
				else
				{
					//there were no messages, pause
					//for jsut a moment
					Thread.Sleep(1000);
				}

			}

			//When we exit, thread ends and state
			//becomes "stopped"
			lock(this)
			{
				mState = State.Stopped;
			}
		}

		//Public interface - thread safe
		public void Start()
		{
			//Check for valid state
			if (mState!=State.Stopped && mState!=State.Stopping) return;
			lock(this)
			{
				//Set new state
				mAbort = false;
				mState = State.Starting;
			
				//Start thread
				mThread = new Thread(new ThreadStart(this.Alpha));
				mThread.Start();
			}
		}

		public void Stop()
		{
			lock(this)
			{
				//Check for valid state
				if (mState!=State.Started &&
					mState!=State.Starting)
					return;

				//Set new state, and set the abort flag
				mState = State.Stopping;
				mAbort = true;
			}
		}

		//State info
		public XMQueryProcessor.State CurrentState
		{
			get
			{
				State temp;
				lock(this)
				{
					temp = mState;
				}
				return temp;
			}
		}

	}

	/// <summary>
	/// Manage the query processors, and track the
	/// queue of queries to be processed.
	/// </summary>
	public class XMQueryEngine
	{
		//Data used by the processor
		private XMAdo mADO = new XMAdo();
		private XMMediaItem mFirstItem;
		public XMMediaItem GetFullDB()
		{
			return mFirstItem;
		}

		//Processor management
		private ObjectList mProcessors = new ObjectList();
		//private int mInitialProcessors = 1;
		public int ProcessorCount
		{
			get
			{
				return mProcessors.Count;
			}
		}
		public void SetProcessorCount(int newCount)
		{
			//expand or remove?
			lock(this)
			{
				if (newCount==mProcessors.Count)
					return;
				if (newCount < mProcessors.Count)
				{
					while (newCount < mProcessors.Count)
					{
						//Stop processor, and remove ref
						((XMQueryProcessor)mProcessors[1]).Stop();
						mProcessors.RemoveAt(1);
					}
				}
				else
				{
					XMQueryProcessor q;
					while (newCount > mProcessors.Count)
					{
						//Start a new processor
						q = new XMQueryProcessor(this);
						q.Start();
						mProcessors.Add(q);
					}
				}
			}
		}

		//Queue Management
		private Queue mQueue = new Queue();
		public int EnqueueQuery(XMMessage q)
		{
			lock(this)
			{
				mQueue.Enqueue(q);
			}
			return mQueue.Count;
		}
		public XMMessage DequeueQuery()
		{
			XMMessage q = null;
			lock(this)
			{
				if (mQueue.Count > 0)
				{
					q = (XMMessage)mQueue.Dequeue();
				}
			}
			return q;
		}
		public void Rebuild()
		{
			lock(this)
			{
				//stop each processor
				for (int i=1;i<=mProcessors.Count;i++)
				{
					((XMQueryProcessor)mProcessors[i]).Stop();
				}

				//wait for them to finish current queries
				bool cont = true, allstop;
				while (cont)
				{
					//test each processor
					allstop = true;
					for (int i=1;(i<=mProcessors.Count) && (allstop);i++)
					{
						if (((XMQueryProcessor)mProcessors[i]).CurrentState!=
							XMQueryProcessor.State.Stopped)
						{
							allstop = false;
						}
					}

					//all stopped?
					if (allstop)
						cont = false;
					else
					{
						//wait a moment before trying again
						Thread.Sleep(0);
					}
				}

				//all processors stopped, clear the collection
				//and do a gc, to be safe
				mProcessors.Clear();
				System.GC.Collect();

				//query
				string sql =
					"select mi.*, m.width, m.height, m.filesize " +
					"from mediaindex mi " +
					"inner join media m on m.md5 = mi.md5 " +
					"order by mi.md5";

				//get an ado recordset with the data we want
				if (!mADO.EnsureConnection()) throw new Exception("Connection failed.");
				ADODB._Recordset rs = mADO.SqlExec(sql);
			
				//build linked list
				mFirstItem = new XMMediaItem();
				XMMediaItem item = mFirstItem;

				//setup first item
				if (!rs.EOF)
					rs2mi(rs, item);

				//copy the rest of the reocrds
				while(!rs.EOF)
				{
					//setup new item, and link to and from the last one
					item.Next = new XMMediaItem();
					item.Next.Previous = item;
					item = item.Next;

					//build new item
					rs2mi(rs, item);
				}

				//loop the last record back to the first
				item.Next = mFirstItem;

				//db finished.. start all the processors
				for(int i=1;i<=mProcessors.Count;i++)
				{
					((XMQueryProcessor)mProcessors[i]).Start();
				}
			}
		}

		public static void rs2mi(ADODB._Recordset rs, XMMediaItem mi)
		{
			//get basic data on this item
			XMGuid md5 = new XMGuid((byte[])rs.Fields["md5"].Value);
			mi.Md5 = md5.ToString();
			mi.Width = rs.Fields["width"].Value.ToString().ToInt32();
			mi.Height = rs.Fields["height"].Value.ToString().ToInt32();
			mi.FileSize = rs.Fields["filesize"].Value.ToString().ToInt32();

			//get each index until we come to a new md5 or eof
			ObjectList temp = new ObjectList();
			XMIndex i;
			while ((!rs.EOF)&&(new XMGuid((byte[])rs.Fields["md5"].Value).Equals(md5)))
			{
				//load data from recordset
				i = new XMIndex();
				i.Age			= rs.Fields["_Age"]			.Value.ToString().ToByte();
				i.Breasts		= rs.Fields["_Breasts"]		.Value.ToString().ToByte();
				i.Build			= rs.Fields["_Build"]		.Value.ToString().ToByte();
				i.Butt			= rs.Fields["_Butt"]		.Value.ToString().ToByte();
				i.Cat1			= rs.Fields["_Catagory"]	.Value.ToString().ToUInt32();
				i.Cat2			= rs.Fields["_Catagory2"]	.Value.ToString().ToUInt32();
				i.Chest			= rs.Fields["_Chest"]		.Value.ToString().ToByte();
				i.Content		= rs.Fields["_Content"]		.Value.ToString().ToByte();
				i.Eyes			= rs.Fields["_Eyes"]		.Value.ToString().ToByte();
				i.FacialHair	= rs.Fields["_FacialHair"]	.Value.ToString().ToByte();
				i.FemaleGen		= rs.Fields["_FemaleGen"]	.Value.ToString().ToByte();
				i.Gender		= rs.Fields["_Content"]		.Value.ToString().ToByte();
				i.HairColor		= rs.Fields["_HairColor"]	.Value.ToString().ToByte();
				i.HairStyle		= rs.Fields["_HairStyle"]	.Value.ToString().ToByte();
				i.Height		= rs.Fields["_Height"]		.Value.ToString().ToByte();
				i.Hips			= rs.Fields["_Hips"]		.Value.ToString().ToByte();
				i.Legs			= rs.Fields["_Legs"]		.Value.ToString().ToByte();
				i.MaleGen		= rs.Fields["_MaleGen"]		.Value.ToString().ToByte();
				i.Nipples		= rs.Fields["_Nipples"]		.Value.ToString().ToByte();
				i.Quality		= rs.Fields["_Quality"]		.Value.ToString().ToByte();
				i.Quantity		= rs.Fields["_Quantity"]	.Value.ToString().ToByte();
				i.Race			= rs.Fields["_Race"]		.Value.ToString().ToByte();
				i.Rating		= rs.Fields["_Rating"]		.Value.ToString().ToByte();
				i.Setting		= rs.Fields["_Setting"]		.Value.ToString().ToByte();
				i.Skin			= rs.Fields["_Skin"]		.Value.ToString().ToByte();

				//add to temp store
				temp.Add(i);

				//next record
				rs.MoveNext();
			}

			//copy index from temp location into mediaitem
			mi.Indices = new XMIndex[temp.Count];
			temp.CopyTo(mi.Indices, 0);
		}

	}
}