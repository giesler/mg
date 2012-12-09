Imports System.Drawing
Imports System.WinForms

Imports System.ComponentModel


Public Class Category
    Inherits System.WinForms.Form

    Public Sub New()
        MyBase.New

        Category = Me

        'This call is required by the Win Form Designer.
        InitializeComponent()

       'TODO: Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Overrides Public Sub Dispose()
        MyBase.Dispose
        components.Dispose
    End Sub 

#Region " Windows Form Designer generated code "

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.Container
  Private WithEvents chkChildren As System.WinForms.CheckBox
  Private WithEvents btnCancel As System.WinForms.Button
  Private WithEvents btnOK As System.WinForms.Button
  Private WithEvents Label1 As System.WinForms.Label
  Private WithEvents txtCategoryName As System.WinForms.TextBox
  
    Dim WithEvents Category As System.WinForms.Form

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Me.txtCategoryName = New System.WinForms.TextBox()
    Me.btnCancel = New System.WinForms.Button()
    Me.chkChildren = New System.WinForms.CheckBox()
    Me.btnOK = New System.WinForms.Button()
    Me.Label1 = New System.WinForms.Label()
    
    '@design Me.TrayHeight = 0
    '@design Me.TrayLargeIcon = False
    '@design Me.TrayAutoArrange = True
    txtCategoryName.Location = New System.Drawing.Point(8, 32)
    txtCategoryName.TabIndex = 0
    txtCategoryName.Anchor = System.WinForms.AnchorStyles.TopLeftRight
    txtCategoryName.Size = New System.Drawing.Size(290, 20)
    
    btnCancel.Location = New System.Drawing.Point(210, 82)
    btnCancel.DialogResult = System.WinForms.DialogResult.Cancel
    btnCancel.Size = New System.Drawing.Size(80, 24)
    btnCancel.TabIndex = 3
    btnCancel.Anchor = System.WinForms.AnchorStyles.BottomRight
    btnCancel.Text = "&Cancel"
    
    chkChildren.Location = New System.Drawing.Point(16, 56)
    chkChildren.Text = "This category contains children categories"
    chkChildren.Size = New System.Drawing.Size(272, 16)
    chkChildren.TabIndex = 4
    
    btnOK.Location = New System.Drawing.Point(122, 82)
    btnOK.Size = New System.Drawing.Size(80, 24)
    btnOK.TabIndex = 2
    btnOK.Anchor = System.WinForms.AnchorStyles.BottomRight
    btnOK.Text = "&OK"
    
    Label1.Location = New System.Drawing.Point(16, 16)
    Label1.Text = "Category Name"
    Label1.Size = New System.Drawing.Size(280, 16)
    Label1.TabIndex = 1
    Me.Text = "Category"
    Me.MaximizeBox = False
    Me.StartPosition = System.WinForms.FormStartPosition.CenterParent
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.CancelButton = btnCancel
    Me.BorderStyle = System.WinForms.FormBorderStyle.FixedDialog
    Me.AutoScroll = True
    Me.AcceptButton = btnOK
    Me.ControlBox = False
    Me.MinimizeBox = False
    Me.ClientSize = New System.Drawing.Size(306, 111)
    
    Me.Controls.Add(chkChildren)
    Me.Controls.Add(btnCancel)
    Me.Controls.Add(btnOK)
    Me.Controls.Add(Label1)
    Me.Controls.Add(txtCategoryName)
  End Sub
  
#End Region
  
  Property CategoryName() As String
    
    Get
      CategoryName = Me.txtCategoryName.Text
    End Get
    
    Set
      Me.txtCategoryName.Text = value
    End Set
    
  End Property
  
  Property HasChildren() As Boolean
    
    Get
      haschildren = Me.chkChildren.Checked
    End Get
    
    Set
      Me.chkChildren.Checked = Value
    End Set
    
  End Property
  
  Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Me.DialogResult = WinForms.DialogResult.Cancel
    Me.Visible = False
  End Sub
  
  Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Me.DialogResult = WinForms.DialogResult.OK
    Me.Visible = False
  End Sub
  
End Class
