using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Drawing.Drawing2D;

using Label = System.Windows.Forms.Label;

namespace KalendarLibrary
{
   
        public partial class TerminGeneratorNew<T> : UserControl, ITerminGenerator where T : ITermin
        {


            private string title;
            protected Size storedSize = Size.Empty;
            private bool isMinimized = true;

            Region _minimizedRegion;

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public DateTime AnfangWeek { get; set; }
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Button BtnCreate { get; set; }
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public GraphicsPath path
            {
                get { GraphicsPath g = new GraphicsPath(); g.AddRectangle(new Rectangle(0, 0, Width, Height)); return g; }
            }

            WeeklyTimePicker weekTimePicker;

            public List<ITermin> GenerateTermins()
            {
                List<ITermin> listT = new List<ITermin>();
                title = "Termin Generator " + typeof(T).Name;

                int i = 0;
                foreach (CheckBox chx in weekTimePicker.checkBoxes)
                {
                    if (chx != null)
                    {
                        if (chx.Checked)
                        {

                            T t = (T)Activator.CreateInstance(typeof(T)); ;
                            foreach (PropertyInfo info in typeof(T).GetProperties())
                            {
                                foreach (Control c in this.Controls)
                                {
                                    if (c is ComboBox && c.Name == info.Name)
                                    {
                                        ComboBox cb = c as ComboBox;
                                        if (info.PropertyType.IsAssignableFrom(cb.SelectedItem.GetType()))
                                        {
                                            info.SetValue(t, cb.SelectedItem);
                                        }
                                    }
                                    else if (c is TextBox && c.Name == info.Name)
                                    {
                                        TextBox tb = c as TextBox;
                                        if (info.PropertyType == typeof(string))
                                        {
                                            info.SetValue(t, tb.Text);
                                        }
                                    }
                                    else if (c is WeeklyTimePicker && info.PropertyType == typeof(DateTime))
                                    {
                                        WeeklyTimePicker wtp = c as WeeklyTimePicker;

                                        if (info.Name.ToLower().Contains("start"))
                                        {
                                            MethodInfo piStart = typeof(WeeklyTimePicker).GetMethod("GetStart");
                                            if (piStart != null)
                                            {
                                                DateTime start = (DateTime)piStart.Invoke(wtp, new object[] { i });
                                                info.SetValue(t, start);
                                            }
                                        }
                                        else if (info.Name.ToLower().Contains("end"))
                                        {
                                            MethodInfo piEnd = typeof(WeeklyTimePicker).GetMethod("GetEnd");
                                            if (piEnd != null)
                                            {
                                                DateTime End = (DateTime)piEnd.Invoke(wtp, new object[] { i });
                                                info.SetValue(t, End);
                                            }
                                        }
                                    }
                                    else if (c is ListSwapper && info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                                    {
                                        if (c.Name == info.Name)
                                        {
                                            ListSwapper ls = c as ListSwapper;
                                            Type elementType = info.PropertyType.GetGenericArguments()[0];
                                            if (ls.GetSelectedItems() != null)
                                            {
                                                var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) as System.Collections.IList;
                                                foreach (var item in ls.GetSelectedItems())
                                                {
                                                    list.Add(item);
                                                }
                                                info.SetValue(t, list);
                                            }
                                        }
                                        ;

                                    }
                                }

                            }
                            if (t.Start.AddMinutes(30) >= t.End)
                            {
                                MessageBox.Show("Fehler: Der Termin am " + t.Start.ToShortDateString() + " hat kein gültiges Ende.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return new List<ITermin>();
                            }
                            listT.Add(t);
                        }
                    }
                    i++;
                }

                return listT;

            }

            public void SetCreateButtonEvent(EventHandler e)
            {
                BtnCreate.Click += e;
            }

            void Minimize(object s, EventArgs e)
            {
                if (!isMinimized)
                {
                    InvisibleButton btn = (InvisibleButton)s;
                    btn.Font = new Font(btn.Font.FontFamily, 12, FontStyle.Bold);
                    GraphicsPath path = new GraphicsPath();
                    path.AddRectangle(new Rectangle(0, 0, 30, 30));
                    this.Region = new Region(path);
                    isMinimized = true;
                }
                else
                {
                    InvisibleButton btn = (InvisibleButton)s;
                    Rectangle r = new Rectangle(0, 0, 30, 30);
                    this.Region = new Region(GetRoundedRect(new Rectangle(0, 0, storedSize.Width, storedSize.Height - 50), 20, r));
                    btn.Font = new Font(btn.Font.FontFamily, 12, FontStyle.Bold);
                    this.Size = storedSize;
                    isMinimized = false;
                    BringToFront();
                    Show();

                }
            }

            private GraphicsPath GetRoundedRect(Rectangle bounds, int radius, Rectangle btn)
            {
                int diameter = radius * 2;
                GraphicsPath path = new GraphicsPath();



                path.AddLine(bounds.X, bounds.Y + 30, bounds.Right, bounds.Y + 30);



                path.AddLine(bounds.Right, bounds.Y + radius, bounds.Right, bounds.Bottom - radius);

                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);

                path.AddLine(bounds.Right - radius, bounds.Bottom, bounds.X + radius, bounds.Bottom);

                path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);

                path.AddLine(bounds.X, bounds.Bottom - radius, bounds.X, bounds.Y + radius);

                path.AddRectangle(btn);

                path.CloseFigure();
                return path;
            }

            public void OnPaintBorder
                (object sender, PaintEventArgs e)

            {
                if (isMinimized) { return; }


                GraphicsPath p = GetRoundedRect(new Rectangle(3, 3, storedSize.Width - 6, storedSize.Height - 56), 20, new Rectangle(0, 0, 30, 30));

                using (Pen pen = new Pen(Color.DarkSlateGray, 3)) { e.Graphics.DrawPath(pen, p); }





            }







            public TerminGeneratorNew(List<List<ITerminProperty>> listProps, DateTime anfangWeek, Control Parent) : base()
            {

                this.Parent = Parent;
                Parent.Controls.Add(this);
                int x = 10;
                int y = 60;
                int x2 = 10;
                int y2 = 60;
                int Width = 150;
                int Height = 70;
                this.BackColor = Color.DarkGray;
                this.title = "Termin Generator " + typeof(T).Name;
                this.BorderStyle = BorderStyle.None;

                foreach (PropertyInfo p in typeof(T).GetProperties())
                {
                    Type propType = p.PropertyType;
                    Debug.WriteLine(p.Name + " " + propType.Name);

                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        Type elementType = propType.GetGenericArguments()[0];
                        var l = listProps.FirstOrDefault(lp => lp != null && lp.Count > 0 && elementType.IsAssignableFrom(lp[0].GetType()));

                        if (l != null)
                        {
                            ListSwapper ls = new ListSwapper(l);
                            ls.Name = p.Name;
                            ls.Location = new Point(x, y);
                            y += ls.Height + 30;
                            Width = ls.Width + 140;
                            Height = y + 30;
                            ls.BringToFront();
                            x2 = ls.Width + 20;
                            this.Controls.Add(ls);
                            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label { Location = new Point(x, y - ls.Height - 50), Text = p.Name, AutoSize = true };
                            lbl.Font = new Font("Calibri ", 10, FontStyle.Bold);
                            this.Controls.Add(lbl); lbl.BringToFront();
                        }
                        else
                        {
                            Debug.WriteLine($"Nessuna lista trovata per la property {p.Name} (elementType={elementType.Name})");
                        }
                    }

                }

                int j = 0;
                foreach (PropertyInfo p in typeof(T).GetProperties())
                {
                    Type propType = p.PropertyType;
                    if (typeof(ITerminProperty).IsAssignableFrom(propType))
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.Name = p.Name;
                        var l = listProps.FirstOrDefault(lp => lp != null && lp.Count > 0 && propType == lp[0].GetType());
                        foreach (var v in l)
                        {
                            comboBox.Items.Add(v);
                        }

                        Label label = new System.Windows.Forms.Label();
                        label.AutoSize = true;
                        label.Text = p.Name;
                        label.Location = new Point(x2, y2);
                        label.Font = new Font("Calibri ", 10, FontStyle.Bold);
                        comboBox.Size = new Size(200, 20);
                        comboBox.Location = new Point(x2, y2 + 25);
                        y2 += comboBox.Height + 25;
                        if (Height < 250)
                        {
                            y += comboBox.Height;
                            Height = y + 60;
                        }

                        this.Controls.Add(comboBox);
                        this.Controls.Add(label);
                        comboBox.BringToFront();

                    }

                    else if (propType == typeof(string))
                    {
                        RoundedTextBox textBox = new RoundedTextBox();
                        Label label = new Label();
                        label.AutoSize = true;
                        label.Text = p.Name;
                        label.Font = new Font("Calibri ", 10, FontStyle.Bold);
                        label.ForeColor = Color.Black;
                        label.Location = new Point(x2, y2);
                        textBox.Size = new Size(200, 20);
                        textBox.Location = new Point(x2, y2 + 25);
                        textBox.Name = p.Name;
                        y2 += textBox.Height + 25;
                        if (Height < 250)
                        {
                            y += textBox.Height;
                            Height = y + 60;
                        }
                        textBox.OnLoad();
                        this.Controls.Add(textBox);
                        this.Controls.Add(label);
                    }
                    j++;
                }
                WeeklyTimePicker weekTimePicker = new WeeklyTimePicker(
    ); weekTimePicker.StartTime = anfangWeek;
                weekTimePicker.SetDays(5);
                int y3;
                if (y2 < y) { y3 = y; } else { y3 = y2; }

                weekTimePicker.Location = new Point(x2 + 50, y2 + 25);

                Width += weekTimePicker.Width + 20;
                Height = y3 + 150;
                this.Controls.Add(weekTimePicker);
                this.storedSize = new Size(Width, Height);

                BtnCreate = new Button();
                BtnCreate.Image = ImageBase64Converter.Base64ToImage(IconAssets.CreateNew);
                BtnCreate.Size = new Size(50, 50);

                BtnCreate.Location = new Point(20, Height - 130);
                BtnCreate.Font = BtnCreate.Font = new Font(BtnCreate.Font.FontFamily, 12, FontStyle.Bold);


                BtnCreate.BackColor = Color.LightGray;

                this.Controls.Add(BtnCreate);
                BtnCreate.BringToFront();
                this.weekTimePicker = weekTimePicker;
                AnfangWeek = anfangWeek;
                this.Size = MinimumSize;
                InvisibleButton btnMinimize = new InvisibleButton(this, IconAssets.UpArrow, IconAssets.DownArrow);

                btnMinimize.Text = "+";
                btnMinimize.Size = new Size(30, 30);

                btnMinimize.Location = new Point(0, 0);
                btnMinimize.Click += Minimize;

                this.Controls.Add(btnMinimize);
                btnMinimize.BringToFront();
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(new Rectangle(0, 0, btnMinimize.Width, btnMinimize.Height));
                _minimizedRegion = new Region(path);
                this.Size = btnMinimize.Size;
                this.Region = _minimizedRegion;
                isMinimized = true;
                this.Paint += OnPaintBorder;




            }


        }
    }

    class RoundedTextBox : TextBox
    {
        public RoundedTextBox() : base()
        {


        }

        public void OnLoad()
        {
            int radius = 10;
            GraphicsPath path = new GraphicsPath();

            Rectangle rect = new Rectangle(0, 0, Width, Height);

            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);



        }

        public void OnPaint(object sender, PaintEventArgs p)
        {
            int radius = 10;
            Rectangle rect = new Rectangle(2, 2, Width - 4, Height - 4);
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            using (Pen f = new Pen(Color.DarkSlateGray, 2))
            {

                p.Graphics.DrawPath(f, path);


            }
            ;



        }


    }

