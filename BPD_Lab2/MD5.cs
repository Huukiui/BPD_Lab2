﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPD_Lab2
{
    public class MD5
    {
        private static readonly uint A0 = 0x67452301;
        private static readonly uint B0 = 0xEFCDAB89;
        private static readonly uint C0 = 0x98BADCFE;
        private static readonly uint D0 = 0x10325476;

        private static readonly uint[] T = {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
            0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };

        private static readonly uint[] S = {
            7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
            5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20,
            4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
            6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };

        public static string ComputeHash(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] paddedInput = PadMessage(inputBytes);

            uint A = A0;
            uint B = B0;
            uint C = C0;
            uint D = D0;

            for(int i = 0; i < paddedInput.Length / 64; i++)
            {
                byte[] block = new byte[64];

                Array.Copy(paddedInput, i * 64, block, 0, 64);

                uint[] M = new uint[16];
                for(int j = 0; j < 16; j++)
                {
                    M[j] = BitConverter.ToUInt32(block, j * 4);
                }
                uint AA = A, BB = B, CC = C, DD = D;

                for (uint j = 0; j < 64; j++)
                {
                    uint F, g;

                    if(j < 16)
                    {
                        F = (B & C) | (~B & D);
                        g = j;
                    }
                    else if(j < 32)
                    {
                        F = (B & D) | (C & ~D);
                        g = (1 + 5 * j) % 16;
                    }
                    else if(j < 48)
                    {
                        F = B ^ C ^ D;
                        g = (5 + 3 * j) % 16;
                    }
                    else
                    {
                        F = C ^ (B | ~D);
                        g = (7 * j) % 16;
                    }

                    uint tempD = D;
                    D = C;
                    C = B;
                    B = LeftRotate((A + F + M[g] + T[j]), (int)S[j]) + B;
                    A = tempD;
                }

                A += AA;
                B += BB;
                C += CC;
                D += DD;
            }

            return GetByteString(A) + GetByteString(B) + GetByteString(C) + GetByteString(D);
        }

        private static byte[] PadMessage(byte[] input)
        {
            int inputLength = input.Length;
            int padLength = (56 + 64 - ((inputLength + 1) % 64)) % 64;

            byte[] paddedInput = new byte[inputLength + 1 + padLength + 8];
            Array.Copy(input, paddedInput, inputLength);

            paddedInput[inputLength] = 0x80;
            ulong messageLengthBits = (ulong)inputLength * 8;

            byte[] lengthBytes = BitConverter.GetBytes(messageLengthBits);

            Array.Copy(lengthBytes, 0, paddedInput, paddedInput.Length - 8, 8);

            return paddedInput;
        }

        private static uint LeftRotate(uint value, int shift)
        {
            return (value << shift) | (value >> (32 - shift));
        }

        private static string ToHex(uint value)
        {
            return value.ToString("X8").ToLower();
        }

        private static string GetByteString(uint x)
        {
            return String.Join("", BitConverter.GetBytes(x).Select(y => y.ToString("X2")));
        }
    }
}