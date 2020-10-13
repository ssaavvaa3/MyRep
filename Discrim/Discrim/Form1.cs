using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using System.Windows.Media;
using LiveCharts.Wpf;

namespace Discrim
{
    public struct MySettings
    {
        public int HV;
        public int Discr_Top;
        public int Discr_Bot;
        public int Gain;
        public int WinSize;
    }
    public interface IView
    {
        MySettings settings { get; }
        event EventHandler GoGo;
        event EventHandler Stop;
        void Draw(int[] args);
    }
    
    public partial class MainForm : Form, IView
    {
        private MySettings mySettings;
        public MySettings settings
        {
            get { return mySettings; }
        }
        public event EventHandler GoGo;
        public event EventHandler Stop;
        public MainForm()
        {
            InitializeComponent();
            But_GOGO.Click += But_GOGO_MyClick;
            But_Stop.Click += But_Stop_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Draw(MyFuncs.ArrGen<int>(255, rnd.Next(0,100), x => rnd.Next(0, 100)));
            
        }
        #region работа с графиком
        public void Draw(int[] args) {
            int sum = args.Sum();
            
            if (cartesianChart1.Series.Count() == 0)
            {
                
                cartesianChart1.Series.Add(new LineSeries
                {
                    Title = "График",
                    Values = new ChartValues<int>(args),
                    PointGeometry = null
                });
                cartesianChart1.AxisX.Add(new Axis
                {
                    Title = "Каналы",
                    Labels = MyFuncs.ArrGen<string>(255, "0", x => (Int32.Parse(x) + 1).ToString())
                });

                cartesianChart1.AxisY.Add(new Axis
                {
                    Title = "импульсы/отн.ед",
                    LabelFormatter = value => String.Format("{0:0} \\ {1:0.00}  ", value, value / sum),
                });
            }
            else
            {
                
                cartesianChart1.Series[0] = new LineSeries
                {
                    Title = "График",
                    Values = new ChartValues<int>(args),
                    PointGeometry = null
                };
            }

        }
        #endregion
        private void But_GOGO_MyClick(object sender, EventArgs e)
        {
            LockAll();
            mySettings.HV = Decimal.ToInt32(numericUpDown_HV.Value);
            mySettings.Gain = Decimal.ToInt32(numericUpDown_Gain.Value);
            mySettings.Discr_Top = Decimal.ToInt32(numericUpDown_Top.Value);
            mySettings.Discr_Bot = Decimal.ToInt32(numericUpDown_Bot.Value);
            mySettings.WinSize = Decimal.ToInt32(numericUpDown_Win.Value);
            if (GoGo != null) GoGo(this, EventArgs.Empty);
        }

        private void But_Stop_Click(object sender, EventArgs e)
        {
            if (Stop != null) Stop(this, EventArgs.Empty);
            UnLockAll();
        }
        #region Возможно полезные события


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
        #endregion

        #region фукции управления доступом к кнопакам
        private void LockAll()
        {
            groupBoxLoad.Enabled = false;
            groupBoxSettings.Enabled = false;
            checkBoxSub.Enabled = false;
        }
        private void UnLockAll()
        {
            groupBoxLoad.Enabled = true;
            groupBoxSettings.Enabled = true;
            checkBoxSub.Enabled = true;
        }
        #endregion
    }

    #region Моя пачка фукций
    public class MyFuncs
    {
        // Генератор массива
        public delegate T Next<T>(T item);
        public static T[] ArrGen<T>(int n,T start, Next<T> act)
        {
            T[] arr = new T[n];
            arr[0] = start;
            for (int i = 1 ; i < n; i++)
            {
                arr[i] = act(arr[i - 1]);
            }
            return arr;
        }
    }
    #endregion
}
