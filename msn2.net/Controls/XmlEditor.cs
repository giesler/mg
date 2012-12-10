#region Using...
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using msn2.net.Configuration;
using System.Xml;
#endregion

namespace msn2.net.Controls
{
	public class XmlEditor : msn2.net.Controls.ShellForm
	{
		#region Declares
		private System.Windows.Forms.TreeView treeViewNodes;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.TextBox textBox1;
		private System.ComponentModel.IContainer components = null;
		private TitleBarControl[] buttons;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private XmlDocument doc;
		#endregion

		#region Constructors
		public XmlEditor()
		{
			InitializeComponent();
		}

		public XmlEditor(Data data): base(data)
		{
			InitializeComponent();

			buttons = new TitleBarControl[1];
			buttons[0] = new TitleBarControl("Open", "o", new EventHandler(Open_Click));
			this.AddButtons(buttons, true);

		}
		#endregion
		#region Disposal
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.treeViewNodes = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.listView1 = new System.Windows.Forms.ListView();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).BeginInit();
			this.SuspendLayout();
			// 
			// treeViewNodes
			// 
			this.treeViewNodes.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewNodes.ImageIndex = -1;
			this.treeViewNodes.Name = "treeViewNodes";
			this.treeViewNodes.SelectedImageIndex = -1;
			this.treeViewNodes.Size = new System.Drawing.Size(121, 206);
			this.treeViewNodes.TabIndex = 5;
			this.treeViewNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewNodes_AfterSelect);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(121, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 206);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// listView1
			// 
			this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
			this.listView1.Location = new System.Drawing.Point(124, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(220, 97);
			this.listView1.TabIndex = 7;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(124, 97);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(220, 3);
			this.splitter2.TabIndex = 8;
			this.splitter2.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(124, 100);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(220, 106);
			this.textBox1.TabIndex = 9;
			this.textBox1.Text = "textBox1";
			// 
			// XmlEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(344, 206);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBox1,
																		  this.splitter2,
																		  this.listView1,
																		  this.splitter1,
																		  this.treeViewNodes});
			this.Name = "XmlEditor";
			((System.ComponentModel.ISupportInitialize)(this.timerFadeOut)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timerFadeIn)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Private Methods
		private void Open_Click(object sender, System.EventArgs e)
		{
			// Get a filename
			openFileDialog1.DefaultExt	= "xml";
			openFileDialog1.Filter		= "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
			openFileDialog1.Title		= "Select an XML file to open";
			if (openFileDialog1.ShowDialog(this) == DialogResult.Cancel)
				return;

            doc = new XmlDocument();
			doc.Load(openFileDialog1.FileName);			

		}
		#endregion

		private void treeViewNodes_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
		
		}
	}
}

