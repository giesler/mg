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
using System.Collections;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Docking
{
	public class DockingManagerBase : IDockingManager
	{
		protected Form _hostForm;
		protected ArrayList _contents;
	
		public DockingManagerBase(Form hostForm)
		{
			// Check for mandatory parameter
			if (null == hostForm)
				throw new ArgumentNullException("Must provide Form");

			_hostForm = hostForm;
			_contents = new ArrayList();
		}

		public virtual Content CreateContent(Control control)
		{
			return new Content(control, control.Text);
		}

		public virtual Content CreateContent(Control control, String title)
		{
			return new Content(control, title);
		}

		public virtual Content CreateContent(Control control, String title, ImageList imageList, int imageIndex)
		{
			return new Content(control, title, imageList, imageIndex);
		}
	
		public virtual void AddSingleContent(Content content, State initialState)
		{
			// Check for mandatory parameter
			if (null == content)
				throw new ArgumentNullException("Must provide Content");

			// Create an initial docking container with resize and handle details
			DockingSingle single = new Magic.Docking.DockingSingle();
			IDockingDetail resize = CreateSingleResize();
			IDockingDetail handle = CreateSingleHandle();

			// Allow special processing of content against docking site
			PreProcessContent(single, content);

			// Initialise the docking container
			single.AddDetail(handle);
			single.AddDetail(resize);
			AddSingleContentToBase(single, content);

			// Set the initial docking state
			single.State = initialState;
			
			// Add docking container to form
			_hostForm.Controls.Add(single);

			// Make new item the least important
			single.RepositionAtStart();

			// Remember in internal list
			_contents.Add(content);
		}

		public virtual Content ContentFromTitle(string title)
		{
			foreach(Object obj in _contents)
			{
				Content content = obj as Content;

				if (content.Title == title)
					return content;
			}

			return null;
		}

		public virtual void ShowAll(bool show)
		{
			foreach(Object obj in _contents)
			{
				Content content = obj as Content;

				content.Hidden = !show;
			}
		}

		public virtual void DockingContextMenu(Point screenPt)
		{
			ContextMenu popupMenu = new ContextMenu();

			foreach(Object obj in _contents)
			{
				Content content = obj as Content;

				// Create a IDE style menu item for this content
				MenuItem menuItem = new MenuItem(content.Title, new EventHandler(OnMenuSelection));

				// Items currently visible should be checked
				menuItem.Checked = !content.Hidden;

				// Add to end of item list
				popupMenu.MenuItems.Add(menuItem);
			}

			// Add the compulsary options
			popupMenu.MenuItems.Add(new MenuItem("-"));
			popupMenu.MenuItems.Add(new MenuItem("Show All", new EventHandler(OnMenuShowAll)));
			popupMenu.MenuItems.Add(new MenuItem("Hide All", new EventHandler(OnMenuHideAll)));

			// Convert screen point to Form point
			screenPt = _hostForm.PointToClient(screenPt);

			// Show the content menu at the given screen position
			popupMenu.Show(_hostForm, screenPt);
		}

		protected void OnMenuSelection(Object sender, EventArgs e)
		{
			// Must always be a MenuItemXP
			MenuItem menuItem = sender as MenuItem;

			// Convert from generic reference to that expected
			Content content = ContentFromTitle(menuItem.Text);

			// Invert the visibility
			content.Hidden = !content.Hidden;
		}

		protected void OnMenuShowAll(Object sender, EventArgs e)
		{
			ShowAll(true);
		}

		protected void OnMenuHideAll(Object sender, EventArgs e)
		{
			ShowAll(false);
		}

		protected void OnContextMenu(Object sender, EventArgs e)
		{
			// Unbox the provided Point
			Point screenPt = (Point)sender;

			DockingContextMenu(screenPt);
		}

		protected virtual void OnCloseSingle(Object sender, EventArgs e)
		{
			IDockingSite singleSite = sender as Magic.Docking.IDockingSite;

			// Check for correct caller type
			if (null == singleSite)
				throw new ArgumentNullException("Calling must provide IDockingSite");

			singleSite.Hidden = true;
		}

		protected virtual IDockingDetail CreateSingleResize() { return null; }
		protected virtual IDockingDetail CreateSingleHandle() { return null; }

		protected virtual void PreProcessContent(DockingBase dockBase, Content content)
		{
			// If the contents are a Form then need to have special handling for focus
			if (content.Control is Form)
			{
				// Create decorator class for a Form
				ControlForForm cff = new ControlForForm(dockBase, content.Control as Form);

				// Use in place of Form
				content.Control = cff;
			}
		}

		protected virtual void AddSingleContentToBase(DockingBase dockBase, Content content) 
		{
			// Check for mandatory parameters
			if (null == dockBase)
				throw new ArgumentNullException("Must provide DockingBase");

			if (null == content)
				throw new ArgumentNullException("Must provide Content");

			dockBase.AddContent(content);
		}
	}
}
