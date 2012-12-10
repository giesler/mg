using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using mshtml;
using System.Runtime.InteropServices;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class WebBrowserControl : System.Windows.Forms.UserControl
	{
		#region Declares

		public AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.ComponentModel.IContainer components;
		private string currentUrl = "";
		private bool isPopup = false;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Timer timerShowStatus;
		private System.Windows.Forms.Panel panelStatus;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItemOpen;
		private System.Windows.Forms.MenuItem menuItemOpenNewWindow;
		private System.Windows.Forms.MenuItem menuItemOpenInIE;
		private System.Windows.Forms.MenuItem menuItemCopyUrl;
		private bool navigating = false;
		private string newUrl = "about:blank";
		private System.Windows.Forms.Timer timerShowContextMenu;
		private string newTitle = "New Window";

		#endregion

		#region Constructor / Disposal

		public WebBrowserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// workaround for BeforeNavigate2 event not firing
			object o = null;
			axWebBrowser1.Navigate("about:blank", ref o, ref o, ref o, ref o);
			object oOcx = axWebBrowser1.GetOcx();

			try
			{
				SHDocVw.WebBrowser_V1 axDocumentV1 = oOcx as SHDocVw.WebBrowser_V1;
				axDocumentV1.BeforeNavigate +=
					new SHDocVw.DWebBrowserEvents_BeforeNavigateEventHandler(this.axDocumentV1_BeforeNavigate);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("BeforeNavigate2 event registration failed. " + ex.ToString());
			}

			// Now tie in to click event
			Document.onclick = this;
			Document.oncontextmenu = this;
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WebBrowserControl));
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.panelStatus = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.timerShowStatus = new System.Windows.Forms.Timer(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItemOpenNewWindow = new System.Windows.Forms.MenuItem();
			this.menuItemOpenInIE = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemCopyUrl = new System.Windows.Forms.MenuItem();
			this.timerShowContextMenu = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.panelStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(304, 288);
			this.axWebBrowser1.TabIndex = 0;
			this.axWebBrowser1.TitleChange += new AxSHDocVw.DWebBrowserEvents2_TitleChangeEventHandler(this.axWebBrowser1_TitleChange);
			this.axWebBrowser1.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser1_NavigateComplete2);
			this.axWebBrowser1.NewWindow2 += new AxSHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(this.axWebBrowser1_NewWindow2);
			// 
			// panelStatus
			// 
			this.panelStatus.BackColor = System.Drawing.Color.DimGray;
			this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelStatus.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.label1});
			this.panelStatus.Location = new System.Drawing.Point(72, 120);
			this.panelStatus.Name = "panelStatus";
			this.panelStatus.Size = new System.Drawing.Size(160, 48);
			this.panelStatus.TabIndex = 1;
			this.panelStatus.Visible = false;
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "loading...";
			// 
			// timerShowStatus
			// 
			this.timerShowStatus.Interval = 1000;
			this.timerShowStatus.Tick += new System.EventHandler(this.timerShowStatus_Tick);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemOpen,
																						 this.menuItemOpenNewWindow,
																						 this.menuItemOpenInIE,
																						 this.menuItem4,
																						 this.menuItemCopyUrl});
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.DefaultItem = true;
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Text = "Open";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// menuItemOpenNewWindow
			// 
			this.menuItemOpenNewWindow.Index = 1;
			this.menuItemOpenNewWindow.Text = "Open in new window";
			this.menuItemOpenNewWindow.Click += new System.EventHandler(this.menuItemOpenNewWindow_Click);
			// 
			// menuItemOpenInIE
			// 
			this.menuItemOpenInIE.Index = 2;
			this.menuItemOpenInIE.Text = "Open in IE";
			this.menuItemOpenInIE.Click += new System.EventHandler(this.menuItemOpenInIE_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "-";
			// 
			// menuItemCopyUrl
			// 
			this.menuItemCopyUrl.Index = 4;
			this.menuItemCopyUrl.Text = "&Copy Url";
			this.menuItemCopyUrl.Click += new System.EventHandler(this.menuItemCopyUrl_Click);
			// 
			// timerShowContextMenu
			// 
			this.timerShowContextMenu.Interval = 10;
			this.timerShowContextMenu.Tick += new System.EventHandler(this.timerShowContextMenu_Tick);
			// 
			// WebBrowserControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelStatus,
																		  this.axWebBrowser1});
			this.Name = "WebBrowserControl";
			this.Size = new System.Drawing.Size(304, 288);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.panelStatus.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region WebBrowser control events

		private void axWebBrowser1_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			navigating					= false;
			timerShowStatus.Enabled		= false;
			panelStatus.Visible			= false;
			
			if (NavigateComplete != null)
				NavigateComplete(this, new NavigateCompleteEventArgs(e.uRL.ToString()));
		
		}

		private void axWebBrowser1_TitleChange(object sender, AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
		{
			Document.onclick = this;
			Document.oncontextmenu = this;

			if (TitleChange != null)
				TitleChange(this, new TitleChangeEventArgs(e.text));
		}

		private void axDocumentV1_BeforeNavigate(string url, int flags, string targetFrameName, 
			ref object postData, string headers, ref bool processed)
		{
			// If we are simply navigating in the current window, just set timer to show status
			if (!isPopup)
			{
				timerShowStatus.Enabled = true;
				navigating = true;

				this.Visible = true;
				return;
			}

			if (MessageBox.Show(this, "Open popup to " + url + "?", "Popup Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				processed = true;
				return;
			}

			this.Parent.Show();
		}

		private void axWebBrowser1_NewWindow2(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow2Event e)
		{
			WebBrowser b = new WebBrowser("Popup", true);
			e.ppDisp = b.webBrowserControl1.axWebBrowser1.GetOcx();
		}

		#endregion

		#region Events

		public event NavigateCompleteDelegate NavigateComplete;
		public event TitleChangeDelegate TitleChange;

		#endregion

		#region Methods

		public void Navigate(string url)
		{
			currentUrl = url;

			object obj1 = 0; object obj2 = ""; object obj3 = ""; object obj4 = "";
			axWebBrowser1.Navigate(url, ref obj1, ref obj2, ref obj3, ref obj4);
		}

		#endregion

		#region Properties

		public bool IsPopup
		{
			get { return isPopup; }
			set { isPopup = value; }
		}

		public HTMLDocumentClass Document
		{
			get 
			{
				return (HTMLDocumentClass) axWebBrowser1.Document;
			}
		}

		public HTMLBodyClass Body
		{
			get 
			{
				return (HTMLBodyClass) Document.body;
			}
		}

		#endregion

		#region Document handlers

		[DispId(0)]
		public void DefaultMethod()
		{
			HTMLWindow2Class win = (HTMLWindow2Class) Document.parentWindow;
			Debug.WriteLine("Object: " + win.@event.srcElement + ", Type: " + win.@event.type);

			// Handle right click
			if (win.@event.type == "contextmenu")
			{
				
				HTMLAnchorElementClass anchor = null;
				
				// Gotta find out if we are right clicking a link
				if (win.@event.srcElement.ToString() == "mshtml.HTMLAnchorElementClass")
				{
					anchor = (HTMLAnchorElementClass) win.@event.srcElement;
				}
				else if (win.@event.srcElement.parentElement.ToString() == "mshtml.HTMLAnchorElementClass")
				{
					anchor = (HTMLAnchorElementClass) win.@event.srcElement.parentElement;
				}
				else
				{
					// bail - we aren't in the right place
					return;
				}
				newUrl			= anchor.href;
				newTitle		= anchor.outerText;
				timerShowContextMenu.Enabled = true;
				x = win.@event.x;
				y = win.@event.y;
				//contextMenu1.Show(this.axWebBrowser1, new Point(win.@event.x, win.@event.y));

				win.@event.returnValue	= false;
				win.@event.cancelBubble	= true;

			}

		}

		int x = 0;
		int y = 0;

		#endregion

		#region Status display

		private void timerShowStatus_Tick(object sender, System.EventArgs e)
		{
			timerShowStatus.Enabled = false;

			if (navigating)
			{
				panelStatus.Left	= this.Width / 2 - panelStatus.Width / 2;
				panelStatus.Top		= this.Height / 2 - panelStatus.Height / 2;
				panelStatus.Visible = true;
			}
		}

		#endregion

		#region Context Menus

		private void menuItemOpen_Click(object sender, System.EventArgs e)
		{
            Navigate(newUrl);		
		}

		private void menuItemOpenNewWindow_Click(object sender, System.EventArgs e)
		{
			WebBrowser b = new WebBrowser(newTitle, newUrl);
			b.Show();		
		}

		private void menuItemOpenInIE_Click(object sender, System.EventArgs e)
		{
			Process p = new Process();
			p.StartInfo = new ProcessStartInfo(newUrl);
			p.Start();            		
		}

		private void menuItemCopyUrl_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Clipboard.SetDataObject(newUrl);
		}

		private void timerShowContextMenu_Tick(object sender, System.EventArgs e)
		{
			// TODO: Get actual mouse coords
			timerShowContextMenu.Enabled = false;
			contextMenu1.Show(this, new Point(x, y));
		}

		#endregion

	}

	#region Delegates

	public delegate void NavigateCompleteDelegate(object sender, NavigateCompleteEventArgs e);
	public delegate void TitleChangeDelegate(object sender, TitleChangeEventArgs e);

	#endregion

	#region EventArgs classes

	public class NavigateCompleteEventArgs: EventArgs
	{
		private string url;

		public NavigateCompleteEventArgs(string url)
		{
			this.url = url;
		}

		public string Url
		{
			get { return url; }
			set { url = value; }
		}
	}

	public class TitleChangeEventArgs: EventArgs
	{
		private string title;

		public TitleChangeEventArgs(string title)
		{
			this.title = title;
		}

		public string Title 
		{
			get { return title; }
			set { title = value; }
		}
	}

	#endregion

}
