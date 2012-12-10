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
	/// Summary description for EditPictureLink.
	/// </summary>
	public class EditPictureLink : System.Windows.Forms.UserControl
	{
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
			// 
			// EditPictureLink
			// 
			this.Name = "EditPictureLink";
			this.Size = new System.Drawing.Size(8, 8);

		}
		#endregion

		public void SetAsCategoryPic(int personId)
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), personId);

			DataSet ds	= context.PictureManager.GetPictureCategories(pictureId);
			if (ds.Tables[0].Rows.Count == 0)
			{
				MessageBox.Show("Please add this picture to a category before setting it as a category picture.", "No category", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (ds.Tables[0].Rows.Count == 1)
			{
				int categoryId = (int) ds.Tables[0].Rows[0]["CategoryId"];
				context.CategoryManager.SetCategoryPictureId(categoryId, pictureId);

				MessageBox.Show("This picture is now the index picture for this category.", "Index Picture Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				// TODO: Implement multiple category select
				MessageBox.Show("This picture is in more then one category.  This feature is not yet implemented.");
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
			f.ShowDialog();
		}

		public void AddToCategory(int personId)
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), personId);

			// Get categoyr id
			fSelectCategory cat	= new fSelectCategory();
			if (cat.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			context.PictureManager.AddToCategory(pictureId, cat.SelectedCategory.CategoryID);
		}

		public void EditPicture(int personId)
		{
			// First log in
			PicContext context	= PicContext.Load(Msn2Config.Load(), personId);

			ShowDetailsForm(pictureId);
		}

		private int pictureId;

		public int PictureId
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
