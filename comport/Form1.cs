using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace comport
{
    public partial class Form1 : Form
    {
        System_config system_config;
        public Form1()
        {
            InitializeComponent();
        }
       
        string[] com_name = new string[2];
      
        private void Form1_Load(object sender, EventArgs e)
        {
            int count = 0;
            Com_setting_box.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Com_setting_box.Items.Add(port);
            }
            if (ports.Length > 0) Com_setting_box.SelectedIndex = 0;
            string[] Baudrate = { "9600", "19200", "38400", "57600", "115200" };
            foreach (string baud in Baudrate)
            {
                Baudrate_box.Items.Add(baud);
            }
            if (Baudrate.Length > 0) Baudrate_box.SelectedIndex = 0;
            using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Console.txt")) 
            {
                while (sr.ReadLine() != null)
                {
                    count++;
                }

            }
            using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Console.txt"))
            {

                for (int i = 0; i < count; i++)
                {
                    com_name[i] = sr.ReadLine();
                }
            }
            
        }

        private void connect_com_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen) serialPort1.Close();
                serialPort1.PortName = com_name[0];
                serialPort1.BaudRate = Convert.ToInt32(com_name[1]);
                serialPort1.Open();
                DialogResult result = MessageBox.Show("Opem " + com_name[0] + " Successfully!");
                
            }
            catch (Exception)
            {
                MessageBox.Show(com_name[0] + " Not Existing or Available, Try other one");

            }
        }

        private void SAVE_btn_Click(object sender, EventArgs e)
        {
            bool success = true;
            int count = 0;
            if (Com_setting_box.Items.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Console.txt")) 
                {
                    sw.WriteLine(Com_setting_box.Text);
                }
            }
            else
            {
                MessageBox.Show("Select COM port first");
                success = false;
            }
            if (Baudrate_box.Items.Count > 0)
            {
                using (StreamWriter sw =  File.AppendText(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Console.txt"))
                {
                    sw.WriteLine(Baudrate_box.Text);
                }
            }
            else
            {
                MessageBox.Show("Select Baudrate first");
                success = false;
            }
            if (success) MessageBox.Show("Com Setting is updated Successfully!");
            if (serialPort1.IsOpen) serialPort1.Close();
            using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Console.txt"))
            {
                while (sr.ReadLine() != null)
                {
                    count++;
                }

            }
            using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Console.txt"))
            {

                for (int i = 0; i < count; i++)
                {
                    com_name[i] = sr.ReadLine();
                }
            }
            serialPort1.PortName = com_name[0];
            serialPort1.BaudRate = int.Parse(com_name[1]);
            serialPort1.Open();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadExisting();
                MethodInvoker inv = delegate
                {
                    StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Output.txt");                  
                        sw.Write(data);
                        sw.Close();
                    
                }; this.Invoke(inv);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message, "Information RS232", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
        }
    }
}
