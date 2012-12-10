using System;
using System.Configuration;
using System.Xml;
using System.Reflection;
using System.Collections;

namespace msn2.net.ProjectF
{
	/// <summary>
	/// Summary description for ShellLoadModulesConfigHandler.
	/// </summary>
	public class ShellLoadModulesConfigHandler: IConfigurationSectionHandler
	{
		#region Public Methods
		public object Create(object parent, object configContext, XmlNode section)
		{
			System.Diagnostics.Debug.WriteLine("ShellLoadModulesConfigHandler.Create");

			ArrayList types = new ArrayList();

			// Look through section nodes
			foreach (XmlNode node in section.ChildNodes)
			{
				// Grab <module> tags
				if (node.Name == "module")
				{
					// Make sure some value is set
					if (node.Attributes.Count == 0)
					{
						throw new ConfigurationException("You must specify an assembly name and type.", node);
					}
					else
					{
						try
						{
							Assembly assembly = Assembly.LoadFrom(node.Attributes["assembly"].Value);
							if (assembly == null)
							{
								throw new ConfigurationException("Invalid assembly name.", node);
							}

							// We have a good assembly, now get the module
							Type type = assembly.GetType(node.Attributes["type"].Value);
							if (type == null)
							{
								throw new ConfigurationException("Invalid type name.", node);
							}
							
							// We have a good module
							types.Add(type);
						}
						catch (Exception ex)
						{
							throw new ConfigurationException(ex.Message, ex, node);
						}
					}
				}
				else
				{
					throw new ConfigurationException("unrecognized tag", node);
				}
			}

			return types;
		}
		#endregion
	}
}
