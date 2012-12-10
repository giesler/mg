using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace msn2.net.Pictures.Controls
{
	/// <summary>
	/// Summary description for fPicture.
	/// </summary>
	public class fPicture : System.Windows.Forms.Form
	{
		#region Declares

		private System.Windows.Forms.Panel panelPic;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;

		public bool mblnCancel;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.CheckBox chkPublish;
		private System.Data.SqlClient.SqlConnection cn;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private msn2.net.Pictures.Controls.CategoryPicker categoryPicker1;
		private System.Data.SqlClient.SqlDataAdapter daPictureCategory;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand2;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand2;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand2;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand2;
		private System.Data.SqlClient.SqlDataAdapter daPicturePerson;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand3;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand3;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand3;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand3;
		private msn2.net.Pictures.Controls.PersonPicker personPicker1;
		private System.Data.SqlClient.SqlDataAdapter daPicture;
		private msn2.net.Pictures.Controls.DataSetPicture dsPicture;
		private fPictureViewer fPV;

		private bool m_blnMoveNext;
		private bool m_blnMovePrevious;
		private System.Windows.Forms.Button btnMoveNext;
		private System.Windows.Forms.Button btnMovePrevious;
		private fMain m_fMain;

		private int m_intLeft;
		private int m_intTop;
		private int m_intHeight;
		private int m_intWidth;
		private int pictureId;
		private System.Windows.Forms.ContextMenu menuImage;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Data.SqlClient.SqlConnection sqlConnection1;
		private System.Windows.Forms.NumericUpDown rating;
		private System.Windows.Forms.Label label4;
		private System.Data.SqlClient.SqlDataAdapter daPictureGroup;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private msn2.net.Pictures.Controls.GroupPicker groupPicker1;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand4;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand4;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand4;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand4;

		private bool buttonPushed = false;
		private msn2.net.Pictures.Controls.PictureDisplay pictureDisplay1;

		private string navigationControlsDataQuery;
		#endregion
		private System.Windows.Forms.TabPage tabPageBasicInfo;
		private System.Windows.Forms.DateTimePicker pictureDatePicker;
		private msn2.net.Pictures.Controls.PersonSelect personSelect1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox setCategoryPicSelection;

		public string NavigationControlsDataQuery
		{
			get
			{
				return navigationControlsDataQuery;
			}
			set
			{
				navigationControlsDataQuery = value;
			}
		}

		public string Title 
		{
			get 
			{
				return txtTitle.Text;
			}
		}

		public bool Publish 
		{
			get 
			{
				return chkPublish.Checked;
			}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public fPicture()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set the connection string
            cn.ConnectionString = PicContext.Current.Config.ConnectionString;
            sqlConnection1.ConnectionString = PicContext.Current.Config.ConnectionString;
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
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.daPicturePerson = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand3 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand3 = new System.Data.SqlClient.SqlCommand();
			this.dsPicture = new msn2.net.Pictures.Controls.DataSetPicture();
			this.btnCancel = new System.Windows.Forms.Button();
			this.daPictureCategory = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand2 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand2 = new System.Data.SqlClient.SqlCommand();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageBasicInfo = new System.Windows.Forms.TabPage();
			this.pictureDatePicker = new System.Windows.Forms.DateTimePicker();
			this.personSelect1 = new msn2.net.Pictures.Controls.PersonSelect();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.personPicker1 = new msn2.net.Pictures.Controls.PersonPicker();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.categoryPicker1 = new msn2.net.Pictures.Controls.CategoryPicker();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.groupPicker1 = new msn2.net.Pictures.Controls.GroupPicker();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnMovePrevious = new System.Windows.Forms.Button();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.menuImage = new System.Windows.Forms.ContextMenu();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.btnMoveNext = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.rating = new System.Windows.Forms.NumericUpDown();
			this.chkPublish = new System.Windows.Forms.CheckBox();
			this.panelPic = new System.Windows.Forms.Panel();
			this.pictureDisplay1 = new msn2.net.Pictures.Controls.PictureDisplay();
			this.daPictureGroup = new System.Data.SqlClient.SqlDataAdapter();
			this.daPicture = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand4 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand4 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand4 = new System.Data.SqlClient.SqlCommand();
			this.sqlUpdateCommand4 = new System.Data.SqlClient.SqlCommand();
			this.panel1 = new System.Windows.Forms.Panel();
			this.setCategoryPicSelection = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dsPicture)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPageBasicInfo.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.rating)).BeginInit();
			this.panelPic.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT PictureGroupID, PictureID, GroupID FROM PictureGroup WHERE (PictureID = @P" +
				"ictureID)";
			this.sqlSelectCommand1.Connection = this.cn;
			this.sqlSelectCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=picdbserver;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// sqlConnection1
			// 
			this.sqlConnection1.ConnectionString = "data source=picdbserver;integrated security=sspi;initial catalog=picdb;persist security " +
				"info=False";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 2;
			this.menuItem8.Text = "X &and Y";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 0;
			this.menuItem9.Text = "";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "&270 Degrees";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem6,
																					  this.menuItem7,
																					  this.menuItem8});
			this.menuItem5.Text = "&Flip Image";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 0;
			this.menuItem6.Text = "&X Axis";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 1;
			this.menuItem7.Text = "&Y Axis";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3,
																					  this.menuItem4});
			this.menuItem1.Text = "&Rotate Image";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "&90 Degrees";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "&180 Degress";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// daPicturePerson
			// 
			this.daPicturePerson.DeleteCommand = this.sqlDeleteCommand3;
			this.daPicturePerson.InsertCommand = this.sqlInsertCommand3;
			this.daPicturePerson.SelectCommand = this.sqlSelectCommand3;
			this.daPicturePerson.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																									  new System.Data.Common.DataTableMapping("Table", "PicturePerson", new System.Data.Common.DataColumnMapping[] {
																																																					   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																					   new System.Data.Common.DataColumnMapping("PersonID", "PersonID")})});
			this.daPicturePerson.UpdateCommand = this.sqlUpdateCommand3;
			// 
			// sqlDeleteCommand3
			// 
			this.sqlDeleteCommand3.CommandText = "DELETE FROM PicturePerson WHERE (PersonID = @PersonID) AND (PictureID = @PictureI" +
				"D)";
			this.sqlDeleteCommand3.Connection = this.cn;
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			// 
			// sqlInsertCommand3
			// 
			this.sqlInsertCommand3.CommandText = "INSERT INTO PicturePerson(PictureID, PersonID) VALUES (@PictureID, @PersonID); SE" +
				"LECT PictureID, PersonID FROM PicturePerson WHERE (PersonID = @Select_PersonID) " +
				"AND (PictureID = @Select_PictureID)";
			this.sqlInsertCommand3.Connection = this.cn;
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, "PersonID"));
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, "PersonID"));
			this.sqlInsertCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// sqlSelectCommand3
			// 
			this.sqlSelectCommand3.CommandText = "SELECT PictureID, PersonID FROM PicturePerson WHERE (PictureID = @PictureID) ORDE" +
				"R BY PicturePersonID";
			this.sqlSelectCommand3.Connection = this.cn;
			this.sqlSelectCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// sqlUpdateCommand3
			// 
			this.sqlUpdateCommand3.CommandText = @"UPDATE PicturePerson SET PictureID = @PictureID, PersonID = @PersonID WHERE (PersonID = @Original_PersonID) AND (PictureID = @Original_PictureID); SELECT PictureID, PersonID FROM PicturePerson WHERE (PersonID = @Select_PersonID) AND (PictureID = @Select_PictureID)";
			this.sqlUpdateCommand3.Connection = this.cn;
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, "PersonID"));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, "PersonID"));
			this.sqlUpdateCommand3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// dsPicture
			// 
			this.dsPicture.DataSetName = "DataSetPicture";
			this.dsPicture.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCancel.Location = new System.Drawing.Point(296, 224);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 16;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// daPictureCategory
			// 
			this.daPictureCategory.DeleteCommand = this.sqlDeleteCommand2;
			this.daPictureCategory.InsertCommand = this.sqlInsertCommand2;
			this.daPictureCategory.SelectCommand = this.sqlSelectCommand2;
			this.daPictureCategory.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "PictureCategory", new System.Data.Common.DataColumnMapping[] {
																																																						   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																						   new System.Data.Common.DataColumnMapping("CategoryID", "CategoryID")})});
			this.daPictureCategory.UpdateCommand = this.sqlUpdateCommand2;
			// 
			// sqlDeleteCommand2
			// 
			this.sqlDeleteCommand2.CommandText = "DELETE FROM PictureCategory WHERE (CategoryID = @CategoryID) AND (PictureID = @Pi" +
				"ctureID)";
			this.sqlDeleteCommand2.Connection = this.cn;
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			// 
			// sqlInsertCommand2
			// 
			this.sqlInsertCommand2.CommandText = "INSERT INTO PictureCategory(PictureID, CategoryID) VALUES (@PictureID, @CategoryI" +
				"D); SELECT PictureID, CategoryID FROM PictureCategory WHERE (CategoryID = @Selec" +
				"t_CategoryID) AND (PictureID = @Select_PictureID)";
			this.sqlInsertCommand2.Connection = this.cn;
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlInsertCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// sqlSelectCommand2
			// 
			this.sqlSelectCommand2.CommandText = "SELECT PictureID, CategoryID FROM PictureCategory WHERE (PictureID = @PictureID)";
			this.sqlSelectCommand2.Connection = this.cn;
			this.sqlSelectCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// sqlUpdateCommand2
			// 
			this.sqlUpdateCommand2.CommandText = @"UPDATE PictureCategory SET PictureID = @PictureID, CategoryID = @CategoryID WHERE (CategoryID = @Original_CategoryID) AND (PictureID = @Original_PictureID); SELECT PictureID, CategoryID FROM PictureCategory WHERE (CategoryID = @Select_CategoryID) AND (PictureID = @Select_PictureID)";
			this.sqlUpdateCommand2.Connection = this.cn;
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_CategoryID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "CategoryID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_CategoryID", System.Data.SqlDbType.Int, 4, "CategoryID"));
			this.sqlUpdateCommand2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPageBasicInfo);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(352, 208);
			this.tabControl1.TabIndex = 11;
			// 
			// tabPageBasicInfo
			// 
			this.tabPageBasicInfo.Controls.Add(this.pictureDatePicker);
			this.tabPageBasicInfo.Controls.Add(this.personSelect1);
			this.tabPageBasicInfo.Controls.Add(this.label3);
			this.tabPageBasicInfo.Controls.Add(this.label2);
			this.tabPageBasicInfo.Controls.Add(this.label1);
			this.tabPageBasicInfo.Controls.Add(this.txtTitle);
			this.tabPageBasicInfo.Location = new System.Drawing.Point(4, 22);
			this.tabPageBasicInfo.Name = "tabPageBasicInfo";
			this.tabPageBasicInfo.Size = new System.Drawing.Size(344, 182);
			this.tabPageBasicInfo.TabIndex = 4;
			this.tabPageBasicInfo.Text = "Picture Info";
			// 
			// pictureDatePicker
			// 
			this.pictureDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureDatePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.dsPicture, "Picture.PictureDate"));
			this.pictureDatePicker.Location = new System.Drawing.Point(56, 32);
			this.pictureDatePicker.Name = "pictureDatePicker";
			this.pictureDatePicker.Size = new System.Drawing.Size(280, 20);
			this.pictureDatePicker.TabIndex = 9;
			// 
			// personSelect1
			// 
			this.personSelect1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.personSelect1.Location = new System.Drawing.Point(56, 56);
			this.personSelect1.Name = "personSelect1";
			this.personSelect1.SelectedPerson = null;
			this.personSelect1.SelectedPersonID = 0;
			this.personSelect1.Size = new System.Drawing.Size(280, 21);
			this.personSelect1.TabIndex = 11;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "By:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Date:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Title:";
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPicture, "Picture.Title"));
			this.txtTitle.Location = new System.Drawing.Point(56, 8);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(280, 20);
			this.txtTitle.TabIndex = 7;
			this.txtTitle.Text = "";
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.txtDescription);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(296, 182);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Description";
			// 
			// txtDescription
			// 
			this.txtDescription.AcceptsReturn = true;
			this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsPicture, "Picture.Description"));
			this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDescription.Location = new System.Drawing.Point(0, 0);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(296, 182);
			this.txtDescription.TabIndex = 0;
			this.txtDescription.Text = "";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.personPicker1);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(344, 182);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "People";
			// 
			// personPicker1
			// 
			this.personPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.personPicker1.Location = new System.Drawing.Point(0, 0);
			this.personPicker1.Name = "personPicker1";
			this.personPicker1.Size = new System.Drawing.Size(344, 182);
			this.personPicker1.TabIndex = 0;
			this.personPicker1.Enter += new System.EventHandler(this.personPicker1_Enter);
			this.personPicker1.RemovedPerson += new msn2.net.Pictures.Controls.RemovedPersonEventHandler(this.personPicker1_RemovedPerson);
			this.personPicker1.AddedPerson += new msn2.net.Pictures.Controls.AddedPersonEventHandler(this.personPicker1_AddedPerson);
			this.personPicker1.Leave += new System.EventHandler(this.personPicker1_Leave);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.categoryPicker1);
			this.tabPage2.Controls.Add(this.panel1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(344, 182);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Categories";
			// 
			// categoryPicker1
			// 
			this.categoryPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.categoryPicker1.Location = new System.Drawing.Point(0, 0);
			this.categoryPicker1.Name = "categoryPicker1";
			this.categoryPicker1.Size = new System.Drawing.Size(344, 150);
			this.categoryPicker1.TabIndex = 0;
			this.categoryPicker1.AddedCategory += new msn2.net.Pictures.Controls.AddedCategoryEventHandler(this.categoryPicker1_AddedCategory);
			this.categoryPicker1.RemovedCategory += new msn2.net.Pictures.Controls.RemovedCategoryEventHandler(this.categoryPicker1_RemovedCategory);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.groupPicker1);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(344, 182);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Security";
			// 
			// groupPicker1
			// 
			this.groupPicker1.AllowRemoveEveryone = true;
			this.groupPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupPicker1.Location = new System.Drawing.Point(0, 0);
			this.groupPicker1.Name = "groupPicker1";
			this.groupPicker1.Size = new System.Drawing.Size(344, 182);
			this.groupPicker1.TabIndex = 0;
			this.groupPicker1.RemovedGroup += new msn2.net.Pictures.Controls.RemovedGroupEventHandler(this.groupPicker1_RemovedGroup);
			this.groupPicker1.AddedGroup += new msn2.net.Pictures.Controls.AddedGroupEventHandler(this.groupPicker1_AddedGroup);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnOK.Location = new System.Drawing.Point(224, 224);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 15;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnMovePrevious
			// 
			this.btnMovePrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMovePrevious.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnMovePrevious.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMovePrevious.Location = new System.Drawing.Point(128, 224);
			this.btnMovePrevious.Name = "btnMovePrevious";
			this.btnMovePrevious.Size = new System.Drawing.Size(40, 24);
			this.btnMovePrevious.TabIndex = 13;
			this.btnMovePrevious.Text = "&<<";
			this.btnMovePrevious.Visible = false;
			this.btnMovePrevious.Click += new System.EventHandler(this.btnMovePrevious_Click);
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO PictureGroup(PictureID, GroupID) VALUES (@PictureID, @GroupID); SELEC" +
				"T PictureGroupID, PictureID, GroupID FROM PictureGroup WHERE (PictureGroupID = @" +
				"@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.sqlConnection1;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE PictureGroup SET PictureID = @PictureID, GroupID = @GroupID WHERE (PictureGroupID = @Original_PictureGroupID) AND (GroupID = @Original_GroupID OR @Original_GroupID1 IS NULL AND GroupID IS NULL) AND (PictureID = @Original_PictureID OR @Original_PictureID1 IS NULL AND PictureID IS NULL); SELECT PictureGroupID, PictureID, GroupID FROM PictureGroup WHERE (PictureGroupID = @Select_PictureGroupID)";
			this.sqlUpdateCommand1.Connection = this.sqlConnection1;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureGroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PictureGroupID", System.Data.SqlDbType.Int, 4, "PictureGroupID"));
			// 
			// menuImage
			// 
			this.menuImage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem9,
																					  this.menuItem10,
																					  this.menuItem1,
																					  this.menuItem5});
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 1;
			this.menuItem10.Text = "-";
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = "DELETE FROM PictureGroup WHERE (PictureGroupID = @PictureGroupID) AND (GroupID = " +
				"@GroupID OR @GroupID1 IS NULL AND GroupID IS NULL) AND (PictureID = @PictureID O" +
				"R @PictureID1 IS NULL AND PictureID IS NULL)";
			this.sqlDeleteCommand1.Connection = this.sqlConnection1;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureGroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureGroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@GroupID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "GroupID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID1", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			// 
			// btnMoveNext
			// 
			this.btnMoveNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveNext.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnMoveNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnMoveNext.Location = new System.Drawing.Point(176, 224);
			this.btnMoveNext.Name = "btnMoveNext";
			this.btnMoveNext.Size = new System.Drawing.Size(40, 24);
			this.btnMoveNext.TabIndex = 14;
			this.btnMoveNext.Text = "&>>";
			this.btnMoveNext.Visible = false;
			this.btnMoveNext.Click += new System.EventHandler(this.btnMoveNext_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(184, 336);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 23);
			this.label4.TabIndex = 7;
			this.label4.Text = "Rating:";
			this.label4.Visible = false;
			// 
			// rating
			// 
			this.rating.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.dsPicture, "Picture.Rating"));
			this.rating.Increment = new System.Decimal(new int[] {
																	 5,
																	 0,
																	 0,
																	 0});
			this.rating.Location = new System.Drawing.Point(232, 336);
			this.rating.Name = "rating";
			this.rating.Size = new System.Drawing.Size(48, 20);
			this.rating.TabIndex = 8;
			this.rating.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.rating.Visible = false;
			// 
			// chkPublish
			// 
			this.chkPublish.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.dsPicture, "Picture.Publish"));
			this.chkPublish.Location = new System.Drawing.Point(88, 328);
			this.chkPublish.Name = "chkPublish";
			this.chkPublish.Size = new System.Drawing.Size(88, 24);
			this.chkPublish.TabIndex = 6;
			this.chkPublish.Text = "Publish";
			this.chkPublish.Visible = false;
			// 
			// panelPic
			// 
			this.panelPic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panelPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelPic.Controls.Add(this.pictureDisplay1);
			this.panelPic.Location = new System.Drawing.Point(16, 336);
			this.panelPic.Name = "panelPic";
			this.panelPic.Size = new System.Drawing.Size(48, 0);
			this.panelPic.TabIndex = 12;
			this.panelPic.Visible = false;
			// 
			// pictureDisplay1
			// 
			this.pictureDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureDisplay1.Location = new System.Drawing.Point(0, 0);
			this.pictureDisplay1.Name = "pictureDisplay1";
			this.pictureDisplay1.Size = new System.Drawing.Size(48, 0);
			this.pictureDisplay1.TabIndex = 0;
			// 
			// daPictureGroup
			// 
			this.daPictureGroup.DeleteCommand = this.sqlDeleteCommand1;
			this.daPictureGroup.InsertCommand = this.sqlInsertCommand1;
			this.daPictureGroup.SelectCommand = this.sqlSelectCommand1;
			this.daPictureGroup.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																									 new System.Data.Common.DataTableMapping("Table", "PictureGroup", new System.Data.Common.DataColumnMapping[] {
																																																					 new System.Data.Common.DataColumnMapping("PictureGroupID", "PictureGroupID"),
																																																					 new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																					 new System.Data.Common.DataColumnMapping("GroupID", "GroupID")})});
			this.daPictureGroup.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// daPicture
			// 
			this.daPicture.DeleteCommand = this.sqlDeleteCommand4;
			this.daPicture.InsertCommand = this.sqlInsertCommand4;
			this.daPicture.SelectCommand = this.sqlSelectCommand4;
			this.daPicture.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																								new System.Data.Common.DataTableMapping("Table", "Picture", new System.Data.Common.DataColumnMapping[] {
																																																		   new System.Data.Common.DataColumnMapping("PictureID", "PictureID"),
																																																		   new System.Data.Common.DataColumnMapping("Filename", "Filename"),
																																																		   new System.Data.Common.DataColumnMapping("PictureDate", "PictureDate"),
																																																		   new System.Data.Common.DataColumnMapping("Title", "Title"),
																																																		   new System.Data.Common.DataColumnMapping("Description", "Description"),
																																																		   new System.Data.Common.DataColumnMapping("Publish", "Publish"),
																																																		   new System.Data.Common.DataColumnMapping("Rating", "Rating"),
																																																		   new System.Data.Common.DataColumnMapping("PictureBy", "PictureBy"),
																																																		   new System.Data.Common.DataColumnMapping("PictureSort", "PictureSort")})});
			this.daPicture.UpdateCommand = this.sqlUpdateCommand4;
			// 
			// sqlDeleteCommand4
			// 
			this.sqlDeleteCommand4.CommandText = @"DELETE FROM Picture WHERE (PictureID = @Original_PictureID) AND (Description = @Original_Description OR @Original_Description IS NULL AND Description IS NULL) AND (Filename = @Original_Filename OR @Original_Filename IS NULL AND Filename IS NULL) AND (PictureBy = @Original_PictureBy OR @Original_PictureBy IS NULL AND PictureBy IS NULL) AND (PictureDate = @Original_PictureDate OR @Original_PictureDate IS NULL AND PictureDate IS NULL) AND (PictureSort = @Original_PictureSort) AND (Publish = @Original_Publish OR @Original_Publish IS NULL AND Publish IS NULL) AND (Rating = @Original_Rating OR @Original_Rating IS NULL AND Rating IS NULL) AND (Title = @Original_Title OR @Original_Title IS NULL AND Title IS NULL)";
			this.sqlDeleteCommand4.Connection = this.cn;
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureSort", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureSort", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Rating", System.Data.SqlDbType.TinyInt, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Rating", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			// 
			// sqlInsertCommand4
			// 
			this.sqlInsertCommand4.CommandText = @"INSERT INTO Picture(Filename, PictureDate, Title, Description, Publish, Rating, PictureBy, PictureSort) VALUES (@Filename, @PictureDate, @Title, @Description, @Publish, @Rating, @PictureBy, @PictureSort); SELECT PictureID, Filename, PictureDate, Title, Description, Publish, Rating, PictureBy, PictureSort FROM Picture WHERE (PictureID = @@IDENTITY)";
			this.sqlInsertCommand4.Connection = this.cn;
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 150, "Filename"));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, "PictureDate"));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, "Title"));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, "Description"));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rating", System.Data.SqlDbType.TinyInt, 1, "Rating"));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, "PictureBy"));
			this.sqlInsertCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureSort", System.Data.SqlDbType.Int, 4, "PictureSort"));
			// 
			// sqlSelectCommand4
			// 
			this.sqlSelectCommand4.CommandText = "SELECT PictureID, Filename, PictureDate, Title, Description, Publish, Rating, Pic" +
				"tureBy, PictureSort FROM Picture WHERE (PictureID = @PictureID)";
			this.sqlSelectCommand4.Connection = this.cn;
			this.sqlSelectCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// sqlUpdateCommand4
			// 
			this.sqlUpdateCommand4.CommandText = @"UPDATE Picture SET Filename = @Filename, PictureDate = @PictureDate, Title = @Title, Description = @Description, Publish = @Publish, Rating = @Rating, PictureBy = @PictureBy, PictureSort = @PictureSort WHERE (PictureID = @Original_PictureID) AND (Description = @Original_Description OR @Original_Description IS NULL AND Description IS NULL) AND (Filename = @Original_Filename OR @Original_Filename IS NULL AND Filename IS NULL) AND (PictureBy = @Original_PictureBy OR @Original_PictureBy IS NULL AND PictureBy IS NULL) AND (PictureDate = @Original_PictureDate OR @Original_PictureDate IS NULL AND PictureDate IS NULL) AND (PictureSort = @Original_PictureSort) AND (Publish = @Original_Publish OR @Original_Publish IS NULL AND Publish IS NULL) AND (Rating = @Original_Rating OR @Original_Rating IS NULL AND Rating IS NULL) AND (Title = @Original_Title OR @Original_Title IS NULL AND Title IS NULL); SELECT PictureID, Filename, PictureDate, Title, Description, Publish, Rating, PictureBy, PictureSort FROM Picture WHERE (PictureID = @PictureID)";
			this.sqlUpdateCommand4.Connection = this.cn;
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Filename", System.Data.SqlDbType.NVarChar, 150, "Filename"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureDate", System.Data.SqlDbType.DateTime, 4, "PictureDate"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Title", System.Data.SqlDbType.NVarChar, 255, "Title"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Description", System.Data.SqlDbType.NVarChar, 2000, "Description"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Publish", System.Data.SqlDbType.Bit, 1, "Publish"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rating", System.Data.SqlDbType.TinyInt, 1, "Rating"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureBy", System.Data.SqlDbType.Int, 4, "PictureBy"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureSort", System.Data.SqlDbType.Int, 4, "PictureSort"));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Description", System.Data.SqlDbType.NVarChar, 2000, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Description", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Filename", System.Data.SqlDbType.NVarChar, 150, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Filename", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureBy", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureBy", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureDate", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PictureSort", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PictureSort", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Publish", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Publish", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Rating", System.Data.SqlDbType.TinyInt, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Rating", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_Title", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand4.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PictureID", System.Data.SqlDbType.Int, 4, "PictureID"));
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.setCategoryPicSelection);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 150);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(344, 32);
			this.panel1.TabIndex = 1;
			// 
			// setCategoryPicSelection
			// 
			this.setCategoryPicSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.setCategoryPicSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.setCategoryPicSelection.Location = new System.Drawing.Point(8, 8);
			this.setCategoryPicSelection.Name = "setCategoryPicSelection";
			this.setCategoryPicSelection.Size = new System.Drawing.Size(216, 21);
			this.setCategoryPicSelection.TabIndex = 15;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(232, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(112, 23);
			this.button1.TabIndex = 14;
			this.button1.Text = "Set as category pic";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// fPicture
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(368, 254);
			this.Controls.Add(this.rating);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnMovePrevious);
			this.Controls.Add(this.btnMoveNext);
			this.Controls.Add(this.chkPublish);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.panelPic);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimizeBox = false;
			this.Name = "fPicture";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Picture Detail";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.fPicture_Closing);
			this.Load += new System.EventHandler(this.fPicture_Load);
			this.VisibleChanged += new System.EventHandler(this.fPicture_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.dsPicture)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPageBasicInfo.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.rating)).EndInit();
			this.panelPic.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			buttonPushed = true;

			mblnCancel = true;
			this.Visible = false;

			if (fPV != null)
				fPV.Dispose();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			buttonPushed			= true;

			// reset next and previous
			m_blnMoveNext			= false;
			m_blnMovePrevious		= false;

			SavePicture();

			if (fPV != null)
				fPV.Dispose();
		}

		private void SavePicture() 
		{
			dsPicture.Picture[0].PictureBy = personSelect1.SelectedPersonID;
			this.BindingContext[dsPicture, "Picture"].EndCurrentEdit();

			daPicture.Update(dsPicture, "Picture");
			daPictureCategory.Update(dsPicture, "PictureCategory");
			daPicturePerson.Update(dsPicture, "PicturePerson");
			daPictureGroup.Update(dsPicture, "PictureGroup");

			mblnCancel = false;
			
			if (m_blnMoveNext || m_blnMovePrevious)
			{

				SqlCommand cmd = new SqlCommand("select PictureId from Picture p where " + navigationControlsDataQuery, cn);
				cn.Open();
                
				SqlDataReader dr	= cmd.ExecuteReader(CommandBehavior.SingleResult);
				int previous		= 0;
				int next			= 0;
				while (dr.Read())
				{
					int current		= dr.GetInt32(0);

					// Check if current record matches and previous was what we wanted
					if (current == pictureId && m_blnMovePrevious)
					{
						next = previous;
						break;
					}

					// Check if previous was pictureId, then we are now on the right rec
					if (previous == pictureId && m_blnMoveNext)
					{
						next = current;
						break;
					}

					// Save this rec
					previous		= current;
				}
				dr.Close();
				cn.Close();

				// If we got a value, load it
				if (next != 0)
				{
					LoadPicture(next);
				}
				else
				{
					this.Visible	= false;
				}
			}
			else
			{
				this.Dispose();
			}

		}

		public bool LoadPicture(int id) 
		{
			pictureId = id;
			
			// clear everything
			dsPicture.Clear();
						
			// Picture details
            daPicture.SelectCommand.Parameters["@PictureID"].Value = pictureId;
			daPicture.Fill(dsPicture, "Picture");
			
			// Person details
			daPicturePerson.SelectCommand.Parameters["@PictureID"].Value = pictureId;
			daPicturePerson.Fill(dsPicture, "PicturePerson");
			
			// Category details
			daPictureCategory.SelectCommand.Parameters["@PictureID"].Value = pictureId;
			daPictureCategory.Fill(dsPicture, "PictureCategory");

			// Group details
			daPictureGroup.SelectCommand.Parameters["@PictureID"].Value = pictureId;
			daPictureGroup.Fill(dsPicture, "PictureGroup");

			personSelect1.ClearSelectedPerson();
			if (!dsPicture.Picture[0].IsPictureByNull())
				personSelect1.SelectedPersonID = dsPicture.Picture[0].PictureBy;

			// load categories
			categoryPicker1.ClearSelectedCategories();
			foreach(DataSetPicture.PictureCategoryRow pcr in dsPicture.PictureCategory.Rows)
			{
                categoryPicker1.AddSelectedCategory(pcr.CategoryID);
			}
			UpdateCategoryList();

			// load people
			personPicker1.ClearSelectedPeople();
			foreach(DataSetPicture.PicturePersonRow ppr in dsPicture.PicturePerson.Rows)
			{
				personPicker1.AddSelectedPerson(ppr.PersonID);
			}

			// Load groups
			groupPicker1.ClearSelectedGroups();
			foreach (DataSetPicture.PictureGroupRow groupRow in dsPicture.PictureGroup.Rows) 
			{
				groupPicker1.AddSelectedGroup(groupRow.GroupID);
			}

			// load picture if possible
//			string strFileName = dsPicture.Picture[0].Filename.ToString();
//			if (strFileName.Length > 0) 
//			{
//				LoadImage();
//			}

			return true;
		}

		public bool LoadImage() 
		{
			pictureDisplay1.LoadImage(pictureId);

			return false;
		}

		private void categoryPicker1_AddedCategory(object sender, msn2.net.Pictures.Controls.CategoryPickerEventArgs e)
		{
			// add the category to the PictureCategory dataset
			dsPicture.PictureCategory.AddPictureCategoryRow
				((DataSetPicture.PictureRow) dsPicture.Picture.Rows[0], e.Category.CategoryId);

			UpdateCategoryList();
		}

		private void categoryPicker1_RemovedCategory(object sender, msn2.net.Pictures.Controls.CategoryPickerEventArgs e)
		{
			DataSetPicture.PictureCategoryRow pcr = 
				dsPicture.PictureCategory.FindByPictureIDCategoryID(dsPicture.Picture[0].PictureID, e.Category.CategoryId);
			
			if (pcr != null) 
			{
                pcr.Delete();
			}

			UpdateCategoryList();
		}

		private void UpdateCategoryList()
		{
			setCategoryPicSelection.Items.Clear();

			foreach (int categoryId in categoryPicker1.selectedCategories)
			{
				Category category = PicContext.Current.CategoryManager.GetCategory(categoryId);
				setCategoryPicSelection.Items.Add(category);
			}
		}

		private void personPicker1_AddedPerson(object sender, msn2.net.Pictures.Controls.PersonPickerEventArgs e)
		{
			// add the person to the PicturePerson dataset
			dsPicture.PicturePerson.AddPicturePersonRow
				((DataSetPicture.PictureRow) dsPicture.Picture.Rows[0], e.PersonID);
		}

		private void personPicker1_RemovedPerson(object sender, msn2.net.Pictures.Controls.PersonPickerEventArgs e)
		{
			DataSetPicture.PicturePersonRow ppr = 
				dsPicture.PicturePerson.FindByPictureIDPersonID(dsPicture.Picture[0].PictureID, e.PersonID);

			if (ppr != null)
			{
				ppr.Delete();
			}
			
		}

		private void btnMoveNext_Click(object sender, System.EventArgs e)
		{
			buttonPushed = true;

			m_blnMoveNext = true;
			m_blnMovePrevious = false;
			SavePicture();

		}

		private void btnMovePrevious_Click(object sender, System.EventArgs e)
		{
			buttonPushed = true;

			m_blnMovePrevious = true;
			m_blnMoveNext = false;
			SavePicture();
		}

		private void fPicture_VisibleChanged(object sender, System.EventArgs e)
		{
			if (this.Visible) 
			{
				if (m_intLeft > 0 || m_intHeight > 0) 
				{
					this.Left = m_intLeft;
					this.Top = m_intTop;
					this.Height = m_intHeight;
					this.Width = m_intWidth;
				}
			} 
			else 
			{
				m_intLeft = this.Left;
				m_intTop = this.Top;
				m_intHeight = this.Height;
				m_intWidth = this.Width;
			}
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			RotateImage(RotateFlipType.Rotate90FlipNone);
		}

		public void RotateImage(RotateFlipType rft)
		{
			// clear image on main form if present
			if (m_fMain != null)
			{
				m_fMain.ClearCurrentImage();
			}

			fStatus status = new fStatus("Please wait while the image is rotated...");
			status.Show();

			picsvc.PictureManager pm			= new picsvc.PictureManager();
			pm.RotateImage(Convert.ToInt32(pictureId), (picsvc.RotateFlipType) Enum.Parse(typeof(picsvc.RotateFlipType), rft.ToString()));

			status.Hide();
			status.Dispose();

			MessageBox.Show("The image has been resized.  Refresh the web page to view the updated image.", "Rotate Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void fPicture_Load(object sender, System.EventArgs e)
		{
//			pbPic.ContextMenu = menuImage;
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			RotateImage(RotateFlipType.Rotate180FlipNone);
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			RotateImage(RotateFlipType.Rotate270FlipNone);
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			RotateImage(RotateFlipType.RotateNoneFlipX);
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			RotateImage(RotateFlipType.RotateNoneFlipY);
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			RotateImage(RotateFlipType.RotateNoneFlipXY);
		}

		private void groupPicker1_AddedGroup(object sender, msn2.net.Pictures.Controls.GroupPickerEventArgs e)
		{
			// add the group to the PictureGroup dataset
			dsPicture.PictureGroup.AddPictureGroupRow
				((DataSetPicture.PictureRow) dsPicture.Picture.Rows[0], e.GroupID);
		}

		private void groupPicker1_RemovedGroup(object sender, msn2.net.Pictures.Controls.GroupPickerEventArgs e)
		{
			int pictureId = dsPicture.Picture[0].PictureID;

			foreach (DataSetPicture.PictureGroupRow row in dsPicture.PictureGroup.Rows) 
			{
				if (row.PictureID == pictureId && row.GroupID == e.GroupID) 
				{
					row.Delete();
					break;
				}
			}
		}

		private void fPicture_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!buttonPushed)
				btnCancel_Click(sender, e);
		}

		private void txtTitle_Enter(object sender, System.EventArgs e)
		{
			if (txtTitle.Text.Equals("(new picture)")) 
			{
				txtTitle.SelectionStart = 0;
				txtTitle.SelectionLength = txtTitle.Text.Length;
			}

		}

		private void personPicker1_Enter(object sender, System.EventArgs e)
		{
			this.AcceptButton = null;
		}

		private void personPicker1_Leave(object sender, System.EventArgs e)
		{
			this.AcceptButton = btnOK;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (setCategoryPicSelection.SelectedIndex < 0)
			{
				return;
			}

			Category category = (Category) setCategoryPicSelection.SelectedItem;
			
			PicContext.Current.CategoryManager.SetCategoryPictureId(category.CategoryId, this.pictureId);
		}

		public bool MoveNext 
		{
			get 
			{
				return m_blnMoveNext;
			}
		}

		public bool MovePrevious
		{
			get 
			{
				return m_blnMovePrevious;
			}
		}

		public fMain MainForm 
		{
			set 
			{
				m_fMain = value;

				btnMoveNext.Visible = true;
				btnMovePrevious.Visible	= true;
			}
		}


	}
}
