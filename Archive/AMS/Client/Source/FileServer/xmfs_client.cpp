//
//	Adult Media Swapper File Server
//	(c)2001 Line2 Systems, Inc.
//
//	Author: Nick Nystrom
//
#include "xmfs.h"


//handler interacce implementation
CClientHandler::CClientHandler()
{
	m_refCount = 1;
}

//IXMSessionHandler Implementation
void CClientHandler::OnMessageReceived(CXMSession *session)
{
	//flush out the receiveed messages queue
	CXMMessage *msg;
	while (msg = session->Receive())
	{
		CXMMessage *reply;
		if (stricmp(msg->GetFor(false), XMMSG_PING)==0)
		{
			//respond to ping
			reply = msg->CreateReply();
			reply->GetField("success")->SetValue("1");
			reply->Send();
		}
		else if (stricmp(msg->GetFor(false), XMMSG_FILE)==0)
		{
			//ignore anything thats non-request
			if (msg->GetType()==XM_REQUEST)
			{
				CXMMessage *reply;
				
				//get info from request
				CMD5 md5(msg->GetField("md5")->GetValue(false));
				DWORD width = atol(msg->GetField("width")->GetValue(false));
				DWORD height = atol(msg->GetField("height")->GetValue(false));

				//try to find this file
				CXMDBFile *f = db()->FindFile(md5.GetValue(), false);
				if (!f)
				{
					//we dont have it
					reply = msg->CreateReply();
					reply->GetField("message")->SetValue("error");
					reply->GetField("error")->SetValue("File not found.");
					reply->GetField("md5")->SetValue(md5.GetString());
					reply->Send();
					session->Close();
					delete msg;
					continue;
				}

				//is it shared?
				if (f->GetFlag(DFF_REMOVED))
				{
					//we dont have it
					reply = msg->CreateReply();
					reply->GetField("message")->SetValue("error");
					reply->GetField("error")->SetValue("File not found.");
					reply->GetField("md5")->SetValue(md5.GetString());
					reply->Send();
					session->Close();
					delete msg;
					continue;
				}

				//setup the response message
				reply = msg->CreateReply();
				reply->GetField("md5")->SetValue(CMD5(f->GetMD5()).GetString());
				reply->GetField("message")->SetValue("file");

				//is this a thumbnail request?
				bool thumb =	((f->GetWidth() != width) || (f->GetHeight() != height)) &&
								(width!=0 && height!=0);
				if (thumb)
				{
					CXMDBThumb *t = f->GetThumb(width, height);
					if (!t) 
					{
						//could not convert
						delete reply;
						reply = msg->CreateReply();
						reply->GetField("message")->SetValue("error");
						reply->GetField("error")->SetValue("Unable to create thumbnail.");
						reply->GetField("md5")->SetValue(md5.GetString());
						reply->Send();
						session->Close();
						delete msg;
						continue;
					}

					//attach thumbnail
					BYTE* buf;
					DWORD bufsize = t->GetImage(&buf);
					reply->SetBinaryData(buf, bufsize);
					t->FreeImage(&buf);

					//set misc fields
					reply->GetField("width")->SetValue(t->GetWidth());
					reply->GetField("height")->SetValue(t->GetHeight());
					reply->GetField("size")->SetValue(t->GetSize());
				}
				else
				{
					//full size image
					reply->GetField("width")->SetValue(f->GetWidth());
					reply->GetField("height")->SetValue(f->GetHeight());
					reply->GetField("size")->SetValue(f->GetFileSize());

					//attach file
					if (!reply->AttachFile(f->GetPath()))
					{
						//file deleted?
						delete reply;
						reply = msg->CreateReply();
						reply->GetField("message")->SetValue("error");
						reply->GetField("error")->SetValue("Error sending file.");
						reply->GetField("md5")->SetValue(md5.GetString());
						reply->Send();
						session->Close();
						delete msg;
						continue;
					}
						
					//set md5
					memcpy(reply->mBinaryMD5, md5.GetValue(), 16);
				}

				//send the file
				reply->Send();
				session->Close();
			}
		}
		delete msg;
	}
}
void CClientHandler::OnMessageSent(CXMSession *session)
{
	//flush out the sent messages queue
	CXMMessage *msg;
	while (msg = session->Sent())
	{
		
		delete msg;
	}
}
void CClientHandler::OnStateChanged(CXMSession *session, int oldState, int newState)
{
	//remove ourself from the session manager
	if (newState == XM_CLOSED)
	{
		sessions()->Detach(
			sessions()->FindByPointer(session));
	}
}
void CClientHandler::OnProgress(CXMSession *session)
{
	
}

//IUnknown Implementation
void CClientHandler::AddRef()
{
	m_refCount++;
}
void CClientHandler::Release()
{
	if (--m_refCount<1)
		delete this;
}