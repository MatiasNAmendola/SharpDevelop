﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using System.Text;
using System.Xml;
using ICSharpCode.NRefactory.Editor;
using NUnit.Framework;
using XmlEditor.Tests.Utils;

namespace XmlEditor.Tests.Utils.Tests
{
	[TestFixture]
	public class MockTextBufferTests
	{
		[Test]
		public void CanGetTextFromTextBufferTextProperty()
		{
			string expectedText = "abc";
			ITextSource textBuffer = new StringTextSource(expectedText);
			Assert.AreEqual(expectedText, textBuffer.Text);
		}
		
		[Test]
		public void CanGetTextFromReaderReturnedFromTextBufferCreateReader()
		{
			string expectedText = "abc";
			ITextSource textBuffer = new StringTextSource("abc");
			
			StringBuilder text = new StringBuilder();
			using (TextReader reader = textBuffer.CreateReader()) {
				Assert.AreEqual(expectedText, reader.ReadToEnd());
			}
		}
	}
}
