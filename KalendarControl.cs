using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public partial class KaledarControl : MobilePanel
    {
        public System.Windows.Forms.ListView listView;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Kalendar Kalendar { get; set; }


        public KaledarControl(Kalendar K) : base()
        {

            InitializeComponent();

            this.TitleString = "Wochen Kalendar";

            MinimumSize = new Size(240, 50);

            Kalendar = K;

            initKalendarView();

            //this.MinimumSize = new System.Drawing.Size(50, 50);
            this.MaximumSize = new System.Drawing.Size(300, 270);
            this.storedSize = this.MaximumSize;
            this.Size = MaximumSize;



        }


        public void initKalendarView()
        {
            listView = new System.Windows.Forms.ListView
            {
                View = View.LargeIcon,
                // Dock = DockStyle.Fill,
                // 
                Size = new Size(240,
                170),
                Location = new Point(50, 50),
                LargeImageList = new ImageList()
            };
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);   // o 64x64
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            Image icon = ImageBase64Converter.Base64ToImage(IconAssets.Calendar);
            listView.BorderStyle = BorderStyle.None;
            
            listView.LargeImageList.Images.Add(icon);
            listView.BackColor = Color.DarkGray;
            listView.ForeColor = Color.Black;
            // Configura le dimensioni delle immagini
            listView.LargeImageList.ImageSize = new Size(32, 32); // Imposta la dimensione delle icone
            listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // Aggiungi immagini all'ImageList
            //listView.LargeImageList.Images.Add(Kiosk.Properties.Pics.calendar_week_17654902);
            for (int i = 0; i < Kalendar.weekLessonKalendar.Count; i++)
            {


                listView.Items.Add(CreateItem(Kalendar.weekLessonKalendar[i]));



            }


            // Aggiungi la ListView al form
            this.Controls.Add(listView);
            listView.BringToFront(); // Porta davanti



            for (int j = 0; j < listView.Items.Count; j++)
            {

                listView.Items[j].ImageIndex = 0;





            }

            //DateTimePicker Year = new DateTimePicker();

            //// Posizionamento
            //Year.Location = new Point(60, this.ClientSize.Height +20); 
            //Year.Size = new System.Drawing.Size(100, 33);

            //// Formato e stile
            //Year.Format = DateTimePickerFormat.Custom;
            //Year.CustomFormat = "yyyy";
            //Year.ShowUpDown = true; // Solo per selezione anno
            //Year.Value = DateTime.Now;

            //// Font
            //Year.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);

            //// Aggiunta al form
            //this.Controls.Add(Year);
            //Year.BringToFront(); // Porta davanti
            //this.Refresh(); // Ridisegna il form


            //int rows = (int)Math.Ceiling(Kalendar.weekLessonKalendar.Count / 4.0);
            //int cols = 4; 

            //WeekButton[][] arrayButtons = new WeekButton[rows][];
            //for (int i = 0; i < rows; i++)
            //{
            //    arrayButtons[i] = new WeekButton[cols];
            //}

            //int z = 0, f = 0;
            //for (int i = 1; i <= Kalendar.weekLessonKalendar.Count; i++)
            //{
            //    arrayButtons[z][f] = CreateButton(x: x, y: y,  Kalendar.weekLessonKalendar[i - 1]);


            //    if (i % cols == 0)
            //    {
            //        y += 35;
            //        x = 0;
            //        z++;
            //        f = 0;
            //    }
            //    else
            //    {
            //        x += 60;
            //        f++;     
            //    }
            //}



            //foreach (WeekButton[] k in arrayButtons) { this.Controls.AddRange(k); }
            //this.MaximumSize = this.Size;  this.AutoSize = false;


        }

        public ListViewItem CreateItem(/*int x , int y ,*/WeekWithTermin week)
        {
            ListViewItem item = new ListViewItem();
            item.Tag = week;
            DateTime ende = week.Week.EndeWoche();
            item.Text = week.Week.AnfangWoche.ToString("dd/MM") + System.Environment.NewLine + ende.ToString("dd/MM");


            return item;


            //WeekButton button = new WeekButton(week);
            //button. Location = new Point( x , y );      

            //button.Size = new Size(60,40);
            //
            //button.Text = week.Week.AnfangWoche.ToString("dd/MM") + System.Environment.NewLine + ende.ToString("dd/MM"); 
            //button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //button.ForeColor = Color.White;
            //button.BorderRadius = 40;
            //button.BorderColor = Color.LightGray;
            //button.ButtonColor = Color.Gray; ;

            //return button;




        }
    }
}
