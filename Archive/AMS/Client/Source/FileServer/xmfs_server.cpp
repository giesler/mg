//
//	Adult Media Swapper File Server
//	(c)2001 Line2 Systems, Inc.
//
//	Author: Nick Nystrom
//
#include "xmfs.h"

//handler interacce implementation
CServerHandler::CServerHandler()
{
	m_refCount = 1;
}

//IXMSessionHandler Implementation
void CServerHandler::OnMessageReceived(CXMSession *session)
{
	//handle received messages
	CXMMessage *msg;
	while (msg = session->Receive())
	{
		//we only care about login messages
		if (stricmp(msg->GetFor(false), XMMSG_LOGIN)==0)
		{
			//failed login?
			if (!msg->CompareField("success", "true"))
			{
				LOG("Server rejected login. Response: \"%s\".\n",
					msg->GetField("error")->GetValue(false));
				ShutdownServer();
			}
			else
			{
				LOG("Login succesfull.\n");

				//what type of listing does the server want?
				bool full = msg->CompareField("listingsize", "full");
				LOG("Sending %s file listing...\n", full?"full":"partial");

				//get xml of listing
				char* xml = dbman()->BuildFileListing(full);
				if (!xml)
				{
					ERROR("Could not create file listing!\n");
					ShutdownServer();
				}
				else
				{
					//send the listing message
					CXMMessage *reply = new CXMMessage(session);
					reply->SetType(XM_REQUEST);
					reply->SetFor(XMMSG_LISTING);
					reply->SetContentFormat("text/xml");
					reply->GetField("listing")->SetXml(xml, false);
					if (!reply->Send())
					{
						ERROR("Could not send file listing!\n");
						ShutdownServer();
					}
					LOG("File list sent. Login sequence complete.\n");
				}
			}
		}
		else if (stricmp(msg->GetFor(false), XMMSG_PING)==0)
		{
			//respond to ping
			CXMMessage *reply = msg->CreateReply();
			reply->GetField("success")->SetValue("1");
			if (!reply->Send())
			{
				ERROR("Could not send server ping response!\n");
			}
		}

		delete msg;
	}	
}

void CServerHandler::OnMessageSent(CXMSession *session)
{
	//handle sent message
	CXMMessage *msg;
	while (msg = session->Sent())
	{
		//we only care about login messages
		if (stricmp(msg->GetFor(false), XMMSG_LOGIN)==0)
		{
			LOG("Login message sent, waiting for reply...\n");
		}
		
		//free the message
		delete msg;
	}
}

void CServerHandler::OnStateChanged(CXMSession *session, int oldState, int newState)
{
	CXMMessage *msg;
	char *username;
	char *password;
	switch (newState)
	{
	case XM_OPEN:

		//connection complete.. send login
		LOG("Server connected. Sending login...\n");
		
		//get the username and password
		username = config()->GetField(FIELD_LOGIN_AUTO_USERNAME, false);
		password = config()->GetField(FIELD_LOGIN_AUTO_PASSWORD, false);

		//send the login message
		msg = new CXMMessage(session);
		msg->SetType(XM_REQUEST);
		msg->SetFor(XMMSG_LOGIN);
		msg->SetContentFormat("text/xml");
		msg->GetField("datarate")->SetValue(config()->GetField(FIELD_NET_DATARATE), false);
		msg->GetField("system")->SetValue(config()->RegGetSystemID().GetString(), true);
		msg->GetField("username")->SetValue(username, true);
		msg->GetField("password")->SetValue(password, true);
		msg->GetField("version")->SetValue(AMSVERSION, true);
		msg->GetField("filecount")->SetValue(dbman()->ServerFileCount());
		if (!msg->Send())
		{
			LOG("Failed to send login message.\n");
			delete msg;
			ShutdownServer();
			break;
		}
		break;
	
	case XM_OPENING:
		break;
	
	case XM_CLOSING:
		break;

	case XM_CLOSED:

		//quit the application
		LOG("Server connection closed!\n");
		ShutdownServer();
		
		break;
	}
}

void CServerHandler::OnProgress(CXMSession *session)
{
	
}

//IUnknown Implementation
void CServerHandler::AddRef()
{
	m_refCount++;
}
void CServerHandler::Release()
{
	if (--m_refCount<1)
		delete this;
}