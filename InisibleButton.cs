using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public class InvisibleButton : Button
    {
        Color backColor;

        string State0 { get; set; }
        string State1 { get; set; }

        bool State { get; set; } = false;
        string ImageText { get { if (State) { return State1; } else { return State0; } } }

        public InvisibleButton(Control Parent, string s0, string s1)
        {

            this.backColor = Parent.BackColor;
            this.BackColor = Parent.BackColor;
            State0 = s0;
            State1 = s1;

            this.Font = new Font("Calibri", 15, FontStyle.Bold);
            this.Region = new Region(new Rectangle(5, 5, Width - 5, Height - 5));
            this.MouseEnter += MouseEnterev;
            this.MouseLeave += MouseLeaveev;
            this.Paint += PaintEvent;
            this.Click += ClickEvent;


        }
        public void PaintEvent(object sender, PaintEventArgs p)
        {
            Graphics g = p.Graphics;

            Image i = ImageBase64Converter.Base64ToImage(ImageText);
            int x = this.Width / 2 - i.Width / 2;

            using (SolidBrush brush = new SolidBrush(this.BackColor)) { g.FillRectangle(brush, new Rectangle(0, 0, this.Width, this.Height)); }
            using (SolidBrush brush = new SolidBrush(Color.White)) { g.DrawImage(i, new Point(x, 0)); }






        }

        public void ClickEvent(object sender, EventArgs e)
        {
            if (State) { State = false; }
            else { State = true; }
            Invalidate();
        }
        public void MouseEnterev(object sender, EventArgs e) { this.BackColor = Color.LightGray; }
        public void MouseLeaveev(object sender, EventArgs e) { this.BackColor = backColor; }




    }

    public class RoundGradientButton : Button
    {
        private Image _buttonImage;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ButtonColor { get; set; } = SystemColors.ControlDark;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ButtonBorderColor { get; set; } = SystemColors.Control;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image ButtonImage { get => _buttonImage; set { _buttonImage = value; Invalidate(); } }

        private bool isPressed = false;
        private bool isHovering = false;

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); isHovering = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); isHovering = false; Invalidate(); }
        protected override void OnMouseDown(MouseEventArgs mevent) { base.OnMouseDown(mevent); isPressed = true; Invalidate(); }
        protected override void OnMouseUp(MouseEventArgs mevent) { base.OnMouseUp(mevent); isPressed = false; Invalidate(); }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            base.OnPaint(pevent);

            Color borderColor = ButtonBorderColor;
            Color centerColor = ButtonColor;
            if (isHovering)
            {
                borderColor = isPressed ? ButtonBorderColor : ButtonColor;
                centerColor = isPressed ? ButtonColor : ButtonBorderColor;
            }

            int diameter = Math.Min(this.Width, this.Height);
            Rectangle rect = new Rectangle(0, 0, diameter, diameter);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(rect);
            this.Region = new Region(path);

            using (PathGradientBrush gradientBrush = new PathGradientBrush(path))
            {
                gradientBrush.CenterColor = centerColor;
                gradientBrush.SurroundColors = new Color[] { borderColor };
                pevent.Graphics.FillEllipse(gradientBrush, rect);
            }

            using (Pen pen = new Pen(Color.Black, 1))
            {
                pevent.Graphics.DrawEllipse(pen, 0, 0, diameter - 1, diameter - 1);
            }

            if (_buttonImage != null)
            {
                int imageSize = (int)(diameter * 0.6);
                Rectangle imageRect = new Rectangle((this.Width - imageSize) / 2, (this.Height - imageSize) / 2, imageSize, imageSize);
                pevent.Graphics.DrawImage(_buttonImage, imageRect);
            }

            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, rect, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
