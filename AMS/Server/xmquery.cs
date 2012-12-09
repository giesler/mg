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
					mServers[mServersCount].Speed = /*beta2:*/System.Convert.ToByte(rs.Fields["datarate"].Value);
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
						case "Cat1":		Cat1		= /*beta2:*/Convert.ToUInt32(e.Attributes["value"].Value);	break;
						case "Cat2":		Cat2		= /*beta2:*/Convert.ToUInt32(e.Attributes["value"].Value);	break;
						case "Setting":		Setting 	= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Rating":		Rating		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Quantity":	Quantity	= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Content":		Content		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Build":		Build		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "HairColor":	HairColor	= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "HairStyle":	HairStyle	= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Eyes":		Eyes		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Height":		Height		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Age":			Age			= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Breasts":		Breasts		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Nipples":		Nipples		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Butt":		Butt		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Race":		Race		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Quality":		Quality		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Skin":		Skin 		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Hips":		Hips 		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Legs":		Legs 		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "FemaleGen":	FemaleGen	= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "MaleGen":		MaleGen		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Chest":		Chest		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "FacialHair":	FacialHair	= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
						case "Gender":		Gender		= /*beta2:*/Convert.ToByte(e.Attributes["value"].Value);		break;
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
		private int mQueryCount = -1;
		private int mRejectionCount = -1;

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
					case "minwidth": 	MinWidth  = /*beta2:*/Convert.ToUInt32(a.Value);	break;
					case "maxwidth": 	MaxWidth  = /*beta2:*/Convert.ToUInt32(a.Value);	break;
					case "minheight": 	MinHeight = /*beta2:*/Convert.ToUInt32(a.Value);	break;
					case "maxheight": 	MaxHeight = /*beta2:*/Convert.ToUInt32(a.Value); break;
					case "minsize": 	MinSize   = /*beta2:*/Convert.ToUInt32(a.Value);	break;
					case "maxsize": 	MaxSize   = /*beta2:*/Convert.ToUInt32(a.Value);	break;
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

			//count the fields
			mQueryCount = QueryMask.CountFields(true);
			mRejectionCount = RejectionMask.CountFields(true);
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

			//if it is a blank query, return true
			if (mQueryCount == 0 &&
				mRejectionCount == 0)
			{
				return true;
			}
			
			//if there are no indexes, then return true if the querymask is empty
			if (item.Indices.Length == 0 &&
				mQueryCount == 0)
				return true;
			
			bool retval = false;
			for (uint i=0;i<item.Indices.Length;i++)
			{
				//if any index matches the rejection mask, we need 
				//to throw the whole picture out
				if (mRejectionCount > 0)
					if (XMIndex.CompareAny(ref RejectionMask, ref item.Indices[i]))
						return false;

				//at least one index must match.. but we still need to go
				//through all indices to see if any get rejected (see above)
				if (mQueryCount == 0)
					retval = true;
				else
					if (XMIndex.CompareStandard(ref QueryMask, ref item.Indices[i]))
						retval = true;
			}

			//all indices tested
			return retval;
		}

		public static XmlElement ResultsToXml(/*beta2:*/ArrayList res, XmlDocument xml)
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
			/*beta2:*/ArrayList ret;
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
					ret = new /*beta2:*/ArrayList(20);
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
						if (msg.Query.Test(item))
						{
							//does anyone have this online?
							lock(item)
							{
								if ((item.GetServersCount(mADO)>0) || (!msg.Query.Filter))
								{
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
					retmsg.QueryResults = ret;
					retmsg.Send();

					//this connection is no longer
					//processing a query
					msg.Connection.InQuery = false;

					//log results
					LogQuery(msg.Query, msg.Connection.UserID, ret.Count);

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

		public void LogQuery(XMQuery query, XMGuid user, int results)
		{
			string sql = String.Format(
				@"insert into querylog
				(UserId, Stamp, Results,

				_S_Setting, _S_Rating, _S_Quantity, _S_Content,
				_S_Catagory, _S_Build, _S_HairColor, _S_HairStyle,
				_S_Eyes, _S_Height, _S_Age, _S_Breasts,
				_S_Nipples, _S_Butt, _S_Race, _S_Quality,
				_S_Skin, _S_Hips, _S_Legs, _S_FemaleGen,
				_S_MaleGen, _S_Chest, _S_FacialHair, _S_Catagory2,

				_F_Setting, _F_Rating, _F_Quantity, _F_Content,
				_F_Catagory, _F_Build, _F_HairColor, _F_HairStyle,
				_F_Eyes, _F_Height, _F_Age, _F_Breasts,
				_F_Nipples, _F_Butt, _F_Race, _F_Quality,
				_F_Skin, _F_Hips, _F_Legs, _F_FemaleGen,
				_F_MaleGen, _F_Chest, _F_FacialHair, _F_Catagory2)

				values(
				{0}, '{1}', {2},

				{3}, {4}, {5}, {6},
				{7}, {8}, {9}, {10},
				{11}, {12}, {13}, {14},
				{15}, {16}, {17}, {18},
				{19}, {20}, {21}, {22},
				{23}, {24}, {25}, {26},

				{27}, {28}, {29}, {30},
				{31}, {32}, {33}, {34},
				{35}, {36}, {37}, {38},
				{39}, {40}, {41}, {42},
				{43}, {44}, {45}, {46},
				{47}, {48}, {49}, {50})",


				user.ToStringDB(), DateTime.Now, results,

				query.QueryMask.Setting, query.QueryMask.Rating, query.QueryMask.Quantity, query.QueryMask.Content,
				query.QueryMask.Cat1, query.QueryMask.Build, query.QueryMask.HairColor, query.QueryMask.HairStyle,
				query.QueryMask.Eyes, query.QueryMask.Height, query.QueryMask.Age, query.QueryMask.Breasts,
				query.QueryMask.Nipples, query.QueryMask.Butt, query.QueryMask.Race, query.QueryMask.Quality,
				query.QueryMask.Skin, query.QueryMask.Hips, query.QueryMask.Legs, query.QueryMask.FemaleGen,
				query.QueryMask.MaleGen, query.QueryMask.Chest, query.QueryMask.FacialHair, query.QueryMask.Cat2,

				query.RejectionMask.Setting, query.RejectionMask.Rating, query.RejectionMask.Quantity, query.RejectionMask.Content,
				query.RejectionMask.Cat1, query.RejectionMask.Build, query.RejectionMask.HairColor, query.RejectionMask.HairStyle,
				query.RejectionMask.Eyes, query.RejectionMask.Height, query.RejectionMask.Age, query.RejectionMask.Breasts,
				query.RejectionMask.Nipples, query.RejectionMask.Butt, query.RejectionMask.Race, query.RejectionMask.Quality,
				query.RejectionMask.Skin, query.RejectionMask.Hips, query.RejectionMask.Legs, query.RejectionMask.FemaleGen,
				query.RejectionMask.MaleGen, query.RejectionMask.Chest, query.RejectionMask.FacialHair, query.RejectionMask.Cat2);

			mADO.SqlExec(sql);
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
		private /*beta2:*/ArrayList mProcessors = new /*beta2:*/ArrayList();
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
			XMMediaItem item;

			//beging building
			DateTime start = DateTime.Now;

			//stop each processor
			foreach(XMQueryProcessor qp in mProcessors)
			{
				qp.Stop();
			}
			
			//wait for them to finish current queries
			bool allstop = false;
			while (!allstop)
			{
				//test each processor
				allstop = true;
				foreach(XMQueryProcessor qp in mProcessors)
				{
					if (qp.CurrentState != XMQueryProcessor.State.Stopped)
					{
						allstop = false;
					}
				}

				//wait a moment
				Thread.Sleep(0);
			}

			lock(this)
			{
				//query
				string sql =
					@"select m.md5 as 'media_md5', m.width, m.height, m.filesize, mi.*
					from media m
					left outer join mediaindex mi on mi.md5 = m.md5
					order by m.md5";

				//get an ado recordset with the data we want
				if (!mADO.EnsureConnection()) throw new Exception("Connection failed.");
				ADODB._Recordset rs = mADO.SqlExec(sql);
			
				//build linked list
				mFirstItem = new XMMediaItem();
				item = mFirstItem;

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

				//new db is build, do a gc to clear all the old crap
				System.GC.Collect();

				//db finished.. start all the processors
				foreach(XMQueryProcessor qp in mProcessors)
				{
					qp.Start();
				}
			}

			//print some output
			item = mFirstItem;
			int countIndexed = 0, countUnindexed = 0;
			do
			{
				if (item.Indices.Length > 0)
					countIndexed++;
				else
					countUnindexed++;
				item = item.Next;
			} while(item.Next != mFirstItem);
			TimeSpan elapsed = DateTime.Now - start;
			string str = String.Format(
				"Built media list.\nIndexed Media: {0}\nUnindexed Media: {1}\nElapsed Time: {2} seconds",
				countIndexed,
				countUnindexed,
				elapsed.TotalSeconds);
			XMLog.WriteLine(str, "QueryEngine");
		}

		public static void rs2mi(ADODB._Recordset rs, XMMediaItem mi)
		{
			//get basic data on this item
			XMGuid md5 = new XMGuid((byte[])rs.Fields["media_md5"].Value);
			mi.Md5 = md5.ToString();
			mi.Width = /*beta2:*/System.Convert.ToInt32(rs.Fields["width"].Value);
			mi.Height = /*beta2:*/System.Convert.ToInt32(rs.Fields["height"].Value);
			mi.FileSize = /*beta2:*/System.Convert.ToInt32(rs.Fields["filesize"].Value);

			//get each index until we come to a new md5 or eof
			/*beta2:*/ArrayList temp = new /*beta2:*/ArrayList();
			XMIndex i;
			while ((!rs.EOF)&&(new XMGuid((byte[])rs.Fields["media_md5"].Value).Equals(md5)))
			{
				//load data from recordset -- only if mediaindex.md5 is non-non
				if (rs.Fields["md5"].Value!=DBNull.Value)	//mi.md5
				{
					i = new XMIndex();
					i.Age			= /*beta2:*/Convert.ToByte(rs.Fields["_Age"]		.Value);
					i.Breasts		= /*beta2:*/Convert.ToByte(rs.Fields["_Breasts"]	.Value);
					i.Build			= /*beta2:*/Convert.ToByte(rs.Fields["_Build"]		.Value);
					i.Butt			= /*beta2:*/Convert.ToByte(rs.Fields["_Butt"]		.Value);
					i.Cat1			= /*beta2:*/Convert.ToUInt32(rs.Fields["_Catagory"]	.Value);
					i.Cat2			= /*beta2:*/Convert.ToUInt32(rs.Fields["_Catagory2"].Value);
					i.Chest			= /*beta2:*/Convert.ToByte(rs.Fields["_Chest"]		.Value);
					i.Content		= /*beta2:*/Convert.ToByte(rs.Fields["_Content"]	.Value);
					i.Eyes			= /*beta2:*/Convert.ToByte(rs.Fields["_Eyes"]		.Value);
					i.FacialHair	= /*beta2:*/Convert.ToByte(rs.Fields["_FacialHair"]	.Value);
					i.FemaleGen		= /*beta2:*/Convert.ToByte(rs.Fields["_FemaleGen"]	.Value);
					i.Gender		= /*beta2:*/Convert.ToByte(rs.Fields["_Content"]	.Value);
					i.HairColor		= /*beta2:*/Convert.ToByte(rs.Fields["_HairColor"]	.Value);
					i.HairStyle		= /*beta2:*/Convert.ToByte(rs.Fields["_HairStyle"]	.Value);
					i.Height		= /*beta2:*/Convert.ToByte(rs.Fields["_Height"]		.Value);
					i.Hips			= /*beta2:*/Convert.ToByte(rs.Fields["_Hips"]		.Value);
					i.Legs			= /*beta2:*/Convert.ToByte(rs.Fields["_Legs"]		.Value);
					i.MaleGen		= /*beta2:*/Convert.ToByte(rs.Fields["_MaleGen"]	.Value);
					i.Nipples		= /*beta2:*/Convert.ToByte(rs.Fields["_Nipples"]	.Value);
					i.Quality		= /*beta2:*/Convert.ToByte(rs.Fields["_Quality"]	.Value);
					i.Quantity		= /*beta2:*/Convert.ToByte(rs.Fields["_Quantity"]	.Value);
					i.Race			= /*beta2:*/Convert.ToByte(rs.Fields["_Race"]		.Value);
					i.Rating		= /*beta2:*/Convert.ToByte(rs.Fields["_Rating"]		.Value);
					i.Setting		= /*beta2:*/Convert.ToByte(rs.Fields["_Setting"]	.Value);
					i.Skin			= /*beta2:*/Convert.ToByte(rs.Fields["_Skin"]		.Value);

					//add to temp store
					temp.Add(i);
				}

				//next record
				rs.MoveNext();
			}

			//copy index from temp location into mediaitem
			mi.Indices = new XMIndex[temp.Count];
			temp.CopyTo(mi.Indices, 0);
		}

	}
}