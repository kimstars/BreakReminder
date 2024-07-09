using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreakReminder
{
    public partial class Form1 : Form
    {
        private Timer mainTimer;
        private Timer displayTimer;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem exitMenuItem;
        private DateTime startTime;
        private int TimeSetup = 30;
        private TimeSpan interval = TimeSpan.FromMinutes(30);
        public Form1()
        {
            InitializeComponent();
            InitializeTimers();
            InitializeNotifyIcon();
        }

        private void InitializeTimers()
        {
            
            mainTimer = new Timer();
            mainTimer.Interval = (int)interval.TotalMilliseconds;
            mainTimer.Tick += MainTimer_Tick;
            mainTimer.Start();

            displayTimer = new Timer();
            displayTimer.Interval = 1000; // 1 giây
            displayTimer.Tick += DisplayTimer_Tick;
            displayTimer.Start();

            startTime = DateTime.Now;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            mainTimer.Stop(); // Dừng timer
            ShowReminder();
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            var elapsedTime = DateTime.Now - startTime;

            timeremider.Text = $"{elapsedTime:mm\\:ss}/{interval:mm\\:ss}";
        }

        private void ShowReminder()
        {
            this.Show();
            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            notifyIcon.ShowBalloonTip(500,"Nhắc nhở!!", $"Bạn đã sử dụng máy tính {TimeSetup} phút. Hãy nghỉ ngơi! Vận động cơ thể nhé", ToolTipIcon.Warning);
            var result = MessageBox.Show($"Bạn đã sử dụng máy tính {TimeSetup} phút. Hãy nghỉ ngơi! Vận động cơ thể nhé", "Nhắc nhở", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {
                var confirmResult = MessageBox.Show("Bạn đã vận động xong chưa và có thể tiếp tục làm việc không?", "Xác nhận", MessageBoxButtons.OK);
                if (confirmResult == DialogResult.OK)
                {
                    startTime = DateTime.Now; // Reset thời gian bắt đầu
                    mainTimer.Start(); // Reset và bắt đầu lại timer
                }
            }
        }

        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            contextMenuStrip = new ContextMenuStrip();
            exitMenuItem = new ToolStripMenuItem("Thoát", null, ExitMenuItem_Click);
            contextMenuStrip.Items.Add(exitMenuItem);

            notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }

  
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.ShowInTaskbar = false;
            base.OnFormClosing(e);
        }

        private void btnSettime_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTime.Text, out TimeSetup))
            {
                MessageBox.Show("Vui lòng chỉ nhập số phút");
                return;
            }

            interval = TimeSpan.FromMinutes(TimeSetup);
            mainTimer.Interval = (int)interval.TotalMilliseconds;

            startTime = DateTime.Now; // Reset thời gian bắt đầu
            mainTimer.Start(); // Reset và bắt đầu lại timer
        }
    }
}
