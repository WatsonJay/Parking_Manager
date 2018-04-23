using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 智能停车场管理系统
{
    public partial class Form1 : Form
    {

        //初始化变量
        #region 初始化变量
        bool open = false;
        public delegate void HandleInterfaceUpdataDelegate(string text);
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;
        int a;
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
                text = Encoding.UTF8.GetString(result);
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

        //串口设备连接状态测试
        #region 串口设备连接状态测试

        #endregion
    }
}
