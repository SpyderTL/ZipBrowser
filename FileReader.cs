namespace ZipBrowser
{
	internal class FileReader
	{
		internal static void Read(string path)
		{
			File.Contents = System.IO.File.ReadAllBytes(path);
		}
	}
}