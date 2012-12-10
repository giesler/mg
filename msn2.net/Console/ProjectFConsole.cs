using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using msn2.net.Controls;
using msn2.net.Configuration;

namespace msn2.net.ProjectF
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ProjectFConsole : msn2.net.Controls.ShellForm
	{
		#region Static Main Startup function

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			ProjectFConsole f = new ProjectFConsole();
			if (f.DialogResult != DialogResult.Cancel)
				Application.Run(f);
		}

		#endregion

		#region Declares

		private System.Windows.Forms.ListView listView1;
		private System.ComponentModel.Container components = null;
		private MessengerAPI.MessengerClass messenger = null;
		private Status status = null;
		private Crownwood.Magic.Docking.DockingManager dockManager = null;

		#endregion

		#region Constructor / Disposal

		public ProjectFConsole()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//			ShellForm.ShellForm_Added		+= new ShellForm_AddedDelegate(ShellForm_ItemAdded);
			//			ShellForm.ShellForm_Removed		+= new ShellForm_RemovedDelegate(ShellForm_ItemRemoved);

			// THE NEXT LINES OF CODE ARE FROM MAGIC LIBRARY 
			// Calculate the IDE background colour as only half as dark as the control colour
			int red = 255 - ((255 - SystemColors.Control.R) / 3);
			int green = 255 - ((255 - SystemColors.Control.G) / 3);
			int blue = 255 - ((255 - SystemColors.Control.B) / 3);
			listView1.BackColor = Color.FromArgb(red, green, blue);

			// Need to sign in as current user
			messenger = new MessengerAPI.MessengerClass();
			//messenger.OnSignin		+= new MessengerAPI.DMessengerEvents_OnSigninEventHandler(Messenger_SignIn);
			//messenger.OnSignout		+= new MessengerAPI.DMessengerEvents_OnSignoutEventHandler(Messenger_SignOut);

			dockManager = new Crownwood.Magic.Docking.DockingManager(this, Crownwood.Magic.Common.VisualStyle.ProjectF);

			// Make sure we are signed in, if not, sign in
			if (messenger.MyStatus == MessengerAPI.MISTATUS.MISTATUS_ONLINE)
			{
				SignIn(messenger.MySigninName);				
			}
			else
			{
				status = new Status("Waiting for you to sign in to Windows Messenger...");
			}
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.listView1 = new System.Windows.Forms.ListView();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// timerFadeOut
			// 
			this.timerFadeOut.Enabled = false;
			// 
			// timerFadeIn
			// 
			this.timerFadeIn.Enabled = false;
			// 
			// listView1
			// 
			this.listView1.CheckBoxes = true;
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(192, 158);
			this.listView1.TabIndex = 1;
			this.listView1.View = System.Windows.Forms.View.List;
			this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
			// 
			// ProjectFConsole
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(192, 158);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listView1});
			this.Name = "ProjectFConsole";
			this.RolledUp = true;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Project F";
			this.TitleVisible = true;
			this.TopMost = true;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Toggle visible on form

		private void listView1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			FormListViewItem item = (FormListViewItem) listView1.Items[e.Index];
			item.Visible = !item.Checked;
		}

		#endregion

		#region Form Closing

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (FormListViewItem item in listView1.Items)
			{
                item.AllowUnload = true;		
                item.Close();				
			}
		}

		#endregion

		#region Static shellform subscriptions

		private void ShellForm_ItemAdded(object sender, msn2.net.Controls.ShellFormAddedEventArgs e)
		{
			listView1.Items.Add(new FormListViewItem(e.Parent, e.ShellForm));
		}

		private void ShellForm_ItemRemoved(object sender, msn2.net.Controls.ShellFormRemovedEventArgs e)
		{
			foreach (FormListViewItem item in listView1.Items)
			{
				if (item.ShellForm == e.ShellForm)
				{
					listView1.Items.Remove(item);
					return;
				}
			}
		}

		#endregion

		#region FormListViewItem class

		private class FormListViewItem: ListViewItem
		{
			private msn2.net.Controls.ShellForm page;

			public FormListViewItem(msn2.net.Controls.ShellForm parent, msn2.net.Controls.ShellForm page): base(page.Text)
			{
				if (parent != null)
					parent.AddOwnedForm(page);

				this.page = page;
				page.VisibleChanged += new EventHandler(Page_VisibleChanged);
				page.Show();

                page.AllowUnload = false;				

			}

			private void Page_VisibleChanged(object sender, EventArgs e)
			{
				this.Checked = page.Visible;
			}

			public bool Visible
			{
				get { return page.Visible; }
				set { page.Visible = value; }
			}

			public bool AllowUnload
			{
				get { return page.AllowUnload; }
				set { page.AllowUnload = value; }
			}

			public void Close()
			{
				page.Close();
			}

			public ShellForm ShellForm
			{
				get { return page; }
			}
		}

		#endregion
	
		#region Messenger Event Handlers
		
		private void Messenger_SignIn(int hr)
		{
			// Check if we are signed in
			if (hr == Convert.ToInt32(MessengerAPI.MSGRConstants.MSGR_S_OK))
			{
				// get root storage location
				SignIn(messenger.MySigninName);
			}
		}

		private void SignIn(string signinName)
		{
			if (status != null)
			{
				status.Hide();
			}
			status		= new Status("Logging in, " + messenger.MyFriendlyName + "...");
			this.Text	= messenger.MyFriendlyName;

			// TODO: Subscribe to new forms being added and removed

			//			Assembly a = Assembly.LoadFrom(@"d:\vss\msn2.net\QueuePlayer\main\client\bin\debug\msn2.net.queuePlayer.Client.dll");
			//
			//			Type [] types = a.GetTypes();
			//
			//			foreach (Type t in types)
			//			{
			//				if (t.IsClass)
			//				{
			//					Debug.WriteLine("Class: " + t.FullName);					
			//					if (t.IsSubclassOf(typeof(msn2.net.Controls.ShellForm))
			//						&& t.FullName == "msn2.net.QueuePlayer.Client.UMPlayer")
			//					{
			//						object o;
			//						o = Activator.CreateInstance(t);
			//                        ShellForm f = (ShellForm) o;
			//
			//						listView1.Items.Add(
			//									new FormListViewItem(this, f));
			//					}
			//				}
			//			}
			
			string storageUrl = System.Configuration.ConfigurationSettings.AppSettings["storageUrl"].ToString();
			ConfigurationSettings.Current.Login(messenger, storageUrl);

			//			login l = new login();
			//			if (l.showdialog(this) == dialogresult.cancel)
			//			{
			//				this.dialogresult = dialogresult.cancel;
			//				this.close();
			//				application.exit();
			//				return;
			//			}

			//			listView1.Items.Add(
			//				new FormListViewItem(this, new msn2.net.QueuePlayer.Client.UMPlayer()));

			// MSNBC Headlines
			string baseUrl = "http://www.msnbc.com/modules/story_stage/stages.asp?0st=S1&nstage=1&scss=e";
			msn2.net.Controls.WebBrowser browser = new msn2.net.Controls.WebBrowser("MSNBC Headlines", 300, 300);
			browser.AddNewTab("Cover", String.Format(baseUrl, 1));
			browser.AddNewTab("News", String.Format(baseUrl, 2));
			browser.AddNewTab("Business", String.Format(baseUrl, 3));
			browser.AddNewTab("Health", String.Format(baseUrl, 4));
			browser.AddNewTab("Technology", String.Format(baseUrl, 5));
			browser.AddNewTab("TV News", String.Format(baseUrl, 6));
			browser.AddNewTab("Opinions", String.Format(baseUrl, 7));
			listView1.Items.Add(new FormListViewItem(this, browser));
				
//			listView1.Items.Add(
//				new FormListViewItem(this, new msn2.net.Controls.MSNBCHeadlines()));
			
			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.MSNBCWeather()));

			Favorites favs = new msn2.net.Controls.Favorites(ConfigurationSettings.Current.Data.Get("Favorites"));
			
			//			Crownwood.Magic.Docking.Content content =
			//				new Crownwood.Magic.Docking.Content(dockManager, favs, "Favorites");
			//			dockManager.Contents.Add(content);
			//	//dockManager.AddContentWithState(content, Crownwood.Magic.Docking.State.DockBottom);
			//			dockManager.ShowContent(content);

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.Favorites(ConfigurationSettings.Current.Data.Get("Favorites"))));

			//			ComputerInfo cInfo = new msn2.net.Controls.ComputerInfo();
			//			dockManager.Contents.Add(cInfo, cInfo.Text);
			//			dockManager.ShowContent(content);

			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.ComputerInfo()));

			//			listView1.Items.Add(
			//				new FormListViewItem(this, new msn2.net.Controls.Notes()));
			//
			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.WebSearch()));
			
			listView1.Items.Add(
				new FormListViewItem(this, new msn2.net.Controls.ShellLaunch()));

			//			listView1.Items.Add(
			//				new FormListViewItem(this, new msn2.net.Controls.RandomPicture()));

			this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - 50;
			this.Top  = 3;

			status.Hide();
		
		}

		private void Messenger_SignOut()
		{
			Application.Exit();
		}

		#endregion
	}
}
