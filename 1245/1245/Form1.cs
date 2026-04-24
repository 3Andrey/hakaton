using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
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
        DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            dt.Clear();
            dt.Columns.Add("Temperature", typeof(double));
            dt.Columns.Add("WaterLevel", typeof(double));
            dt.Columns.Add("Radiation", typeof(int));

            //Попытки отрисовать график
            //chart1.Series.Clear();
            /*var seriesTemp = chart1.Series.Add("Температура");
            seriesTemp.ChartType = SeriesChartType.Line;
            seriesTemp.Color = Color.Red;
            seriesTemp.BorderWidth = 2;
            seriesTemp.XValueMember = "Time";
            seriesTemp.YValueMembers = "Temperature";*/

            /*chart1.Series["Температура"].Points.Clear();
            chart1.Series["Уровень воды"].Points.Clear();
            chart1.Series["Радиация"].Points.Clear();*/
        }

        private int secondsCounter = 0;
        float tempLim = 1180;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AutoRegul()
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double water = Convert.ToDouble(textBox1.Text);
            string url1 = "https://mephi.opentoshi.net/api/v1/reactor/refill-water?team_id=b3bbb1e4&amount={water}";

            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(url1);
            req1.Method = "POST";
            req1.ContentLength = 0;

            using (HttpWebResponse res1 = (HttpWebResponse)req1.GetResponse())
            using (StreamReader reader1 = new StreamReader(res1.GetResponseStream()))
            {
                string responseText1 = reader1.ReadToEnd();
                Console.WriteLine(responseText1);
            }

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            string url = "https://mephi.opentoshi.net/api/v1/reactor/data?team_id=b3bbb1e4";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream());
            string response = reader.ReadToEnd();
            richTextBox1.Text = response;
            reactorResponse reactor = JsonConvert.DeserializeObject<reactorResponse>(response);
            label1.Text = "Темп " + reactor.data.reactor_state.temperature.ToString();
            label2.Text = "Рад " + reactor.data.reactor_state.radiation.ToString();
            label3.Text = "Вода " + reactor.data.reactor_state.water_level.ToString();

            DateTime timeNow = DateTime.Now;
            double time = timeNow.ToOADate();
            double temperature = reactor.data.reactor_state.temperature;
            double radiation = reactor.data.reactor_state.radiation;
            double waterLevel = reactor.data.reactor_state.water_level;

            //chart1.Series[0].Points.AddXY(timeNow, temperature);
            

            secondsCounter++;

            if(secondsCounter == 60)
            {
                secondsCounter = 0;

                chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();
                chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate();

                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;                
                chart1.ChartAreas[0].AxisX.Interval = 15;
            }

            dt.Clear();
            dt.Rows.Add(temperature, waterLevel, radiation);
            dataGridView1.DataSource = dt;
            if (Convert.ToDouble(dataGridView1.Rows[0].Cells[0].Value) >= 1200)
            {
                dataGridView1.Rows[0].Cells[0].Style.BackColor=Color.Red;
            }
            else
            {
                dataGridView1.Rows[0].Cells[0].Style.BackColor = Color.White;
            }
            if (Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value) == 0)
            {
                dataGridView1.Rows[0].Cells[1].Style.BackColor=Color.Red;
            }
            else
            {
                dataGridView1.Rows[0].Cells[1].Style.BackColor = Color.White;
            }
            if (Convert.ToDouble(dataGridView1.Rows[0].Cells[2].Value) >= 150)
            {
                dataGridView1.Rows[0].Cells[2].Style.BackColor=Color.Red;
            }
            else
            {
                dataGridView1.Rows[0].Cells[2].Style.BackColor = Color.White;
            }


            ////Получаем данные о реакторе
            //string url = "https://mephi.opentoshi.net/api/v1/reactor/data?team_id=b3bbb1e4";
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            //StreamReader reader = new StreamReader(res.GetResponseStream());
            //string response = reader.ReadToEnd();
            //reactorResponse reactor = JsonConvert.DeserializeObject<reactorResponse>(response);

            //int temp = Convert.ToInt32(reactor.data.reactor_state.temperature);
            //int water_level = Convert.ToInt32(reactor.data.reactor_state.temperature);
            //int radiation = Convert.ToInt32(reactor.data.reactor_state.temperature);


            if (temperature > 1180 && temperature < 1200)
            {
                //охлаждаем на 10
                string url4 = "https://mephi.opentoshi.net/api/v1/reactor/activate-cooling?team_id=b3bbb1e4&duration=10";

                HttpWebRequest req4 = (HttpWebRequest)WebRequest.Create(url4);
                req4.Method = "POST";
                req4.ContentLength = 0;

                using (HttpWebResponse res4 = (HttpWebResponse)req4.GetResponse())
                using (StreamReader reader4 = new StreamReader(res4.GetResponseStream()))
                {
                    string responseText4 = reader4.ReadToEnd();
                    Console.WriteLine(responseText4);
                }
            }
            else if (temperature >= 1200)
            {
                //Охлаждаем на 20
                string url5 = "https://mephi.opentoshi.net/api/v1/reactor/activate-cooling?team_id=b3bbb1e4&duration=20";

                HttpWebRequest req5 = (HttpWebRequest)WebRequest.Create(url5);
                req5.Method = "POST";
                req5.ContentLength = 0;

                using (HttpWebResponse res5 = (HttpWebResponse)req5.GetResponse())
                using (StreamReader reader5 = new StreamReader(res5.GetResponseStream()))
                {
                    string responseText5 = reader5.ReadToEnd();
                    Console.WriteLine(responseText5);
                }
            }
            if (waterLevel < 40)
            {
                //Доливаем 30 воды
                string url1 = "https://mephi.opentoshi.net/api/v1/reactor/refill-water?team_id=b3bbb1e4&amount=30";

                HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(url1);
                req1.Method = "POST";
                req1.ContentLength = 0;

                using (HttpWebResponse res1 = (HttpWebResponse)req1.GetResponse())
                using (StreamReader reader1 = new StreamReader(res1.GetResponseStream()))
                {
                    string responseText1 = reader1.ReadToEnd();
                    Console.WriteLine(responseText1);
                }
            }
            if (radiation > 120 && radiation < 150)
            {
                //охлаждаем на 10
                string url7 = "https://mephi.opentoshi.net/api/v1/reactor/activate-cooling?team_id=b3bbb1e4&duration=10";

                HttpWebRequest req7 = (HttpWebRequest)WebRequest.Create(url7);
                req7.Method = "POST";
                req7.ContentLength = 0;

                using (HttpWebResponse res7 = (HttpWebResponse)req7.GetResponse())
                using (StreamReader reader7 = new StreamReader(res7.GetResponseStream()))
                {
                    string responseText7 = reader7.ReadToEnd();
                    Console.WriteLine(responseText7);
                }

                // доливаем 20 воды
                string url2 = "https://mephi.opentoshi.net/api/v1/reactor/refill-water?team_id=b3bbb1e4&amount=20";

                HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create(url2);
                req2.Method = "POST";
                req2.ContentLength = 0;

                using (HttpWebResponse res2 = (HttpWebResponse)req2.GetResponse())
                using (StreamReader reader2 = new StreamReader(res2.GetResponseStream()))
                {
                    string responseText2 = reader2.ReadToEnd();
                    Console.WriteLine(responseText2);
                }
            }
            else if (radiation >= 150)
            {
                //охлаждаем на 20
                string url6 = "https://mephi.opentoshi.net/api/v1/reactor/activate-cooling?team_id=b3bbb1e4&duration=20";

                HttpWebRequest req6 = (HttpWebRequest)WebRequest.Create(url6);
                req6.Method = "POST";
                req6.ContentLength = 0;

                using (HttpWebResponse res6 = (HttpWebResponse)req6.GetResponse())
                using (StreamReader reader6 = new StreamReader(res6.GetResponseStream()))
                {
                    string responseText6 = reader6.ReadToEnd();
                    Console.WriteLine(responseText6);
                }

                //доливаем 30 воды
                string url3 = "https://mephi.opentoshi.net/api/v1/reactor/refill-water?team_id=b3bbb1e4&amount=30";

                HttpWebRequest req3 = (HttpWebRequest)WebRequest.Create(url3);
                req3.Method = "POST";
                req3.ContentLength = 0;

                using (HttpWebResponse res3 = (HttpWebResponse)req3.GetResponse())
                using (StreamReader reader3 = new StreamReader(res3.GetResponseStream()))
                {
                    string responseText3 = reader3.ReadToEnd();
                    Console.WriteLine(responseText3);
                }
            }
            if (temperature > 1200 && waterLevel == 0 && radiation > 150)
            {
                //экстренное выключение
                string url8 = "https://mephi.opentoshi.net/api/v1/reactor/reset_reactor?team_id=b3bbb1e4";

                HttpWebRequest req8 = (HttpWebRequest)WebRequest.Create(url8);
                req8.Method = "POST";
                req8.ContentLength = 0;

                using (HttpWebResponse res8 = (HttpWebResponse)req8.GetResponse())
                using (StreamReader reader8 = new StreamReader(res8.GetResponseStream()))
                {
                    string responseText8 = reader8.ReadToEnd();
                    Console.WriteLine(responseText8);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            double ohlad = Convert.ToDouble(textBox2.Text);
            string url6 = "https://mephi.opentoshi.net/api/v1/reactor/activate-cooling?team_id=b3bbb1e4&duration={ohlad}";

            HttpWebRequest req6 = (HttpWebRequest)WebRequest.Create(url6);
            req6.Method = "POST";
            req6.ContentLength = 0;

            using (HttpWebResponse res6 = (HttpWebResponse)req6.GetResponse())
            using (StreamReader reader6 = new StreamReader(res6.GetResponseStream()))
            {
                string responseText6 = reader6.ReadToEnd();
                Console.WriteLine(responseText6);
            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
