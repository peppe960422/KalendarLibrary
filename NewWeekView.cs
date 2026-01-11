using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public partial class NewWeekView : UserControl
    {
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
        #region Fields & Properties
        // campi, flag, liste, stato
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Random rnd = new Random();
        int deltaY = 0;
        ITerminGenerator _generator;
        bool cliccatoLeft = false;
        bool Saving = false;
        List<ITermin> newTermins = new List<ITermin>();
        List<ITermin> updatedTermins = new List<ITermin>();
        List<ITermin> deletedTermins = new List<ITermin>();
        int IndexGridStart = 0;
        Button ButtonSave = new Button();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Dragged { get; set; }
        public double ScaleHourFactor = 1.0f;
        bool schieb = false;
        bool rightMouse = false;
        private DateTime _currentWeekStart { get; set; }
        public WeekViewControlBar controlBar;

        private int AnfangsUhr = 8;
        Rectangle[,] grid;
        public Size GridSize = new Size(10, 10);
        NewInfoPanel InfoPanelNew = null;



        RectTerminFactory RectTerminFactory;
        int index = 0;
        Point? _dragStart = null;
        Point? _originalRectLocation = null;
        List<RectTermin> rectTermins = new List<RectTermin>();
        IDataProvider<ITermin> provider;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MinutiSnap { get; set; } = 5;
        #endregion

        #region Constructors

        public NewWeekView(IDataProvider<ITermin> dataProvider, DateTime weekStartDate, int numberOfDays, int stunden, int anfangUhr, List<List<ITerminProperty>> terminProps, ITermin terminObj, Size s)
        {
            InitializeComponent();

            _currentWeekStart = new DateTime(weekStartDate.Ticks);
            this.Size = s;
            controlBar = new WeekViewControlBar(this, SaveTermis);
            controlBar.BringToFront();
            InitTerminGenerator(terminProps, terminObj.GetType(), weekStartDate, this);

            this.Controls.Add(controlBar);

            provider = dataProvider;

            this.DoubleBuffered = true;
            grid = new Rectangle[numberOfDays, stunden];

            GridSize = new Size((Width - 60) / numberOfDays, (Height - 50) / stunden);
            ScaleHourFactor = (double)Height / stunden / 60.0;


            this.MouseDown += NewWeekView_MouseDown;
            this.MouseUp += NewWeekView_MouseUp;
            this.MouseMove += TerminView_MouseMove;
            this.Paint += (s, e) => OnPaint(e);





            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Rectangle(i * GridSize.Width + 60, 50 + (j * GridSize.Height), GridSize.Width, GridSize.Height);

                }
            }

            List<ITermin> termins = provider.GetItems(_currentWeekStart, _currentWeekStart.AddDays(numberOfDays)).Result;
            RectTerminFactory = new RectTerminFactory(termins);

            foreach (ITermin termin in termins)
            {
                RectTermin rt = RectTerminFactory.ProduceUIelement(termin);
                FindYPosition(rt);
                int indexDay = (termin.Start.Date - _currentWeekStart.Date).Days; ;

                rt.x = grid[indexDay, 0].X;
                SetHeight(rt);
                rt.x = grid[indexDay, 0].Location.X;
                rt.y = FindYPosition(rt);
                rectTermins.Add(rt);





            }
            FindFirstLocation();




        }

        #endregion
        #region Initializers
        private void InitTerminGenerator(List<List<ITerminProperty>> props, Type typo, DateTime anfang, Control parent)
        {

            var genericType = typeof(TerminGeneratorNew<>).MakeGenericType(typo);

            _generator = (ITerminGenerator)Activator.CreateInstance(genericType, props, anfang, parent);

            _generator.SetCreateButtonEvent(Generate_Click);

            Control g = (Control)_generator;
            g.Location = new Point(70, 5);

            this.Controls.Add(g);

            g.BringToFront();

        }
        #endregion



        #region UI Events



        public void Generate_Click(object sender, EventArgs e)
        {
            List<ITermin> newTermins = _generator.GenerateTermins();

            for (int i = 0; i < newTermins.Count; i++)
            {
                int indexDay = newTermins[i].Start.Day - _currentWeekStart.Day + 1;
                var tv = RectTerminFactory.ProduceUIelement(newTermins[i]);
                SetHeight(tv);
                tv.x = grid[indexDay - 1, 0].Location.X;
                tv.y = FindYPosition(tv);
                rectTermins.Add(tv);



            }
            FindFirstLocation();

            Invalidate();

        }

        async void SaveTermis(object sender, EventArgs e)
        {
            Saving = true; Saving = false;
            LoadingControl L = new LoadingControl(this.Width, this.Height);
            L.Location = new Point(0, 0);
            this.Controls.Add(L);
            L.BringToFront();
            await provider.SaveChanges(newTermins, updatedTermins, deletedTermins);
            this.Controls.Remove(L);


        }
        void DeleteTermin(object sender, EventArgs e)
        {
            Control parent = sender as Control;

            this.Controls.Remove(parent.Parent);
            this.deletedTermins.Add(rectTermins[index].Termin);
            this.rectTermins = rectTermins.Where(x => x != rectTermins[index]).ToList();
            RiposizionaElementi();
            Invalidate();



        }
        private void dateTimePickerButtonUp(object sender, EventArgs e)
        {
            Button dp = (Button)sender;
            TimePickerUpDown p = (TimePickerUpDown)dp.Parent;






            if (p.Name == "dateTimePickerStart")
            {

                rectTermins[index].Termin.Start =
                    rectTermins[index].Termin.Start.AddMinutes(-MinutiSnap);
                p.DateTime = p.DateTime.AddMinutes(-MinutiSnap);


                rectTermins[index].y = MinutesToPixel();
                SetHeight(rectTermins[index]);

                FindFirstLocation();
                Invalidate();
                p.Invalidate();


            }
            else if (p.Name == "dateTimePickerEnd")
            {

                p.DateTime = p.DateTime.AddMinutes(-MinutiSnap);
                rectTermins[index].Termin.End = p.DateTime;

                SetHeight(rectTermins[index]);

                FindFirstLocation();
                Invalidate();
                p.Invalidate();



            }




        }
        private void dateTimePickerButtonDown(object sender, EventArgs e)
        {
            Button dp = (Button)sender;
            TimePickerUpDown p = (TimePickerUpDown)dp.Parent;

            Debug.WriteLine(rectTermins[index].y);




            if (p.Name == "dateTimePickerStart")
            {


                rectTermins[index].Termin.Start =
                    rectTermins[index].Termin.Start.AddMinutes(MinutiSnap);
                p.DateTime = p.DateTime.AddMinutes(MinutiSnap);

                rectTermins[index].y = MinutesToPixel();

                SetHeight(rectTermins[index]);

                FindFirstLocation();
                Invalidate();
                p.Invalidate();






            }
            else if (p.Name == "dateTimePickerEnd")
            {

                rectTermins[index].Termin.End =
                    rectTermins[index].Termin.End.AddMinutes(MinutiSnap);
                p.DateTime = p.DateTime.AddMinutes(MinutiSnap);


                SetHeight(rectTermins[index]);

                Invalidate();
                p.Invalidate();

                //int minuti = 5;
                //p.DateTime = p.DateTime.AddMinutes(MinutiSnap);
                //rectTermins[index].Termin.End = p.DateTime;

                //SetHeight(rectTermins[index]);

                //FindFirstLocation();
                //Invalidate();
                //p.Invalidate();



            }




        }


        #endregion

        #region Mouse Handling
        public void TerminView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (schieb)
                {
                    deltaY = e.Y - rectTermins[index].y - rectTermins[index].Height;
                    if (deltaY % (int)Math.Round((double)(int)controlBar.comboBoxEmpfindlichkei.SelectedItem * ScaleHourFactor) == 0)
                    {
                        if (rectTermins[index].Height >= 30)
                        {
                            rectTermins[index].Height = rectTermins[index].Height + deltaY;
                            schieb = true;

                            Invalidate();
                        }
                        else
                        {
                            this.rectTermins[index].Height = 30;

                        }
                    }

                }
                if (_dragStart.HasValue && Dragged && _originalRectLocation.HasValue)
                {
                    int dx = e.X - _dragStart.Value.X;
                    int dy = e.Y - _dragStart.Value.Y;

                    int newX = _originalRectLocation.Value.X + dx;
                    int newY = _originalRectLocation.Value.Y + dy;


                    rectTermins[index].x = newX;
                    rectTermins[index].y = newY;




                    Invalidate();
                }
            }
        }


        private void NewWeekView_MouseDown(object sender, MouseEventArgs e)
        {
            if (InfoPanelNew !=
                null)
            {
                this.Controls.Remove(InfoPanelNew);
                InfoPanelNew = null;
            }

            if (e.Button == MouseButtons.Left)
            {
                rightMouse = false;
                for (int i = 0; i < rectTermins.Count; i++)
                {
                    if (rectTermins[i].Rect.Contains(e.Location))
                    {

                        index = i;
                        _dragStart = e.Location;
                        _originalRectLocation = rectTermins[i].Rect.Location;
                        ;
                        // Salva la posizione originale
                        Dragged = true;
                        break; // Aggiungi break per evitare di selezionare più rettangoli

                    }
                    else if (rectTermins[i].schiebButton.Contains(e.Location))
                    {
                        Dragged = true;
                        if (deltaY % (int)Math.Round((double)(int)controlBar.comboBoxEmpfindlichkei.SelectedItem * ScaleHourFactor) == 0)
                        {

                            if (rectTermins[i].Height >= 30)
                            {
                                deltaY = e.Y - rectTermins[i].y - rectTermins[i].Height;
                                rectTermins[i].Height = rectTermins[i].Height + deltaY;



                                schieb = true;
                                index = i;
                                this.Parent.Invalidate();
                            }
                            else
                            {
                                this.Parent.Height = 30;
                                this.Location = new Point(2, 1);

                            }
                        }
                    }
                }

            }
            else if (e.Button == MouseButtons.Right)
            {

                rightMouse = true;
                for (int i = 0; i < rectTermins.Count; i++)
                {
                    if (rectTermins[i].Rect.Contains(e.Location))
                    {
                        index = i;
                        NewInfoPanel t = new NewInfoPanel(rectTermins[i].Termin);
                        t.Location = new Point(rectTermins[i].x + rectTermins[i].Width, rectTermins[i].y + (rectTermins[i].Height / 3));
                        this.Controls.Add(t);
                        InfoPanelNew = t;
                        InfoPanelNew.EliminateBtn.Click += DeleteTermin;
                        InfoPanelNew.DateTimePickerStart.btnDown.Click += dateTimePickerButtonDown;
                        InfoPanelNew.DateTimePickerStart.btnUp.Click += dateTimePickerButtonUp;
                        InfoPanelNew.DateTimePickerEnd.btnDown.Click += dateTimePickerButtonDown;
                        InfoPanelNew.DateTimePickerEnd.btnUp.Click += dateTimePickerButtonUp;

                        break;
                    }
                }



            }
        }

        private void NewWeekView_MouseUp(object sender, MouseEventArgs e)
        {
            if (rightMouse || Dragged == false)
            {
                rightMouse = false;
                return;
            }
            IndexGridStart = rectTermins[index].Rect.X / GridSize.Width;
            rectTermins[index].x = grid[IndexGridStart, 0].X;
            _dragStart = null;
            if (!schieb)
            {
                int[] date = NewDate(rectTermins[index]);

                rectTermins[index].Termin.Start = new DateTime(_currentWeekStart.Year, _currentWeekStart.Month, date[0], date[1], date[2], 0);
                rectTermins[index].y = CorrectedPointNachEmpfindlichkeit(rectTermins[index]);
            }
            calculateNewEndeMouseUp(rectTermins[index]);
            SetHeight(rectTermins[index]);
            if (schieb)
            {
                //rectTermins[index].Height = CorrectEndNachEmpfindlichkeit(rectTermins[index]);
                deltaY = 0;
                schieb = false;

            }
            _originalRectLocation = null; // Resetta anche questa
            Dragged = false;
            RiposizionaElementi();
            Invalidate();
        }

        #endregion

        #region Time & Layout Calculations
        private void SetHeight(RectTermin rt)
        {
            int Hours = rt.Termin.End.Hour - rt.Termin.Start.Hour;

            if (Hours < 0)
            {
                double totalminutes = (rt.Termin.End - rt.Termin.Start).TotalMinutes;

                rt.Height = (int)Math.Round(totalminutes * ScaleHourFactor);
            }
            else
            {
                rt.Height = (int)Math.Round(((Hours * GridSize.Height)
                    ) + (rt.Termin.End.Minute - rt.Termin.Start.Minute) * ScaleHourFactor);
            }




        }
        public int FindYPosition(RectTermin t)
        {
            int hours = t.Termin.Start.Hour - AnfangsUhr;

            //return grid[0, hours].Y +
            //       (int)Math.Round(t.Termin.Start.Minute * ScaleHourFactor);

            int posY = grid[0, 0].Y;
            int day = t.Termin.Start.Day;
            int month = t.Termin.Start.Month;
            int year = t.Termin.Start.Year;

            DateTime dayBegin = new DateTime(year, month, day, AnfangsUhr, 00, 0);

            TimeSpan a = (t.Termin.Start - dayBegin);

            int Hour = (int)a.TotalHours;
            int Minu = (int)a.Minutes;

            int mins = GridSize.Height * Hour + (int)Math.Round(ScaleHourFactor * Minu);

            return posY + mins;



        }

        int MinutesToPixel()
        {
            int hours = rectTermins[index].Termin.Start.Hour - AnfangsUhr;

            return grid[IndexGridStart, hours].Y +
                   (int)Math.Round(rectTermins[index].Termin.Start.Minute * ScaleHourFactor);
        }

        public void FindFirstLocation()
        {


            foreach (RectTermin r in rectTermins)
            {

                bool positionAdjusted;
                do
                {
                    positionAdjusted = false;
                    foreach (RectTermin rj in rectTermins)
                    {
                        if (r != rj && Intersect(r.Rect,
                           rj.Rect

                        ))
                        {

                            r.x += r.Width;

                            positionAdjusted = true;
                        }
                    }
                } while (positionAdjusted);


            }

        }
        public int[] NewDate(RectTermin termin)
        {


            int i = 0;
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                Rectangle t = grid[j, 0];

                if (Intersect(new Rectangle(t.Location.X, t.Location.Y, t.Width, this.Height), new Rectangle(termin.x, termin.y, termin.Width, termin.Height)))
                {
                    DateTime dayDate = _currentWeekStart.AddDays(i);


                    int offsetY = termin.y - t.Location.Y;


                    double minutesFromStart = offsetY / ScaleHourFactor;

                    int empf = (int)controlBar.comboBoxEmpfindlichkei.SelectedItem;

                    int minutedCorrected = (int)Math.Round(minutesFromStart) - (int)(Math.Round(minutesFromStart) % empf);


                    DateTime startOfDay = new DateTime(dayDate.Year, dayDate.Month, dayDate.Day, AnfangsUhr, 0, 0);


                    DateTime newDate = startOfDay.AddMinutes(minutedCorrected);


                    return new int[]{newDate.Day,
                newDate.Hour,
                newDate.Minute};

                }


                i++;
            }


            return null;


        }





        private int CorrectedPointNachEmpfindlichkeit(RectTermin rt)
        {

            int tolleranza = (int)controlBar.comboBoxEmpfindlichkei.SelectedItem;
            if (tolleranza >= 5)
            {
                int hours = rt.Termin.Start.Hour - AnfangsUhr;
                int y = grid[IndexGridStart, 0].Location.Y + (GridSize.Height * hours);
                //if (tolleranza == 60)

                //{

                //    return  y;

                //}

                int divideQ = (int)Math.Round(GridSize.Height / (tolleranza * ScaleHourFactor));
                int f = (int)Math.Round((double)GridSize.Height / divideQ);
                ;
                int counterMin = 0;
                while (counterMin < rt.Termin.Start.Minute)

                {

                    counterMin += tolleranza;
                    y += f;

                }




                return y /*-corr*/;
            }

            return rt.y;



            // int corr = ((int)math.round(30 * pixelperminuto));
            // restituisce il nuovo punto "snapato" alla griglia temporale


        }





        public void calculateNewEndeMouseUp(RectTermin t)
        {

            //TagView tv = (TagView)uv.Parent;
            int intMin = (int)Math.Round(t.Height / ScaleHourFactor);
            TimeSpan minutes = new TimeSpan(0, intMin, 0);
            // minutes = new TimeSpan(0, intMin- (intMin % (int)comboBoxEmpfindlichkei.SelectedItem), 0);
            t.Termin.End = t.Termin.Start + minutes;



        }

        public void RiposizionaElementi()
        {


            var sortedTermine = rectTermins.OrderBy(t => t.Rect.Y).ThenBy(t => t.Rect.X).ToList();


            List<Rectangle> occupati = new List<Rectangle>();

            foreach (RectTermin t in sortedTermine)
            {
                int dayIndex = t.Rect.X / GridSize.Width;
                if (dayIndex >= grid.GetLength(0)) { dayIndex--; }
                int x = this.grid[dayIndex, 0].X;
                int y = t.Rect.Y;
                int width = t.Rect.Width;
                int height = t.Rect.Height;

                Rectangle nuovo = new Rectangle(x, y, width, height);


                bool overlap;
                do
                {
                    overlap = false;
                    foreach (var occ in occupati)
                    {
                        if (Intersect(nuovo, occ))
                        {
                            x = occ.Right;
                            nuovo = new Rectangle(x, y, width, height);
                            overlap = true;
                            break;
                        }
                    }
                } while (overlap);


                t.x = nuovo.X;
                t.y = nuovo.Y;




                occupati.Add(nuovo);
            }
        }
        public bool Intersect(Rectangle rect1, Rectangle rect2)
        {

            return rect1.Left < rect2.Right &&
                   rect1.Right > rect2.Left &&
                   rect1.Top < rect2.Bottom &&
                   rect1.Bottom > rect2.Top;
        }


        #endregion

        #region Rendering  
        void OnPaint(PaintEventArgs e)


        {

            if (Saving) { return; }
            Graphics g = e.Graphics;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {


                    if (j % 2 == 0)
                    {
                        g.FillRectangle(Brushes.GhostWhite, grid[i, j]);
                    }
                    g.DrawRectangle(Pens.DarkGray, grid[i, j]);
                    if (i == 0)
                    {
                        g.FillRectangle(Brushes.Beige, 0, grid[i, j].Y, 60, GridSize.Height);
                        g.DrawRectangle(Pens.Black, 0, grid[i, j].Y, 60, GridSize.Height);
                        g.DrawString((AnfangsUhr + j).ToString(), new Font(new FontFamily("Calibri"), 15f), Brushes.Black, new Point(10, grid[i, j].Y + 10));
                    }


                }
            }


            for (int i = 0; i < rectTermins.Count; i++)
            {
                var rt = rectTermins[i];
                if (i != index)
                {
                    using (Pen b = new Pen(rt.mainColor, 3))
                    {
                        using (SolidBrush p = new SolidBrush(RectTermin.ToPastel(rt.mainColor)))
                        {
                            g.FillRoundedRectangle(Brushes.DarkGray, rt.schiebButton, new Size(20, 20));
                            g.DrawRoundedRectangle(Pens.Black, rt.schiebButton, new Size(20, 20));
                            g.FillRectangle(p, rt.Rect);
                            g.DrawRectangle(b, rt.Rect);
                        }
                    }
                }


            }
            if (index >= 0 && index < rectTermins.Count && rectTermins[index] != null)
            {
                using (Pen b = new Pen(rectTermins[index].mainColor, 3))
                {
                    var rt = rectTermins[index];
                    using (SolidBrush p = new SolidBrush(RectTermin.ToPastel(rt.mainColor)))
                    {
                        g.FillRoundedRectangle(Brushes.DarkGray, rt.schiebButton, new Size(20, 20));
                        g.DrawRoundedRectangle(Pens.Black, rt.schiebButton, new Size(20, 20));
                        g.FillRectangle(p, rt.Rect);
                        g.DrawRectangle(b, rt.Rect);
                    }
                }
            }

        }

        #endregion

    }

    public class RectTerminFactory
    {
        Color[] palette =
{
    Color.FromArgb(255, 231, 76, 60),   // rosso vivo
    Color.FromArgb(255, 52, 152, 219),  // blu acceso
    Color.FromArgb(255, 46, 204, 113),  // verde brillante
    Color.FromArgb(255, 241, 196, 15),  // giallo intenso
    Color.FromArgb(255, 230, 126, 34),  // arancio forte
    Color.FromArgb(255, 155, 89, 182),  // viola acceso
    Color.FromArgb(255, 26, 188, 156),  // turchese
    Color.FromArgb(255, 243, 156, 18),  // ambra
    Color.FromArgb(255, 211, 84, 0),    // arancio scuro
    Color.FromArgb(255, 192, 57, 43)    // rosso scuro
};

        Dictionary<string, Color> colorMap = new();
        List<ITermin> termins = new List<ITermin>();
        private void assingColor()
        {
            colorMap = termins
    .Select(i => i.GetInfoData()[0])
    .Distinct()
    .Select((key, i) => new
    {
        key,
        color = palette[i % palette.Length]
    })
    .ToDictionary(x => x.key, x => x.color);
        }
        public RectTerminFactory(List<ITermin> t)
        {
            foreach (ITermin f in t)
            {

                termins.Add(f);
            }
            assingColor();
        }


        public RectTermin ProduceUIelement(ITermin t)

        {

            Color c;
            if (!colorMap.TryGetValue(t.GetInfoData()[0], out c))
            {
                termins.Add(t);
                assingColor();

            }

            return new RectTermin(t, colorMap[t.GetInfoData()[0]]);

        }



    }
    public class RectTermin
    {
        public int x { get; set; }
        public int y { get; set; }

        public Color mainColor { get; set; }

        private int _width;
        public int Width { get { return 50; } }

        public int Height { get; set; }
        public Point Position { get { return new Point(x, y); } }

        public Rectangle schiebButton { get { return new Rectangle(x, y + Height - 21, Width, 20); } }
        public Rectangle Rect
        {
            get { return new Rectangle(x, y, Width, Height - 10); }
            set
            {
                x = value.X;
                y = value.Y;

                Height = value.Height;
            }
        }
        public ITermin Termin { get; set; }

        public RectTermin()
        {


        }

        public RectTermin(ITermin t, Color c)
        {

            Termin = t;

            this.mainColor = c;

        }
        public static Color ToPastel(Color c)
        {
            int r = (c.R + 255) / 2;
            int g = (c.G + 255) / 2;
            int b = (c.B + 255) / 2;

            return Color.FromArgb(r, g, b);
        }

    }
}

