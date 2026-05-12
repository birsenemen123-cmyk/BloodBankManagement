using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Drawing2D;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Nesne Tanýmlamalarý
        private TextBox txtAdi, txtSoyadi, txtTC, txtYasi;
        private ComboBox cmbKanGrubu;
        private Label lblBaslik;
        private Button btnOnay, btnSil;
        private DataGridView dgwListe;
        private DataTable tablo;
        private PictureBox pbLogo;

        public Form1()
        {
            // Tasarýmý yükle
            InitializeCustomDesign();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form yüklendiðinde yapýlacak iþlemler buraya gelebilir
            TabloYapisiKur();
        }

        private void TabloYapisiKur()
        {
            tablo = new DataTable();
            tablo.Columns.Add("TC NO");
            tablo.Columns.Add("AD");
            tablo.Columns.Add("SOYAD");
            tablo.Columns.Add("YAÞ");
            tablo.Columns.Add("KAN GRUBU");
            dgwListe.DataSource = tablo;
        }

        private void InitializeCustomDesign()
        {
            // --- Form Ayarlarý ---
            this.Text = "Kan Bankasý Yönetim Sistemi";
            this.Size = new Size(700, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Load += new EventHandler(Form1_Load); // Load olayýný baðladýk

            // --- Logo ---
            pbLogo = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point(30, 15),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = CreateDropIcon() // Dinamik çizilen ikon
            };

            // --- Baþlýk ---
            lblBaslik = new Label
            {
                Text = "KAN BANKASI OTOMASYONU",
                Location = new Point(110, 30),
                Size = new Size(400, 40),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 0, 0)
            };

            // --- Giriþ Grubu ---
            GroupBox gbGiris = new GroupBox
            {
                Text = " Baðýþçý Kayýt Paneli ",
                Location = new Point(30, 90),
                Size = new Size(450, 230),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            int xL = 20, xT = 150, y = 35, gap = 35;

            txtAdi = CreateInput(gbGiris, "AD:", xL, xT, y);
            txtSoyadi = CreateInput(gbGiris, "SOYAD:", xL, xT, y + gap);
            txtTC = CreateInput(gbGiris, "TC KÝMLÝK:", xL, xT, y + (gap * 2));
            txtYasi = CreateInput(gbGiris, "YAÞ:", xL, xT, y + (gap * 3));

            // Kan Grubu ComboBox
            Label lblKG = new Label { Text = "KAN GRUBU:", Location = new Point(xL, y + (gap * 4)), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            cmbKanGrubu = new ComboBox
            {
                Location = new Point(xT, y + (gap * 4) - 3),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbKanGrubu.Items.AddRange(new object[] { "A Rh+", "A Rh-", "B Rh+", "B Rh-", "AB Rh+", "AB Rh-", "0 Rh+", "0 Rh-" });
            cmbKanGrubu.SelectedIndex = 0;
            gbGiris.Controls.Add(lblKG);
            gbGiris.Controls.Add(cmbKanGrubu);

            // --- Butonlar ---
            btnOnay = CreateButton("KAYDET", 500, 110, Color.FromArgb(46, 204, 113));
            btnOnay.Click += btnOnay_Click;

            btnSil = CreateButton("TEMÝZLE", 500, 180, Color.FromArgb(231, 76, 60));
            btnSil.Click += btnSil_Click;

            // --- Veri Tablosu ---
            dgwListe = new DataGridView
            {
                Location = new Point(30, 340),
                Size = new Size(620, 240),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            // Kontrolleri Forma Ekle
            this.Controls.AddRange(new Control[] { pbLogo, lblBaslik, gbGiris, btnOnay, btnSil, dgwListe });
        }

        // Yardýmcý Metotlar
        private TextBox CreateInput(GroupBox gb, string txt, int xL, int xT, int y)
        {
            Label lbl = new Label { Text = txt, Location = new Point(xL, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            TextBox tb = new TextBox { Location = new Point(xT, y - 3), Width = 250 };
            gb.Controls.Add(lbl); gb.Controls.Add(tb);
            return tb;
        }

        private Button CreateButton(string txt, int x, int y, Color c)
        {
            return new Button
            {
                Text = txt,
                Location = new Point(x, y),
                Size = new Size(140, 50),
                BackColor = c,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private Bitmap CreateDropIcon()
        {
            Bitmap bmp = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(Brushes.Crimson, 12, 20, 40, 40);
                Point[] p = { new Point(32, 2), new Point(12, 35), new Point(52, 35) };
                g.FillPolygon(Brushes.Crimson, p);
            }
            return bmp;
        }

        private void btnOnay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAdi.Text) || string.IsNullOrWhiteSpace(txtTC.Text))
            {
                MessageBox.Show("Lütfen zorunlu alanlarý doldurun!", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tablo.Rows.Add(txtTC.Text, txtAdi.Text, txtSoyadi.Text, txtYasi.Text, cmbKanGrubu.Text);
            Temizle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            txtAdi.Clear(); txtSoyadi.Clear(); txtTC.Clear(); txtYasi.Clear();
            cmbKanGrubu.SelectedIndex = 0;
            txtAdi.Focus();
        }
    }
}