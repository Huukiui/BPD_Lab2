using System;
using System.Security.Cryptography;
using System.Text;

namespace BPD_Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MD5 Hash of '': " + MD5.ComputeHash(""));
            Console.WriteLine("MD5 Hash of 'a': " + MD5.ComputeHash("a"));
            Console.WriteLine("MD5 Hash of 'abc': " + MD5.ComputeHash("abc"));
            Console.WriteLine("MD5 Hash of 'message digest': " + MD5.ComputeHash("message digest"));
        }

        
    }
}
