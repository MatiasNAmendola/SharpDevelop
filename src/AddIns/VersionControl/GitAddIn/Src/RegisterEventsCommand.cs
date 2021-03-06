﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;
using System.Windows.Forms;

namespace ICSharpCode.GitAddIn
{
	public class RegisterEventsCommand : AbstractCommand
	{
		public override void Run()
		{
			FileService.FileCreated += (sender, e) => {
				AddFile(e.FileName);
			};
			FileService.FileCopied += (sender, e) => {
				AddFile(e.TargetFile);
			};
			FileService.FileRemoved += (sender, e) => {
				RemoveFile(e.FileName);
			};
			FileService.FileRenamed += (sender, e) => {
				RenameFile(e.SourceFile, e.TargetFile);
			};
			FileUtility.FileSaved += (sender, e) => {
				ClearStatusCacheAndEnqueueFile(e.FileName);
			};
			AbstractProjectBrowserTreeNode.OnNewNode += TreeNodeCreated;
		}
		
		async void AddFile(string fileName)
		{
			await Git.AddAsync(fileName);
			ClearStatusCacheAndEnqueueFile(fileName);
		}
		
		async void RemoveFile(string fileName)
		{
			if (GitStatusCache.GetFileStatus(fileName) == GitStatus.Added) {
				await Git.RemoveAsync(fileName, true);
				ClearStatusCacheAndEnqueueFile(fileName);
			}
		}
		
		async void RenameFile(string sourceFileName, string targetFileName)
		{
			await Git.AddAsync(targetFileName);
			ClearStatusCacheAndEnqueueFile(targetFileName);
			RemoveFile(sourceFileName);
		}
		
		void TreeNodeCreated(object sender, TreeViewEventArgs e)
		{
			SolutionNode sn = e.Node as SolutionNode;
			if (sn != null) {
				GitStatusCache.ClearCachedStatus(sn.Solution.FileName);
				OverlayIconManager.Enqueue(sn);
			} else {
				DirectoryNode dn = e.Node as DirectoryNode;
				if (dn != null) {
					OverlayIconManager.Enqueue(dn);
				} else {
					FileNode fn = e.Node as FileNode;
					if (fn != null) {
						OverlayIconManager.Enqueue(fn);
					}
				}
			}
		}
		
		void ClearStatusCacheAndEnqueueFile(string fileName)
		{
			GitStatusCache.ClearCachedStatus(fileName);
			
			ProjectBrowserPad pad = ProjectBrowserPad.Instance;
			if (pad == null) return;
			FileNode node = pad.ProjectBrowserControl.FindFileNode(fileName);
			if (node == null) return;
			OverlayIconManager.EnqueueParents(node);
		}
	}
}
