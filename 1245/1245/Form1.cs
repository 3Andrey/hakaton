using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _1245
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private int secondsCounter = 0;
        float tempLim = 1180;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://mephi.opentoshi.net/api/v1/reactor/data?team_id=b3bbb1e4";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse  res = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream());
            string response  = reader.ReadToEnd();
            richTextBox1.Text = response;
            reactorResponse reactor = JsonConvert.DeserializeObject<reactorResponse>(response);
            label1.Text = reactor.data.reactor_state.temperature.ToString();

        }

        private void chart1_Click(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            timer1.Enabled = true;

            chart1.ChartAreas[0].AxisY.Minimum = 1100;
            chart1.ChartAreas[0].AxisY.Maximum = 1350;
            
        
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm:ss";
            chart1.Series[0].XValueType = ChartValueType.DateTime;

            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();

            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.Interval = 5;

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string url = "https://mephi.opentoshi.net/api/v1/reactor/data?team_id=b3bbb1e4";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream());
            string response = reader.ReadToEnd();
            richTextBox1.Text = response;
            reactorResponse reactor = JsonConvert.DeserializeObject<reactorResponse>(response);
            label1.Text = reactor.data.reactor_state.temperature.ToString();

            DateTime timeNow = DateTime.Now;
            double time = timeNow.ToOADate();
            double temperature = reactor.data.reactor_state.temperature;

            //chart1.Series[0].Points.AddXY(time, temperature);

            secondsCounter++;

            if(secondsCounter == 60)
            {
                secondsCounter = 0;

                chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();

                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;                
                chart1.ChartAreas[0].AxisX.Interval = 15;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "https://mephi.opentoshi.net/api/v1/reactor/refill-water?team_id=b3bbb1e4&amount=30";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentLength = 0; 

            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            using (StreamReader reader = new StreamReader(res.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();
                Console.WriteLine(responseText);
            }

        }
    }
}
