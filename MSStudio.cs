using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _11_10_inclass
{
    public partial class Form1 : Form
    {
        int startflag = 0;
        int flag_sensor;
        string RxString;
        string temp;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            {
                if (!string.Equals(textBox1.Text, ""))
                {
                    if (serialPort1.IsOpen) serialPort1.Close();
                    try
                    {
                        if (RxString[0] == 'B')
                        {
                            flag_sensor = 10;
                        }
                        const string WRITEKEY = "ZSUO2EV8K5J2LM65";
                        string strUpdateBase = "http://api.thingspeak.com/update";
                        string strUpdateURI = strUpdateBase + "?api_key=" + WRITEKEY;
                        string strField1 = textBox1.Text;
                        string strField2 = textBox2.Text;
                        HttpWebRequest ThingsSpeakReq;
                        HttpWebResponse ThingsSpeakResp;
                        //flag_sensor = 0;

                        if (flag_sensor == 11)
                        {
                            strUpdateURI += "&field1=" + strField1;
                            // flag_sensor++;
                        }
                        if (flag_sensor == 12)
                        {
                            strUpdateURI += "&field2=" + strField1;
                            flag_sensor = 10;
                        }
                        flag_sensor++;


                        /*if (flag_sensor == 11)
                        {
                            strUpdateURI += "&field1=" + strField1;
                        }
                        else if (flag_sensor == 12)
                        {
                            strUpdateURI += "&field2=" + strField1;
                        }
                        else if (flag_sensor == 13)
                        {
                            strUpdateURI += "&field3=" + strField1;
                        }
                        else if (flag_sensor == 14)
                        {
                            strUpdateURI += "&field4=" + strField1;
                        }
                        else
                        {
                        }*/
                        ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(strUpdateURI);
                        ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();
                        ThingsSpeakResp.Close();
                        if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                        {
                            Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                            throw exData;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    textBox1.Text = "";
                    serialPort1.Open();
                }
            }
        }

        private void Serial_Start_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM4";
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                // startSerial.Enabled = false;
                // serialStop.Enabled = true;
                textBox1.ReadOnly = false;
            }
        }

        private void Serial_Stop_Click(object sender, EventArgs e)
        {
            {
                serialPort1.Close();
                //  startSerial.Enabled = true;
                //  serialStop.Enabled = false;
                textBox1.ReadOnly = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
            serialPort1.PortName = "COM4";
            serialPort1.BaudRate = 9600;
        }

        private void Read_in_Thingspeak_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            //ThingSpeakData.Text = 
            client.DownloadString("http://api.thingspeak.com/channels/1566867/feed.json");
            label1.Text = client.DownloadString("http://api.thingspeak.com/channels/1566867/field/field1/last.text");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Current_data_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (i == 0)
            {
                textBox1.AppendText(RxString);
                i++;
            }
            else if (i == 1)
            {
                textBox2.AppendText(RxString);
                i = 0;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(Current_data_Click));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

