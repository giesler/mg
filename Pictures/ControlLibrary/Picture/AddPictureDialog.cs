using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for fAddPictures.
	/// </summary>
	public class AddPictureDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblFiles;
		private System.Windows.Forms.ListBox lstFiles;
		private System.Windows.Forms.Button btnAddPictures;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialogPic;
        private System.Windows.Forms.Button btnRemovePictures;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private msn2.net.Pictures.Controls.CategoryPicker categoryPicker1;
		private msn2.net.Pictures.Controls.GroupPicker groupPicker1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DateTimePicker dtPictureDate;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.RadioButton radioCustomDate;
		private System.Windows.Forms.RadioButton radioPictureDate;
		private System.Windows.Forms.Label label1;
        private msn2.net.Pictures.Controls.PersonSelect personSelect1;
        private System.Windows.Forms.CheckBox checkboxSortList;
        /// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private CheckBox autoRotate;
        private fStatus stat;
        private PicContext context;

		public AddPictureDialog(PicContext context)
		{
            this.context = context;

            InitializeComponent();

            // add everyone group by default
			groupPicker1.AddSelectedGroup(1);

            this.AcceptButton = this.btnAdd;
            this.CancelButton = this.btnCancel;
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.personSelect1 = new msn2.net.Pictures.Controls.PersonSelect();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupPicker1 = new msn2.net.Pictures.Controls.GroupPicker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.autoRotate = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dtPictureDate = new System.Windows.Forms.DateTimePicker();
            this.radioCustomDate = new System.Windows.Forms.RadioButton();
            this.radioPictureDate = new System.Windows.Forms.RadioButton();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.categoryPicker1 = new msn2.net.Pictures.Controls.CategoryPicker();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.lblFiles = new System.Windows.Forms.Label();
            this.btnAddPictures = new System.Windows.Forms.Button();
            this.btnRemovePictures = new System.Windows.Forms.Button();
            this.openFileDialogPic = new System.Windows.Forms.OpenFileDialog();
            this.checkboxSortList = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(421, 430);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // personSelect1
            // 
            this.personSelect1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personSelect1.Location = new System.Drawing.Point(96, 91);
            this.personSelect1.Name = "personSelect1";
            this.personSelect1.SelectedPerson = null;
            this.personSelect1.SelectedPersonID = 0;
            this.personSelect1.Size = new System.Drawing.Size(373, 21);
            this.personSelect1.TabIndex = 9;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(341, 430);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupPicker1
            // 
            this.groupPicker1.AllowRemoveEveryone = true;
            this.groupPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPicker1.Location = new System.Drawing.Point(0, 0);
            this.groupPicker1.Name = "groupPicker1";
            this.groupPicker1.RightListColumnHeaderText = "Shared With";
            this.groupPicker1.Size = new System.Drawing.Size(485, 236);
            this.groupPicker1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(8, 160);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(493, 262);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.autoRotate);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.personSelect1);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(485, 236);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Details";
            // 
            // autoRotate
            // 
            this.autoRotate.AutoSize = true;
            this.autoRotate.Location = new System.Drawing.Point(16, 121);
            this.autoRotate.Name = "autoRotate";
            this.autoRotate.Size = new System.Drawing.Size(158, 17);
            this.autoRotate.TabIndex = 10;
            this.autoRotate.Text = "&Automatically rotate pictures";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Picture By:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.dtPictureDate);
            this.groupBox1.Controls.Add(this.radioCustomDate);
            this.groupBox1.Controls.Add(this.radioPictureDate);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(469, 76);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Picture Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Location = new System.Drawing.Point(150, 43);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(285, 20);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // dtPictureDate
            // 
            this.dtPictureDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtPictureDate.Location = new System.Drawing.Point(88, 160);
            this.dtPictureDate.Name = "dtPictureDate";
            this.dtPictureDate.Size = new System.Drawing.Size(269, 20);
            this.dtPictureDate.TabIndex = 4;
            // 
            // radioCustomDate
            // 
            this.radioCustomDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioCustomDate.Location = new System.Drawing.Point(17, 43);
            this.radioCustomDate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.radioCustomDate.Name = "radioCustomDate";
            this.radioCustomDate.Size = new System.Drawing.Size(165, 24);
            this.radioCustomDate.TabIndex = 1;
            this.radioCustomDate.Text = "Use the following date:";
            this.radioCustomDate.CheckedChanged += new System.EventHandler(this.radioCustomDate_CheckedChanged);
            // 
            // radioPictureDate
            // 
            this.radioPictureDate.Checked = true;
            this.radioPictureDate.Location = new System.Drawing.Point(16, 20);
            this.radioPictureDate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.radioPictureDate.Name = "radioPictureDate";
            this.radioPictureDate.Size = new System.Drawing.Size(408, 24);
            this.radioPictureDate.TabIndex = 0;
            this.radioPictureDate.TabStop = true;
            this.radioPictureDate.Text = "Use date on picture file (Date Picture Taken / Last Modified date)";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.categoryPicker1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(485, 236);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Categories";
            // 
            // categoryPicker1
            // 
            this.categoryPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryPicker1.Location = new System.Drawing.Point(0, 0);
            this.categoryPicker1.Name = "categoryPicker1";
            this.categoryPicker1.Size = new System.Drawing.Size(485, 236);
            this.categoryPicker1.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupPicker1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(485, 236);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Share With";
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(8, 24);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFiles.Size = new System.Drawing.Size(493, 95);
            this.lstFiles.TabIndex = 1;
            // 
            // lblFiles
            // 
            this.lblFiles.Location = new System.Drawing.Point(8, 8);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(100, 23);
            this.lblFiles.TabIndex = 0;
            this.lblFiles.Text = "Pictures to add:";
            // 
            // btnAddPictures
            // 
            this.btnAddPictures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPictures.Location = new System.Drawing.Point(341, 128);
            this.btnAddPictures.Name = "btnAddPictures";
            this.btnAddPictures.Size = new System.Drawing.Size(72, 23);
            this.btnAddPictures.TabIndex = 2;
            this.btnAddPictures.Text = "A&dd";
            this.btnAddPictures.Click += new System.EventHandler(this.btnAddPictures_Click);
            // 
            // btnRemovePictures
            // 
            this.btnRemovePictures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePictures.Location = new System.Drawing.Point(421, 128);
            this.btnRemovePictures.Name = "btnRemovePictures";
            this.btnRemovePictures.Size = new System.Drawing.Size(72, 23);
            this.btnRemovePictures.TabIndex = 2;
            this.btnRemovePictures.Text = "Remove";
            this.btnRemovePictures.Click += new System.EventHandler(this.btnRemovePictures_Click);
            // 
            // openFileDialogPic
            // 
            this.openFileDialogPic.DefaultExt = "jpg";
            this.openFileDialogPic.Filter = "Supported Graphics Files (*.jpg, *.tif)|*.tif;*.jpg|JPEG Files (*.jpg)|*.jpg|TIF " +
                "Files (*.tif)|*.tif";
            this.openFileDialogPic.Multiselect = true;
            this.openFileDialogPic.Title = "Select picture(s):";
            // 
            // checkboxSortList
            // 
            this.checkboxSortList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkboxSortList.Checked = true;
            this.checkboxSortList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxSortList.Location = new System.Drawing.Point(16, 128);
            this.checkboxSortList.Name = "checkboxSortList";
            this.checkboxSortList.Size = new System.Drawing.Size(245, 24);
            this.checkboxSortList.TabIndex = 11;
            this.checkboxSortList.Text = "Sort list by filename before adding";
            // 
            // AddPictureDialog
            // 
            this.AcceptButton = this.btnAdd;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(517, 460);
            this.Controls.Add(this.checkboxSortList);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnRemovePictures);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnAddPictures);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.lblFiles);
            this.Name = "AddPictureDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Pictures";
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            this.DialogResult = DialogResult.Cancel;
            this.Close();
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			
			// make sure we have files
			if (lstFiles.Items.Count == 0) 
			{
				MessageBox.Show("You must add files to add them to the database.", "Add Pictures", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			// sort files if we are supposed to
			if (checkboxSortList.Checked)
				SortPictureList();

            // disable controls
			foreach (Control c in this.Controls)
			{
				c.Enabled = false;
			}

			// open a status window, file copying could take time
			stat = new fStatus("Adding pictures...", lstFiles.Items.Count);
			stat.Show();
			stat.Refresh();

			Thread t	= new Thread(new ThreadStart(ImportFile));
			t.Start();
		}

		private void ImportFile()
		{

			// figure out the current max PictureSort val
            SqlConnection cn = new SqlConnection(this.context.Config.ConnectionString);
            cn.Open();
			SqlCommand cmdMaxVal = new SqlCommand("select Max(PictureSort) from Picture", cn);
			SqlDataReader drMaxVal = cmdMaxVal.ExecuteReader(CommandBehavior.SingleResult);
			int intCurPicSort = 1;
			if (drMaxVal.Read())
				intCurPicSort = drMaxVal.GetInt32(0);
			drMaxVal.Close();
            cn.Close();

			// add pictures to dataset
			int intFile = 0;
            List<Picture> added = new List<Picture>();
            foreach (string file in lstFiles.Items)
            {
                DateTime date = GetPictureDate(file);

                string dateString = date.Year.ToString("0000") + "\\" 
					+ date.Month.ToString("00") + "\\" + date.Day.ToString("00") + "\\";
                
				// Build the full directory to use, and create it if not there
				string targetDirectory = PicContext.Current.Config.PictureDirectory + dateString;
				if (!Directory.Exists(targetDirectory))
					Directory.CreateDirectory(targetDirectory);

				// increment sort val
				intCurPicSort++;

				// get filename extension
				String fileExtension = file.Substring(file.LastIndexOf("."));
				String targetFile = "";

				// figure out the target filename
				int i = 0;
				for (i = 0; i<1000; i++) 
				{
					targetFile = "tmp" + i.ToString("0000") + fileExtension;
                    if (!File.Exists(targetDirectory + targetFile))
						break;
				}
                
				// copy file to target
				File.Copy(file, targetDirectory + targetFile);

				// add row to dataset
				targetFile = dateString + targetFile;

                Picture picture = new Picture();
				
				picture.Filename		= targetFile;
				picture.PictureDate  = date;
				string fileTitle	    = file.Substring(file.LastIndexOf(@"\")+1);
				fileTitle			    = fileTitle.Substring(0, fileTitle.LastIndexOf("."));  // strip off extension
				picture.Title	    = fileTitle;
                picture.Publish      = true;
                picture.Rating = 50;
				picture.PictureSort  = intCurPicSort;
				picture.PictureAddDate = DateTime.Now;
				picture.PictureUpdateDate = DateTime.Now;

                if (personSelect1.SelectedPerson != null)
                {
                    picture.PictureBy = personSelect1.SelectedPersonID;
                }

                this.context.PictureManager.AddPicture(picture);
                added.Add(picture);

				// add categories to dataset
				foreach(int CategoryID in categoryPicker1.SelectedCategoryIds) 
				{
                    this.context.PictureManager.AddToCategory(picture.Id, CategoryID);
				}

				// add groups to dataset
				foreach (int groupId in groupPicker1.SelectedGroupIds) 
				{
                    this.context.PictureManager.AddToSecurityGroup(picture.Id, groupId);
				}

				// update progress bar
				intFile++;
				stat.Current = intFile;                
			}

			// now update picture names
			stat.StatusText = "Updating picture names...";
			foreach (Picture picture in added)
			{
				// figure out the current directory portion of filename
				string directory = picture.Filename.Substring(0, picture.Filename.LastIndexOf("\\")+1);
				string extension = picture.Filename.Substring(picture.Filename.LastIndexOf("."));

				// figure out new name
                string oldFilename = picture.Filename;
                string newFilename = directory + picture.PictureID.ToString("000000") + extension;

				// rename the file
				string picDirectory = this.context.Config.PictureDirectory;
                this.MoveFile(picDirectory + oldFilename, picDirectory + newFilename);

				// Allow move to complete
				Thread.Sleep(500);

                picture.Filename = newFilename;
                this.context.SubmitChanges();
			}

            this.autoRotateFlag = this.autoRotate.Checked;

            ThreadPool.QueueUserWorkItem(new WaitCallback(CreateThumbs), added);

            FinishImport();
        }

        private DateTime GetPictureDate(string file)
        {
            // Figure out the directory based on either file date/time or custom date
            Utilities.ExifMetadata ex = new Utilities.ExifMetadata();
            Utilities.ExifMetadata.Metadata data;
            DateTime date = DateTime.Now;
            if (radioCustomDate.Checked)
            {
                date = dateTimePicker1.Value;
            }
            else
            {
                // Try to read from metadata
                string dateTakenString = fMain.GetDatePictureTaken(file);
                if (dateTakenString != null)
                {
                    date = DateTime.Parse(dateTakenString);
                }
                else
                {
                    // Default to last modified time
                    date = File.GetLastWriteTime(file);

                    try
                    {
                        // Get 'date picture taken' if available
                        data = ex.GetExifMetadata(file);

                        if (data.DatePictureTaken.DisplayValue != null)
                        {
                            date = DateTime.Parse(data.DatePictureTaken.DisplayValue);
                        }
                    }
                    catch (Exception exc)
                    {
                        Trace.WriteLine(exc.Message);
                    }
                }
            }
            return date;
        }

        private bool autoRotateFlag = false;

        private void CreateThumbs(object o)
        {
            List<Picture> addedPictures = (List<Picture>)o;
            Utilities.ExifMetadata ex = new Utilities.ExifMetadata();

            // Now create the thumbnails
            ImageUtilities utils = new ImageUtilities();
            stat.Current = 0;
            stat.StatusText = "Creating thumbnail images...";
            foreach (Picture picture in addedPictures)
            {
                // Check rotate
                if (this.autoRotateFlag == true)
                {
                    stat.StatusText = "Rotating image...";
                    string file = PicContext.Current.Config.PictureDirectory + picture.Filename;
                    Utilities.ExifMetadata.Metadata data = ex.GetExifMetadata(file);
                    RotatePicture(picture, data.Orientation.Orientation);
                    stat.StatusText = "Creating thumbnail images...";
                }

                utils.CreateUpdateCache(picture.Id);
                stat.Current = stat.Current + 1;
            }

            // close dialog
            this.stat.Invoke(new MethodInvoker(CloseStatus));
        }

        private void CloseStatus()
        {
            stat.Close();
        }

        private void MoveFile(string source, string dest)
        {
            bool moved = false;
            int count = 0;

            while (!moved)
            {
                try
                {
                    File.Move(source, dest);
                    moved = true;
                }
                catch (IOException)
                {
                    if (count > 200)
                    {
                        MessageBox.Show("Unable to copy " + source + " to " + dest);
                        throw;
                    }
                    Thread.Sleep(100);
                }

                count++;
            }
        }

        private void FinishImport()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(FinishImport));
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void RotatePicture(Picture picture, Utilities.PictureOrientation orientation)
        {
            PictureManager pm = this.context.PictureManager;

            switch (orientation)
            {
                case Utilities.PictureOrientation.Rotate90:
                    pm.RotateImage(picture.Id, RotateFlipType.Rotate90FlipNone);
                    break;
                case Utilities.PictureOrientation.Rotate180:
                    pm.RotateImage(picture.Id, RotateFlipType.Rotate180FlipNone);
                    break;
                case Utilities.PictureOrientation.Roteate270:
                    pm.RotateImage(picture.Id, RotateFlipType.Rotate270FlipNone);
                    break;
            }
        }

        public void AddCategory(int categoryId)
		{
			categoryPicker1.AddSelectedCategory(categoryId);
		}

		private void btnAddPictures_Click(object sender, System.EventArgs e)
		{
			// show the browse dialog
			if (openFileDialogPic.ShowDialog(this) == DialogResult.Cancel)
				return;

			// add files if they aren't already added
			foreach(String strFile in openFileDialogPic.FileNames) 
			{
				if (!lstFiles.Items.Contains(strFile))
					lstFiles.Items.Add(strFile);
			}

		}

		private void btnRemovePictures_Click(object sender, System.EventArgs e)
		{
			while (lstFiles.SelectedItems.Count > 0)
				lstFiles.Items.Remove(lstFiles.SelectedItems[0]);
		}

		private void radioCustomDate_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioCustomDate.Checked) 
			{
				dateTimePicker1.Enabled = true;
			} 
			else 
			{
				dateTimePicker1.Enabled = false;
			}
		}
        
		private void SortPictureList()
		{
			ArrayList files = new ArrayList(lstFiles.Items.Count);
			foreach (string file in lstFiles.Items)
				files.Add(file);
            
			lstFiles.Items.Clear();
			files.Sort();

			foreach (string file in files) 
				lstFiles.Items.Add(file);
		}

        public Category ImportCategory
        {
            get
            {
                Category category = null;
                if (this.categoryPicker1.selectedCategories.Count > 0)
                {
                    category = PicContext.Current.CategoryManager.GetCategory(this.categoryPicker1.SelectedCategoryIds[0]);
                }
                return category;
            }
        }
	
	}
}
