'-------------------------------------------------------------------------- BuildFields

'Generates C++ code to populate data structures with
'field names and values

'STRUCTURES:
'
'	struct field
'	{
'		char *name;
'		int count;
'		char **value;
'	};
'	int mFieldCount;
'	field *mFields;

'SAMPLE OUTPUT:
'
'	//create field array
'	mFieldCount = 24;
'	mFields = new field[24];
'
'	//<name>
'	f = &mFields[0];
'	f->name = "<name>";
'	f->count = 8;
'	f->value = new char*[8];
'	f->value[0] = "<val0>";

'Open ADO connection
Dim ado, rsf, rsv, fcount, vcount
Set ado = CreateObject("ADODB.Connection")
ado.Open "PROVIDER=SQLOLEDB;Data Source=SERVER;Initial Catalog=xmcatalog", "sa", ""

'Open output file
Dim fso, out
Set fso = CreateObject("Scripting.FileSystemObject")
Set out = fso.CreateTextFile("buildfields.txt", True)

'Retrieve field count, fields
fcount = ado.Execute("select count(*) as 'c' from _fields")("c")
Set rsf = ado.Execute("select * from _fields")

'Write header
out.WriteLine "void CIndexer::PopulateFields()"
out.WriteLine "{" 
out.WriteLine "	//create field array"
out.WriteLine "	field *f;"
out.WriteLine "	mFieldCount = " & fcount & ";"
out.WriteLine "	mFields = new field[" & fcount & "];"

'Build fields
Dim x, y
x = 0
While Not rsf.Eof

	'Retrieve value count, values
	vcount = ado.Execute("select count(*) as 'c' from " & rsf("table"))("c")
	Set rsv = ado.Execute("select * from " & rsf("table"))

	'Write field header
	out.WriteLine "	"
	out.WriteLine "	//" & rsf("field")
	out.WriteLine "	f = &mFields[" & x & "];"
	out.WriteLine "	f->name = """ & rsf("field") & """;"
	out.WriteLine "	f->count = " & vcount & ";"
	out.WriteLine "	f->value = new char*[" & vcount & "];"

	'Write each value
	y = 0
	While Not rsv.Eof

		'Write value
		out.WriteLine "	f->value[" & y & "] = """ & rsv("name") & """;"

		'Next value
		rsv.MoveNext
		y = y + 1

	Wend

	'Next field
	rsf.MoveNext
	x = x + 1

Wend

'Write closing brackets
out.WriteLine "}"

'Finished writing
out.Close