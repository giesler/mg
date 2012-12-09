namespace XMedia
{
    using System;
	using ADODB;

	public class XMAdo
	{
		private ADODB.Connection mConnection;

		public bool EnsureConnection()
		{
			//make sure that our database is connected
			if (mConnection==null)
			{
				mConnection = new ADODB.Connection();
			}
			if (mConnection.State==(int)ADODB.ObjectStateEnum.adStateOpen)
			{
				return true;
			}

			//database is not connected, open it
			string con;
			con = "PROVIDER=SQLOLEDB;Initial Catalog=xmcatalog;Data Source=amsdb01;Integrated Security=SSPI";
			try 
			{
				mConnection.Open(con, "", "", 0);
			}
			catch
			{
				//failed
				return false;
			}

			//database is now conencted
			return true;
		}

		public ADODB._Recordset SqlExec(string s)
		{
			//helper function
			int i = 0;
			object ri = i;
			return mConnection.Execute(s, ref ri, 0);
		}
	}
}