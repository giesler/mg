using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace PicAdminCS
{
	/// <summary>
	/// Summary description for PersonSelect.
	/// </summary>
	public class PersonSelect : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btnSelectPictureBy;
		private System.Windows.Forms.TextBox txtPictureBy;
		private System.Windows.Forms.TextBox txtPersonName;
		private System.Windows.Forms.Button btnSelectPerson;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Data.SqlClient.SqlDataAdapter daPerson;
		private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
		private System.Data.SqlClient.SqlCommand sqlUpdateCommand1;
		private System.Data.SqlClient.SqlCommand sqlDeleteCommand1;
		private System.Data.SqlClient.SqlConnection cn;
		private PicAdminCS.DataSetPerson dsPerson;

		private DataSetPerson.PersonRow pr;

		public PersonSelect()
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
			this.sqlUpdateCommand1 = new System.Data.SqlClient.SqlCommand();
			this.cn = new System.Data.SqlClient.SqlConnection();
			this.txtPersonName = new System.Windows.Forms.TextBox();
			this.daPerson = new System.Data.SqlClient.SqlDataAdapter();
			this.sqlDeleteCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			this.txtPictureBy = new System.Windows.Forms.TextBox();
			this.btnSelectPictureBy = new System.Windows.Forms.Button();
			this.btnSelectPerson = new System.Windows.Forms.Button();
			this.dsPerson = new PicAdminCS.DataSetPerson();
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).BeginInit();
			this.SuspendLayout();
			// 
			// sqlUpdateCommand1
			// 
			this.sqlUpdateCommand1.CommandText = @"UPDATE Person SET LastName = @LastName, FirstName = @FirstName, FullName = @FullName WHERE (PersonID = @Original_PersonID) AND (FirstName = @Original_FirstName OR @Original_FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @Original_FullName OR @Original_FullName1 IS NULL AND FullName IS NULL) AND (LastName = @Original_LastName OR @Original_LastName1 IS NULL AND LastName IS NULL); SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (PersonID = @Select_PersonID)";
			this.sqlUpdateCommand1.Connection = this.cn;
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlUpdateCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Select_PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			// 
			// cn
			// 
			this.cn.ConnectionString = "data source=kyle;initial catalog=picdb;integrated security=SSPI;persist security " +
				"info=False;workstation id=CHEF;packet size=4096";
			// 
			// txtPersonName
			// 
			this.txtPersonName.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtPersonName.Name = "txtPersonName";
			this.txtPersonName.ReadOnly = true;
			this.txtPersonName.Size = new System.Drawing.Size(288, 20);
			this.txtPersonName.TabIndex = 8;
			this.txtPersonName.Text = "";
			// 
			// daPerson
			// 
			this.daPerson.DeleteCommand = this.sqlDeleteCommand1;
			this.daPerson.InsertCommand = this.sqlInsertCommand1;
			this.daPerson.SelectCommand = this.sqlSelectCommand1;
			this.daPerson.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							   new System.Data.Common.DataTableMapping("Table", "Person", new System.Data.Common.DataColumnMapping[] {
																																																		 new System.Data.Common.DataColumnMapping("PersonID", "PersonID"),
																																																		 new System.Data.Common.DataColumnMapping("LastName", "LastName"),
																																																		 new System.Data.Common.DataColumnMapping("FirstName", "FirstName"),
																																																		 new System.Data.Common.DataColumnMapping("FullName", "FullName")})});
			this.daPerson.UpdateCommand = this.sqlUpdateCommand1;
			// 
			// sqlDeleteCommand1
			// 
			this.sqlDeleteCommand1.CommandText = @"DELETE FROM Person WHERE (PersonID = @PersonID) AND (FirstName = @FirstName OR @FirstName1 IS NULL AND FirstName IS NULL) AND (FullName = @FullName OR @FullName1 IS NULL AND FullName IS NULL) AND (LastName = @LastName OR @LastName1 IS NULL AND LastName IS NULL)";
			this.sqlDeleteCommand1.Connection = this.cn;
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName1", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			this.sqlDeleteCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName1", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Original, null));
			// 
			// sqlInsertCommand1
			// 
			this.sqlInsertCommand1.CommandText = "INSERT INTO Person(LastName, FirstName, FullName) VALUES (@LastName, @FirstName, " +
				"@FullName); SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (Pe" +
				"rsonID = @@IDENTITY)";
			this.sqlInsertCommand1.Connection = this.cn;
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "LastName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FirstName", System.Data.DataRowVersion.Current, null));
			this.sqlInsertCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FullName", System.Data.SqlDbType.NVarChar, 100, System.Data.ParameterDirection.Input, true, ((System.Byte)(0)), ((System.Byte)(0)), "FullName", System.Data.DataRowVersion.Current, null));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT PersonID, LastName, FirstName, FullName FROM Person WHERE (PersonID = @Per" +
				"sonID)";
			this.sqlSelectCommand1.Connection = this.cn;
			this.sqlSelectCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PersonID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PersonID", System.Data.DataRowVersion.Current, null));
			// 
			// txtPictureBy
			// 
			this.txtPictureBy.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtPictureBy.BackColor = System.Drawing.SystemColors.Control;
			this.txtPictureBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtPictureBy.Location = new System.Drawing.Point(72, 56);
			this.txtPictureBy.Name = "txtPictureBy";
			this.txtPictureBy.Size = new System.Drawing.Size(320, 20);
			this.txtPictureBy.TabIndex = 6;
			this.txtPictureBy.TabStop = false;
			this.txtPictureBy.Text = "";
			// 
			// btnSelectPictureBy
			// 
			this.btnSelectPictureBy.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnSelectPictureBy.Location = new System.Drawing.Point(400, 56);
			this.btnSelectPictureBy.Name = "btnSelectPictureBy";
			this.btnSelectPictureBy.Size = new System.Drawing.Size(24, 20);
			this.btnSelectPictureBy.TabIndex = 7;
			this.btnSelectPictureBy.Text = "...";
			// 
			// btnSelectPerson
			// 
			this.btnSelectPerson.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.btnSelectPerson.Location = new System.Drawing.Point(288, 0);
			this.btnSelectPerson.Name = "btnSelectPerson";
			this.btnSelectPerson.Size = new System.Drawing.Size(24, 20);
			this.btnSelectPerson.TabIndex = 9;
			this.btnSelectPerson.Text = "...";
			this.btnSelectPerson.Click += new System.EventHandler(this.btnSelectPerson_Click);
			// 
			// dsPerson
			// 
			this.dsPerson.DataSetName = "DataSetPerson";
			this.dsPerson.Locale = new System.Globalization.CultureInfo("en-US");
			this.dsPerson.Namespace = "http://www.tempuri.org/DataSetPerson.xsd";
			// 
			// PersonSelect
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnSelectPerson,
																		  this.txtPersonName,
																		  this.btnSelectPictureBy,
																		  this.txtPictureBy});
			this.Name = "PersonSelect";
			this.Size = new System.Drawing.Size(312, 21);
			((System.ComponentModel.ISupportInitialize)(this.dsPerson)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSelectPerson_Click(object sender, System.EventArgs e)
		{
			fPersonSelect fPS = new fPersonSelect();
			fPS.ShowDialog();

			if (!fPS.Cancel) 
			{
				pr = fPS.SelectedPerson;
				txtPersonName.Text = pr.FullName;
			}

		}

		public DataSetPerson.PersonRow SelectedPerson
		{
			get 
			{
				return pr;
			}
			set 
			{
				if (value == null) 
					return;

				// fill the da with the selected row
				daPerson.SelectCommand.Parameters["@PersonID"].Value = value.PersonID;
				daPerson.Fill(dsPerson, "Person");

				pr = (DataSetPerson.PersonRow) dsPerson.Person.Rows[0];
                txtPersonName.Text = pr.FullName;
			}
		}

		public int SelectedPersonID 
		{
			get 
			{
				if (pr == null)
					return 0;
				else
                    return pr.PersonID;
			}
			set 
			{
				if (value == 0)
					return;

				// fill the da with the selected row
				daPerson.SelectCommand.Parameters["@PersonID"].Value = value;
				daPerson.Fill(dsPerson, "Person");

				pr = (DataSetPerson.PersonRow) dsPerson.Person.Rows[0];
				txtPersonName.Text = pr.FullName;
			}
		}
	}
}
