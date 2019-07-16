using System;
using System.IO;
using System.Threading;

namespace Rampant_Stripper
{
    class Program
    {
        public static int VideoCount { get; set; } = 0;

        static void Main(string[] args)
        {
            // Setup logging
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@"log.txt", true);
            sw.AutoFlush = true;
            Console.SetOut(sw);

            if (args.Length <= 0)
            {
                Console.WriteLine("No path specified.");
            }
            else
            {
                String destinationRoot = args[0];
                String path = args[1];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Path does not exist.");
                    return;
                }

                // Wait a few seconds for the files to be "released"
                Thread.Sleep(5000);
                                
                try
                {
                    // Start log
                    Console.WriteLine("------------------------------------------------------------------------------------------");
                    Console.WriteLine("Processing: " + path);
                    Console.WriteLine("------------------------------------------------------------------------------------------");


                    // Process the folder
                    ProcessFolder(path);

                    String destination = "";
                    String directoryName = new DirectoryInfo(path).Name;
                    Console.WriteLine("Directory Name: " + directoryName);

                    destination = Path.Combine(destinationRoot, directoryName);

                    Console.WriteLine("Moving from: " + path);
                    Console.WriteLine("Moving to: " + destination);

                    Directory.Move(path, destination);

                    Console.WriteLine("Finished processing");
                    Console.WriteLine("");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);                    
                }
            }            
        }

        private static void ProcessFolder(String path)
        {            

            // Recursively process subfolders first
            foreach (String directory in Directory.GetDirectories(path))
                ProcessFolder(directory);

            // Process the files in the folder    
            foreach(String fileName in Directory.GetFiles(path))
            {
                Console.WriteLine("Processing file: " + fileName);

                if(Path.GetExtension(fileName).Equals(".txt") ||
                    Path.GetExtension(fileName).Equals(".png") ||
                    Path.GetExtension(fileName).Equals(".jpg") ||
                    Path.GetExtension(fileName).Equals(".jpeg") ||
                    Path.GetExtension(fileName).Equals(".nfo") ||
                    Path.GetExtension(fileName).Equals(".exe"))
                {
                    File.Delete(fileName);
                    Console.WriteLine("Removed: " + fileName);
                    continue;
                }
                else if(Path.GetExtension(fileName).Equals(".mkv"))
                {
                    ProcessMkv(fileName);
                }
                else if(Path.GetExtension(fileName).Equals(".mp4"))
                {
                    ProcessMp4(fileName);
                }               

                Console.WriteLine("Finished Processing");
            }

            // If the folder is now empty delete it
            foreach (String directory in Directory.GetDirectories(path))
            {
                if (Directory.GetFiles(directory).Length == 0)
                {
                    Directory.Delete(directory);
                    Console.WriteLine("Removed empty directory: " + directory);
                }                    
            }
        }

        private static void ProcessMkv(String fileName)
        {
            var file = TagLib.Matroska.File.Create(fileName);
            
            PrintTagType(file.TagTypes);

            // strip the title & comments
            file.Tag.Title = "";
            file.Tag.Comment = "";
            file.Save();

            VideoCount++;
        }

        private static void ProcessMp4(String fileName)
        {
            var file = TagLib.Mpeg4.File.Create(fileName);

            PrintTagType(file.TagTypes);

            // strip the title & comments
            file.Tag.Title = "";
            file.Tag.Comment = "";
            file.Save();

            VideoCount++;
        }

        private static void PrintTagType(TagLib.TagTypes tags)
        {
            Console.WriteLine("--- Tag Types ---");

            if (tags.HasFlag(TagLib.TagTypes.Ape))
                Console.WriteLine("Ape");
            if (tags.HasFlag(TagLib.TagTypes.Apple))
                Console.WriteLine("Apple");
            if (tags.HasFlag(TagLib.TagTypes.Asf))
                Console.WriteLine("Asf");
            if (tags.HasFlag(TagLib.TagTypes.AudibleMetadata))
                Console.WriteLine("Audible");
            if (tags.HasFlag(TagLib.TagTypes.DivX))
                Console.WriteLine("DivX");
            if (tags.HasFlag(TagLib.TagTypes.FlacMetadata))
                Console.WriteLine("Flac");
            if (tags.HasFlag(TagLib.TagTypes.GifComment))
                Console.WriteLine("Gif");
            if (tags.HasFlag(TagLib.TagTypes.Id3v1))
                Console.WriteLine("Id3v1");
            if (tags.HasFlag(TagLib.TagTypes.Id3v2))
                Console.WriteLine("Id3v2");
            if (tags.HasFlag(TagLib.TagTypes.IPTCIIM))
                Console.WriteLine("IPTCIIM");
            if (tags.HasFlag(TagLib.TagTypes.JpegComment))
                Console.WriteLine("Jpeg");
            if (tags.HasFlag(TagLib.TagTypes.MovieId))
                Console.WriteLine("MovieId");
            if (tags.HasFlag(TagLib.TagTypes.Png))
                Console.WriteLine("Png");
            if (tags.HasFlag(TagLib.TagTypes.RiffInfo))
                Console.WriteLine("RiffInfo");
            if (tags.HasFlag(TagLib.TagTypes.TiffIFD))
                Console.WriteLine("TiffIFD");
            if (tags.HasFlag(TagLib.TagTypes.Xiph))
                Console.WriteLine("Xiph");
            if (tags.HasFlag(TagLib.TagTypes.XMP))
                Console.WriteLine("XMP");

            Console.WriteLine("--- End of Tag Types ---");
        }
    }
}
