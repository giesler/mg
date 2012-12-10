using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;

namespace pics.Controls
{
	/// <summary>
	///	Displays a list of tasks, raises event when task clicked
	/// </summary>
	public class TaskListControl : System.Web.UI.UserControl
	{
		protected TaskListItemCollection taskListItems;

		public TaskListControl()
		{
			taskListItems			= new TaskListItemCollection();
		}

		public TaskListControl(TaskListItemCollection taskListItems)
		{
			this.taskListItems		= taskListItems;
		}

		protected override void Render(HtmlTextWriter output)
		{
			#region Table start

			output.WriteBeginTag("table");
			output.WriteAttribute("cellpadding", "0");
			output.WriteAttribute("cellspacing", "0");
			
			#endregion

			#region Output rows

			foreach (TaskListItem item in taskListItems)
			{
				output.WriteBeginTag("tr");
				
				output.WriteBeginTag("td");
				output.WriteAttribute("width", "9");

				PngImage itemImage		= new PngImage(item.ImageSrc, 9, 9);
				itemImage.RenderControl(output);
                
				output.WriteEndTag("td");

				output.WriteBeginTag("td");
				output.WriteAttribute("class", "sidebarTaskLink");

				output.WriteBeginTag("");


				output.WriteEndTag("tr");

			}

			#endregion

			#region Table end

			output.WriteEndTag("table");

			#endregion

			}

		public TaskListItemCollection TaskListItems
		{
			get
			{
				return taskListItems;
			}
			set
			{
				taskListItems		= value;
			}
		}

	}

	[Serializable]
	public class TaskListItemCollection: CollectionBase
	{
		public void Add(TaskListItem item)
		{
			InnerList.Add(item);
		}

		public TaskListItem this[int index]
		{
			get
			{
				return (TaskListItem) InnerList[index];
			}
		}
	}

	[Serializable]
	public class TaskListItem
	{
		public TaskListItem()
		{
		}

		public TaskListItem(string text, string imageSrc)
		{
			this.Text		= text;
			this.ImageSrc	= imageSrc;
		}

		public string ImageSrc;
		public string Text;
	}
}
