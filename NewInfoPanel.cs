using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public partial class TimePickerUpDown : UserControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
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
            SuspendLayout();
            // 
            // TimePickerUpDown
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "TimePickerUpDown";
            Size = new Size(41, 22);
            ResumeLayout(false);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime DateTime { get; set; }

        int sensibility { get { NewWeekView n = (NewWeekView)Parent.Parent; return (int)n.controlBar.comboBoxEmpfindlichkei.SelectedValue; } }
        public string DateString { get { return DateTime.ToString("HH:mm"); } }

        public Button btnUp;
        public Button btnDown;
        public TimePickerUpDown(ITermin t, bool b = true)
        {
            if (b) { DateTime = t.Start; }

            else { DateTime = t.End; }

            InitializeComponent();
            this.Paint += PaintString;



            this.HandleCreated += InitButton;

        }

        void InitButton(object sender, EventArgs e)
        {
            GraphicsPath pathUp = new GraphicsPath();
            pathUp.AddPolygon(new Point[] { new Point(2, 13), new Point(13, 2), new Point(28, 13) });
            GraphicsPath pathDown = new GraphicsPath();
            pathDown.AddPolygon(new Point[] { new Point(2, 2), new Point(13, 13), new Point(28, 2) });
            int y = (this.Height / 2) - 1;
            int x = 30;
            btnUp = new Button
            {
                BackColor = Color.DarkGray,
                Location = new Point(this.Width - x, 0),
                Size = new Size(x, 15)

                ,
                Region = new Region(pathUp)
            };
            btnDown = new Button
            { Region = new Region(pathDown), BackColor = Color.DarkGray, Location = new Point(this.Width - x, y + 3), Size = new Size(x, 15) };
            this.Controls.Add(btnUp);
            this.Controls.Add(btnDown);
        }

        void PaintString(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString(DateString, new Font("Calibri", 11f), Brushes.Black, new PointF(3, 3));



        }


    }
    public partial class NewInfoPanel : UserControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
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
    
        ITermin termin;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimePickerUpDown DateTimePickerStart { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimePickerUpDown DateTimePickerEnd { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button EliminateBtn { get; set; }

        public NewInfoPanel(ITermin termin)
        {
            InitializeComponent();

            this.termin = termin;
            this.Size = new Size(FindMaxWidth(termin.GetInfoData()), (termin.GetInfoData().Length + 3) * 25 + 10);
            this.DoubleBuffered = true;
            this.AutoSize = false;
            this.BackColor = Color.Beige;
            GraphicsPath path = new GraphicsPath();
            path.AddRoundedRectangle(new Rectangle(0, 0, this.Width, this.Height), new Size(20, 20));


            this.Region = new Region(path);

            this.Paint += (s, e) => OnPaint(e);
            setLabels(termin);
            EliminateBtn = new Button { Location = new Point(this.Width - 40, this.Height - 40), Size = new Size(30, 30), Text = "🗑", ForeColor = Color.DarkSlateGray };
            this.Controls.Add(EliminateBtn);
        }

        void setLabels(ITermin termin)
        {
            for (int i = 0; i < (termin.GetInfoData().Length + 2); i++)
            {
                if (i < termin.GetInfoData().Length)
                {
                    Label lbl = new Label();
                    lbl.Text = termin.GetInfoData()[i];
                    lbl.Location = new Point(10, 10 + i * 25);
                    lbl.AutoSize = true;
                    this.Controls.Add(lbl);
                }
                else
                {
                    TimePickerUpDown dateTimePicker;


                    if (i == termin.GetInfoData().Length)
                    {
                        dateTimePicker = new TimePickerUpDown(termin);

                        dateTimePicker.Location = new Point(10, 5 + i * 30);
                        dateTimePicker.Size = new Size(80, 27);


                        dateTimePicker.Name = $"dateTimePickerStart";
                        DateTimePickerStart = dateTimePicker;
                    }


                    else
                    {
                        dateTimePicker = new TimePickerUpDown(termin, false);

                        dateTimePicker.Location = new Point(10, 10 + i * 30);
                        dateTimePicker.Size = new Size(80, 27);

                        dateTimePicker.Name = $"dateTimePickerEnd";
                        DateTimePickerEnd = dateTimePicker;
                    }

                    this.Controls.Add(dateTimePicker);
                }

            }
        }


        void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.Clear(Color.LightYellow);
            using (Pen pen = new Pen(Color.DarkBlue, 2))
            {
                g.DrawRoundedRectangle(pen, new Rectangle(1, 1, this.Width - 1, this.Height - 1), new Size(20, 20));
            }
        }
        int FindMaxWidth(string[] datainfo)
        {
            int maxWidth = 0;
            using (Graphics g = this.CreateGraphics())
            {
                foreach (string info in datainfo)
                {
                    SizeF size = g.MeasureString(info, this.Font);
                    if (size.Width > maxWidth)
                    {
                        maxWidth = (int)size.Width;
                    }
                }
            }
            return maxWidth + 20; // Add some padding
        }
    }
}
#endregion