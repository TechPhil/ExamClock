using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
//using System.Threading;

namespace ExamClock
{
    public partial class ExamClock : Form
    {
        //Graphics stuff
        Timer t = new Timer();
        int WIDTH = 600, HEIGHT = 600, secHAND = 140, minHAND = 110, hrHAND = 80;
        int cx, cy;
        Bitmap bmp;
        Graphics g;
        //END GRAPHICS STUFF

        //Global Vars
        TimeSpan DebugFinish,SWRemainingTime, ElapsedTime, PausedFor, StartTime, FinishTime;
        bool doStopwatchTick = false, examStarted = false;
        private void showSetupWizard()
        {
            showNameSetup();
            showDurSetup();
            ExamReset();
        }
        private void showNameSetup()
        {
            ExamNameDialog examNameForm = new ExamNameDialog();
            examNameForm.ShowDialog();
            ExamReset();
            
        }
        private void showDurSetup()
        {
            DurationDialog durationForm = new DurationDialog();
            durationForm.ShowDialog();
            ExamReset();
            
        }
        private void ExamReset()
        {
            label2.Text = Properties.Settings.Default.ExamName;
            ElapsedTime = TimeSpan.Parse("00:00");
            SWRemainingTime = Properties.Settings.Default.Duration;
            PausedFor = TimeSpan.Parse("00:00");
            label7.Text = Properties.Settings.Default.Duration.ToString();
            label12.Text = SWRemainingTime.ToString();
            label11.Text = ElapsedTime.ToString();
            label14.Text = PausedFor.ToString();
            label6.Text = "Waiting for start";
            label8.Text = "Waiting for start";
            examStarted = false;
            doStopwatchTick = false;
            this.BackColor = Color.White;
            label12.ForeColor = Color.Black;
            label11.ForeColor = Color.Black;
            label8.ForeColor = Color.Black;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            showNameSetup();
        }

        private void setupWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showSetupWizard();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            showDurSetup();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            doStopwatchTick = true;
            examStarted = true;
            StartTime = DateTime.Now.TimeOfDay;
            label6.Text = StartTime.ToString(@"hh\:mm\:ss");
            FinishTime = StartTime.Add(Properties.Settings.Default.Duration);
            label8.Text = FinishTime.ToString(@"hh\:mm\:ss");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            doStopwatchTick = !doStopwatchTick;
        }
        /// <summary>
        /// Occurs when clicking the "Exam Reset button"
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            ExamReset();
        }

        public ExamClock()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Occurs on loading of the form
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            label6.Text = "Waiting for setup";
            label8.Text = "Waiting for setup";
            label11.Text = "Waiting for setup";
            label12.Text = "Waiting for setup";
            label14.Text = "Waiting for setup";
            //create bitmap
            bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);

            //center
            cx = WIDTH / 2;
            cy = HEIGHT / 2;

            //backcolor
            this.BackColor = Color.White;

            //timer
            t.Interval = 1000; //in milliseconds
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
        }
        //Run every 0.1s
        /// <summary>
        /// Runs every 0.1 seconds - Update cycle. TODO: Cleanup and split into functions
        /// </summary>
        private void t_Tick(object sender, EventArgs e)
        {

            //DO THE OTHER TIMER
            if (doStopwatchTick)
            {
                this.BackColor = Color.White;
                SWRemainingTime = SWRemainingTime.Subtract(TimeSpan.Parse("00:00:01"));
                if (SWRemainingTime < TimeSpan.Parse("00:00"))
                {
                    label12.Text = TimeSpan.Parse("00:00").ToString();
                    label12.ForeColor = Color.White;
                    label11.ForeColor = Color.White;
                    label8.ForeColor = Color.White;
                    this.BackColor = Color.Tomato;
                }
                else
                {
                    label12.Text = SWRemainingTime.ToString(@"hh\:mm\:ss");
                    label12.ForeColor = Color.Black;
                    label11.ForeColor = Color.Black;
                    label8.ForeColor = Color.Black;
                    this.BackColor = Color.White;
                }

                ElapsedTime = ElapsedTime.Add(TimeSpan.Parse("00:00:01"));
                label11.Text = ElapsedTime.ToString(@"hh\:mm\:ss");
            }
            else
            {
                if (examStarted)
                {
                    PausedFor = PausedFor.Add(TimeSpan.Parse("00:00:01"));
                    label14.Text = PausedFor.ToString(@"hh\:mm\:ss");
                    FinishTime = DateTime.Now.TimeOfDay.Add(SWRemainingTime);
                    label8.Text = FinishTime.ToString(@"hh\:mm\:ss");
                    this.BackColor = Color.LightGray;
                }
                else
                {
                    label14.Text = TimeSpan.Parse("00:00").ToString();
                }

            }
            
            //get time
            TimeSpan TimeNow = DateTime.Now.TimeOfDay;
            int ss = TimeNow.Seconds;
            int mm = TimeNow.Minutes;
            int hh = TimeNow.Hours;
            int ms = TimeNow.Milliseconds;

            //put time into text format
            string sstring = ss.ToString();
            string mstring = mm.ToString();
            string hstring = hh.ToString();
            string textTime = TimeNow.ToString(@"hh\:mm\:ss");
            label1.Text = textTime;

            //Set up labels
            //label6.Text = Properties.Settings.Default.StartTime.ToString();
            //TimeSpan FinishTime = Properties.Settings.Default.StartTime.Add(Properties.Settings.Default.Duration);
            //label8.Text = FinishTime.ToString();

            //Do stopwatch labels
            //TimeSpan TimeNow = DateTime.Now.TimeOfDay;
            //TimeSpan StartTime = Properties.Settings.Default.StartTime;
            //TimeSpan Duration = Properties.Settings.Default.Duration;
            //if (TimeNow.Subtract(StartTime) < TimeSpan.Parse("00:00"))
            //{
            //    label11.Text = "00:00:00.0";
            //}
            //else if (TimeNow.Subtract(StartTime) > Duration)
            //{
            //    label11.Text = Duration.ToString(@"hh\:mm\:ss\.f");
            //}
            //else
            //{
            //    label11.Text = TimeNow.Subtract(StartTime).ToString(@"hh\:mm\:ss\.f");
            //}

            //if (FinishTime.Subtract(TimeNow) > Duration)
            //{
            //    label12.Text = Duration.ToString(@"hh\:mm\:ss\.f");
            //}
            //else if (FinishTime.Subtract(TimeNow) <= TimeSpan.Parse("00:00"))
            //{
            //    label12.Text = "00:00:00.0";
            //}
            //else
            //{
            //    label12.Text = FinishTime.Subtract(TimeNow).ToString(@"hh\:mm\:ss\.f");
            //}

            //Debug Text Box
            DebugFinish = DateTime.Now.TimeOfDay.Add(SWRemainingTime);
            //textBox1.Text = "DEBUG\r\nElapsedTime " + ElapsedTime.ToString() + "\r\nSWRemainingTime " + SWRemainingTime.ToString() + "\r\nTime Now" + DateTime.Now.TimeOfDay + "\r\nFinishTime " + DebugFinish.ToString() + "\r\n Difference " + DateTime.Now.TimeOfDay.Subtract(DebugFinish).ToString();


            //create graphics
            g = Graphics.FromImage(bmp);
            int[] handCoord = new int[2];

            //clear
            if (!doStopwatchTick && examStarted)
            {
                g.Clear(Color.LightGray);
            }
            else if (SWRemainingTime < TimeSpan.Parse("00:00"))
            {
                g.Clear(Color.Tomato);
            }
            else
            {
                g.Clear(Color.White);
            }
            

            //draw circle
            g.DrawEllipse(new Pen(Color.Black, 1f), 0, 0, WIDTH, HEIGHT);

            //draw figure
            g.DrawString("12", new Font("Arial", 12), Brushes.Black, new PointF(285, 9));
            g.DrawString("3", new Font("Arial", 12), Brushes.Black, new PointF(577, 285));
            g.DrawString("6", new Font("Arial", 12), Brushes.Black, new PointF(289, 569));
            g.DrawString("9", new Font("Arial", 12), Brushes.Black, new PointF(5, 285));
            //second hand
            handCoord = msCoord(ss, secHAND);
            g.DrawLine(new Pen(Color.Red, 1f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            //minute hand
            handCoord = msCoord(mm, minHAND);
            g.DrawLine(new Pen(Color.Black, 2f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            //hour hand
            handCoord = hrCoord(hh % 12, mm, hrHAND);
            g.DrawLine(new Pen(Color.Gray, 3f), new Point(cx, cy), new Point(handCoord[0], handCoord[1]));

            //load bmp in picturebox1
            pictureBox1.Image = bmp;

            //disp time
            this.Text = "Exam Clock -  Exam:"+ Properties.Settings.Default.ExamName+" "+ SWRemainingTime.ToString(@"hh\:mm\:ss") + " Remaining";

            //dispose
            g.Dispose();
        }

        //coord for minute and second hand
        private int[] msCoord(int val, int hlen)
        {
            int[] coord = new int[2];
            val *= 6;   //each minute and second make 6 degree

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        //coord for hour hand
        private int[] hrCoord(int hval, int mval, int hlen)
        {
            int[] coord = new int[2];

            //each hour makes 30 degree
            //each min makes 0.5 degree
            int val = (int)((hval * 30) + (mval * 0.5));

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }
    }
    
}
