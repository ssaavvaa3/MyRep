using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace BinSpace
{

 
    class BinExchangePackage
    {
        public enum StatusCodes
        {
            OK = 0,
            CRC_ERROR = 1,
            PACK_TOO_LONG = 2
        };

        public UInt16 Len = 0;
        public UInt16 crc16 = 0;
        public StatusCodes Status;
        public byte[] Data;
    }

    class BinExchangeChar
    {
        public bool Start;
        public bool CharDetect;
        public byte Ch;


        public BinExchangeChar(bool start, bool ch_detect, byte ch)
        {
            Start = start;
            CharDetect = ch_detect;
            Ch = ch;
        }
    }

}