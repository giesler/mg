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

namespace Crownwood.Magic.Docking
{
	public class DockingManagerIDE : DockingManagerBase
	{
		public DockingManagerIDE(Form hostForm)
			: base(hostForm)
		{
		}

		protected override IDockingDetail CreateSingleResize()
		{
			return new Magic.Docking.DockingDetailResizeIDE(true);
		}

		protected override IDockingDetail CreateSingleHandle()
		{
			return new Magic.Docking.DockingDetailHandleIDE(new EventHandler(OnCloseSingle), 
														    new EventHandler(OnContextMenu));
		}
	}
}
