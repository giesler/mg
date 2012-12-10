using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using msn2.net.Pictures;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for EditCategoryLink.
	/// </summary>
	public class EditCategoryLink : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button button1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private bool signalRefresh;

		public EditCategoryLink()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private int categoryId;
		private int personId;

		public int CategoryId
		{
			get
			{
				return categoryId;
			}
			set
			{
				categoryId = value;
			}
		}

		public int PersonId
		{
			get
			{
				return personId;
			}
			set
			{
				personId = value;
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
// 
// button1
// 
            this.button1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 16);
            this.button1.TabIndex = 1;
            this.button1.Text = "Tasks...";
            this.button1.Click += new System.EventHandler(this.button1_Click);
// 
// EditCategoryLink
// 
            this.Controls.Add(this.button1);
            this.Name = "EditCategoryLink";
            this.Size = new System.Drawing.Size(62, 17);
            this.ResumeLayout(false);

        }
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
//			MenuCommand editPic				= new MenuCommand("Edit details");
//			MenuCommand addPics				= new MenuCommand("Add new pictures...");
//			MenuCommand saveSlideshow		= new MenuCommand("Save slideshow to computer...");
//			MenuCommand addCategory			= new MenuCommand("Add folder...");
//			MenuCommand publishToRecentCats	= new MenuCommand("Publish to home page...");
//
//			PopupMenu popup					= new PopupMenu();
//			popup.MenuCommands.Add(addCategory);
//			popup.MenuCommands.Add(editPic);
//			popup.MenuCommands.Add(addPics);
//			popup.MenuCommands.Add(saveSlideshow);
//			popup.MenuCommands.Add(publishToRecentCats);
//
//			Point p						= button1.PointToScreen(new Point(button1.Left, button1.Top + button1.Height));
//			MenuCommand selected	= popup.TrackPopup(p);
//
//			if (selected != null)
//			{
//				if (addCategory == selected)
//				{
//					fEditCategory ec = new fEditCategory();
//					ec.NewCategory(categoryId);
//					ec.ShowDialog();
//				}
//				else if (editPic == selected)
//				{
//					fEditCategory ec = new fEditCategory();
//					ec.CategoryID = categoryId;
//					ec.Show();
//				}
//				else if (selected == addPics)
//				{
//					AddPics();
//				}
//				else if (selected == saveSlideshow)
//				{
//					SaveSlideshow(175, 1);
//				}
//				else if (selected == publishToRecentCats)
//				{
//					PublishCat(categoryId);
//				}
//			}
		}

		private void AddPics()
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), personId);

			fAddPictures f = new fAddPictures();
			f.AddCategory(categoryId);
			f.ShowDialog();
		}

		public void SaveSlideshow(int categoryId, int personId)
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), personId);

			SaveSlideshow ss	= new SaveSlideshow(context, categoryId);
			ss.ShowDialog();
		}

		public void PublishCat(int categoryId)
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), 1);

			context.CategoryManager.PublishCategory(categoryId);

			MessageBox.Show("This category was published and will now appear on the home page of anyone with permission to view the category.", "Category Published", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public bool AddPicsToCategory(string pictureList, int personId)
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), personId);
			
			string[] ar = pictureList.Split(',');
			int []  ids = new int[ar.Length];
			for (int i = 0; i < ar.Length; i++)
			{
				ids[i] = int.Parse(ar[i]);
			}

			// Get a category
			fSelectCategory cat	= new fSelectCategory();
			if (cat.ShowDialog() == DialogResult.OK)
			{
				AddPicturesToCategory ap = new AddPicturesToCategory(ids, cat.SelectedCategory);
				if (ap.ShowDialog() == DialogResult.OK)
				{
					return true;
				}
			}	
		
			return false;
		}

		public bool AddGroupsToPics(string pictureList, int personId)
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), personId);
			
			string[] ar = pictureList.Split(',');
			int []  ids = new int[ar.Length];
			for (int i = 0; i < ar.Length; i++)
			{
				ids[i] = int.Parse(ar[i]);
			}

			fGroupSelect gs		= new fGroupSelect();
			if (gs.ShowDialog() == DialogResult.OK)
			{
				int groupId		= gs.SelectedGroup.GroupID;
				foreach (int id in ids)
				{
					context.PictureManager.AddToSecurityGroup(id, groupId);
				}

				return true;
			}

			return false;
		}

		public bool SignalRefresh
		{
			get
			{
				return signalRefresh;
			}
			set
			{
				signalRefresh = value;
			}
		}

	}
}
