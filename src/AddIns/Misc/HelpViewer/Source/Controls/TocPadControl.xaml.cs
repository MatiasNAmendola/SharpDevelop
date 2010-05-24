﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Sebastian Ullrich" email=""/>
//     <version>$Revision: 5842 $</version>
// </file>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.Core;
using MSHelpSystem.Core;

namespace MSHelpSystem.Controls
{
	public partial class TocPadControl : UserControl
	{
		public TocPadControl()
		{
			InitializeComponent();
			LoadToc();
			Help3Service.ConfigurationUpdated += new EventHandler(Help3ServiceConfigurationUpdated);
		}

		void LoadToc()
		{
			if (!Help3Environment.IsLocalHelp) DataContext = null;
			// TODO: Needs a localization
			else DataContext = new[] { new TocEntry("-1") { Title = "Help Library" } };
		}

		void Help3TocItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			string topicId = (string)tocTreeView.SelectedValue;
			if (!string.IsNullOrEmpty(topicId)) {
				LoggingService.Debug(string.Format("Help 3.0: [TOC] Calling page with Id \"{0}\"", topicId));
				DisplayHelp.Page(topicId);
			}
		}
		
		void Help3ServiceConfigurationUpdated(object sender, EventArgs e)
		{
			LoadToc();
		}
	}
}