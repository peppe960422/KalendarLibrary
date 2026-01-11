using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public class WeekWithTermin
    {
        public Woche Week { get; set; }

        public List<ITermin> Lessons { get; set; } = new List<ITermin>();

        public WeekWithTermin(Woche week)
        {
            this.Week = week;
        }




    }

    public class Kalendar
    {
        public uint id { get; set; }

        public string name { get; set; }

        private int lngt = 5;
        int LangeWoche
        {
            get { return lngt; }
            set
            {
                if (value < 5 || value > 7) { throw new ArgumentOutOfRangeException("Lange der Woche muss zwischen 5 und 7 liegen"); }
                lngt = value;

            }
        }
        public DateTime Anfang


        {
            get { return weekLessonKalendar[0].Week.AnfangWoche; }

        }
        public DateTime Ende

        {


            get { return weekLessonKalendar[weekLessonKalendar.Count - 1].Week.EndeWoche(); }


        }


        public List<WeekWithTermin> weekLessonKalendar { get; set; } = new List<WeekWithTermin>();

        public Kalendar(DateTime anfangJahrMontag)
        {
            GenerateKalendar(anfangJahrMontag);


        }
        public Kalendar(uint id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public void GenerateKalendar(DateTime anfangJahrMontag)
        {



            for (int i = 0; i < 52; i++)
            {
                Woche settimana = new Woche(anfangJahrMontag, 7);
                settimana.LangeWoche = lngt;
                WeekWithTermin w = new WeekWithTermin(settimana);

                weekLessonKalendar.Add(w);

                anfangJahrMontag = anfangJahrMontag.AddDays(7);
            }




        }




    }

    public struct Woche
    {
        public DateTime AnfangWoche { get; set; }


        public int LangeWoche { get; set; } = 5;
        public DateTime EndeWoche()
        {
            TimeSpan t = TimeSpan.FromDays(LangeWoche);

            return AnfangWoche + t;
        }
        public Woche(DateTime AnfangWoche, int lange)
        {
            LangeWoche = lange;
            this.AnfangWoche = AnfangWoche;

        }


    }
    class GenericKalendarView : UserControl
    {


        public string PfadName { get { return "Kalendar View"; } }
        public int NestLvl { get { return 0; } }
        public Control GetControl() { return this; }
        public Panel WorkSpace { get { IHaveAWorkspace s = (IHaveAWorkspace)this.ParentForm; return s.WorkSpace; } }

        public string PluginName { get { return "Kalendar View"; } }

        public Control Control { get { return this; } }

        public string Category { get { return "Data"; } }

        IDataProvider<ITermin> dataProvider;
        Kalendar CurrentKalendar;
        public NewWeekView WView;
        public Panel panelWeekView;
        //WeekButton lastButton;
        List<List<ITerminProperty>> propertyList;
        ITermin Termin;
        int days = 5;
        int hours = 16;
        int dayBegin = 8;



        //  IControlKalendarUndFernseher KalendarController { get; set; }
        public GenericKalendarView(Kalendar k, IDataProvider<ITermin> controller, List<List<ITerminProperty>> props, int days, int hours, int dayBegin, ITermin t)
        {
            Termin = t;
            this.days = days;
            this.hours = hours;
            this.dayBegin = dayBegin;


            CurrentKalendar = k;
            dataProvider = controller;
            this.DoubleBuffered = true;
            InitializeComponent();

            propertyList = props;
            KaledarControl kalendar = new KaledarControl(k);
            kalendar.Location = (new Point(300, 200));
            this.Dock = DockStyle.Fill;

            this.panelWeekView.Dock = DockStyle.Fill;
            this.Controls.Add(kalendar);
            this.BackColor = Color.WhiteSmoke;
            kalendar.Show();
            kalendar.BringToFront();


            kalendar.listView.SelectedIndexChanged += new EventHandler(KalendarWocheOnClick);
            //foreach (Control btn in Kalendar.Controls)
            //{
            //    if (btn is WeekButton) { btn.Click += new EventHandler(KalendarWocheOnClick); }


            //}


            //kalendar.Dock = DockStyle.Right;    
        }

        private void KalendarWocheOnClick(object sender, EventArgs e)
        {


            this.panelWeekView.Controls.Clear();

            ListView b = (ListView)sender;

            if (b.SelectedItems.Count > 0)
            {
                ListViewItem c = b.SelectedItems[0];
                WeekWithTermin d = (WeekWithTermin)c.Tag;

                NewWeekView WocheView = new NewWeekView(dataProvider, d.Week.AnfangWoche, days, hours, dayBegin, propertyList, Termin, new Size(this.WorkSpace.Width, this.WorkSpace.Height));
                WocheView.Location = new Point(0, 0);
                WocheView.Dock = DockStyle.Fill;

                this.panelWeekView.Controls.Add(WocheView);

                WView = WocheView;

            }



            //lastButton = b;



        }

        private void InitializeComponent()
        {
            this.panelWeekView = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelWeekView
            // 
            this.panelWeekView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWeekView.AutoSize = true;
            this.panelWeekView.BackColor = System.Drawing.Color.Transparent;
            this.panelWeekView.Location = new System.Drawing.Point(3, 3);
            this.panelWeekView.Name = "panelWeekView";
            this.panelWeekView.Size = new System.Drawing.Size(1233, 843);
            this.panelWeekView.TabIndex = 1;
            // 
            // KalendarView
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelWeekView);
            this.Name = "KalendarView";
            this.Size = new System.Drawing.Size(1244, 849);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


    }
}
