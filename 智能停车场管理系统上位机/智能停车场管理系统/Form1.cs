using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace 智能停车场管理系统
{
    public partial class Form1 : Form
    {

        //初始化变量
        #region 初始化变量
        bool open = false;
        public delegate void HandleInterfaceUpdataDelegate(string text);
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;
        int total_ParkingA = 9;
        int total_ParkingB = 9;
        string a="";
        string Car = "";
        string test="";
        SoundPlayer player = new SoundPlayer();
        #endregion

        //窗体初始化
        #region 窗体初始化
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnnumber.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());//从系统获取已有串口
            if (btnnumber.Items.Count > 0)
            {
                btnnumber.SelectedIndex = 0;//串口变量初始化
                serialPort1.RtsEnable = true;//DataReceived事件委托
                serialPort1.ReceivedBytesThreshold = 1;//设置 DataReceived 事件发生前内部输入缓冲区中的字节数
                serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                btnspeed.SelectedIndex = 6;
            }
            else
            {
                MessageBox.Show("未检测到设备\r\n");

            }
            totalcarnum.Text = (total_ParkingA + total_ParkingB).ToString();
            parkAnum.Text = total_ParkingA.ToString();
            parkBnum.Text = total_ParkingB.ToString();
            btnnumber.Enabled = true;
            btnspeed.Enabled = true;
            Carin.Enabled = false;
            Carout.Enabled = false;
            CarPark.Enabled = false;
            radioButton2.Enabled = false;
            radioButton1.Enabled = false;
            btnReserch.Enabled = true;
            AcanPark.BackColor = Color.Lime;
            BcanPark.BackColor = Color.Lime;
        }
        #endregion

        //串口设备监听
        #region 串口设备监听
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string text = string.Empty;
                byte[] result = new byte[serialPort1.BytesToRead];
                serialPort1.Read(result, 0, serialPort1.BytesToRead);
                test = Encoding.UTF8.GetString(result);
                if (test=="1")
                {
                    if (total_ParkingB == 0 && total_ParkingB == 0)
                    {
                        player.SoundLocation = "n.wav";
                        player.Load();
                        player.Play();
                    }
                    else if (total_ParkingA <= 3 && total_ParkingA < total_ParkingB)
                    {
                        player.SoundLocation = "A.wav";
                        player.Load();
                        player.Play();
                    }
                    else if(total_ParkingB <= 3 && total_ParkingB < total_ParkingA)
                    {
                        player.SoundLocation = "B.wav";
                        player.Load();
                        player.Play();
                    }
                    else 
                    {
                        player.SoundLocation = "t.wav";
                        player.Load();
                        player.Play();
                    }
                }
            }
            catch
            {

            }
        }
        #endregion

        //串口设备连接及刷新
        #region 串口设备连接及刷新
        private void btnReserch_Click(object sender, EventArgs e)//串口设备刷新
        {
            btnnumber.Items.Clear();
            btnnumber.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            if (btnnumber.Items.Count > 0)
            {
                btnnumber.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("未检测到串口\r\n");
            }

        }

        private void btnOpen_Click(object sender, EventArgs e)//打开串口设备
        {
            if (open == false)
            {

                if (serialPort1.IsOpen)
                {
                    MessageBox.Show("串口已经打开", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //串口

                if (btnnumber.Text == string.Empty)
                {
                    MessageBox.Show("请选择串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //波特率

                if (btnspeed.Text == string.Empty)
                {
                    MessageBox.Show("请选择波特率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                serialPort1.BaudRate = int.Parse(btnspeed.Text);
                try
                {
                    serialPort1.PortName = btnnumber.SelectedItem.ToString();
                    serialPort1.Open();
                }
                catch
                {
                    try
                    {
                        btnnumber.SelectedIndex = btnnumber.SelectedIndex + 1;
                    }
                    catch
                    {
                        btnnumber.SelectedIndex = 0;
                    }
                    serialPort1.Close();
                }
                btnOpen.Text = "关闭";
                btnnumber.Enabled = false;
                btnspeed.Enabled = false;
                Carin.Enabled = true;
                CarPark.Enabled = true;
                Carout.Enabled = true;
                open = true;
                btnReserch.Enabled = false;
                radioButton2.Enabled = true;
                radioButton1.Enabled = true;
                pictureBox1.BackColor = Color.Lime;
                btnconnect.Text = btnnumber.Text;
            }
            else
            {
                try
                {
                    serialPort1.Close();
                    btnOpen.Text = "打开";
                    open = false;
                    btnnumber.Enabled = true;
                    btnspeed.Enabled = true;
                    Carin.Enabled = false;
                    Carout.Enabled = false;
                    CarPark.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton1.Enabled = false;
                    btnReserch.Enabled = true;
                    pictureBox1.BackColor = Color.Silver;
                    btnconnect.Text = "未连接";
                }
                catch
                {

                }
            }
        }
        #endregion

        //串口设备连接状态测试
        #region 串口设备连接状态测试
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                PortWrite("1");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                PortWrite("0");
            }
        }

        private void PortWrite(string message)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(message);
                //port.WriteLine(message);
            }
        }

        #endregion


        //车辆进场数据模拟显示
        #region 车辆进场数据模拟显示
        private void button1_Click(object sender, EventArgs e)//车辆停入
        {
            Car = CarPark.Text;
            try
            {
                CarParkinWhere(Car);
                totalcarnum.Text = (total_ParkingA + total_ParkingB).ToString();
                parkAnum.Text = total_ParkingA.ToString();
                parkBnum.Text = total_ParkingB.ToString();
                if(total_ParkingA==0)
                {
                    AcanPark.BackColor = Color.Silver;
                }
                else
                {
                    AcanPark.BackColor = Color.Lime;
                }
                if (total_ParkingB == 0)
                {
                    BcanPark.BackColor = Color.Silver;
                }
                else
                {
                    BcanPark.BackColor = Color.Lime;
                }
                a = totalcarnum.Text + "|" + parkAnum.Text + "|" + parkBnum.Text;
                PortWrite(a);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

         private void Carout_Click(object sender, EventArgs e)
        {
            
            Car = CarPark.Text;
            try
            {
                CarParkoutWhere(Car);
                totalcarnum.Text = (total_ParkingA + total_ParkingB).ToString();
                parkAnum.Text = total_ParkingA.ToString();
                parkBnum.Text = total_ParkingB.ToString();
                if (total_ParkingA == 0)
                {
                    AcanPark.BackColor = Color.Silver;
                }
                else
                {
                    AcanPark.BackColor = Color.Lime;
                }
                if (total_ParkingB == 0)
                {
                    BcanPark.BackColor = Color.Silver;
                }
                else
                {
                    BcanPark.BackColor = Color.Lime;
                }
                a = totalcarnum.Text + "|" + parkAnum.Text + "|" + parkBnum.Text;
                PortWrite(a);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


         //车辆进场数据模拟显示函数（没什么用别看）
        #region 车辆进场数据模拟显示函数（真的没什么用）
        //怎么还是点开呀QAQ，哼
        private void CarParkinWhere(string message)//进场分区
         {
             string[] sArray = Car.Split('0');
             if (sArray[0] == "A")
             {
                 CarParkinA(Car);
                 if (total_ParkingA > 0)
                 {
                     total_ParkingA -= 1;
                 }
                 else
                 {
                     throw new Exception("该区无车位");
                 }
                 
             }
             else if (sArray[0] == "B")
             {
                 CarParkinB(Car);
                 if (total_ParkingB > 0)
                 {
                   total_ParkingB -= 1;
                 }
                 else
                 {
                     throw new Exception("该区无车位");
                 }
                 
             }
             else
             {
                 throw new Exception("未找到相应车位");
             }
         }
        //别往下翻(*￣3￣)╭♡❀小花花砸你
        private void CarParkinA(string message)//A区进库模拟
         {
             switch (message)
             {
                 case "A01": if (ParkingA1.BackColor == Color.Silver)
                     {
                         ParkingA1.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A02": if (ParkingA2.BackColor == Color.Silver)
                     {
                         ParkingA2.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A03": if (ParkingA3.BackColor == Color.Silver)
                     {
                         ParkingA3.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A04": if (ParkingA4.BackColor == Color.Silver)
                     {
                         ParkingA4.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A05": if (ParkingA5.BackColor == Color.Silver)
                     {
                         ParkingA5.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A06": if (ParkingA6.BackColor == Color.Silver)
                     {
                         ParkingA6.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A07": if (ParkingA7.BackColor == Color.Silver)
                     {
                         ParkingA7.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A08": if (ParkingA8.BackColor == Color.Silver)
                     {
                         ParkingA8.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "A09": if (ParkingA9.BackColor == Color.Silver)
                     {
                         ParkingA9.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 default: throw new Exception("车位信息有误");
             }
         }
        //还在看要生气了，눈_눈
        private void CarParkinB(string message)//B区进库模拟
         {
             switch (message)
             {
                 case "B01": if (ParkingB1.BackColor == Color.Silver)
                     {
                         ParkingB1.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B02": if (ParkingB2.BackColor == Color.Silver)
                     {
                         ParkingB2.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B03": if (ParkingB3.BackColor == Color.Silver)
                     {
                         ParkingB3.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B04": if (ParkingB4.BackColor == Color.Silver)
                     {
                         ParkingB4.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B05": if (ParkingB5.BackColor == Color.Silver)
                     {
                         ParkingB5.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B06": if (ParkingB6.BackColor == Color.Silver)
                     {
                         ParkingB6.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B07": if (ParkingB7.BackColor == Color.Silver)
                     {
                         ParkingB7.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B08": if (ParkingB8.BackColor == Color.Silver)
                     {
                         ParkingB8.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 case "B09": if (ParkingB9.BackColor == Color.Silver)
                     {
                         ParkingB9.BackColor = Color.Lime;
                     }
                     else
                     {
                         throw new Exception("该车位已经有车");
                     }
                     break;
                 default: throw new Exception("车位信息有误");
             }
         }
        //还看，生气了(╯‵□′)╯︵┻━┻
        private void CarParkoutWhere(string message)//出场分区
        {
            string[] sArray = Car.Split('0');
            if (sArray[0] == "A")
            {
                CarParkoutA(Car);
                if (total_ParkingA < 9)
                {
                    total_ParkingA += 1;
                }  
                else
                    throw new Exception("该区无车辆");
            }
            else if (sArray[0] == "B")
            {
                CarParkoutB(Car);
                if (total_ParkingB < 9)
                {
                    total_ParkingB += 1;
                }
                    
                else
                    throw new Exception("该区无车辆");
            }
            else
            {
                throw new Exception("未找到相应车位");
            }
        }
        //居然不听我的，哭唧唧( •ˍ•̀ू )
        private void CarParkoutA(string message)//A区出库模拟
        {
            switch (message)
            {
                case "A01": if (ParkingA1.BackColor == Color.Lime)
                    {
                        ParkingA1.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A02": if (ParkingA2.BackColor == Color.Lime)
                    {
                        ParkingA2.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A03": if (ParkingA3.BackColor == Color.Lime)
                    {
                        ParkingA3.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A04": if (ParkingA4.BackColor == Color.Lime)
                    {
                        ParkingA4.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A05": if (ParkingA5.BackColor == Color.Lime)
                    {
                        ParkingA5.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A06": if (ParkingA6.BackColor == Color.Lime)
                    {
                        ParkingA6.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A07": if (ParkingA7.BackColor == Color.Lime)
                    {
                        ParkingA7.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A08": if (ParkingA8.BackColor == Color.Lime)
                    {
                        ParkingA8.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "A09": if (ParkingA9.BackColor == Color.Lime)
                    {
                        ParkingA9.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                default: throw new Exception("车位信息有误");
            }
        }
        //不理你了_(•̀ω•́ 」∠)_
        private void CarParkoutB(string message)//B区进库模拟
        {
            switch (message)
            {
                case "B01": if (ParkingB1.BackColor == Color.Lime)
                    {
                        ParkingB1.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B02": if (ParkingB2.BackColor == Color.Lime)
                    {
                        ParkingB2.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B03": if (ParkingB3.BackColor == Color.Lime)
                    {
                        ParkingB3.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B04": if (ParkingB4.BackColor == Color.Lime)
                    {
                        ParkingB4.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B05": if (ParkingB5.BackColor == Color.Lime)
                    {
                        ParkingB5.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B06": if (ParkingB6.BackColor == Color.Lime)
                    {
                        ParkingB6.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B07": if (ParkingB7.BackColor == Color.Lime)
                    {
                        ParkingB7.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B08": if (ParkingB8.BackColor == Color.Lime)
                    {
                        ParkingB8.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                case "B09": if (ParkingB9.BackColor == Color.Lime)
                    {
                        ParkingB9.BackColor = Color.Silver;
                    }
                    else
                    {
                        throw new Exception("该车位无车");
                    }
                    break;
                default: throw new Exception("车位信息有误");
            }
        }
        #endregion
    }
}
