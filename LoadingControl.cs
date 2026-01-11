using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public partial class LoadingControl : UserControl
    {   /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
        GraphicsPath[] Frames { get; set; } = new GraphicsPath[20];

        int index = 0;
        float angle;
        System.Windows.Forms.Timer Timer { get; set; } = new System.Windows.Forms.Timer();
        public LoadingControl(int width, int height)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Height = height;
            this.Width = width;

            Point pointMiddle = new Point(Width / 2, Height / 2);
            LoadFrames(pointMiddle);
            Timer.Tick += TimerTick;
            Timer.Interval = 20;
            Timer.Start();


        }



        void LoadFrames(Point center)
        {
            int count = Frames.Length;
            float radius = 80f;

            for (int i = 0; i < count; i++)
            {
                float a = angle + (float)(2 * Math.PI * i / count);

                float x = center.X + (float)Math.Cos(a) * radius;
                float y = center.Y + (float)Math.Sin(a) * radius;

                var path = new GraphicsPath();
                path.AddEllipse(x - 8, y - 8, 36, 16);

                Frames[i]?.Dispose();
                Frames[i] = path;
            }
        }
        public void TimerTick(object sender, EventArgs e)
        {

            angle += 0.08f;   // velocità rotazione
            LoadFrames(new Point(Width / 2, Height / 2));
            Invalidate();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var brush = new SolidBrush(Color.DodgerBlue);

            foreach (var p in Frames)
                if (p != null)
                    e.Graphics.FillPath(brush, p);


        }
    }
}
