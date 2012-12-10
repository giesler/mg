using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using mshtml;
using System.Runtime.InteropServices;
using msn2.net.Configuration;
using msn2.net.Controls;
using System.Drawing.Drawing2D;

namespace msn2.net.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class WebBrowserControl : System.Windows.Forms.UserControl
	{
		#region DefaultClickBehavior

		public enum DefaultClickBehavior
		{
			OpenLink,
			OpenInNewTab,
			OpenInNewWindow
		}

		#endregion

		#region Declares

		public AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.ComponentModel.IContainer components;
		private bool isPopup = false;
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
		private System.Windows.Forms.MenuItem menuItemOpenNewTab;
		private string newTitle = "New Window";
		private System.Windows.Forms.MenuItem menuItemSaveTo;
		private Crownwood.Magic.Controls.TabPage tabPage = null;
		private DefaultClickBehavior defaultClickBehavior = DefaultClickBehavior.OpenLink;
		private System.Windows.Forms.Label labelStatus;
		private string url = "";
		private System.Timers.Timer refreshTimer = null;
		private bool clickHandled = false;

		#endregion

		#region Constructor / Disposal

		public WebBrowserControl()
		{
			InternalConstructor();
		}

		public WebBrowserControl(DefaultClickBehavior clickBehavior)
		{
			this.defaultClickBehavior = clickBehavior;
			InternalConstructor();
		}

		public WebBrowserControl(DefaultClickBehavior clickBehavior, TimeSpan refresh)
		{
			this.defaultClickBehavior = clickBehavior;
			InternalConstructor();

			if (refresh.TotalMilliseconds > 0)
			{
				refreshTimer = new System.Timers.Timer(refresh.TotalMilliseconds);
				refreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(Refresh_Elapsed);
			}
		}

		private void InternalConstructor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// workaround for BeforeNavigate2 event not firing
			object o = null;
			axWebBrowser1.Navigate("about:blank", ref o, ref o, ref o, ref o);
			object oOcx = axWebBrowser1.GetOcx();
			axWebBrowser1.NavigateError += new AxSHDocVw.DWebBrowserEvents2_NavigateErrorEventHandler(axWebBrowser1_NavigateError);

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
			Document.onclick		= this;
			Document.oncontextmenu	= this;
			
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
			this.labelStatus = new System.Windows.Forms.Label();
			this.timerShowStatus = new System.Windows.Forms.Timer(this.components);
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItemOpenNewTab = new System.Windows.Forms.MenuItem();
			this.menuItemOpenNewWindow = new System.Windows.Forms.MenuItem();
			this.menuItemOpenInIE = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemCopyUrl = new System.Windows.Forms.MenuItem();
			this.menuItemSaveTo = new System.Windows.Forms.MenuItem();
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
			this.panelStatus.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.labelStatus});
			this.panelStatus.Location = new System.Drawing.Point(72, 120);
			this.panelStatus.Name = "panelStatus";
			this.panelStatus.Size = new System.Drawing.Size(160, 48);
			this.panelStatus.TabIndex = 1;
			this.panelStatus.Visible = false;
			this.panelStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.panelStatus_Paint);
			// 
			// labelStatus
			// 
			this.labelStatus.ForeColor = System.Drawing.Color.White;
			this.labelStatus.Location = new System.Drawing.Point(24, 16);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(128, 23);
			this.labelStatus.TabIndex = 0;
			this.labelStatus.Text = "loading...";
			this.labelStatus.Visible = false;
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
																						 this.menuItemOpenNewTab,
																						 this.menuItemOpenNewWindow,
																						 this.menuItemOpenInIE,
																						 this.menuItem4,
																						 this.menuItemCopyUrl,
																						 this.menuItemSaveTo});
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.DefaultItem = true;
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Text = "Open";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// menuItemOpenNewTab
			// 
			this.menuItemOpenNewTab.Index = 1;
			this.menuItemOpenNewTab.Text = "&Open in new tab";
			this.menuItemOpenNewTab.Click += new System.EventHandler(this.menuItemOpenNewTab_Click);
			// 
			// menuItemOpenNewWindow
			// 
			this.menuItemOpenNewWindow.Index = 2;
			this.menuItemOpenNewWindow.Text = "Open in new window";
			this.menuItemOpenNewWindow.Click += new System.EventHandler(this.menuItemOpenNewWindow_Click);
			// 
			// menuItemOpenInIE
			// 
			this.menuItemOpenInIE.Index = 3;
			this.menuItemOpenInIE.Text = "Open in IE";
			this.menuItemOpenInIE.Click += new System.EventHandler(this.menuItemOpenInIE_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 4;
			this.menuItem4.Text = "-";
			// 
			// menuItemCopyUrl
			// 
			this.menuItemCopyUrl.Index = 5;
			this.menuItemCopyUrl.Text = "&Copy Url";
			this.menuItemCopyUrl.Click += new System.EventHandler(this.menuItemCopyUrl_Click);
			// 
			// menuItemSaveTo
			// 
			this.menuItemSaveTo.Index = 6;
			this.menuItemSaveTo.Text = "&Save To...";
			this.menuItemSaveTo.Click += new System.EventHandler(this.menuItemSaveTo_Click);
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

		private void axWebBrowser1_NavigateError(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateErrorEvent e)
		{
			Debug.WriteLine(e.uRL + ": " + e.statusCode);
		}

		private void axWebBrowser1_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			navigating					= false;
			timerShowStatus.Enabled		= false;
			panelStatus.Visible			= false;
			clickHandled				= false;

			this.url = axWebBrowser1.LocationURL.ToString();

			if (NavigateComplete != null)
				NavigateComplete(this, new NavigateCompleteEventArgs(e.uRL.ToString()));
		
			// Enable refresh timer if we have one
			if (refreshTimer != null)
			{
				refreshTimer.Enabled = true;
			}
			// TODO: Save to history
		}

		private void axWebBrowser1_TitleChange(object sender, AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
		{
			Document.onclick = this;
			Document.oncontextmenu = this;

			if (TitleChange != null)
				TitleChange(this, new TitleChangeEventArgs(e.text));
		}

		public event BeforeNavigateDelegate BeforeNavigateEvent;

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
			if (!clickHandled)
			{
				if (this.defaultClickBehavior == DefaultClickBehavior.OpenInNewTab)
				{
					e.cancel = true;
				}
				else
				{
					WebBrowser b = new WebBrowser("Popup", true);
					e.ppDisp = b.webBrowserControl1.axWebBrowser1.GetOcx();
					b.Show();
				}
			}
			else
			{
				e.cancel = true;
			}
		}

		#endregion

		#region Events

		public event NavigateCompleteDelegate NavigateComplete;
		public event TitleChangeDelegate TitleChange;

		#endregion

		#region Methods

		public void Navigate(string url)
		{
			DefaultClickBehavior clickBehavior = defaultClickBehavior;

			// call anyone to let them change click behavior
			BeforeNavigateEventArgs args = new BeforeNavigateEventArgs(url, this.defaultClickBehavior);
			if (BeforeNavigateEvent != null)
				BeforeNavigateEvent(this, args);
			clickBehavior = args.ClickBehavior;

			if (args.Cancel)
			{
				throw new ApplicationException("Navigate canceled before first navigate");
			}
						
			object obj1 = 0; object obj2 = ""; object obj3 = ""; object obj4 = "";
			axWebBrowser1.Navigate(args.Url, ref obj1, ref obj2, ref obj3, ref obj4);
		}

		public void RefreshPage()
		{
			object level = 3;
			axWebBrowser1.Refresh2(ref level);
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

		public Crownwood.Magic.Controls.TabPage TabPage
		{
			get 
			{ 
				return tabPage;
			}
			set 
			{ 
				tabPage = value;
			}
		}

		public string Url
		{
			get
			{
				return url;
			}
			set
			{
				Navigate(value);
			}
		}

		#endregion

		#region Document handlers

		[DispId(0)]
		public void DefaultMethod()
		{
			try
			{
				clickHandled = false;

				HTMLWindow2Class win = (HTMLWindow2Class) Document.parentWindow;
				Debug.WriteLine("Object: " + win.@event.srcElement + ", Type: " + win.@event.type);

				HTMLAnchorElementClass anchor = null;
			
				// Handle right click and click
				if (win.@event.type == "contextmenu" || win.@event.type == "click")
				{
					// Gotta find out if we are right clicking a link
					if (win.@event.srcElement.ToString() == "mshtml.HTMLAnchorElementClass")
					{
						anchor = (HTMLAnchorElementClass) win.@event.srcElement;
						newUrl = anchor.href.Trim();
						
						try
						{
							newTitle = anchor.outerText.ToString().Trim();
						}
						catch (Exception)
						{}
					}
					else if (win.@event.srcElement.parentElement.ToString() == "mshtml.HTMLAnchorElementClass")
					{
						anchor = (HTMLAnchorElementClass) win.@event.srcElement.parentElement;
						newUrl = anchor.href.Trim();

						try
						{
							newTitle = anchor.outerText.ToString().Trim();
						}
						catch (Exception)
						{}
					}
					else if (win.@event.srcElement.ToString() == "mshtml.HTMLAreaElementClass")
					{
                        HTMLAreaElementClass area = (HTMLAreaElementClass) win.@event.srcElement;
						newUrl = area.href.Trim();
						if (area.alt != null)
						{
							newTitle = area.alt.ToString();
						}
						else
						{
							newTitle = "New Window";
						}
					}
					else
					{
						Debug.WriteLine("Parent: " + win.@event.srcElement.parentElement.ToString());
						// bail - we aren't in the right place
						return;
					}

					// Now branch based on what the actual event was
					switch (win.@event.type)
					{
						case "contextmenu":
							timerShowContextMenu.Enabled = true;
							x = win.@event.x;
							y = win.@event.y;
							break;
						case "click":
							DefaultClickBehavior clickBehavior = defaultClickBehavior;
							
							// call anyone to let them change click behavior
							BeforeNavigateEventArgs args = new BeforeNavigateEventArgs(newUrl, this.defaultClickBehavior);
							if (BeforeNavigateEvent != null)
								BeforeNavigateEvent(this, args);
							clickBehavior = args.ClickBehavior;

							if (!args.Cancel)
							{
								// We will, by default, handle this click
								clickHandled = true;

								// See what the default 'click' action is
								switch (clickBehavior)
								{
									case DefaultClickBehavior.OpenLink:
										Navigate(args.Url);
										break;
									case DefaultClickBehavior.OpenInNewTab:
										if (OpenNewTabEvent != null)
											OpenNewTabEvent(this, new NavigateEventArgs(newTitle, args.Url));
										break;
									case DefaultClickBehavior.OpenInNewWindow:
										WebBrowser b = new WebBrowser(newTitle, args.Url);
										b.Show();		
										break;
									default:
										clickHandled = false;
										break;
								}
							}
							break;
					}
					//contextMenu1.Show(this.axWebBrowser1, new Point(win.@event.x, win.@event.y));

					win.@event.returnValue	= false;
					win.@event.cancelBubble	= true;

				} 

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

		}

		int x = 0;
		int y = 0;

		#endregion

		#region Status display

		public void ShowStatus(string text)
		{
			labelStatus.Text = text;
			
			panelStatus.Left	= this.Width / 2 - panelStatus.Width / 2;
			panelStatus.Top		= this.Height / 2 - panelStatus.Height / 2;
			panelStatus.Visible = true;

			panelStatus.Refresh();
		}

		private void timerShowStatus_Tick(object sender, System.EventArgs e)
		{
			timerShowStatus.Enabled = false;

			if (navigating)
			{
				ShowStatus("loading...");
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

		private void menuItemSaveTo_Click(object sender, System.EventArgs e)
		{
			// Save to location
			ShellSave shellSave	= new ShellSave();
			
			if (shellSave.ShowShellDialog(this.ParentForm) == DialogResult.OK)
			{
				shellSave.Data.Get(newTitle, newUrl, new FavoriteConfigData(), typeof(FavoriteConfigData));
			}
		}

		private void menuItemCopyUrl_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Clipboard.SetDataObject(newUrl);
		}

		private void timerShowContextMenu_Tick(object sender, System.EventArgs e)
		{
			timerShowContextMenu.Enabled = false;
			contextMenu1.Show(this, this.PointToClient(Cursor.Position));
		}

		public OpenNewTabDelegate OpenNewTabEvent;

		private void menuItemOpenNewTab_Click(object sender, System.EventArgs e)
		{
			if (OpenNewTabEvent != null)
				OpenNewTabEvent(this, new NavigateEventArgs(newTitle, newUrl));
		}

		#endregion

		#region Refresh Timer

		private void Refresh_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (this.url != null && this.url.Length > 0)
			{
				System.Diagnostics.Debug.WriteLine("Refreshing " + url);
				this.Url = this.url;
			}
		}

		#endregion

		#region Paint

		private void panelStatus_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int middle = e.ClipRectangle.Height / 2;

			Rectangle bottom		= new Rectangle(e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width, middle);
			Rectangle top	= new Rectangle(e.ClipRectangle.Left, e.ClipRectangle.Top + middle, e.ClipRectangle.Width, middle);

			LinearGradientBrush topBrush
								= new LinearGradientBrush(top, Color.LightGray, msn2.net.Common.Drawing.LightenColor(Color.LightGray), LinearGradientMode.Vertical);
			e.Graphics.FillRectangle(topBrush, top);
			LinearGradientBrush bottomBrush
								= new LinearGradientBrush(bottom, msn2.net.Common.Drawing.LightenColor(Color.LightGray), Color.LightGray, LinearGradientMode.Vertical);
			e.Graphics.FillRectangle(bottomBrush, bottom);

//			msn2.net.Common.Drawing.ShadeRegion(e, Color.DimGray);

			e.Graphics.DrawString(this.labelStatus.Text, this.labelStatus.Font, new SolidBrush(SystemColors.ControlText), new RectangleF(labelStatus.Location, labelStatus.Size));
		}

		#endregion
	}

	#region Delegates

	public delegate void NavigateCompleteDelegate(object sender, NavigateCompleteEventArgs e);
	public delegate void TitleChangeDelegate(object sender, TitleChangeEventArgs e);
	public delegate void OpenNewTabDelegate(object sender, NavigateEventArgs e);
	public delegate void BeforeNavigateDelegate(object sender, BeforeNavigateEventArgs e);

	#endregion

	#region EventArgs classes

	public class BeforeNavigateEventArgs: EventArgs
	{
		private string url;
		private WebBrowserControl.DefaultClickBehavior clickBehavior;
		private bool cancel;

		public BeforeNavigateEventArgs(string url, WebBrowserControl.DefaultClickBehavior clickBehavior)
		{
			this.url				= url;
			this.clickBehavior		= clickBehavior;
		}

		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		public WebBrowserControl.DefaultClickBehavior ClickBehavior
		{
			get { return clickBehavior; }
			set { clickBehavior = value; }
		}

		public bool Cancel
		{
			get { return cancel; }
			set { cancel = value; }
		}
	}

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

	public class NavigateEventArgs: EventArgs
	{
		private string url;
		private string title;

		public NavigateEventArgs(string title, string url)
		{
			this.title	= title;
			this.url	= url;
		}

		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		public string Title
		{
			get { return title; }
			set { title = value; }
		}
	}

	#endregion

}
