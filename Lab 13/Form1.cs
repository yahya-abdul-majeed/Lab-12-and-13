using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_13
{
    public partial class Form1 : Form
    {
        double btcInitial;
        double ethInitial;
        double dt = 1.0 / 252.0; // time increment of one trading day
        double t = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();            }
            else
            {
                btcInitial = (double)BtcBox.Value;
                ethInitial = (double)EthBox.Value;
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                t = 0;
                timer1.Interval = 100;
                timer1.Start();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var btcPrice = GeometricBrownianMotion.CalculateS1(btcInitial, 0.09, 2, dt);
            var ethPrice = GeometricBrownianMotion.CalculateS1(ethInitial, 0.02, -0.9, dt);
            t += dt;
            chart1.Series[0].Points.AddXY(t,btcPrice);
            chart1.Series[1].Points.AddXY(t,ethPrice);

        }
    }
    public class GeometricBrownianMotion
    {
        public static double CalculateS1(double S0, double mu, double sigma, double dt)
        {
            Random random = new Random();
            double deltaW = Math.Sqrt(dt) * random.NextGaussian();
            double drift = (mu - 0.5 * sigma * sigma) * dt;
            double diffusion = sigma * deltaW;
            double S1 = S0 * Math.Exp(drift + diffusion);
            return S1;
        }
    }

    public static class RandomExtensions
    {
        public static double NextGaussian(this Random random)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return randStdNormal;
        }
    }
}
