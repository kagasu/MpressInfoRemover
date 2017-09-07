using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpressInfoRemover
{
    class Program
    {
        static int SearchSig(byte[] bytes, byte[] sig)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                for (var j = 0; j < sig.Length; j++)
                {
                    if (bytes[i + j] != sig[j])
                    {
                        break;
                    }
                    else if (j == sig.Length - 1)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("MpressInfoRemover [exe]");
                return;
            }

            var fileName = args[0];
            var bytes = File.ReadAllBytes(fileName);

            // x86, x64
            // MPRESS1
            var sig = new byte[] { 0x4D, 0x50, 0x52, 0x45, 0x53, 0x53, 0x31 };
            var offset = SearchSig(bytes, sig);
            if (offset != -1)
            {
                foreach (var x in Enumerable.Range(0, 7))
                {
                    bytes[offset + x] = 0x00;
                }
            }

            // x86, x64
            // MPRESS2
            sig = new byte[] { 0x4D, 0x50, 0x52, 0x45, 0x53, 0x53, 0x32 };
            offset = SearchSig(bytes, sig);
            if (offset != -1)
            {
                foreach (var x in Enumerable.Range(0, 7))
                {
                    bytes[offset + x] = 0x00;
                }
            }

            // .NET
            // mpress.mscorelib
            sig = new byte[] { 0x6D, 0x70, 0x72, 0x65, 0x73, 0x73, 0x00, 0x6D, 0x73, 0x63, 0x6F, 0x72, 0x6C, 0x69, 0x62 };
            offset = SearchSig(bytes, sig);
            if (offset != -1)
            {
                foreach (var x in Enumerable.Range(0, 6))
                {
                    bytes[offset + x] = 0x00;
                }
            }
            
            // .NET
            // MPRESS
            sig = new byte[] { 0x4D, 0x00, 0x50, 0x00, 0x52, 0x00, 0x45, 0x00, 0x53, 0x00, 0x53 };
            offset = SearchSig(bytes, sig);
            if (offset != -1)
            {
                foreach (var x in Enumerable.Range(0, 11))
                {
                    bytes[offset + x] = 0x00;
                }
            }

            File.WriteAllBytes(fileName, bytes);
        }
    }
}
