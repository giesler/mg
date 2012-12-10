using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Crownwood.Magic.Menus;

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
			this.button1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(48, 16);
			this.button1.TabIndex = 1;
			this.button1.Text = "Tasks...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// EditCategoryLink
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button1});
			this.Name = "EditCategoryLink";
			this.Size = new System.Drawing.Size(48, 16);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			MenuCommand editPic	= new MenuCommand("Edit details");
			MenuCommand addPics = new MenuCommand("Add new pictures...");
			MenuCommand saveSlideshow = new MenuCommand("Save slideshow to computer...");
			MenuCommand addCategory	= new MenuCommand("Add folder...");

			PopupMenu popup		= new PopupMenu();
			popup.MenuCommands.Add(addCategory);
			popup.MenuCommands.Add(editPic);
			popup.MenuCommands.Add(addPics);
			popup.MenuCommands.Add(saveSlideshow);

			Point p						= button1.PointToScreen(new Point(button1.Left, button1.Top + button1.Height));
			MenuCommand selected	= popup.TrackPopup(p);

			if (selected != null)
			{
				if (addCategory == selected)
				{
					fEditCategory ec = new fEditCategory();
					ec.NewCategory(categoryId);
					ec.ShowDialog();
				}
				else if (editPic == selected)
				{
					fEditCategory ec = new fEditCategory();
					ec.CategoryID = categoryId;
					ec.Show();
				}
				else if (selected == addPics)
				{
					fAddPictures f = new fAddPictures();
					f.AddCategory(categoryId);
					f.ShowDialog();
					signalRefresh = true;
				}
				else if (selected == saveSlideshow)
				{
					SaveSlideshow ss	= new SaveSlideshow();
					ss.CategoryId		= categoryId;
					ss.ShowDialog();
				}
			}
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
