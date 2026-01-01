using System;
using System.IO;
using System.Text;

namespace Code_Editor
{
    internal class FileManager
    {
        public static string ReadFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }
                return File.ReadAllText(filePath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading file: {ex.Message}", ex);
            }
        }

        public static void SaveFile(string filePath, string content)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("File path cannot be empty.");
                }

                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error saving file: {ex.Message}", ex);
            }
        }

        public static bool FileExists(string filePath)
        {
            return !string.IsNullOrEmpty(filePath) && File.Exists(filePath);
        }
    }
}
