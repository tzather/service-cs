using System.Text;

namespace Zuhid.Tools;

public static class FileExtension
{
  //private static readonly Encoding Common_Encoding = Encoding.UTF8;
  private static readonly Encoding Common_Encoding = new UTF8Encoding(false);

  public static void CreateReplace(string fileName, string contents)
  {
    var fileInfo = new FileInfo(fileName);
    if (!fileInfo.Directory.Exists)
    {
      fileInfo.Directory.Create(); // Create the directory if it does not exist
    }
    if (fileInfo.Exists && fileInfo.IsReadOnly)
    {
      fileInfo.IsReadOnly = false; // set the file to allow writing
    }

    // Write the file
    using var fileStream = new FileStream(fileName, FileMode.Create);
    using var writer = new StreamWriter(fileStream, Common_Encoding);
    writer.Write(contents);
    writer.Flush();
  }

  public static void Create(string fileName, string contents)
  {
    var fileInfo = new FileInfo(fileName);
    if (fileInfo.Exists && fileInfo.Length > 0)
    {
      return; // do not change the file if it exists
    }

    if (!fileInfo.Directory.Exists)
    {
      fileInfo.Directory.Create(); // Create the directory if it does not exist
    }
    if (fileInfo.Exists && fileInfo.IsReadOnly)
    {
      fileInfo.IsReadOnly = false; // set the file to allow writing
    }

    // Write the file
    using var fileStream = new FileStream(fileName, FileMode.Create);
    using var writer = new StreamWriter(fileStream, Common_Encoding);
    writer.Write(contents);
    writer.Flush();
  }

  public static void Replace(string fileName, string contents)
  {
    var fileInfo = new FileInfo(fileName);
    if (!fileInfo.Exists)
    {
      throw new FileNotFoundException($"{fileName} not found");
    }

    if (fileInfo.IsReadOnly)
    {
      fileInfo.IsReadOnly = false; // set the file to allow writing
    }

    // Write the file
    using var fileStream = new FileStream(fileName, FileMode.Open);
    using var writer = new StreamWriter(fileStream, Common_Encoding);
    writer.Write(contents);
    writer.Flush();
  }

  public static void Append(string fileName, string contents)
  {
    using var fileStream = new FileStream(fileName, FileMode.Append);
    using var writer = new StreamWriter(fileStream, Common_Encoding);
    writer.Write(contents);
    writer.Flush();
  }

  public static void Delete(string path)
  {
    var dir = new DirectoryInfo(path);
    if (dir.Exists)
    {
      // Delete all the files
      foreach (var file in dir.GetFiles())
      {
        file.Delete();
      }
      // Delete all the files in the subdirectory
      foreach (var subDir in dir.GetDirectories())
      {
        Delete(subDir.FullName);
      }
      dir.Delete();
    }
  }
}
