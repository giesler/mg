// ***************************************************************************** 
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the confidential and proprietary information 
//  of Crownwood Consulting Limited, Haxey, North Lincolnshire, England 
//  and are supplied subject to licence terms. In no event may the Licensee 
//  reverse engineer, decompile, or otherwise attempt to discover the 
//  underlying source code or confidential information herein. 
// 
// ***************************************************************************** 

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace Crownwood.Magic.Docking
{
	public enum State
	{
		DockTop,
		DockBottom,
		DockLeft,
		DockRight,
		Floating,
	}

	[FlagsAttribute]
	public enum Permissions
	{
		None 		= 0,
		DockTop 	= 1,
		DockBottom 	= 2,
		DockLeft 	= 4,
		DockRight 	= 8,
		OnlyDocking 	= 15,
		Floating 	= 16,
		TabIn 		= 32,
		TabOut 		= 64,
		All		= 127
	}

	public enum Orientation
	{
		Standard,
		Inline
	}

	public interface IDockingSite
	{
		// Docking control state
		State State { get; set; }

		// State changes allowed
		Permissions Permissions { get; set; }

		// Can it be seen
		bool Hidden { get; set; }

		// Access to user interface control
		DockingBase Base { get; }

		// Hosting Form and Restore location when floating
		Control DockingParent { get; }

		// Location/Sizing properties
		Size DockingMinimumSize { get; set; }
		Size DockingMaximumSize { get; set; }
		Size DockingSize { get; set; }
		Size FloatingSize { get; set; }
		Point FloatingLocation { get; set; }

		// Content
		int Count { get; }
		Content this[int index] { get; }
		void AddContent(Content content);
		void RemoveContent(Content content);
		void ClearContents();
		void HideContent(Content content);
		void ShowContent(Content content);
		void StateContent(Content content, State state);

		// Detail management
		int DetailCount { get; }
		IDockingDetail GetDetail(int index);
		void AddDetail(IDockingDetail detail);
		void RemoveDetail(IDockingDetail detail);
		void ClearDetails();

		// Container functionality
		bool IsContainer { get; }
		Rectangle AcceptRectangle();
		bool CanAccept(IDockingSite dockingSite);
		bool CanAccept(IDockingSite dockingSite, Content content);
		bool Accept(IDockingSite dockingSite);
		bool Accept(IDockingSite dockingSite, Content content);
		IDockingSite DockingSiteFromContent(Content content);

		// Methods
		void Kill();
		bool DetailHasFocus(IDockingDetail sender);
		bool MatchingExternalForm(ExternalForm external);
	}

	public interface IDockingNotify
	{
		void NotifyResize(IDockingDetail sender, int xDelta, int yDelta);
		void NotifyCloseRequest(IDockingDetail sender);
		void NotifyDetailGotFocus(IDockingDetail sender);
		void NotifyDetailLostFocus(IDockingDetail sender);
		void NotifyContentGotFocus(IDockingDetail sender);
		void NotifyContentLostFocus(IDockingDetail sender);
	}

	public interface IDockingDetail
	{
		// Properties
		string Category { get; set; }
		IDockingSite DockingSite { get; set; }
		IDockingNotify DockingNotify { get; set; }
		Orientation Orientation { get; set; }		
		Content Content { get; set; }

		// Methods
		void OnCloseRequest();
		void OnDockingGotFocus();
		void OnDockingLostFocus();
		void OnTextChanged(string title);
		void OnStateChanged(State value);
	}

	public interface IDockingManager
	{
		// Create content items
		Content CreateContent(Control control);
		Content CreateContent(Control control, String title);
		Content CreateContent(Control control, String title, ImageList imageList, int imageIndex);

		// Create docking for content
		void AddSingleContent(Content content, State initialState);

		// Content menu
		void DockingContextMenu(Point screenPt);

		// Visibility
		void ShowAll(bool show);
	}

	public delegate IDockingDetail DockingDetailDelegate();
	public delegate IDockingSite DockingFromContentDelegate(Content content);
}