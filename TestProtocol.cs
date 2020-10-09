using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinSpace;
using System.Diagnostics;


namespace Tests
{
   static class Test
    {
        static void testLength1()
        {
           
            Random rnd = new Random();
            BinExchangeProtocol protocol = new BinExchangeProtocol(250);
            Queue<BinExchangePackage> PackFIFO  = new Queue<BinExchangePackage>();

            byte[] start = { 0xF4, 0x00 };
            byte[] Length = { 0xF4, 0xF4, 0x00 };
            byte[] data = new byte[0xF4];
            rnd.NextBytes(data);            

            UInt16 Len = (UInt16)(Length[0] | Length[2] << 8);
            UInt16 Sum = protocol.Crc16(data, Len);

            byte startSum = (byte)Sum;
            byte endSum = (byte)(Sum >> 8);

            byte[] DATA = start.Concat(Length).Concat(data).Append(startSum).Append(endSum).ToArray();

            for (int i = 0; i < DATA.Length; i++)
            {
                BinExchangePackage d = protocol.Parse(DATA[i]);
                if (d != null)
                {
                    PackFIFO.Enqueue(d);
                    foreach (byte x in d.Data)
                    {
                        System.Console.WriteLine(x);
                    }

                    System.Console.WriteLine(d.Status);

                    System.Console.WriteLine("stop");
                    System.Console.Read();
                }
            }

        }
    }
   
}
