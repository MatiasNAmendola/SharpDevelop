﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TextTemplating
{
	public class TextTemplatingEnvironment : ITextTemplatingEnvironment
	{
		public string ExpandEnvironmentVariables(string name)
		{
			return Environment.ExpandEnvironmentVariables(name);
		}
	}
}
