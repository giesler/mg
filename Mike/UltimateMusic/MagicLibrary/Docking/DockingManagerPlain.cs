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
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Docking
{
	public class DockingManagerPlain : DockingManagerBase
	{
		public DockingManagerPlain(Form hostForm)
			: base(hostForm)
		{
		}

		protected override IDockingDetail CreateSingleResize()
		{
			return new Magic.Docking.DockingDetailResizePlain(true);
		}

		protected override IDockingDetail CreateSingleHandle()
		{
			return new Magic.Docking.DockingDetailHandlePlain(new EventHandler(OnCloseSingle),
															  new EventHandler(OnContextMenu));
		}

		protected override void AddSingleContentToBase(DockingBase dockBase, Content content) 
		{
			// Check for mandatory parameters
			if (null == dockBase)
				throw new ArgumentNullException("Must provide DockingBase");

			if (null == content)
				throw new ArgumentNullException("Must provide Content");
			
			// Create a border for the provided control
			BorderForControl border = new BorderForControl(dockBase, content.Control, 4);
			
			// Use the border instead of provided control
			content.Control = border;

			// Call base class to finish processing
			base.AddSingleContentToBase(dockBase, content);
		}
	}
}
