Imports System.ComponentModel
Imports System.Drawing
Imports System.WinForms

Private Class CategoryNode
  Inherits TreeNode
  
  Public CategoryID As Integer
  
  Public Sub CategoryNode(ByVal CategoryName As String, ByVal CategoryID As String)
    
  End Sub
  
  
End Class


Public Class Form1
  Inherits System.WinForms.Form
  
  Public Sub New()
    MyBase.New()
    
    Form1 = Me
    
    'This call is required by the Win Form Designer.
    InitializeComponent()
    
    'TODO: Add any initialization after the InitializeComponent() call
    
    cmdCategory.FillDataSet(dsCategory)
    
    Dim rootNode As TreeNode = tvCat.Nodes.Add("Category")
    
    LoadCategories(dsCategory, 1, rootNode)
    
  End Sub
  
  'Form overrides dispose to clean up the component list.
  Public Overrides Sub Dispose()
    MyBase.Dispose()
    components.Dispose()
  End Sub
  
  Public Sub LoadCategories(ByVal dsData As CdsCategory, ByVal intParentID As Integer, ByVal ParentNode As treenode)
    
    Dim li As ListItem, dv As DataView, child As TreeNode, i As Integer
    
    dv = New DataView(dsData.Tables("Category"))
    dv.RowFilter = "ParentID = " & intParentID & " AND CategoryID <> " & intParentID
    
    For i = 0 To dv.Count - 1
      child = ParentNode.Nodes.Add(dv(i)("CategoryName").ToString)
      LoadCategories(dsData, CInt(dv(i)("CategoryID")), child)
    Next i
    
    li = Me.lvSubCat.ListItems.Add("Add new category")
    
    
  End Sub
  
  
#Region " Windows Form Designer generated code "
  
  'Required by the Windows Form Designer
  Private components As System.ComponentModel.Container
  Private WithEvents dsCategory As Pic_Admin.CdsCategory
  
  Private WithEvents cmdCategory As System.Data.SQL.SQLDataSetCommand
  Private chName As System.WinForms.ColumnHeader
  
  Private WithEvents cnPicDB As System.Data.SQL.SQLConnection
  
  
  
  
  
  
  Private WithEvents lvSubCat As System.WinForms.ListView
  Private WithEvents tvCat As System.WinForms.TreeView
  
  Dim WithEvents Form1 As System.WinForms.Form
  
  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Me.chName = New System.WinForms.ColumnHeader()
    Me.lvSubCat = New System.WinForms.ListView()
    Me.dsCategory = New Pic_Admin.CdsCategory()
    Me.tvCat = New System.WinForms.TreeView()
    Me.cmdCategory = New System.Data.SQL.SQLDataSetCommand()
    Me.cnPicDB = New System.Data.SQL.SQLConnection()
    
    '@design Me.TrayHeight = 90
    '@design Me.TrayLargeIcon = False
    '@design Me.TrayAutoArrange = True
    chName.Text = "Name"
    chName.Width = 206
    chName.TextAlign = System.WinForms.HorizontalAlignment.Left
    
    lvSubCat.Location = New System.Drawing.Point(216, 32)
    lvSubCat.Size = New System.Drawing.Size(312, 344)
    lvSubCat.FullRowSelect = True
    lvSubCat.View = System.WinForms.View.Report
    lvSubCat.ForeColor = System.Drawing.SystemColors.WindowText
    lvSubCat.TabIndex = 1
    lvSubCat.Anchor = System.WinForms.AnchorStyles.All
    Dim a__1(1) As System.WinForms.ColumnHeader
    a__1(0) = chName
    lvSubCat.Columns.All = a__1
    
    '@design dsCategory.SetLocation(New System.Drawing.Point(194, 7))
    dsCategory.Prefix = ""
    dsCategory.DataSetName = "CdsCategory"
    dsCategory.Locale = New System.Globalization.CultureInfo("en-US")
    dsCategory.Namespace = "http://www.tempuri.org/CdsCategory.xsd"
    dsCategory.Category.Prefix = ""
    dsCategory.Category.TableName = "Category"
    
    tvCat.Location = New System.Drawing.Point(8, 32)
    tvCat.Size = New System.Drawing.Size(200, 344)
    tvCat.TabIndex = 0
    tvCat.Anchor = System.WinForms.AnchorStyles.TopBottomLeft
    tvCat.ShowRootLines = False
    
    '@design cmdCategory.SetLocation(New System.Drawing.Point(89, 7))
    Dim a__2() As System.Data.SQL.SQLParameter
    cmdCategory.UpdateCommand = New System.Data.SQL.SQLCommand(cnPicDB, "UPDATE Category SET ParentID = ?,  CategoryName = ?,  FullCategory = ?,  AccessLevel = ? WHERE (CategoryID = ?) ;SELECT CategoryID,  ParentID,  CategoryName,  FullCategory,  AccessLevel FROM Category WHERE (CategoryID = ?) ", System.Data.CommandType.Text, a__2, System.Data.UpdateRowSource.Both)
    Dim a__3() As System.Data.SQL.SQLParameter
    cmdCategory.DeleteCommand = New System.Data.SQL.SQLCommand(cnPicDB, "DELETE FROM Category WHERE (CategoryID = ?) ", System.Data.CommandType.Text, a__3, System.Data.UpdateRowSource.Both)
    Dim a__4() As System.Data.SQL.SQLParameter
    cmdCategory.InsertCommand = New System.Data.SQL.SQLCommand(cnPicDB, "INSERT INTO Category( ParentID,  CategoryName,  FullCategory,  AccessLevel ) VALUES( ?,  ?,  ?,  ? ) ;SELECT CategoryID,  ParentID,  CategoryName,  FullCategory,  AccessLevel FROM Category WHERE (CategoryID = @@IDENTITY) ", System.Data.CommandType.Text, a__4, System.Data.UpdateRowSource.Both)
    Dim a__5() As System.Data.SQL.SQLParameter
    cmdCategory.SelectCommand = New System.Data.SQL.SQLCommand(cnPicDB, "SELECT CategoryID,  ParentID,  CategoryName,  FullCategory,  AccessLevel FROM Category ", System.Data.CommandType.Text, a__5, System.Data.UpdateRowSource.Both)
    Dim a__6(5) As System.Data.Internal.DataColumnMapping
    a__6(0) = New System.Data.Internal.DataColumnMapping("CategoryID", "CategoryID")
    a__6(1) = New System.Data.Internal.DataColumnMapping("ParentID", "ParentID")
    a__6(2) = New System.Data.Internal.DataColumnMapping("CategoryName", "CategoryName")
    a__6(3) = New System.Data.Internal.DataColumnMapping("FullCategory", "FullCategory")
    a__6(4) = New System.Data.Internal.DataColumnMapping("AccessLevel", "AccessLevel")
    Dim a__7(1) As System.Data.Internal.DataTableMapping
    a__7(0) = New System.Data.Internal.DataTableMapping("Table", "Category", a__6)
    cmdCategory.TableMappings.All = a__7
    
    '@design cnPicDB.SetLocation(New System.Drawing.Point(7, 7))
    cnPicDB.ConnectionString = "data source=cheese; initial catalog=picdb; integrated security=sspi;"
    
    Me.Text = "Form1"
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(536, 381)
    
    Me.Controls.Add(lvSubCat)
    Me.Controls.Add(tvCat)
  End Sub
  
#End Region
  
  Public Sub lvSubCat_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvSubCat.DoubleClick
    
    Dim li As ListItem, intResult As Integer
    li = lvSubCat.SelectedItems(0)
    
    Dim f As New Category()
    Try
      If f.ShowDialog(Me) = DialogResult.OK Then
        Me.tvCat.Nodes.Add(f.CategoryName)
        
        
        
        msgbox("Dialog Result is OK", Microsoft.VisualBasic.MsgBoxStyle.Information)
      End If
      
      
    Catch ec As Exception
      msgbox(ec.ToString, Microsoft.VisualBasic.MsgBoxStyle.Information)
    End Try
    
    
  End Sub
  
  
End Class
