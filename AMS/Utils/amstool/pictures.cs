using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace XMAdmin
{
	public class Picture
	{
		public XMGuid Md5;
		public string Path;
		public string Name
		{
			get
			{
				return (new FileInfo(Path)).Name;
			}
		}
		public Image Image
		{
			get
			{
				try 
				{
					return Image.FromFile(Path);
				}
				catch
				{
					return null;
				}
			}
		}
	}

	/// <summary>
	/// Store references to pictures on the hard drive
	/// </summary>
	public class Pictures
	{
		/// <summary>
		/// Pictures stored
		/// </summary>
		public static Hashtable Files = new Hashtable();

		public static string Path
		{
			get
			{
				return Application.UserAppDataPath + "\\pictures.bin";
			}
		}

		public static void Load()
		{
			//load the file
			BinaryReader bin = new BinaryReader(File.OpenRead(Path));

			//read the number of pictures
			int count = bin.ReadInt32();

			//read each picture
			Picture pic;
			while(count>0)
			{
				//create a new picture struct
				pic = new Picture();
				pic.Md5 = new XMGuid(bin.ReadBytes(16));
				pic.Path = bin.ReadString();

				//insert into hastable
				Files.Add(pic.Md5, pic);
				
				//next picture
				count--;
			}
			bin.Close();
		}

		public static void Save()
		{
			//create file
			BinaryWriter bin = new BinaryWriter(File.OpenWrite(Path));

			//write the number of records
			bin.Write(Files.Count);

			//read each record
			Picture pic;
			foreach(DictionaryEntry entry in Files)
			{
				pic = (Picture)entry.Value;
				bin.Write(pic.Md5.Buffer);
				bin.Write(pic.Path);
			}
			bin.Close();
		}

		public static void SearchPath(string search)
		{
			//search for all jpg/jpeg files in 'search'
			_SearchPath(new DirectoryInfo(search));
		}

		private static void _SearchPath(DirectoryInfo folder)
		{
			//search all files in folder
			XMMd5Engine md5;
			Picture pic;
			foreach(FileInfo file in folder.GetFiles())
			{
				//is it jpeg?
				if (file.Extension == ".jpeg" ||
					file.Extension == ".jpg")
				{
					//read data from file
					FileStream fs = File.OpenRead(file.FullName);
					byte[] buf = new byte[fs.Length];
					fs.Read(buf, 0, (int)fs.Length);
					fs.Close();

					//calculate md5
					md5 = new XMMd5Engine();
					md5.Update(buf, (uint)buf.Length);
					md5.Finish();

					//build picture
					pic = new Picture();
					pic.Md5 = md5.Md5;
					pic.Path = file.FullName;

					//insert picture (if new)
					if (!Files.ContainsKey(pic.Md5))
						Files.Add(pic.Md5, pic);	
				}
			}

			//search for subfolders
			foreach (DirectoryInfo subf in folder.GetDirectories())
			{
				//chicks dig recursive functions!
				_SearchPath(subf);
			}
		}
	}

	public class formChooseDirectory : Form
	{
		private System.Windows.Forms.TextBox textFolder;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelFolder;
		private System.Windows.Forms.Button buttonCancel;

		private void InitializeComponent()
		{
			this.textFolder = new System.Windows.Forms.TextBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelFolder = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textFolder
			// 
			this.textFolder.Location = new System.Drawing.Point(16, 32);
			this.textFolder.Name = "textFolder";
			this.textFolder.Size = new System.Drawing.Size(264, 20);
			this.textFolder.TabIndex = 1;
			this.textFolder.Text = "";
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(112, 80);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(80, 24);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "&OK";
			this.buttonOK.Click += new EventHandler(buttonOK_OnClick);
			// 
			// labelFolder
			// 
			this.labelFolder.Location = new System.Drawing.Point(16, 16);
			this.labelFolder.Name = "labelFolder";
			this.labelFolder.Size = new System.Drawing.Size(160, 16);
			this.labelFolder.TabIndex = 0;
			this.labelFolder.Text = "Folder";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(200, 80);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(80, 24);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "&Cancel";
			// 
			// formChooseDirectory
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(308, 121);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.textFolder,
																		  this.labelFolder});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "formChooseDirectory";
			this.ResumeLayout(false);

		}

		public void buttonOK_OnClick(object sender, EventArgs e)
		{
			//does path exist?
			if (!Directory.Exists(textFolder.Text))
			{
				MessageBox.Show(this, "Folder does not exist", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//success
			DialogResult = DialogResult.OK;
			Close();
		}

		public string Path
		{
			get
			{
				return textFolder.Text;
			}
		}

		public formChooseDirectory()
		{
			InitializeComponent();
		}
	}
}
