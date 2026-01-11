using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public partial class MobilePanel : UserControl
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Dragged { get; set; }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private string title;
        protected Size storedSize = Size.Empty;
        private bool isMinimized = false;

        // header controls
        private InvisibleButton btnMinimize;
        public Label Title;
        private Panel contentPanel;
        GraphicsPath pathFill;

        public MobilePanel()
        {
            InitializeComponent();

            // evento drag sul titolo
            Title.MouseMove += TableView_MouseMove;
            contentPanel.MouseMove += TableView_MouseMove;

            // click del pulsante
            btnMinimize.Click += Minimize;

            // inizializzo storedSize se già ho una size valida
            if (this.Size != Size.Empty && this.Size != MinimumSize)
                storedSize = this.Size;
            btnMinimize.BringToFront();
        }

        public void InitializeComponent()
        {
            // Header label
            Title = new Label
            {
                AutoSize = false,
                Text = "MobilePanel",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.LightGray,
                BackColor = Color.Transparent,
                Location = new Point(10, 6),
                Size = new Size(200, 24)
            };
            this.BackColor = Color.DarkGray;
            // Minimize button
            btnMinimize = new InvisibleButton(this, IconAssets.UpArrow, IconAssets.DownArrow)
            {
                Size = new Size(28, 28),
                Location = new Point(Width - 38, 6), // verrà aggiornato in OnResize

                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // contentPanel (tieni qui i controlli "interni")
            contentPanel = new Panel
            {
                Location = new Point(0, 36),
                Size = new Size(Width, Math.Max(0, Height - 36)),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.Transparent
            };

            // default sizes
            this.MinimumSize = new Size(120, 50);
            this.Size = new Size(360, 240);

            // aggiungo ai controlli
            this.Controls.Add(Title);
            this.Controls.Add(btnMinimize);
            this.Controls.Add(contentPanel);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TitleString
        {
            get => title ?? "MobilePanel";
            set
            {
                title = value;
                if (Title != null) Title.Text = value;
                Invalidate();
            }
        }

        public int Radius => (this.Size != MinimumSize) ? 24 : 16;

        public Color ColorMinimizeBtn => isMinimized ? Color.DarkGreen : Color.DarkRed;
        public Color ColorMinimizeBtnBrd => isMinimized ? Color.LightGreen : Color.Coral;

        protected virtual void TableView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Parent?.Invalidate();
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                this.Parent?.Invalidate();
                this.BringToFront();
            }
            else
            {
                Dragged = false;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // aggiorna posizione pulsante e contentPanel
            btnMinimize.Location = new Point(this.ClientSize.Width - btnMinimize.Width - 8, 6);
            contentPanel.Location = new Point(0, Title.Bottom + 6);
            contentPanel.Size = new Size(this.ClientSize.Width, Math.Max(0, this.ClientSize.Height - contentPanel.Location.Y));
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            // aggiorno i colori del pulsante minimize (RoundGradientButton legge queste proprietà)


            Color backgroundColor = Color.DarkGray;
            GraphicsPath path = new GraphicsPath();
            int borderRadius = Radius;
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            path.AddArc(rect.X, rect.Y, borderRadius, borderRadius, 180, 90);
            path.AddArc(rect.Right - borderRadius, rect.Y, borderRadius, borderRadius, 270, 90);
            path.AddArc(rect.Right - borderRadius, rect.Bottom - borderRadius, borderRadius, borderRadius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - borderRadius, borderRadius, borderRadius, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);

            using (Brush brush = new SolidBrush(backgroundColor))
            {
                pevent.Graphics.FillPath(brush, path);
            }
            GraphicsPath path2 = new GraphicsPath();
            Rectangle rect2 = new Rectangle(3, 3, this.Width - 6, this.Height - 6);
            path2.AddArc(rect2.X, rect2.Y, borderRadius, borderRadius, 180, 90);
            path2.AddArc(rect2.Right - borderRadius, rect2.Y, borderRadius, borderRadius, 270, 90);
            path2.AddArc(rect2.Right - borderRadius, rect2.Bottom - borderRadius, borderRadius, borderRadius, 0, 90);
            path2.AddArc(rect2.X, rect2.Bottom - borderRadius, borderRadius, borderRadius, 90, 90);
            path2.CloseFigure();
            using (Pen blackPen = new Pen(Color.DarkSlateGray, 3))
            {
                pevent.Graphics.DrawPath(blackPen, path2);
            }
        }

        // Minimizza solo il contentPanel e salva/ripristina l'altezza completa
        protected virtual void Minimize(object sender, EventArgs e)
        {

            if (!isMinimized)
            {
                // salvo la size completa se non salvata o se è uguale a Minimum (fallback)
                if (storedSize == Size.Empty || storedSize == this.MinimumSize)
                {
                    storedSize = this.Size;
                    if (storedSize == this.MinimumSize)
                    {
                        // fallback ad una size sensata
                        storedSize = new Size(this.Width, Math.Max(this.Height, 200));
                    }
                }


                int headerHeight = Title.Bottom + 6;
                this.Height = headerHeight + 4; // qualche pixel di margine
                isMinimized = true;
            }
            else
            {

                contentPanel.Visible = true;
                if (storedSize != Size.Empty)
                {
                    this.Size = storedSize;
                }
                isMinimized = false;
            }

            // forza layout e ridisegno
            this.PerformLayout();
            this.Invalidate();



        }

        // Esporre il Panel di contenuto così puoi aggiungere controlli dall'esterno
        public Control ContentPanel => contentPanel;
    }

}
