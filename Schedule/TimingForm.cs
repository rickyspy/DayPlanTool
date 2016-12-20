using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Schedule
{
    public partial class TimingForm : Form
    {
        public TimingForm()
        {
            InitializeComponent();
        }

        public TimingForm(int width, int height, Color c, double o)
            : this()
        {
            this.BackColor = c;
            this.Width = width;
            this.Height = height;
            this.Opacity = o;
        }


        DateTime ObjTime;
        TimeSpan TSpan;
        string[] ss; // 表示时间 
        bool isTopmost = false;
        bool istimingPause = false;


        private void Alarm_Load(object sender, EventArgs e)
        {
            ss = Form1.ObjTask2.ObjectTime.Split(new string[] { ":" }, StringSplitOptions.None);
            ObjTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                int.Parse(ss[0]), int.Parse(ss[1]), int.Parse(ss[2]), DateTimeKind.Local);
            timer1.Start();
            label2.Text = Form1.ObjTask2.Name;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isTopmost)
            {
                this.TopMost = false;
                this.BringToFront();
                this.TopMost = true;
            }

            if (istimingPause)
                ObjTime = ObjTime.AddSeconds(1);

            TSpan = ObjTime - DateTime.Now;
            label1.Text = (TSpan.Hours * 60 + TSpan.Minutes).ToString() + ":" + (TSpan.Seconds.ToString("D2"));
            if (TSpan.Hours + TSpan.Minutes + TSpan.Seconds == 0)
            {
                timer1.Stop();
                MusicPlay();
            }

        }

        public static uint SND_ASYNC = 0x0001;
        public static uint SND_FILENAME = 0x00020000;
        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand,
        string lpstrReturnString, uint uReturnLength, uint hWndCallback);
        public void MusicPlay()
        {

            string selectmusic = "Music\\" + Form1.ObjTask2.Music;
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString( string.Format(@"open ""{0}"" alias temp_alias ",selectmusic), null, 0, 0);
     
            mciSendString("play temp_alias repeat", null, 0, 0);

        }

        public void MusicStop()
        {
            mciSendString(@"close temp_alias", null, 0, 0);
        }


        //public static class WavPlayer
        //{
        //    [DllImport("winmm.dll", SetLastError = true)]
        //    static extern bool PlaySound(string pszSound, UIntPtr hmod, uint fdwSound);
        //    [DllImport("winmm.dll", SetLastError = true)]
        //    static extern long mciSendString(string strCommand,
        //        StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        //    [DllImport("winmm.dll")]
        //    private static extern long sndPlaySound(string lpszSoundName, long uFlags);

        //    [Flags]
        //    public enum SoundFlags
        //    {
        //        /// <summary>play synchronously (default)</summary>
        //        SND_SYNC = 0x0000,
        //        /// <summary>play asynchronously</summary>
        //        SND_ASYNC = 0x0001,
        //        /// <summary>silence (!default) if sound not found</summary>
        //        SND_NODEFAULT = 0x0002,
        //        /// <summary>pszSound points to a memory file</summary>
        //        SND_MEMORY = 0x0004,
        //        /// <summary>loop the sound until next sndPlaySound</summary>
        //        SND_LOOP = 0x0008,
        //        /// <summary>don’t stop any currently playing sound</summary>
        //        SND_NOSTOP = 0x0010,
        //        /// <summary>Stop Playing Wave</summary>
        //        SND_PURGE = 0x40,
        //        /// <summary>don’t wait if the driver is busy</summary>
        //        SND_NOWAIT = 0x00002000,
        //        /// <summary>name is a registry alias</summary>
        //        SND_ALIAS = 0x00010000,
        //        /// <summary>alias is a predefined id</summary>
        //        SND_ALIAS_ID = 0x00110000,
        //        /// <summary>name is file name</summary>
        //        SND_FILENAME = 0x00020000,
        //        /// <summary>name is resource name or atom</summary>
        //        SND_RESOURCE = 0x00040004
        //    }

        //    public static void Play(string strFileName)
        //    {
        //        PlaySound(strFileName, UIntPtr.Zero,
        //           (uint)(SoundFlags.SND_FILENAME | SoundFlags.SND_SYNC | SoundFlags.SND_NOSTOP));
        //    }
        //    public static void mciPlay(string strFileName)
        //    {
        //        string playCommand = "open " + strFileName + " type WAVEAudio alias MyWav";
        //        mciSendString(playCommand, null, 0, IntPtr.Zero);
        //        mciSendString("play MyWav", null, 0, IntPtr.Zero);

        //    }
        //    public static void sndPlay(string strFileName)
        //    {
        //        sndPlaySound(strFileName, (long)SoundFlags.SND_SYNC);
        //    }
        //}






        #region 窗体移动
        private bool formMove = false;//窗体是否移动
        private Point formPoint;//记录窗体的位置

        private void Alarm_MouseDown(object sender, MouseEventArgs e)
        {
            formPoint = new Point();
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {

                xOffset = -e.X;// -SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y;// -SystemInformation.FrameBorderSize.Height; ;
                formPoint = new Point(xOffset, yOffset);
                formMove = true;//开始移动
            }
        }
        private void Alarm_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMove == true)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(formPoint.X, formPoint.Y);
                Location = mousePos;

            }
        }
        private void Alarm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//按下的是鼠标左键
            {
                formMove = false;//停止移动
            }

        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            formPoint = new Point();
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {

                xOffset = -e.X;// -SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y;// -SystemInformation.FrameBorderSize.Height; ;
                formPoint = new Point(xOffset, yOffset);
                formMove = true;//开始移动
            }
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMove == true)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(formPoint.X, formPoint.Y);
                Location = mousePos;

            }
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//按下的是鼠标左键
            {
                formMove = false;//停止移动
            }

        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            formPoint = new Point();
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {

                xOffset = -e.X;// -SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y;// -SystemInformation.FrameBorderSize.Height; ;
                formPoint = new Point(xOffset, yOffset);
                formMove = true;//开始移动
            }
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMove == true)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(formPoint.X, formPoint.Y - label1.Location.Y);

                Location = mousePos;

            }
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//按下的是鼠标左键
            {
                formMove = false;//停止移动
            }
        }


        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Alarm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = null;
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
        }


        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusicStop();
            this.Dispose();
           // this.Owner.Show();
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = null;
                contextMenuStrip1.Show(this.label1, new Point(e.X, e.Y));
            }
        }

        private void topmostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isTopmost)
            {
                isTopmost = false;
                this.TopMost = false;
            }
            else
                isTopmost = true;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (istimingPause)
                istimingPause = false;
            else
                istimingPause = true;
        }

        private void label2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = null;
                contextMenuStrip1.Show(this.label2, new Point(e.X, e.Y));
            }

        }







    }
}
