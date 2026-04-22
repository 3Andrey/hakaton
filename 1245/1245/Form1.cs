using Newtonsoft.Json;
using System;
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
        }

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
            data reactor = JsonConvert.DeserializeObject<data>(response);
            label1.Text = reactor.reactor_state.temperature.ToString();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Create a Series and set type to Bar
            Series series = new Series("Temp")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = true
            };
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
