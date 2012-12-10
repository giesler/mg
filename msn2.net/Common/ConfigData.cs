using System;
using System.Collections;

namespace msn2.net.Common
{

	/// <summary>
	/// Base class for data stored in config system
	/// </summary>
	public class ConfigData
	{
		private Guid itemKey = Guid.Empty;
		protected string typeName = "ConfigData";
		private ShellActionList shellActionList = new ShellActionList();
		
		public virtual int IconIndex
		{
			get { return 0; }
		}

		public Guid ItemKey
		{
			get { return itemKey; }
			set { itemKey = value; }
		}

		public string TypeName
		{
			get { return typeName; }
		}

		/// <summary>
		/// List of actions associated with class
		/// </summary>
//		public ShellActionList ShellActionList
//		{
//			get
//			{
//				return shellActionList;
//			}
//		}

		protected void AddShellAction(ShellAction action)
		{
			shellActionList.Add(action);

			if (action.EventHandler != null)
			{
				//eventHandler += new EventHandler(AddItem);
			}
		}
	}

	public class ShellAction
	{
		private string name;
		private string description;
		private string help;
		private EventHandler eventHandler;

		public ShellAction(string name, string description, string help, EventHandler eventHandler)
		{
			this.name			= name;
			this.description	= description;
			this.help			= help;
			if (eventHandler != null)
			{
				this.eventHandler	+= eventHandler;
			}
		}

		#region Properties

		public string Name
		{
			get 
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		public string Help
		{
			get
			{
				return help;
			}
			set
			{
				help = value;
			}
		}

		public EventHandler EventHandler
		{
			get
			{
				return eventHandler;
			}
		}

		#endregion

	}

	public class ShellActionList: System.Collections.ReadOnlyCollectionBase
	{
		// TODO: Change to use hash table
		public ShellAction this[string name]
		{
			get
			{
				foreach (ShellAction action in InnerList)
				{
					if (action.Name == name)
					{
						return action;
					}
				}

				return null; 
			}
		}

		internal void Add(ShellAction action)
		{
			InnerList.Add(action);
		}
	}

}
