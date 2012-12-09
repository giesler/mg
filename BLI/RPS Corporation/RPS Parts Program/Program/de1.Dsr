VERSION 5.00
Begin {C0E45035-5775-11D0-B388-00A0C9055D8E} de1 
   ClientHeight    =   7035
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   8775
   _ExtentX        =   15478
   _ExtentY        =   12409
   FolderFlags     =   1
   TypeLibGuid     =   "{A799F843-D246-4210-8679-B9F0245482AB}"
   TypeInfoGuid    =   "{B854612A-BE19-4229-A054-06056946A0FE}"
   TypeInfoCookie  =   0
   Version         =   4
   NumConnections  =   3
   BeginProperty Connection1 
      ConnectionName  =   "cnParts"
      ConnDispId      =   1001
      SourceOfData    =   3
      ConnectionSource=   $"de1.dsx":0000
      Expanded        =   -1  'True
      QuoteChar       =   96
      SeparatorChar   =   46
   EndProperty
   BeginProperty Connection2 
      ConnectionName  =   "cnRemoteParts"
      ConnDispId      =   1010
      SourceOfData    =   3
      ConnectionSource=   $"de1.dsx":0099
      QuoteChar       =   34
      SeparatorChar   =   46
   EndProperty
   BeginProperty Connection3 
      ConnectionName  =   "Connection1"
      ConnDispId      =   1011
      SourceOfData    =   3
      QuoteChar       =   34
      SeparatorChar   =   46
   EndProperty
   NumRecordsets   =   5
   BeginProperty Recordset1 
      CommandName     =   "Parts"
      CommDispId      =   1002
      RsDispId        =   1004
      CommandText     =   "SELECT PartID, RPSPartNum, PartName, RPSPNSort, USADealerNet, USASuggestedList, `Note` FROM tblDealerParts ORDER BY RPSPNSort"
      ActiveConnectionName=   "cnParts"
      CommandType     =   1
      Expanded        =   -1  'True
      IsRSReturning   =   -1  'True
      NumFields       =   7
      BeginProperty Field1 
         Precision       =   10
         Size            =   4
         Scale           =   0
         Type            =   3
         Name            =   "PartID"
         Caption         =   "PartID"
      EndProperty
      BeginProperty Field2 
         Precision       =   0
         Size            =   50
         Scale           =   0
         Type            =   202
         Name            =   "RPSPartNum"
         Caption         =   "RPS Part Num"
      EndProperty
      BeginProperty Field3 
         Precision       =   0
         Size            =   150
         Scale           =   0
         Type            =   202
         Name            =   "PartName"
         Caption         =   "Part Name"
      EndProperty
      BeginProperty Field4 
         Precision       =   0
         Size            =   30
         Scale           =   0
         Type            =   202
         Name            =   "RPSPNSort"
         Caption         =   "RPSPNSort"
      EndProperty
      BeginProperty Field5 
         Precision       =   19
         Size            =   8
         Scale           =   0
         Type            =   6
         Name            =   "USADealerNet"
         Caption         =   "Dealer Net"
      EndProperty
      BeginProperty Field6 
         Precision       =   19
         Size            =   8
         Scale           =   0
         Type            =   6
         Name            =   "USASuggestedList"
         Caption         =   "Suggested List"
      EndProperty
      BeginProperty Field7 
         Precision       =   0
         Size            =   255
         Scale           =   0
         Type            =   202
         Name            =   "Note"
         Caption         =   "Note"
      EndProperty
      NumGroups       =   0
      ParamCount      =   0
      RelationCount   =   0
      AggregateCount  =   0
   EndProperty
   BeginProperty Recordset2 
      CommandName     =   "PartsModels"
      CommDispId      =   -1
      RsDispId        =   -1
      CommandText     =   "SELECT fkPartID, Model, Quantity, Optional AS Options FROM tblDealerPartsModels"
      ActiveConnectionName=   "cnParts"
      CommandType     =   1
      RelateToParent  =   -1  'True
      ParentCommandName=   "Parts"
      Expanded        =   -1  'True
      IsRSReturning   =   -1  'True
      NumFields       =   4
      BeginProperty Field1 
         Precision       =   10
         Size            =   4
         Scale           =   0
         Type            =   3
         Name            =   "fkPartID"
         Caption         =   "fkPartID"
      EndProperty
      BeginProperty Field2 
         Precision       =   0
         Size            =   20
         Scale           =   0
         Type            =   202
         Name            =   "Model"
         Caption         =   "Model"
      EndProperty
      BeginProperty Field3 
         Precision       =   15
         Size            =   8
         Scale           =   0
         Type            =   5
         Name            =   "Quantity"
         Caption         =   "Quantity"
      EndProperty
      BeginProperty Field4 
         Precision       =   5
         Size            =   2
         Scale           =   0
         Type            =   2
         Name            =   "Options"
         Caption         =   "Options"
      EndProperty
      NumGroups       =   0
      ParamCount      =   0
      RelationCount   =   1
      BeginProperty Relation1 
         ParentField     =   "PartID"
         ChildField      =   "fkPartID"
         ParentType      =   0
         ChildType       =   0
      EndProperty
      AggregateCount  =   0
   EndProperty
   BeginProperty Recordset3 
      CommandName     =   "PartsOnly"
      CommDispId      =   1005
      RsDispId        =   1009
      CommandText     =   $"de1.dsx":0139
      ActiveConnectionName=   "cnParts"
      CommandType     =   1
      IsRSReturning   =   -1  'True
      NumFields       =   5
      BeginProperty Field1 
         Precision       =   0
         Size            =   50
         Scale           =   0
         Type            =   202
         Name            =   "RPS Part Num"
         Caption         =   "RPS Part Num"
      EndProperty
      BeginProperty Field2 
         Precision       =   0
         Size            =   150
         Scale           =   0
         Type            =   202
         Name            =   "Part Name"
         Caption         =   "Part Name"
      EndProperty
      BeginProperty Field3 
         Precision       =   19
         Size            =   8
         Scale           =   0
         Type            =   6
         Name            =   "Dealer Net"
         Caption         =   "Dealer Net"
      EndProperty
      BeginProperty Field4 
         Precision       =   19
         Size            =   8
         Scale           =   0
         Type            =   6
         Name            =   "Suggested List"
         Caption         =   "Suggested List"
      EndProperty
      BeginProperty Field5 
         Precision       =   0
         Size            =   255
         Scale           =   0
         Type            =   202
         Name            =   "Note"
         Caption         =   "Note"
      EndProperty
      NumGroups       =   0
      ParamCount      =   0
      RelationCount   =   0
      AggregateCount  =   0
   EndProperty
   BeginProperty Recordset4 
      CommandName     =   "PartsPretty"
      CommDispId      =   1012
      RsDispId        =   1024
      CommandText     =   $"de1.dsx":01F6
      ActiveConnectionName=   "cnParts"
      CommandType     =   1
      Expanded        =   -1  'True
      IsRSReturning   =   -1  'True
      NumFields       =   7
      BeginProperty Field1 
         Precision       =   10
         Size            =   4
         Scale           =   0
         Type            =   3
         Name            =   "PartID"
         Caption         =   "PartID"
      EndProperty
      BeginProperty Field2 
         Precision       =   0
         Size            =   50
         Scale           =   0
         Type            =   202
         Name            =   "RPSPartNum"
         Caption         =   "RPSPartNum"
      EndProperty
      BeginProperty Field3 
         Precision       =   0
         Size            =   150
         Scale           =   0
         Type            =   202
         Name            =   "PartName"
         Caption         =   "PartName"
      EndProperty
      BeginProperty Field4 
         Precision       =   0
         Size            =   30
         Scale           =   0
         Type            =   202
         Name            =   "RPSPNSort"
         Caption         =   "RPSPNSort"
      EndProperty
      BeginProperty Field5 
         Precision       =   0
         Size            =   255
         Scale           =   0
         Type            =   202
         Name            =   "USADealerNet"
         Caption         =   "USADealerNet"
      EndProperty
      BeginProperty Field6 
         Precision       =   0
         Size            =   255
         Scale           =   0
         Type            =   202
         Name            =   "USASuggestedList"
         Caption         =   "USASuggestedList"
      EndProperty
      BeginProperty Field7 
         Precision       =   0
         Size            =   255
         Scale           =   0
         Type            =   202
         Name            =   "Note"
         Caption         =   "Note"
      EndProperty
      NumGroups       =   0
      ParamCount      =   0
      RelationCount   =   0
      AggregateCount  =   0
   EndProperty
   BeginProperty Recordset5 
      CommandName     =   "PartsModelsPretty"
      CommDispId      =   -1
      RsDispId        =   -1
      CommandText     =   "SELECT fkPartID, Model, Quantity, Optional AS Options FROM tblDealerPartsModels"
      ActiveConnectionName=   "cnParts"
      CommandType     =   1
      RelateToParent  =   -1  'True
      ParentCommandName=   "PartsPretty"
      Expanded        =   -1  'True
      IsRSReturning   =   -1  'True
      NumFields       =   4
      BeginProperty Field1 
         Precision       =   10
         Size            =   4
         Scale           =   0
         Type            =   3
         Name            =   "fkPartID"
         Caption         =   "fkPartID"
      EndProperty
      BeginProperty Field2 
         Precision       =   0
         Size            =   20
         Scale           =   0
         Type            =   202
         Name            =   "Model"
         Caption         =   "Model"
      EndProperty
      BeginProperty Field3 
         Precision       =   15
         Size            =   8
         Scale           =   0
         Type            =   5
         Name            =   "Quantity"
         Caption         =   "Quantity"
      EndProperty
      BeginProperty Field4 
         Precision       =   5
         Size            =   2
         Scale           =   0
         Type            =   2
         Name            =   "Options"
         Caption         =   "Options"
      EndProperty
      NumGroups       =   0
      ParamCount      =   0
      RelationCount   =   1
      BeginProperty Relation1 
         ParentField     =   "PartID"
         ChildField      =   "fkPartID"
         ParentType      =   0
         ChildType       =   0
      EndProperty
      AggregateCount  =   0
   EndProperty
End
Attribute VB_Name = "de1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
