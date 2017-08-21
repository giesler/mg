; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CUpdateWarning
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "autorun.h"
LastPage=0

ClassCount=6
Class1=CAutorunApp
Class2=CAboutDlg
Class3=CAutorunDlg
Class4=CRestartDlg
Class5=CSetupDlg

ResourceCount=5
Resource1=IDD_ABOUTBOX
Resource2=IDD_AUTORUN_DIALOG
Resource3=IDD_SETUP
Resource4=IDD_RESTART
Class6=CUpdateWarning
Resource5=IDD_UPDATEWARNING

[CLS:CAutorunApp]
Type=0
BaseClass=CWinApp
HeaderFile=Autorun.h
ImplementationFile=Autorun.cpp
LastObject=CAutorunApp

[CLS:CAboutDlg]
Type=0
BaseClass=CDialog
HeaderFile=AutorunDlg.cpp
ImplementationFile=AutorunDlg.cpp
LastObject=CAboutDlg
Filter=D
VirtualFilter=dWC

[CLS:CAutorunDlg]
Type=0
BaseClass=CDialog
HeaderFile=AutorunDlg.h
ImplementationFile=AutorunDlg.cpp
Filter=D
VirtualFilter=dWC
LastObject=CAutorunDlg

[CLS:CRestartDlg]
Type=0
BaseClass=CDialog
HeaderFile=RestartDlg.h
ImplementationFile=RestartDlg.cpp
Filter=D
VirtualFilter=dWC

[CLS:CSetupDlg]
Type=0
BaseClass=CDialog
HeaderFile=SetupDlg.h
ImplementationFile=SetupDlg.cpp

[DLG:IDD_ABOUTBOX]
Type=1
Class=CAboutDlg
ControlCount=8
Control1=IDOK,button,1342242817
Control2=IDC_STATIC,static,1342177283
Control3=IDC_STATIC,static,1342308352
Control4=IDC_STATIC,static,1342308352
Control5=IDC_STATIC,static,1342308352
Control6=IDC_STATIC,static,1342308352
Control7=IDC_HYPERLINK,static,1342308352
Control8=IDC_STATIC,static,1342308352

[DLG:IDD_AUTORUN_DIALOG]
Type=1
Class=CAutorunDlg
ControlCount=3
Control1=IDOK,button,1073741825
Control2=IDCANCEL,button,1073741824
Control3=IDC_PIC,static,1342177294

[DLG:IDD_RESTART]
Type=1
Class=CRestartDlg
ControlCount=4
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_PBAR,msctls_progress32,1342177280
Control4=IDC_MESSAGE,static,1342308352

[DLG:IDD_SETUP]
Type=1
Class=CSetupDlg
ControlCount=5
Control1=IDC_SETUPDLG_MSG,static,1342308352
Control2=IDC_STATIC,static,1342177283
Control3=IDC_PROGRESS1,msctls_progress32,1342177280
Control4=IDC_CURSTATUS,static,1342308352
Control5=IDC_CANCEL,button,1342242816

[DLG:IDD_UPDATEWARNING]
Type=1
Class=CUpdateWarning
ControlCount=5
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_MESSAGE,static,1342308352
Control4=IDC_COMPONENTLIST,SysListView32,1350682625
Control5=IDC_STATIC,static,1342177283

[CLS:CUpdateWarning]
Type=0
HeaderFile=UpdateWarning.h
ImplementationFile=UpdateWarning.cpp
BaseClass=CDialog
Filter=D
LastObject=IDC_COMPONENTLIST
VirtualFilter=dWC

