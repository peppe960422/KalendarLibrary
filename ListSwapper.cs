 using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Security.Cryptography.X509Certificates;

namespace KalendarLibrary
{
  
        public class ListSwapper : UserControl
        {
            System.Windows.Forms.TextBox SearchBar = new System.Windows.Forms.TextBox();
            InvisibleButton btnSearch;
            ImageList imageList = new ImageList();
            public ListSwapper(List<ITerminProperty> list)
            {
                InitializeComponent();
                SearchBar.Location = new Point(listViewNotPart.Left, 0);
                SearchBar.Size = new Size(80, 30);
                btnSearch = new InvisibleButton(this, IconAssets.Lupe, IconAssets.Lupe);
                btnSearch.Location = new Point(SearchBar.Right + 2, 0);
                btnSearch.Size = new Size(30, 27);
                Image i = new Bitmap(ImageBase64Converter.Base64ToImage(IconAssets.ObjImg));
                imageList.Images.Add(i);
                btnSearch.Click += SearchClick;
                btnSearch.Enabled = false;

                listViewNotPart.LargeImageList = imageList;
                listViewPart.LargeImageList = imageList;


                this.Controls.Add(SearchBar);
                this.Controls.Add(btnSearch);
                SearchBar.TextChanged += SearchBar_TextChanged;
                foreach (var item in list)
                {
                    ListViewItem r = new ListViewItem();
                    r.Tag = item;
                    r.Text = item.GetDisplayName();
                    r.ImageIndex = 0;
                    listViewNotPart.Items.Add(r);
                }

                this.roundGradientButtonAdd.Click += new EventHandler(btnAddFach_Click);
                this.roundGradientButtonSub.Click += new EventHandler(btnRemoveFach_Click);
            }

            private void SearchBar_TextChanged(object? sender, EventArgs e)
            {
                for (int i = 0; i < SearchBar.Text.Length; i++)
                {

                    if (i > 2)
                    {
                        btnSearch.Enabled = true;
                        return;
                    }

                }

                btnSearch.Enabled = false;

            }


            private void SearchClick(object sender, EventArgs e)
            {
                string text = SearchBar.Text;


                for (int i = 0; i < listViewNotPart.Items.Count; i++)
                {
                    ITerminProperty p = (ITerminProperty)listViewNotPart.Items[i].Tag;
                    if (p.GetDisplayName().Contains(text))
                    {
                        listViewNotPart.Items[i].Selected = true;
                        listViewNotPart.Select();
                        break;

                    }
                 ;


                }
            }







            public List<ITerminProperty> GetSelectedItems()
            {
                List<ITerminProperty> result = new List<ITerminProperty>();
                foreach (ListViewItem item in listViewPart.Items)
                {
                    if (item.Tag is ITerminProperty prop)
                    {
                        result.Add(prop);
                    }
                }
                return result;
            }

            private void btnAddFach_Click(object sender, EventArgs e)
            {
                SwapElementsListView(listViewPart, listViewNotPart);
            }

            private void btnRemoveFach_Click(object sender, EventArgs e)
            {
                SwapElementsListView(listViewNotPart, listViewPart);

            }

            protected void SwapElementsListView(ListView Reciver, ListView Sender)
            {




                if (Sender.SelectedItems.Count > 0)
                {
                    for (int i = Sender.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        ListViewItem item = (ListViewItem)Sender.SelectedItems[i].Clone();

                        Sender.Items.Remove(Sender.SelectedItems[i]);

                        Reciver.Items.Add(item);
                    }

                }
                for (int j = 0; j < Reciver.Items.Count; j++)
                {

                    if (Reciver.Items[j].ImageIndex == -1)
                    {
                        Reciver.Items[j].ImageIndex = 0;


                    }


                }

            }


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
        this.BackColor = Color.DarkGray;
        roundGradientButtonAdd = new InvisibleButton(this, IconAssets.ArrowLeft, IconAssets.ArrowLeft);
        roundGradientButtonSub = new InvisibleButton(this, IconAssets.ArrowRight, IconAssets.ArrowRight);
        listViewPart = new ListView();
        listViewNotPart = new ListView();
        SuspendLayout();
        // 
        // roundGradientButtonAdd
        // 

        roundGradientButtonAdd.Location = new Point(235, 74);
        roundGradientButtonAdd.Name = "roundGradientButtonAdd";
        roundGradientButtonAdd.Size = new Size(30, 30);
        roundGradientButtonAdd.TabIndex = 0;
        roundGradientButtonAdd.Text = "+";
        roundGradientButtonAdd.UseVisualStyleBackColor = true;
        // 
        // roundGradientButtonSub
        // 

        roundGradientButtonSub.Location = new Point(235, 127);
        roundGradientButtonSub.Name = "roundGradientButtonSub";
        roundGradientButtonSub.Size = new Size(30, 30);
        roundGradientButtonSub.TabIndex = 1;
        roundGradientButtonSub.Text = "-";
        roundGradientButtonSub.UseVisualStyleBackColor = true;
        // 
        // listViewPart
        // 
        listViewPart.Location = new Point(12, 30);
        listViewPart.Name = "listViewPart";
        listViewPart.Size = new Size(217, 198);
        listViewPart.TabIndex = 2;
        listViewPart.UseCompatibleStateImageBehavior = false;
        // 
        // listViewNotPart
        // 
        listViewNotPart.Location = new Point(271, 30);
        listViewNotPart.Name = "listViewNotPart";
        listViewNotPart.Size = new Size(217, 198);
        listViewNotPart.TabIndex = 3;
        listViewNotPart.UseCompatibleStateImageBehavior = false;
        // 
        // ListSwapper
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(listViewNotPart);
        Controls.Add(listViewPart);
        Controls.Add(roundGradientButtonSub);
        Controls.Add(roundGradientButtonAdd);
        Name = "ListSwapper";
        Size = new Size(504, 225);
        ResumeLayout(false);
    }

    #endregion

    private InvisibleButton roundGradientButtonAdd;
    private InvisibleButton roundGradientButtonSub;
    private ListView listViewPart;
    private ListView listViewNotPart;


        }
    

}
