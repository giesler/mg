using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Crownwood.Magic.Controls;
using Crownwood.Magic.Menus;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for EditPictureLink.
	/// </summary>
	public class EditPictureLink : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button button1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EditPictureLink()
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
			this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(48, 16);
			this.button1.TabIndex = 0;
			this.button1.Text = "Tasks...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// EditPictureLink
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.button1});
			this.Name = "EditPictureLink";
			this.Size = new System.Drawing.Size(48, 16);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			MenuCommand editPic			= new MenuCommand("Edit details...");

			MenuCommand addToCat		= new MenuCommand("Add to category...");
			
			MenuCommand rotate			= new MenuCommand("Rotate");
            MenuCommand rotate90		= new MenuCommand("&90 degrees");
			MenuCommand rotate180		= new MenuCommand("&180 degrees");
			MenuCommand rotate270		= new MenuCommand("&270 degrees");

			rotate.MenuCommands.Add(rotate90);
			rotate.MenuCommands.Add(rotate180);
			rotate.MenuCommands.Add(rotate270);

			MenuCommand tmp = new MenuCommand("nothing");

			// Create popup menu
			PopupMenu popup				= new PopupMenu();
			popup.MenuCommands.Add(editPic);
			popup.MenuCommands.Add(addToCat);

			popup.MenuCommands.Add(rotate);

			popup.MenuCommands.Add(tmp);

			// Show menu
			Point p						= button1.PointToScreen(new Point(button1.Left, button1.Top + button1.Height));
			MenuCommand selected		= popup.TrackPopup(p);

			if (selected != null)
			{
				if (selected == editPic)
				{
					ShowDetailsForm(Convert.ToInt32(pictureId));
				}
				else if (selected == addToCat)
				{

				}
				else if (selected == rotate90)
				{
					RotateImage(RotateFlipType.Rotate90FlipNone);
				}
				else if (selected == rotate180)
				{
					RotateImage(RotateFlipType.Rotate180FlipNone);
				}
				else if (selected == rotate270)
				{
					RotateImage(RotateFlipType.Rotate270FlipNone);
				}
			}
			

		}

		public void RotateImage(RotateFlipType rft)
		{
			fStatus status = new fStatus("Please wait while the image is rotated...");
			status.Max = 10;
			status.Show();

			picsvc.PictureManager pm			= new picsvc.PictureManager();
			status.Current = 2;
			picsvc.RotateFlipType picsRft		= (picsvc.RotateFlipType) Enum.Parse(typeof(picsvc.RotateFlipType), rft.ToString());
			pm.RotateImage(Convert.ToInt32(pictureId), picsRft);
            status.Current = 8;

			status.Dispose();
		}

		private void ShowDetailsForm(int pictureId)
		{
			fPicture f = new fPicture();

			// Load the selected picture
			f.LoadPicture(pictureId);
			f.Show();
		
		}

		private string pictureId;

		public string PictureId
		{
			get
			{
				return pictureId;
			}
			set
			{
				pictureId = value;
			}
		}
	}
}
