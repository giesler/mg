'-------------------------------------------------------------------------- BuildFields

'Generates C# code to populate data structures with
'field names and values

'STRUCTURES:
'
'	public struct IndexField
'	{
'		public string Name;
'		public string[] Values;
'
'		public static IndexFields[] Fields;	
'		public static IndexField()
'		{
'			#region Field Definitions
'			<insert output here>
'			#endregion
'		}
'	}
'	

'SAMPLE OUTPUT:
'
'	//create field array
'	IndexField f;
'	Fields = new IndexField[<fieldcount>];
'
'	//<name>
'	f = Fields[0];
'	f.Name = "<name>";
'	f.Values = new string[] {
'		"<val0>",
'		"<val2>"
'	};
'
'	<repeat>

'Open ADO connection
Dim ado, rsf, rsv, fcount, vcount
Set ado = CreateObject("ADODB.Connection")
ado.Open "PROVIDER=SQLOLEDB;Data Source=amstest;Initial Catalog=xmcatalog", "sa", "%makeme$%"

'Open output file
Dim fso, out
Set fso = CreateObject("Scripting.FileSystemObject")
Set out = fso.CreateTextFile("buildfields.txt", True)

'Retrieve field count, fields
fcount = ado.Execute("select count(*) as 'c' from _fields")("c")
Set rsf = ado.Execute("select * from _fields")

'Write header
out.WriteLine "	//create field array"
out.WriteLine "	IndexField f;"
out.WriteLine "	Fields = new IndexField[" & fcount & "];"

'Build fields
Dim x, n
x = 0
While Not rsf.Eof

	'Retrieve value count, values
	vcount = ado.Execute("select count(*) as 'c' from " & rsf("table"))("c")
	Set rsv = ado.Execute("select * from " & rsf("table"))

	'Write field header
	out.WriteLine "	"
	out.WriteLine "	//" & rsf("field")
	out.WriteLine "	f = Fields[" & x & "];"
	out.WriteLine "	f.Name = """ & rsf("field") & """;"
	out.WriteLine "	f.Values = new string[] {"

	'Write each value
	While Not rsv.Eof

		'Read name, move to next record
		n = rsv("name")
		rsv.MoveNext

		'Write value
		If rsv.EOF Then
			out.WriteLine "	""" & n & """"
		Else
			out.WriteLine "	""" & n & ""","
		End If

	Wend

	'Next field
	out.WriteLine "	};"
	rsf.MoveNext
	x = x + 1

Wend

'Finished writing
out.Close