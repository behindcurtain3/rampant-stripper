﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rampant_Stripper
{
    class Program
    {
        public static int VideoCount { get; set; } = 0;

        static void Main(string[] args)
        {
            if(args.Length <= 0)
            {
                Console.WriteLine("No path specified.");
            }
            else
            {
                String path = args[0];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Path does not exist.");
                    return;
                }

                ProcessFolder(path);
                
                // TV Show
                if(VideoCount > 3)
                {
                    Directory.Move(path, Path.Combine("E:\\TV\\", Path.GetDirectoryName(path)));
                }
                // Movie
                else
                {
                    Directory.Move(path, Path.Combine("E:\\Movies\\", Path.GetDirectoryName(path)));
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
                    Path.GetExtension(fileName).Equals(".nfo"))
                {
                    File.Delete(fileName);
                    Console.WriteLine("Removed" + fileName);
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