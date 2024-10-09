using System;
using System.Security.Cryptography;
using System.Text;

namespace BPD_Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Enter a string");
                Console.WriteLine("2. Choose a file");
                Console.WriteLine("3. Compare file hash with saved hash");
                Console.WriteLine("4. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter a string: ");
                        string inputString = Console.ReadLine();
                        string hash = MD5.ComputeMD5ForString(inputString);
                        Console.WriteLine($"MD5 Hash: {hash}");
                        SaveHashToFile(hash);
                        break;

                    case "2":
                        Console.Write("Enter the file path: ");
                        string filePath = Console.ReadLine();
                        if (File.Exists(filePath))
                        {
                            string fileHash = MD5.ComputeMD5ForFile(filePath);
                            Console.WriteLine($"MD5 Hash: {fileHash}");
                            SaveHashToFile(fileHash);
                        }
                        else
                        {
                            Console.WriteLine("File not found.");
                        }
                        break;
                    case "3":
                        CompareHashes();
                        break;
                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void SaveHashToFile(string hash)
        {
            Console.Write("\nEnter the filename to save the hash: ");
            string fileName = Console.ReadLine();
            File.WriteAllText(fileName, hash);
            Console.WriteLine($"\nHash saved to file {fileName}");
            Console.WriteLine("Press any key to return to main menu..");
            Console.ReadKey();
        }

        private static void CompareHashes()
        {
            Console.Write("Enter the file path to calculate the hash: ");
            string filePath = Console.ReadLine();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            string calculatedHash = MD5.ComputeMD5ForFile(filePath);
            Console.WriteLine($"MD5 Hash: {calculatedHash}");

            Console.Write("Enter the file path where the saved hash is stored: ");
            string savedHashFilePath = Console.ReadLine();
            if (!File.Exists(savedHashFilePath))
            {
                Console.WriteLine("Saved hash file not found.");
                return;
            }

            string savedHash = File.ReadAllText(savedHashFilePath).Trim();

            if (calculatedHash == savedHash)
            {
                Console.WriteLine("The hash matches the saved hash.");
            }
            else
            {
                Console.WriteLine("The hash does not match the saved hash.");
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
