''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Filename: StartDbServices.vbs
'
' Author:   Roger Doherty (lrdohert@microsoft.com)
'
' Date:     9/2/1999
' Modified:
'
' Description: VB Script file to be used on Win95/Win98 to autostart  
'              SQL Server or MSDE and SQLAgent.
'
' This file is part of the SQL / MSDE Delpoyment Toolkit
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT
' WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED,
' INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES
' OF MERCHANTABILITY AND/OR FITNESS FOR A  PARTICULAR
' PURPOSE.
'
' Copyright (C) 1999 Microsoft Corporation, All rights reserved
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Option Explicit

Call Main

Sub Main()
	Dim oDbServer

	On Error Resume Next

	'
	' Step 1: Create shell object
	'

	Set oDbServer = WScript.CreateObject("SQLDMO.SQLServer")
	TestForError

	'
	' Step 2: Start database server
	'

	oDbServer.Start False, "(local)"
	TestForError

	WScript.Quit 0		
End Sub

Sub TestForError()
	If Err.Number <> 0 Then
		WScript.Quit 1
	End If
End Sub