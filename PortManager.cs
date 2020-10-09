using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BinSpace
{
    interface PortHandler
    {
        string FindPortName();
        void Init(string name, int boundRate);
        void Open();
        void Close();
        void Write(byte[] tx);
        byte[] Read(int wait);
        bool isOpen();
    }
    class PortManager:PortHandler
    {
        private static BinExchange BinEx = new BinExchange(128);
        private string _portName;
        private int _boundRate;
        public string FindPortName()
        {
            //TO DO  Реализовать//
            return "";
        }
        public void Init(string name,int boundRate)
        {
            _portName = name;
            _boundRate = boundRate;
        }
        public void Write(byte[] tx)
        {
            BinEx.Write(tx);
        }
        public byte[] Read(int wait=0)
        {
            return BinEx.Read(wait);
        }
        public void Open()
        {
            BinEx.Open(_portName, _boundRate);
        }
        public void Close()
        {
            BinEx.Close();
        }
        public bool isOpen()
        {
            return BinEx.isOpen();
        }
    }
}