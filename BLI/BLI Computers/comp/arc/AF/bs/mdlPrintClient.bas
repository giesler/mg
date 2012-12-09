Attribute VB_Name = "mdlPrintClient"

Public Function DoPrintJob(Barcode As String, Count As Integer) As Boolean
On Error GoTo DoPrintJob_Error

Dim MaxBufferSize, JobSize, Job, x
Dim Batch As New Collection

'Try and open the port now, before paginating
Status "Opening communication port", True
'If Not OpenComPort Then
'    Status "Unable to open port on COM" & frmStatus.MSComm1.CommPort, False
'    DoPrintJob = False
'    Exit Function
'End If
ComPortNumber = 1
Open "com" & ComPortNumber For Binary As #1

'Repeat barcode [count] times
For x = 1 To Count
    Job = Job & Barcode
Next x

'Maximum amount of data allowed in buffer
'MaxBufferSize = frmStatus.MSComm1.OutBufferSize

ChunkSize = 32767
'Break up the job into 32k chunks
Status "Paginating labels", True
JobSize = Len(Job)
While JobSize > 0
    Batch.Add Left(Job, ChunkSize)
    Job = Right(Job, IIf(JobSize >= ChunkSize, JobSize - ChunkSize, 0))
    JobSize = Len(Job)
Wend

'Send the barcodes, one 32k block at a time
Status "Sending data to printer", True
For Each Job In Batch
    'Wait for the buffer to empty
    WaitForEmptyBuffer

    'Buffer cleared to 0, send next batch
    Put #1, , Job
Next Job

'Close port
WaitForEmptyBuffer
If CloseComPort Then
    'Com port did not close
    Status "Unable to close port on COM" & ComPortNumber & ", you must close it manually before attempting to print again", False
    DoPrintJob = True
End If
    
'Succesfull print.. exit
Status "Print job complete", False
DoPrintJob = True
Exit Function

'Handle any errors
DoPrintJob_Error:
    DoPrintJob = False
    Status "Error occured while attempting to print", False

End Function

Public Sub WaitForEmptyBuffer()
    'Loop until the buffer empties out
'    While frmStatus.MSComm1.OutBufferCount > 0
'        DoEvents
'    Wend
End Sub


Public Sub Status(NewStatus As String, Optional IsBusy As Variant)
'Display status in the visible window

On Error Resume Next

'Set status line
frmStatus.txtStatus.Text = NewStatus

If Not IsMissing(State) Then
    'Do busy/idle indicator
    If IsBusy = True Then
        Set frmStatus.imgStatus.Picture = frmStatus.img_Busy.Picture
    Else
        Set frmStatus.imgStatus.Picture = frmStatus.img_Idle.Picture
    End If
End If

'Add to the log box
With frmStatus.txtLog
.SelStart = Len(.Text)
.SelLength = 0
.SelText = Format$(Now, "General Date") & "  " & NewStatus & Chr(13) & Chr(10)
End With

End Sub

Public Function OpenComPort() As Boolean
'    'Attempt to open com port
'    NOTE: This code is obsolete due to change in commmunication paradigm
'    frmStatus.MSComm1.PortOpen = True
'    OpenComPort = UpdateConnectionIcon(frmStatus.MSComm1.PortOpen)
End Function

Public Function CloseComPort() As Boolean
'    'Attempt to close com port
'    NOTE: This code is obsolete due to change in commmunication paradigm
'    frmStatus.MSComm1.PortOpen = False
'    CloseComPort = UpdateConnectionIcon(frmStatus.MSComm1.PortOpen)
    Close #1
End Function

Public Function UpdateConnectionIcon(Status As Boolean) As Boolean
    'Change the connection icon, return status
    If Status Then
        'Set frmStatus.imgConnection.Picture = frmStatus.img_Coneected.Picture
    Else
        'Set frmStatus.imgConnection.Picture = frmStatus.img_Disconnected.Picture
    End If
    UpdateConnectionIcon = Status
End Function
