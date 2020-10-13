using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinSpace;


namespace Discrim
{
    
    class MainManager
    {
        private readonly IView _view;
        private readonly PortHandler _port;
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        public MainManager(IView view,PortHandler port)
        {
            _view = view;
            _port = port;
            view.GoGo += _GoGoFun;
            view.Stop += _StopFun;
        }
        private void _GoGoFun(object sender, EventArgs e)
        {
            /*
            //_port.FindPortName();  -реализовать
            _port.Init("COM3", 115200);
            _port.Open();//обработать все ответы
            
            _port.Write(new byte[] {0x01,(byte)(_view.settings.HV / 4) });
            Write("-------");
            _port.Read(500);
            _port.Write(new byte[] { 0x02, (byte)(_view.settings.Discr_Top) });
            Write("-------");
            _port.Read(500);
            _port.Write(new byte[] { 0x03, (byte)(_view.settings.Discr_Bot) });
            Write("-------");
            _port.Read(500);
            _port.Write(new byte[] { 0x04, (byte)(_view.settings.Gain) });
            Write("-------");
            _port.Read(500);
            _port.Write(new byte[] { 0x05, (byte)(_view.settings.WinSize) });
            Write("-------");
            _port.Read(500);
            
            _port.Write(new byte[] { 0x08, 0 });
            
            _port.Write(new byte[] { 0x0B});
            _timer.Enabled = true;
            _timer.Interval = 100;
            _timer.Tick += _GetAmpl;
            */
            System.Console.WriteLine("1");

        }
        private void _GetAmpl(object sender, EventArgs e)
        {
            /*byte[] ampl = new byte[255];
            for (int j = 0; j < 16; j++)
            {
                byte[] arr = _port.Read(200);
                for (int i = 0; i < 255; i++)
                {
                    ampl[i]=arr[i]*
                }
            }
            */
            byte[] arr = _port.Read(200);
            foreach (byte x in arr)
            {
                System.Console.Write(x + " ");
            }
            System.Console.WriteLine();
        }
        private void _StopFun(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            if (_port.isOpen())
            {
                _port.Write(new byte[] { 0x09, 0 });
                _port.Close();
            }
            
        }
        private void Write(string str,string del = " ")
        {
            System.Console.WriteLine(str + del);
        }


    }
}
