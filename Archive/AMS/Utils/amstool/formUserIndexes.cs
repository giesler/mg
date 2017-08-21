using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace XMAdmin
{
	/// <summary>
	/// Summary description for formUserIndexes.
	/// </summary>
	public class formUserIndexes : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label labelFields;
		private System.Windows.Forms.Label labelIndexes;
		private System.Windows.Forms.ListBox listIndexes;
		private System.Windows.Forms.Label labelUserLabel;
		private System.Windows.Forms.Label labelUser;
		private System.Windows.Forms.Button buttonDisable;
		private System.Windows.Forms.ListBox listFields;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.PictureBox picturePreview;

		private User mUser;
		public formUserIndexes(User newUser)
		{
			//load controls
			InitializeComponent();

			//assign user
			mUser = newUser;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listFields = new System.Windows.Forms.ListBox();
			this.labelUser = new System.Windows.Forms.Label();
			this.picturePreview = new System.Windows.Forms.PictureBox();
			this.listIndexes = new System.Windows.Forms.ListBox();
			this.labelFields = new System.Windows.Forms.Label();
			this.labelUserLabel = new System.Windows.Forms.Label();
			this.buttonDisable = new System.Windows.Forms.Button();
			this.labelIndexes = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// listFields
			// 
			this.listFields.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.listFields.Location = new System.Drawing.Point(8, 304);
			this.listFields.Name = "listFields";
			this.listFields.Size = new System.Drawing.Size(200, 303);
			this.listFields.TabIndex = 3;
			this.listFields.SelectedIndexChanged += new System.EventHandler(this.listFields_SelectedIndexChanged);
			// 
			// labelUser
			// 
			this.labelUser.Location = new System.Drawing.Point(48, 8);
			this.labelUser.Name = "labelUser";
			this.labelUser.Size = new System.Drawing.Size(112, 16);
			this.labelUser.TabIndex = 5;
			this.labelUser.Text = "...";
			// 
			// picturePreview
			// 
			this.picturePreview.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.picturePreview.Location = new System.Drawing.Point(216, 16);
			this.picturePreview.Name = "picturePreview";
			this.picturePreview.Size = new System.Drawing.Size(578, 602);
			this.picturePreview.TabIndex = 2;
			this.picturePreview.TabStop = false;
			// 
			// listIndexes
			// 
			this.listIndexes.Location = new System.Drawing.Point(8, 64);
			this.listIndexes.Name = "listIndexes";
			this.listIndexes.Size = new System.Drawing.Size(200, 186);
			this.listIndexes.TabIndex = 1;
			this.listIndexes.SelectedIndexChanged += new System.EventHandler(this.listIndexes_SelectedIndexChanged);
			// 
			// labelFields
			// 
			this.labelFields.Location = new System.Drawing.Point(8, 288);
			this.labelFields.Name = "labelFields";
			this.labelFields.Size = new System.Drawing.Size(72, 16);
			this.labelFields.TabIndex = 2;
			this.labelFields.Text = "Fields";
			// 
			// labelUserLabel
			// 
			this.labelUserLabel.Location = new System.Drawing.Point(8, 8);
			this.labelUserLabel.Name = "labelUserLabel";
			this.labelUserLabel.Size = new System.Drawing.Size(32, 16);
			this.labelUserLabel.TabIndex = 4;
			this.labelUserLabel.Text = "User";
			// 
			// buttonDisable
			// 
			this.buttonDisable.Enabled = false;
			this.buttonDisable.Location = new System.Drawing.Point(8, 256);
			this.buttonDisable.Name = "buttonDisable";
			this.buttonDisable.Size = new System.Drawing.Size(80, 24);
			this.buttonDisable.TabIndex = 6;
			this.buttonDisable.Text = "Disable";
			this.buttonDisable.Click += new System.EventHandler(this.buttonDisable_Click);
			// 
			// labelIndexes
			// 
			this.labelIndexes.Location = new System.Drawing.Point(8, 48);
			this.labelIndexes.Name = "labelIndexes";
			this.labelIndexes.Size = new System.Drawing.Size(96, 16);
			this.labelIndexes.TabIndex = 0;
			this.labelIndexes.Text = "Indexes";
			// 
			// formUserIndexes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(810, 623);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonDisable,
																		  this.labelUser,
																		  this.labelUserLabel,
																		  this.labelFields,
																		  this.labelIndexes,
																		  this.listIndexes,
																		  this.listFields,
																		  this.picturePreview});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "formUserIndexes";
			this.Text = "Index Viewer";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.formUserIndexes_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void listIndexes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//clear everything if nothing selected
			if (listIndexes.SelectedItem == null)
			{
				listFields.Items.Clear();
				picturePreview.Image = null;
			}

			//find the selected index
			Index index = ((Index)((IndexWrapper)listIndexes.SelectedItem).index);

			//find the picture in our picture hasttable
			if (Pictures.Files.ContainsKey(index.Md5))
			{
				//display the picture
				picturePreview.Image =
					((Picture)Pictures.Files[index.Md5]).Image;
			}
			else
			{
				//we don't have the picture
				picturePreview.Image = null;
			}

			//setup the listFields box
			listFields.Items.Clear();

			_fillListFields(IndexField.Fields[ 0], index.Setting);
			_fillListFields(IndexField.Fields[ 1], index.Rating);
			_fillListFields(IndexField.Fields[ 2], index.Quantity);
			_fillListFields(IndexField.Fields[ 3], index.Content);
			_fillListFields(IndexField.Fields[ 4], index.Build);
			_fillListFields(IndexField.Fields[ 5], index.HairColor);
			_fillListFields(IndexField.Fields[ 6], index.HairStyle);
			_fillListFields(IndexField.Fields[ 7], index.Eyes);
			_fillListFields(IndexField.Fields[ 8], index.Height);
			_fillListFields(IndexField.Fields[ 9], index.Age);
			_fillListFields(IndexField.Fields[10], index.Breasts);
			_fillListFields(IndexField.Fields[11], index.Nipples);
			_fillListFields(IndexField.Fields[12], index.Butt);
			_fillListFields(IndexField.Fields[13], index.Cat1);
			_fillListFields(IndexField.Fields[14], index.Race);
			_fillListFields(IndexField.Fields[15], index.Quality);
			_fillListFields(IndexField.Fields[16], index.Skin);
			_fillListFields(IndexField.Fields[17], index.Hips);
			_fillListFields(IndexField.Fields[18], index.Legs);
			_fillListFields(IndexField.Fields[19], index.FemaleGen);
			_fillListFields(IndexField.Fields[20], index.MaleGen);
			_fillListFields(IndexField.Fields[21], index.Chest);
			_fillListFields(IndexField.Fields[22], index.FacialHair);
			_fillListFields(IndexField.Fields[23], index.Cat2);


		}

		private void _fillListFields(IndexField field, byte value)
		{
			_fillListFields(field, (uint)value);
		}

		private void _fillListFields(IndexField field, uint value)
		{
			StringBuilder sb = new StringBuilder(field.Name, 64);
			sb.Append(": ");
			bool any = false;
			for (int i=0;i<field.Values.Length;i++)
			{
				if ((1<<i & value)>0)
				{
					if (any)
						sb.Append(", ");
					sb.Append(field.Values[i]);
					any = true;
				}
			}
			if (any)
			{
				listFields.Items.Add(sb.ToString());
			}
		}

		private void buttonDisable_Click(object sender, System.EventArgs e)
		{

		}

		private void listFields_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
		}

		private void formUserIndexes_Load(object sender, System.EventArgs e)
		{
			//load indexes
			IndexWrapper wrap;
			int n = 1;
			foreach(Index i in mUser.Indexes)
			{
				//insert index, wrapped in struct that
				//can name the index properly
				wrap = new IndexWrapper();
				wrap.index = i;
				wrap.count = n;
				listIndexes.Items.Add(wrap);
				n++;
			}

			//show user name
			labelUser.Text = mUser.Login;
		}
	
		internal struct IndexWrapper
		{
			public int count;
			public Index index;
			public override string ToString()
			{
				return "Index " + count;
			}
		}
	}


}
