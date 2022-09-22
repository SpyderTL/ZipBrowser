namespace ZipBrowser
{
	internal static class ZipFile
	{
		internal static DirectoryEntry[] Directory = Array.Empty<DirectoryEntry>();
		internal static FileHeader[] FileHeaders = Array.Empty<FileHeader>();
		internal static byte[][] FileData = Array.Empty<byte[]>();

		internal static class EndOfDirectory
		{
			internal static int Address;
			internal static byte[] Signature = Array.Empty<byte>();
			internal static ushort DiskNumber;
			internal static ushort DirectoryDiskNumber;
			internal static ushort DirectoryCount;
			internal static ushort TotalDirectoryCount;
			internal static uint DirectoryLength;
			internal static uint DirectoryOffset;
			internal static ushort CommentLength;
			internal static string Comment = string.Empty;
		}

		internal class DirectoryEntry
		{
			internal int Address;
			internal byte[] Signature = Array.Empty<byte>();
			internal ushort Version;
			internal ushort MinimumVersion;
			internal ushort Flags;
			internal ushort Compression;
			internal ushort ModifiedTime;
			internal ushort ModifiedDate;
			internal uint Crc32;
			internal uint CompressedLength;
			internal uint UncompressedLength;
			internal ushort FileNameLength;
			internal ushort ExtraFieldLength;
			internal ushort FileCommentLength;
			internal ushort DiskNumber;
			internal ushort InternalFileAttributes;
			internal uint ExternalFileAttributes;
			internal uint RelativeOffset;
			internal string FileName = string.Empty;
			internal byte[] ExtraField = Array.Empty<byte>();
			internal string FileComment = string.Empty;
		}

		internal class FileHeader
		{
			internal int Address;
			internal byte[] Signature = Array.Empty<byte>();
			internal ushort MinimumVersion;
			internal ushort Flags;
			internal ushort Compression;
			internal ushort ModifiedTime;
			internal ushort ModifiedDate;
			internal uint Crc32;
			internal uint CompressedLength;
			internal uint UncompressedLength;
			internal ushort FileNameLength;
			internal ushort ExtraFieldLength;
			internal ushort DiskNumber;
			internal ushort InternalFileAttributes;
			internal uint ExternalFileAttributes;
			internal uint RelativeOffset;
			internal string FileName = string.Empty;
			internal byte[] ExtraField = Array.Empty<byte>();
		}
	}
}