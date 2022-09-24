namespace ZipBrowser
{
	internal static class ProgramWindow
	{
		internal static BrowserForm Form = new BrowserForm();

		internal static void Show()
		{
			ApplicationConfiguration.Initialize();

			FileReader.Read("../../../Examples/Projects.zip");

			ZipFileReader.Read();

			CreateNodes();

			Form.TreeView.AfterSelect += TreeView_AfterSelect;

			Application.Run(Form);
		}

		private static void TreeView_AfterSelect(object? sender, TreeViewEventArgs e)
		{
			switch (e.Node?.Name)
			{
				case "file":
					var file = (int)(e.Node?.Tag ?? 0);
					Form.PropertyGrid.SelectedObject = new
					{
						ZipFile.FileHeaders[file].Address,
						ZipFile.FileHeaders[file].MinimumVersion,
						ZipFile.FileHeaders[file].Flags,
						ZipFile.FileHeaders[file].Compression,
						ZipFile.FileHeaders[file].ModifiedTime,
						ZipFile.FileHeaders[file].ModifiedDate,
						ZipFile.FileHeaders[file].Crc32,
						ZipFile.FileHeaders[file].CompressedLength,
						ZipFile.FileHeaders[file].UncompressedLength,
						ZipFile.FileHeaders[file].FileNameLength,
						ZipFile.FileHeaders[file].ExtraFieldLength,
						ZipFile.FileHeaders[file].DiskNumber,
						ZipFile.FileHeaders[file].InternalFileAttributes,
						ZipFile.FileHeaders[file].ExternalFileAttributes,
						ZipFile.FileHeaders[file].FileName,
						ZipFile.FileHeaders[file].ExtraField,
						Data = ZipFile.FileData[file]
					};
					break;

				case "directoryEntry":
					var directoryEntry = (int)(e.Node?.Tag ?? 0);
					Form.PropertyGrid.SelectedObject = new
					{
						ZipFile.Directory[directoryEntry].Address,
						ZipFile.Directory[directoryEntry].Signature,
						ZipFile.Directory[directoryEntry].Version,
						ZipFile.Directory[directoryEntry].MinimumVersion,
						ZipFile.Directory[directoryEntry].Flags,
						ZipFile.Directory[directoryEntry].Compression,
						ZipFile.Directory[directoryEntry].ModifiedTime,
						ZipFile.Directory[directoryEntry].ModifiedDate,
						ZipFile.Directory[directoryEntry].Crc32,
						ZipFile.Directory[directoryEntry].CompressedLength,
						ZipFile.Directory[directoryEntry].UncompressedLength,
						ZipFile.Directory[directoryEntry].FileNameLength,
						ZipFile.Directory[directoryEntry].ExtraFieldLength,
						ZipFile.Directory[directoryEntry].FileCommentLength,
						ZipFile.Directory[directoryEntry].DiskNumber,
						ZipFile.Directory[directoryEntry].InternalFileAttributes,
						ZipFile.Directory[directoryEntry].ExternalFileAttributes,
						ZipFile.Directory[directoryEntry].RelativeOffset,
						ZipFile.Directory[directoryEntry].FileName,
						ZipFile.Directory[directoryEntry].ExtraField,
						ZipFile.Directory[directoryEntry].FileComment
					};
					break;

				case "endOfDirectory":
					Form.PropertyGrid.SelectedObject = new
					{
						ZipFile.EndOfDirectory.Address,
						ZipFile.EndOfDirectory.DiskNumber,
						ZipFile.EndOfDirectory.DirectoryDiskNumber,
						ZipFile.EndOfDirectory.DirectoryCount,
						ZipFile.EndOfDirectory.DirectoryLength,
						ZipFile.EndOfDirectory.DirectoryOffset,
						ZipFile.EndOfDirectory.CommentLength,
						ZipFile.EndOfDirectory.Comment
					};
					break;

				default:
					Form.PropertyGrid.SelectedObject = null;
					break;
			}
		}

		private static void CreateNodes()
		{
			var root = Form.TreeView.Nodes.Add("Projects.zip");
			root.Name = "root";

			var filesNode = root.Nodes.Add("Files");
			filesNode.Name = "files";
			filesNode.Nodes.AddRange(ZipFile.FileHeaders.Select((x, i) => new TreeNode
			{
				Text = x.FileName,
				Name = "file",
				Tag = i
			}).ToArray());

			var directoryNode = root.Nodes.Add("Directory");
			directoryNode.Name = "directory";
			directoryNode.Nodes.AddRange(ZipFile.Directory.Select((x, i) => new TreeNode
			{
				Text = x.FileName,
				Name = "directoryEntry",
				Tag = i
			}).ToArray());

			var endOfDirectoryNode = root.Nodes.Add("EndOfDirectory");
			endOfDirectoryNode.Name = "endOfDirectory";
		}
	}
}