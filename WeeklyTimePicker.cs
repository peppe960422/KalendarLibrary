using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalendarLibrary
{
    public partial class WeeklyTimePicker : UserControl
    {
        string[] weekDays = { "Mo.", "Di.", "Mi.", "Do.", "Fr.", "Sa.", "So." };
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private Label label1;
        private Label label2;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckBox[] checkBoxes { get; set; }
        AngularLabel[] angularLabels = new AngularLabel[7];
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime StartTime { get; set; }
        public DateTime GetStart(int i)
        {
            DateTime date = StartTime.AddDays(i);
            int Minutes = dateTimePicker1.Value.Hour * 60 + dateTimePicker1.Value.Minute;

            date = date.AddMinutes(Minutes);

            return date;

        }

        public DateTime GetEnd(int i)
        {
            DateTime date = StartTime.AddDays(i);
            int Minutes = dateTimePicker2.Value.Hour * 60 + dateTimePicker2.Value.Minute;

            date = date.AddMinutes(Minutes);

            return date;

        }

        public WeeklyTimePicker()
        {
            InitializeComponent();
            checkBoxes = new CheckBox[7];
        }

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
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CustomFormat = "HH:mm";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(77, 131);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Size = new Size(92, 27);
            dateTimePicker1.TabIndex = 14;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.CustomFormat = "HH:mm";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.Location = new Point(255, 131);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.ShowUpDown = true;
            dateTimePicker2.Size = new Size(92, 27);
            dateTimePicker2.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 131);
            label1.Name = "label1";
            label1.Size = new Size(40, 20);
            label1.Font = new Font("Calibri ", 10, FontStyle.Bold);
            label1.TabIndex = 16;
            label1.Text = "von: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(218, 131);
            label2.Name = "label2";
            label2.Size = new Size(31, 20);
            label2.TabIndex = 17;
            label2.Text = "bis:";
            label2.Font = new Font("Calibri ", 10, FontStyle.Bold);
            // 
            // WeeklyTimePicker
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dateTimePicker2);
            Controls.Add(dateTimePicker1);
            Name = "WeeklyTimePicker";
            Size = new Size(393, 206);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
       
    
        public void SetDays(int days)
        {
            if (days < 4 || days > 7) { throw new ArgumentOutOfRangeException("days", "Value must be between 4 and 7"); }
            int y = 50;
            int x = 50;
            for (int i = 0; i < days; i++)
            {
                CheckBox cb = new CheckBox { Size = new Size(40, 40), Location = new Point(x + 5, y + 40) };
                AngularLabel lb = new AngularLabel { Location = new Point(x, y), NewText = weekDays[i], Size = new Size(40, 40), RotateAngle = 45, Font = new Font("Calibri ", 10, FontStyle.Bold) };
                this.Controls.Add(cb);
                this.Controls.Add(lb);
                checkBoxes[i] = cb;
                angularLabels[i] = lb;
                x += 50;
            }
        }




    }
    internal class AngularLabel : System.Windows.Forms.Label
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RotateAngle { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string NewText { get; set; }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Func<double, double> DegToRad = (angle) => Math.PI * angle / 180.0;

            Brush b = new SolidBrush(this.ForeColor);
            SizeF size = e.Graphics.MeasureString(this.NewText, this.Font, this.Parent.Width);

            int normalAngle = ((RotateAngle % 360) + 360) % 360;
            double normaleRads = DegToRad(normalAngle);

            int hSinTheta = (int)Math.Ceiling((size.Height * Math.Sin(normaleRads)));
            int wCosTheta = (int)Math.Ceiling((size.Width * Math.Cos(normaleRads)));
            int wSinTheta = (int)Math.Ceiling((size.Width * Math.Sin(normaleRads)));
            int hCosTheta = (int)Math.Ceiling((size.Height * Math.Cos(normaleRads)));

            int rotatedWidth = Math.Abs(hSinTheta) + Math.Abs(wCosTheta);
            int rotatedHeight = Math.Abs(wSinTheta) + Math.Abs(hCosTheta);

            this.Width = rotatedWidth;
            this.Height = rotatedHeight;

            int numQuadrants =
                (normalAngle >= 0 && normalAngle < 90) ? 1 :
                (normalAngle >= 90 && normalAngle < 180) ? 2 :
                (normalAngle >= 180 && normalAngle < 270) ? 3 :
                (normalAngle >= 270 && normalAngle < 360) ? 4 :
                0;

            int horizShift = 0;
            int vertShift = 0;

            if (numQuadrants == 1)
            {
                horizShift = Math.Abs(hSinTheta);
            }
            else if (numQuadrants == 2)
            {
                horizShift = rotatedWidth;
                vertShift = Math.Abs(hCosTheta);
            }
            else if (numQuadrants == 3)
            {
                horizShift = Math.Abs(wCosTheta);
                vertShift = rotatedHeight;
            }
            else if (numQuadrants == 4)
            {
                vertShift = Math.Abs(wSinTheta);
            }

            e.Graphics.TranslateTransform(horizShift, vertShift);
            e.Graphics.RotateTransform(this.RotateAngle);

            e.Graphics.DrawString(this.NewText, this.Font, b, 0f, 0f);
            base.OnPaint(e);
        }
    }



}



