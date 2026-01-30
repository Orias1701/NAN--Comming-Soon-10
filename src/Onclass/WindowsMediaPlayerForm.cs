using System;
using System.Drawing;
using System.Windows.Forms;
using AxWMPLib;

namespace WindowsAss.src.Onclass
{
    public static class WindowsMediaRunner
    {
        public static void Run()
        {
            try
            {
                Application.Run(new WindowsMediaPlayerForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi Windows Media Player");
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    public class WindowsMediaPlayerForm : Form
    {
        private AxWindowsMediaPlayer? _mediaPlayer;
        private MenuStrip _menuStrip = null!;
        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _statusLabel = null!;
        private Panel _controlPanel = null!;
        private Button _btnOpen = null!, _btnBack5 = null!, _btnPlayPause = null!, _btnForward5 = null!;
        private TrackBar _trackBar = null!;
        private Label _lblTime = null!;
        private System.Windows.Forms.Timer _clockTimer = null!;
        private System.Windows.Forms.Timer _titleScrollTimer = null!;
        private System.Windows.Forms.Timer _progressTimer = null!;
        private bool _userSeeking;
        private const string TitleText = "    CHÀO MỪNG CÁC BẠN ĐẾN VỚI CHƯƠNG TRÌNH WINDOWS MEDIA    ";
        private string _currentTitle = TitleText;

        private const string MediaFilter = "Media files (MP4, MPEG, AVI, WAV, MIDI)|*.mp4;*.m4v;*.mpeg;*.mpg;*.avi;*.wav;*.mid;*.midi|MP4 (*.mp4;*.m4v)|*.mp4;*.m4v|MPEG (*.mpeg;*.mpg)|*.mpeg;*.mpg|AVI (*.avi)|*.avi|WAV (*.wav)|*.wav|MIDI (*.mid;*.midi)|*.mid;*.midi|All files (*.*)|*.*";

        public WindowsMediaPlayerForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _currentTitle = TitleText;
            this.Text = _currentTitle.Trim();
            this.Size = new Size(900, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(700, 450);
            this.KeyPreview = true;
            this.BackColor = Color.FromArgb(30, 30, 30);

            _menuStrip = new MenuStrip();
            _menuStrip.BackColor = Color.FromArgb(45, 45, 48);
            _menuStrip.ForeColor = Color.White;
            var menuFile = new ToolStripMenuItem("&File");
            var openItem = new ToolStripMenuItem("&Open...", null, MenuOpen_Click)
            {
                ShortcutKeys = Keys.Control | Keys.O,
                ShowShortcutKeys = true
            };
            var exitItem = new ToolStripMenuItem("E&xit", null, MenuExit_Click)
            {
                ShortcutKeys = Keys.Alt | Keys.F4,
                ShowShortcutKeys = true
            };
            menuFile.DropDownItems.Add(openItem);
            menuFile.DropDownItems.Add(new ToolStripSeparator());
            menuFile.DropDownItems.Add(exitItem);
            _menuStrip.Items.Add(menuFile);
            this.MainMenuStrip = _menuStrip;
            this.Controls.Add(_menuStrip);

            _controlPanel = new Panel
            {
                Height = 90,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(45, 45, 48),
                Padding = new Padding(8, 6, 8, 6)
            };
            var progressPanel = new Panel { Dock = DockStyle.Top, Height = 28, Padding = new Padding(0, 0, 0, 4) };
            _trackBar = new TrackBar
            {
                Dock = DockStyle.Fill,
                Minimum = 0,
                Maximum = 1000,
                Value = 0,
                TickFrequency = 100,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.FromArgb(200, 200, 200)
            };
            _trackBar.MouseDown += TrackBar_MouseDown;
            _trackBar.MouseUp += TrackBar_MouseUp;
            _lblTime = new Label
            {
                AutoSize = true,
                Text = "0:00 / 0:00",
                ForeColor = Color.FromArgb(200, 200, 200),
                Dock = DockStyle.Right
            };
            progressPanel.Controls.Add(_trackBar);
            progressPanel.Controls.Add(_lblTime);
            var flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0)
            };
            _btnOpen = new Button
            {
                Text = "Mở file (Ctrl+O)",
                Size = new Size(120, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            _btnOpen.FlatAppearance.BorderSize = 0;
            _btnOpen.Click += MenuOpen_Click;

            _btnBack5 = new Button
            {
                Text = "◀ 5s",
                Size = new Size(56, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 63),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            _btnBack5.FlatAppearance.BorderSize = 0;
            _btnBack5.Click += BtnBack5_Click;

            _btnPlayPause = new Button
            {
                Text = "▶",
                Size = new Size(56, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 63),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            _btnPlayPause.FlatAppearance.BorderSize = 0;
            _btnPlayPause.Click += BtnPlayPause_Click;

            _btnForward5 = new Button
            {
                Text = "5s ▶",
                Size = new Size(56, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 63),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            _btnForward5.FlatAppearance.BorderSize = 0;
            _btnForward5.Click += BtnForward5_Click;

            flow.Controls.Add(_btnOpen);
            flow.Controls.Add(new Panel { Width = 12 });
            flow.Controls.Add(_btnBack5);
            flow.Controls.Add(_btnPlayPause);
            flow.Controls.Add(_btnForward5);
            _controlPanel.Controls.Add(flow);
            _controlPanel.Controls.Add(progressPanel);
            progressPanel.BringToFront();
            this.Controls.Add(_controlPanel);

            _statusStrip = new StatusStrip();
            _statusStrip.BackColor = Color.FromArgb(37, 37, 38);
            _statusStrip.ForeColor = Color.FromArgb(200, 200, 200);
            _statusStrip.Dock = DockStyle.Bottom;
            _statusLabel = new ToolStripStatusLabel
            {
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _statusStrip.Items.Add(_statusLabel);
            this.Controls.Add(_statusStrip);

            _mediaPlayer = new AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)_mediaPlayer).BeginInit();
            _mediaPlayer.Dock = DockStyle.Fill;
            _mediaPlayer.Enabled = true;
            _mediaPlayer.Name = "axWindowsMediaPlayer1";
            _mediaPlayer.PlayStateChange += MediaPlayer_PlayStateChange;
            ((System.ComponentModel.ISupportInitialize)_mediaPlayer).EndInit();
            this.Controls.Add(_mediaPlayer);

            _clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();
            UpdateStatusClock();

            _titleScrollTimer = new System.Windows.Forms.Timer { Interval = 200 };
            _titleScrollTimer.Tick += TitleScrollTimer_Tick;
            _titleScrollTimer.Start();

            _progressTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _progressTimer.Tick += ProgressTimer_Tick;
            _progressTimer.Start();

            this.KeyDown += Form_KeyDown;
            this.Load += Form_Load;
        }

        private static string FormatTime(double seconds)
        {
            if (double.IsNaN(seconds) || seconds < 0) return "0:00";
            int m = (int)(seconds / 60);
            int s = (int)(seconds % 60);
            return $"{m}:{s:D2}";
        }

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            if (_userSeeking || _mediaPlayer?.currentMedia == null) return;
            try
            {
                double cur = _mediaPlayer.Ctlcontrols.currentPosition;
                double dur = _mediaPlayer.currentMedia.duration;
                if (dur > 0 && !double.IsNaN(cur))
                {
                    _trackBar.Maximum = 1000;
                    _trackBar.Value = Math.Min(1000, Math.Max(0, (int)((cur / dur) * 1000)));
                    _lblTime.Text = $"{FormatTime(cur)} / {FormatTime(dur)}";
                }
                else
                {
                    _trackBar.Value = 0;
                    _lblTime.Text = "0:00 / 0:00";
                }
            }
            catch
            {
                _lblTime.Text = "0:00 / 0:00";
            }
        }

        private void TrackBar_MouseDown(object? sender, MouseEventArgs e)
        {
            _userSeeking = true;
            if (e.Button != MouseButtons.Left) return;
            int w = _trackBar.ClientSize.Width;
            if (w <= 0) return;
            int value = (int)((e.X / (double)w) * (_trackBar.Maximum - _trackBar.Minimum) + _trackBar.Minimum);
            value = Math.Clamp(value, _trackBar.Minimum, _trackBar.Maximum);
            _trackBar.Value = value;
            ApplySeekFromTrackBar();
        }

        private void TrackBar_MouseUp(object? sender, MouseEventArgs e)
        {
            ApplySeekFromTrackBar();
            _userSeeking = false;
        }

        private void ApplySeekFromTrackBar()
        {
            if (_mediaPlayer?.currentMedia == null) return;
            try
            {
                double dur = _mediaPlayer.currentMedia.duration;
                if (dur > 0)
                {
                    double pos = (_trackBar.Value / 1000.0) * dur;
                    _mediaPlayer.Ctlcontrols.currentPosition = pos;
                    _lblTime.Text = $"{FormatTime(pos)} / {FormatTime(dur)}";
                }
            }
            catch { /* ignore */ }
        }

        private void Form_Load(object? sender, EventArgs e)
        {
            if (_mediaPlayer == null) return;
            BeginInvoke(new Action(() =>
            {
                try
                {
                    _mediaPlayer.uiMode = "full";
                }
                catch { /* ignore */ }
            }));
        }

        private void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            if (_mediaPlayer == null) return;
            if (e.Control && e.KeyCode == Keys.O)
            {
                MenuOpen_Click(sender, e);
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.Space)
            {
                BtnPlayPause_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode == Keys.Left)
            {
                SeekRelative(-5);
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode == Keys.Right)
            {
                SeekRelative(5);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SeekRelative(int seconds)
        {
            if (_mediaPlayer?.currentMedia == null) return;
            try
            {
                double pos = _mediaPlayer.Ctlcontrols.currentPosition;
                double dur = _mediaPlayer.currentMedia.duration;
                double newPos = Math.Max(0, Math.Min(dur, pos + seconds));
                _mediaPlayer.Ctlcontrols.currentPosition = newPos;
            }
            catch { /* ignore */ }
        }

        private void BtnBack5_Click(object? sender, EventArgs e) => SeekRelative(-5);
        private void BtnForward5_Click(object? sender, EventArgs e) => SeekRelative(5);

        private void MediaPlayer_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (_btnPlayPause.IsDisposed) return;
            try
            {
                var state = (WMPLib.WMPPlayState)e.newState;
                _btnPlayPause.BeginInvoke(() =>
                {
                    if (state == WMPLib.WMPPlayState.wmppsMediaEnded)
                        KeepVideoOnScreen();
                    _btnPlayPause.Text = state == WMPLib.WMPPlayState.wmppsPlaying ? "❚❚" : "▶";
                });
            }
            catch { /* ignore */ }
        }

        private void KeepVideoOnScreen()
        {
            if (_mediaPlayer?.currentMedia == null) return;
            try
            {
                _mediaPlayer.Ctlcontrols.pause();
                double duration = _mediaPlayer.currentMedia.duration;
                if (duration > 0)
                    _mediaPlayer.Ctlcontrols.currentPosition = Math.Max(0, duration - 0.05);
            }
            catch { /* ignore */ }
        }

        private void BtnPlayPause_Click(object? sender, EventArgs e)
        {
            if (_mediaPlayer == null) return;
            try
            {
                if (_mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    _mediaPlayer.Ctlcontrols.pause();
                    _btnPlayPause.Text = "▶";
                }
                else
                {
                    _mediaPlayer.Ctlcontrols.play();
                    _btnPlayPause.Text = "❚❚";
                }
            }
            catch
            {
                _mediaPlayer.Ctlcontrols.play();
                _btnPlayPause.Text = "❚❚";
            }
        }

        private void MenuOpen_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Filter = MediaFilter,
                FilterIndex = 1
            };
            if (dlg.ShowDialog() != DialogResult.OK || _mediaPlayer == null) return;
            _mediaPlayer.URL = dlg.FileName;
            _mediaPlayer.Ctlcontrols.play();
            _btnPlayPause.Text = "❚❚";
        }

        private void MenuExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            UpdateStatusClock();
        }

        private void UpdateStatusClock()
        {
            var now = DateTime.Now;
            _statusLabel.Text = $"Hôm nay là ngày {now:dd/MM/yyyy} - Bây giờ là {now:HH:mm:ss}";
        }

        private void TitleScrollTimer_Tick(object? sender, EventArgs e)
        {
            if (_currentTitle.Length == 0) return;
            _currentTitle = _currentTitle.Substring(1) + _currentTitle[0];
            this.Text = _currentTitle;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _clockTimer?.Stop();
            _clockTimer?.Dispose();
            _clockTimer = null!;
            _titleScrollTimer?.Stop();
            _titleScrollTimer?.Dispose();
            _titleScrollTimer = null!;
            _progressTimer?.Stop();
            _progressTimer?.Dispose();
            _progressTimer = null!;
            if (_mediaPlayer != null)
            {
                try
                {
                    _mediaPlayer.PlayStateChange -= MediaPlayer_PlayStateChange;
                    _mediaPlayer.Ctlcontrols.stop();
                    _mediaPlayer.URL = "";
                }
                catch { /* ignore */ }
                _mediaPlayer = null;
            }
            base.OnFormClosing(e);
        }
    }
}
