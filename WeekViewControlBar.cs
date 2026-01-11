using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    public partial class WeekViewControlBar : UserControl
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
        InvisibleButton btnSave { get; set; }



        NewWeekView Parent { get; set; }

        event EventHandler generating;

        ITerminGenerator _generator;

        public ComboBox comboBoxEmpfindlichkei = new ComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,



        };
        public WeekViewControlBar(NewWeekView parent, EventHandler SaveEvent)
        {
            InitializeComponent();
            Parent = parent;
            this.Width = parent.Width; this.Height = 40;
            this.Location = (new Point(0, 0));
            this.BackColor = Color.DarkGray;
            this.ForeColor = Color.White;

            btnSave = new InvisibleButton(this, IconAssets.Floppy, IconAssets.Floppy);
            btnSave.Click += SaveEvent;
            this.btnSave.Location = new Point(100, 5);
            this.btnSave.Size = new Size(35, 30);
            this.btnSave.ForeColor = Color.White;

            this.Region = new Region(new RectangleF(0, 0, Width, Height));


            comboBoxEmpfindlichkei.Items.AddRange(new object[] { 1, 5, 10, 15, 30 });
            comboBoxEmpfindlichkei.Size = new Size(50, 20);
            comboBoxEmpfindlichkei.Location = new Point(10, 5);
            comboBoxEmpfindlichkei.SelectedIndex = 1;
            comboBoxEmpfindlichkei.SelectedItem = 5;
            comboBoxEmpfindlichkei.SelectedIndexChanged += OnChnageIndex;
            this.Controls.Add(comboBoxEmpfindlichkei);
            comboBoxEmpfindlichkei.BringToFront();
            this.Controls.Add(btnSave);
            this.Paint += OnPaint;
            
        }


        private void OnChnageIndex(object sender, EventArgs e)

        {
            Parent.MinutiSnap = Convert.ToInt32(comboBoxEmpfindlichkei.SelectedItem);

        }

        void OnPaint(object sender, PaintEventArgs e)
        {


            using (Pen p = new Pen(Color.DarkSlateGray, 3))
            {

                e.Graphics.DrawRectangle(p, new Rectangle(2, 2, Width - 2, Height - 6));



            }


        }

    }
}
