using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace BinSpace
{

    public class BinExchange_Exception : Exception
    {
        public BinExchange_Exception()
        {
        }

        public BinExchange_Exception(string message)
            : base(message)
        {
        }

        public BinExchange_Exception(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class BinExchange_TimeoutException : Exception
    {
        public BinExchange_TimeoutException()
        {
            Console.WriteLine("Пизда");
        }

        public BinExchange_TimeoutException(string message)
            : base(message)
        {
            Console.WriteLine("Пизда");
        }

        public BinExchange_TimeoutException(string message, Exception inner)
            : base(message, inner)
        {
            Console.WriteLine("Пизда");
        }
    }


    class BinExchange
    {
        public int PackMaxLenght { get; private set; }
        public delegate void DataReceivedDelegate();

        public event DataReceivedDelegate DataReceived;
        public bool isOpen()
        {
            return _serialPort.IsOpen;
        }


        SerialPort _serialPort;
        Queue<BinExchangePackage> PackFIFO;
        BinExchangeProtocol protocol;

        public BinExchange(int PackMaxLenght)
        {
            this.PackMaxLenght = PackMaxLenght;

            _serialPort = new SerialPort();
            PackFIFO = new Queue<BinExchangePackage>();
            protocol = new BinExchangeProtocol(PackMaxLenght);
        }

        public BinExchange() : this(32)
        {

        }

        /// <summary>
        /// Открыть порт
        /// </summary>
        /// <param name="Port">Имя COM-порта (например, "COM1")</param>
        /// <param name="Baud">Скорость передачи данных</param>
        public void Open(string Port, int Baud)
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();

            _serialPort.PortName = Port;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.DataReceived += _serialPort_DataReceived;
            _serialPort.BaudRate = Baud;
            _serialPort.Open();
        }

        /// <summary>
        /// Прочитать пакет данных
        /// </summary>
        /// <returns>Полученные данные</returns>
        public byte[] Read(int Timeout)
        {
            int cntr = 0;

            while (PackFIFO.Count == 0)
            {
                
             
                if (Timeout > 0)
                {
                    Task.Delay(10).GetAwaiter().GetResult();
                    if (cntr >= Timeout)
                    {
                        
                        throw new BinExchange_TimeoutException("Timeout error");
                    }
                    cntr+=10;
                    
                }
            }

            
            BinExchangePackage ret = PackFIFO.Dequeue();
            if (ret.Status == BinExchangePackage.StatusCodes.OK)
            {
                return ret.Data;
            }
            else
            {
                throw new BinExchange_Exception("CRC error");
            }
        }

        public byte[] Read()
        {
            return Read(-1);
        }

        /// <summary>
        /// Отправить пакет данных
        /// </summary>
        /// <param name="data">Передаваемые данные. Длина не должна превышать .....</param>
        public void Write(byte[] data)
        {
            if (data.Length > PackMaxLenght)
                return;

            byte[] wr = protocol.Convert(data);
            
            foreach(byte x in wr)
                Console.WriteLine(x);
            _serialPort.Write(wr, 0, wr.Length);
        }


        /// <summary>
        /// Закрывает соединение порта.
        /// </summary>
        public void Close()
        {
            _serialPort.Close();
        }

        /// <summary>
        /// Возвращает массив имен последовательных портов для текущего компьютера.
        /// </summary>
        /// <returns>Массив имен последовательных портов для текущего компьютера.</returns>
        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[_serialPort.BytesToRead];

            _serialPort.Read(data, 0, data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                BinExchangePackage d = protocol.Parse(data[i]);
                if (d != null)
                {
                    PackFIFO.Enqueue(d);

                    DataReceived?.Invoke();
                }
            }
        }




    }
}