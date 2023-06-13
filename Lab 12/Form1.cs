using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_12
{
    public partial class Form1 : Form
    {
        private double[,] transitionRateMatrix = {
            { -0.4, 0.3, 0.1},
            { 0.4, -0.8, 0.4 },
            { 0.1, 0.4, -0.5 }
        };
        private double[,] CDFS = new double[3,3];
        private Dictionary<int, string> states = new Dictionary<int, string>()
        {
            {0,"C:\\Users\\yahya\\source\\repos\\Lab 12 and 13\\Lab 12\\Images\\cloudy.png" },
            {1,"C:\\Users\\yahya\\source\\repos\\Lab 12 and 13\\Lab 12\\Images\\raining.png" },
            {2,"C:\\Users\\yahya\\source\\repos\\Lab 12 and 13\\Lab 12\\Images\\sun.png" }
        };
        private int currentState = 0;
        private int T = 10000;
        private int N = 6;
        private int count = 0;
        private Random random = new Random();
        public Form1()
        {
            InitializeComponent();
            CalculateCDFs();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled) timer1.Stop();
            else
            {
                pictureBox1.SizeMode =PictureBoxSizeMode.StretchImage;
                timer1.Interval = CalculateHoldingTimeInMilliSeconds(transitionRateMatrix[currentState,currentState]*-1);
                pictureBox1.ImageLocation = states[currentState];
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            currentState = GetNextState();
            pictureBox1.ImageLocation = states[currentState];
            timer1.Interval = CalculateHoldingTimeInMilliSeconds(transitionRateMatrix[currentState,currentState]*-1);

        }
        private int GetNextState()
        {
            var U = random.NextDouble();
            for(int i = 0; i < 3; i++)
            {
                if (U - CDFS[currentState, i] <= 0)
                {
                    return i;
                }
            }
            return 0;
        }
        private void CalculateCDFs()
        {
            for(int i = 0; i < 3; i++)
            {
                double sum = 0;
                for(int j = 0; j < 3; j++)
                {
                    if (i == j)
                        sum += 0;
                    else
                        sum += (transitionRateMatrix[i, j] / -transitionRateMatrix[i,i]);
                    CDFS[i,j] = sum;
                }
            }
        }
        private int CalculateHoldingTimeInMilliSeconds(double rate)
        {
            return (int)((-(Math.Log(random.NextDouble())) / rate)*1000);
        }

    }
}
