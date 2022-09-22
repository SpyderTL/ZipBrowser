using System.Text;

namespace ZipBrowser
{
	internal class ZipFileReader
	{
		internal static bool Success;

		internal static void Read()
		{
			Success = false;

			var address = File.Contents.Length - 5;

			while (address >= 0)
			{
				if (File.Contents[address] == 'P' &&
					File.Contents[address + 1] == 'K' &&
					File.Contents[address + 2] == 5 &&
					File.Contents[address + 3] == 6)
				{
					break;
				}

				address--;
			}

			if (address < 0)
				return;

			ZipFile.EndOfDirectory.Address = address;
			ZipFile.EndOfDirectory.Signature = File.Contents.AsSpan(address, 4).ToArray();
			ZipFile.EndOfDirectory.DiskNumber = BitConverter.ToUInt16(File.Contents, address + 4);
			ZipFile.EndOfDirectory.DirectoryDiskNumber = BitConverter.ToUInt16(File.Contents, address + 6);
			ZipFile.EndOfDirectory.DirectoryCount = BitConverter.ToUInt16(File.Contents, address + 8);
			ZipFile.EndOfDirectory.TotalDirectoryCount = BitConverter.ToUInt16(File.Contents, address + 10);
			ZipFile.EndOfDirectory.DirectoryLength = BitConverter.ToUInt32(File.Contents, address + 12);
			ZipFile.EndOfDirectory.DirectoryOffset = BitConverter.ToUInt32(File.Contents, address + 16);
			ZipFile.EndOfDirectory.CommentLength = BitConverter.ToUInt16(File.Contents, address + 20);
			ZipFile.EndOfDirectory.Comment = Encoding.ASCII.GetString(File.Contents.AsSpan(address + 22, ZipFile.EndOfDirectory.CommentLength));

			address = (int)ZipFile.EndOfDirectory.DirectoryOffset;

			var directory = new List<ZipFile.DirectoryEntry>();

			for (var i = 0; i < ZipFile.EndOfDirectory.DirectoryCount; i++)
			{
				var entry = new ZipFile.DirectoryEntry
				{
					Address = address,
					Signature = File.Contents.AsSpan(address, 4).ToArray(),
					Version = BitConverter.ToUInt16(File.Contents, address + 4),
					MinimumVersion = BitConverter.ToUInt16(File.Contents, address + 6),
					Flags = BitConverter.ToUInt16(File.Contents, address + 8),
					Compression = BitConverter.ToUInt16(File.Contents, address + 10),
					ModifiedTime = BitConverter.ToUInt16(File.Contents, address + 12),
					ModifiedDate = BitConverter.ToUInt16(File.Contents, address + 14),
					Crc32 = BitConverter.ToUInt32(File.Contents, address + 16),
					CompressedLength = BitConverter.ToUInt32(File.Contents, address + 20),
					UncompressedLength = BitConverter.ToUInt32(File.Contents, address + 24),
					FileNameLength = BitConverter.ToUInt16(File.Contents, address + 28),
					ExtraFieldLength = BitConverter.ToUInt16(File.Contents, address + 30),
					FileCommentLength = BitConverter.ToUInt16(File.Contents, address + 32),
					DiskNumber = BitConverter.ToUInt16(File.Contents, address + 34),
					InternalFileAttributes = BitConverter.ToUInt16(File.Contents, address + 36),
					ExternalFileAttributes = BitConverter.ToUInt32(File.Contents, address + 38),
					RelativeOffset = BitConverter.ToUInt32(File.Contents, address + 42)
				};

				entry.FileName = Encoding.ASCII.GetString(File.Contents, address + 46, entry.FileNameLength);
				entry.ExtraField = File.Contents.AsSpan(address + 46 + entry.FileNameLength, entry.ExtraFieldLength).ToArray();
				entry.FileComment = Encoding.ASCII.GetString(File.Contents, address + 46 + entry.FileNameLength + entry.ExtraFieldLength, entry.FileCommentLength);

				directory.Add(entry);
			}

			ZipFile.Directory = directory.ToArray();

			var fileHeaders = new List<ZipFile.FileHeader>();
			var files = new List<byte[]>();

			for (var i = 0; i < ZipFile.Directory.Length; i++)
			{
				address = (int)ZipFile.Directory[i].RelativeOffset;

				var header = new ZipFile.FileHeader
				{
					Address = address,
					Signature = File.Contents.AsSpan(address, 4).ToArray(),
					MinimumVersion = BitConverter.ToUInt16(File.Contents, address + 4),
					Flags = BitConverter.ToUInt16(File.Contents, address + 6),
					Compression = BitConverter.ToUInt16(File.Contents, address + 8),
					ModifiedTime = BitConverter.ToUInt16(File.Contents, address + 10),
					ModifiedDate = BitConverter.ToUInt16(File.Contents, address + 12),
					Crc32 = BitConverter.ToUInt32(File.Contents, address + 14),
					CompressedLength = BitConverter.ToUInt32(File.Contents, address + 18),
					UncompressedLength = BitConverter.ToUInt32(File.Contents, address + 22),
					FileNameLength = BitConverter.ToUInt16(File.Contents, address + 26),
					ExtraFieldLength = BitConverter.ToUInt16(File.Contents, address + 28)
				};

				header.FileName = Encoding.ASCII.GetString(File.Contents, address + 30, header.FileNameLength);
				header.ExtraField = File.Contents.AsSpan(address + 30 + header.FileNameLength, header.ExtraFieldLength).ToArray();

				fileHeaders.Add(header);

				files.Add(File.Contents.AsSpan(address + 30 + header.FileNameLength + header.ExtraFieldLength, (int)header.CompressedLength).ToArray());
			}

			ZipFile.FileHeaders = fileHeaders.ToArray();
			ZipFile.FileData = files.ToArray();

			Success = true;
		}
	}
}