using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace KanBankası
{
    public partial class Form1 : Form
    {
        private List<Bagisci> bagisciListesi = new List<Bagisci>();
        private int seciliIndex = -1; // Düzenleme yaparken hangi satırda olduğumuzu tutar

        // Nesneler
        private TextBox txtAdSoyad, txtYas, txtTelefon;
        private ComboBox cmbKanGrubu;
        private Button btnKaydet, btnResimSec, btnGuncelle, btnSil;
        private DataGridView dataGridView1;
        private PictureBox pbVesikalik;
        private string secilenResimYolu = "";

        public Form1()
        {
            this.Text = "Kan Bankası - Kayıt Düzenleme Sistemi";
            this.Size = new Size(850, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            ArayuzuOlustur();
            ListeyiGuncelle();
        }

        private void ArayuzuOlustur()
        {
            // Sol Giriş Paneli
            Label lblAd = new Label() { Text = "Ad Soyad:", Left = 20, Top = 20, Width = 80 };
            txtAdSoyad = new TextBox() { Left = 110, Top = 20, Width = 150 };

            Label lblYas = new Label() { Text = "Yaş:", Left = 20, Top = 50, Width = 80 };
            txtYas = new TextBox() { Left = 110, Top = 50, Width = 150 };

            Label lblTel = new Label() { Text = "Telefon:", Left = 20, Top = 80, Width = 80 };
            txtTelefon = new TextBox() { Left = 110, Top = 80, Width = 150 };

            Label lblKan = new Label() { Text = "Kan Grubu:", Left = 20, Top = 110, Width = 80 };
            cmbKanGrubu = new ComboBox() { Left = 110, Top = 110, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbKanGrubu.Items.AddRange(new string[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "0+", "0-" });

            // Resim Alanı
            pbVesikalik = new PictureBox() { Left = 300, Top = 20, Size = new Size(120, 150), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.WhiteSmoke };
            btnResimSec = new Button() { Text = "Fotoğraf Seç", Left = 300, Top = 175, Width = 120 };
            btnResimSec.Click += (s, e) => {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Resimler|*.jpg;*.png" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        secilenResimYolu = ofd.FileName;
                        pbVesikalik.Image = Image.FromFile(ofd.FileName);
                    }
                }
            };

            // Butonlar
            btnKaydet = new Button() { Text = "Yeni Kayıt", Left = 20, Top = 160, Width = 80, Height = 35, BackColor = Color.LightGreen };
            btnKaydet.Click += btnKaydet_Click;

            btnGuncelle = new Button() { Text = "Güncelle", Left = 105, Top = 160, Width = 80, Height = 35, BackColor = Color.LightSkyBlue, Enabled = false };
            btnGuncelle.Click += btnGuncelle_Click;

            btnSil = new Button() { Text = "Sil", Left = 190, Top = 160, Width = 70, Height = 35, BackColor = Color.Tomato, Enabled = false };
            btnSil.Click += btnSil_Click;

            // Tablo (DataGridView)
            dataGridView1 = new DataGridView() { Left = 20, Top = 230, Size = new Size(790, 350), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dataGridView1.CellClick += DataGridView1_CellClick; // TIKLAMA OLAYI

            // Ekleme
            this.Controls.Add(lblAd); this.Controls.Add(txtAdSoyad);
            this.Controls.Add(lblYas); this.Controls.Add(txtYas);
            this.Controls.Add(lblTel); this.Controls.Add(txtTelefon);
            this.Controls.Add(lblKan); this.Controls.Add(cmbKanGrubu);
            this.Controls.Add(pbVesikalik); this.Controls.Add(btnResimSec);
            this.Controls.Add(btnKaydet); this.Controls.Add(btnGuncelle); this.Controls.Add(btnSil);
            this.Controls.Add(dataGridView1);
        }

        // Tablodan bir isme veya satıra tıklayınca verileri kutulara doldurur
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                seciliIndex = e.RowIndex;
                Bagisci secili = bagisciListesi[seciliIndex];

                txtAdSoyad.Text = secili.AdSoyad;
                txtYas.Text = secili.Yas.ToString();
                txtTelefon.Text = secili.Telefon;
                cmbKanGrubu.SelectedItem = secili.KanGrubu;
                secilenResimYolu = secili.ResimYolu;

                if (!string.IsNullOrEmpty(secilenResimYolu) && File.Exists(secilenResimYolu))
                    pbVesikalik.Image = Image.FromFile(secilenResimYolu);
                else
                    pbVesikalik.Image = null;

                btnGuncelle.Enabled = true;
                btnSil.Enabled = true;
                btnKaydet.Enabled = false;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAdSoyad.Text)) return;
            bagisciListesi.Add(BilgileriAl());
            ListeyiGuncelle();
            FormuTemizle();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (seciliIndex >= 0)
            {
                bagisciListesi[seciliIndex] = BilgileriAl();
                ListeyiGuncelle();
                FormuTemizle();
                MessageBox.Show("Kayıt güncellendi.");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (seciliIndex >= 0)
            {
                bagisciListesi.RemoveAt(seciliIndex);
                ListeyiGuncelle();
                FormuTemizle();
            }
        }

        private Bagisci BilgileriAl()
        {
            return new Bagisci
            {
                AdSoyad = txtAdSoyad.Text,
                KanGrubu = cmbKanGrubu.SelectedItem?.ToString(),
                Yas = int.TryParse(txtYas.Text, out int y) ? y : 0,
                Telefon = txtTelefon.Text,
                ResimYolu = secilenResimYolu
            };
        }

        private void ListeyiGuncelle()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bagisciListesi;
        }

        private void FormuTemizle()
        {
            txtAdSoyad.Clear(); txtYas.Clear(); txtTelefon.Clear();
            cmbKanGrubu.SelectedIndex = -1;
            pbVesikalik.Image = null;
            secilenResimYolu = "";
            seciliIndex = -1;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
            btnKaydet.Enabled = true;
        }
    }

    public class Bagisci
    {
        public string? AdSoyad { get; set; }
        public string? KanGrubu { get; set; }
        public int Yas { get; set; }
        public string? Telefon { get; set; }
        public string? ResimYolu { get; set; }
    }
}