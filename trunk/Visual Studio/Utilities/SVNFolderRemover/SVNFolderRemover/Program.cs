using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace SVNFolderRemover
{
    class Program
    {
        /// <summary>
        /// Gets the SVN folder name from configuration
        /// </summary>
        static string SVNFolderName
        {
            get
            {
                if (string.IsNullOrEmpty(_svnFolderName))
                    _svnFolderName = ConfigurationManager.AppSettings["SVNFolderName"];
                return _svnFolderName;
            }
        }
        static string _svnFolderName;

        /// <summary>
        /// Entry point for the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // initialize path to empty
            string path = string.Empty;

            // while a valid path has not been provided, continue to prompt the user
            while (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                // prompt for path to directory
                Console.WriteLine("Enter the path from which to remove SVN folders:");

                // get path
                path = Console.ReadLine();

                // if the path exists, look for and remove SVN directories
                if (Directory.Exists(path))
                    RemoveSVNDirectories(path);
                else
                    Console.WriteLine("Path not found.");
            }

            // indicate completion and wait for user to confirm
            Console.WriteLine("Completed SVN folder removal. Press any key to quit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Removes all SVN directories for a given folder path
        /// </summary>
        /// <param name="path"></param>
        static void RemoveSVNDirectories(string path)
        {            
            Console.WriteLine("Checking {0} ...", path);

            // get all subdirectories
            IEnumerable<string> subdirectories = null;

            try
            {
                subdirectories = Directory.GetDirectories(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get subdirectories: {0}", ex.Message);
            }

            if (subdirectories != null)
            {
                // get svn directories
                IEnumerable<string> svnDirectories = subdirectories.Where(d => new DirectoryInfo(d).Name == SVNFolderName);
                if (svnDirectories.Any())
                {
                    // indicate directories are being deleted
                    Console.WriteLine("SVN directories found.");

                    // delete svn directories
                    foreach (string svnDirectory in svnDirectories)
                    {
                        // indicate deletion to user
                        Console.WriteLine("Deleting {0}", svnDirectory);

                        // delete
                        DeleteFileSystemInfo(new DirectoryInfo(svnDirectory));
                    }
                }

                // get non-SVN directories
                IEnumerable<string> otherDirectories = subdirectories.Where(d => new DirectoryInfo(d).Name != SVNFolderName);
                if (otherDirectories.Any())
                    foreach (string subdirectory in otherDirectories)
                        RemoveSVNDirectories(subdirectory);
            }
        }

        /// <summary>
        /// Delete file or directory
        /// </summary>
        /// <param name="fsi"></param>
        private static void DeleteFileSystemInfo(FileSystemInfo fileSystemInfo)
        {
            // ensure not read-only
            fileSystemInfo.Attributes = FileAttributes.Normal;

            // loop through children, if directory
            if (fileSystemInfo is DirectoryInfo)
                foreach (var childFileSystemInfo in ((DirectoryInfo)fileSystemInfo).GetFileSystemInfos())
                    DeleteFileSystemInfo(childFileSystemInfo);

            // delete
            fileSystemInfo.Delete();
        }

    }
}
