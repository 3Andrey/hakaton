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
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void CreateChart(DataGridView dgv, Chart chart, string nameTitle, string SeriesName)
        {
            try
            {
                chart.Series.Clear();
                chart.Series.Add(SeriesName);
                
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    var name = dgv.Rows[i].Cells[1].Value?.ToString() ?? "";
                    var value = dgv.Rows[i].Cells[0].Value?.ToString() ?? "";
                    chart.Series[SeriesName].Points.AddXY(name, value);
                }
                chart.Titles.Clear();
                chart.Titles.Add(nameTitle);

                chart.ChartAreas[0].AxisX.Title = dgv.Columns[1].HeaderText;
                chart.ChartAreas[0].AxisY.Title = dgv.Columns[0].HeaderText;

                MessageBox.Show("График сформирован", "Успех");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Ошибка: Не достаточно столбцов в DataGridView", "Ошибка");
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка: недопустимые данные в DataGridView", "Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка");
            }
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
        { // Create a Series and set type to Bar
            Series series = new Series("Temp")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = true
            };
            
        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
