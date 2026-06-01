using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BinaPlus.BL;
using BinaPlus.DAL;

namespace BinaPlus.UI
{
    public class AnaForm : Form
    {
        private TabControl tabControl;
        private TabPage tabDaireler, tabKisiler, tabAidatlar, tabOdemeler, tabBakim, tabSakinler;

        private DaireBL daireBL = new DaireBL();
        private KisiBL kisiBL = new KisiBL();
        private AidatBL aidatBL = new AidatBL();
        private OdemeBL odemeBL = new OdemeBL();
        private BakimTalepBL bakimBL = new BakimTalepBL();
        private DaireSakinBL sakinBL = new DaireSakinBL();

        public AnaForm()
        {
            InitializeComponent();
            this.Load += AnaForm_Load;
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            YukleTab_Daireler();
            YukleTab_Kisiler();
            YukleTab_Aidatlar();
            YukleTab_Odemeler();
            YukleTab_Bakim();
            YukleTab_Sakinler();
        }

        private void InitializeComponent()
        {
            this.Text = "BinaPlus - Apartman Yönetim Sistemi";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9f);
            this.BackColor = Color.WhiteSmoke;

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);

            tabDaireler = new TabPage("🏠 Daireler");
            tabKisiler = new TabPage("👤 Kişiler");
            tabAidatlar = new TabPage("📋 Aidatlar");
            tabOdemeler = new TabPage("💰 Ödemeler");
            tabBakim = new TabPage("🔧 Bakım Talepleri");
            tabSakinler = new TabPage("🏘 Daire Sakinleri");

            tabControl.TabPages.AddRange(new TabPage[] {
                tabDaireler, tabKisiler, tabAidatlar, tabOdemeler, tabBakim, tabSakinler
            });

            this.Controls.Add(tabControl);
        }

        // ====================================================
        // YARDIMCI - standart panel oluşturma
        // ====================================================
        private (DataGridView dgv, TextBox txtAra, Panel panelBtnTop, Panel panelForm) OlusturStandartLayout(TabPage tab)
        {
            var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };

            // Üst toolbar
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(52, 73, 94) };
            var lblTitle = new Label
            {
                Text = "BinaPlus",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 12)
            };
            panelTop.Controls.Add(lblTitle);

            // Arama
            var panelArama = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.FromArgb(236, 240, 241) };
            var lblAra = new Label { Text = "Ara:", AutoSize = true, Location = new Point(10, 12) };
            var txtAra = new TextBox { Location = new Point(45, 8), Width = 300, Height = 24 };
            panelArama.Controls.AddRange(new Control[] { lblAra, txtAra });

            // Buton paneli
            var panelBtnTop = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.FromArgb(236, 240, 241) };

            // Form paneli (sağda)
            var panelForm = new Panel
            {
                Dock = DockStyle.Right,
                Width = 350,
                BackColor = Color.White,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Grid
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(189, 195, 199),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(245, 247, 249)
                }
            };

            mainPanel.Controls.Add(dgv);
            mainPanel.Controls.Add(panelForm);

            tab.Controls.Add(mainPanel);
            tab.Controls.Add(panelBtnTop);
            tab.Controls.Add(panelArama);
            tab.Controls.Add(panelTop);

            return (dgv, txtAra, panelBtnTop, panelForm);
        }

        private Button OlusturButon(string text, Color back, int x)
        {
            return new Button
            {
                Text = text,
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(x, 8),
                Size = new Size(100, 26),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private Label OlusturLabel(string text, int y, Panel p)
        {
            var lbl = new Label { Text = text, AutoSize = true, Location = new Point(10, y), ForeColor = Color.FromArgb(44, 62, 80) };
            p.Controls.Add(lbl);
            return lbl;
        }

        private TextBox OlusturTextBox(int y, Panel p, int width = 300)
        {
            var txt = new TextBox { Location = new Point(10, y), Width = width, Height = 24 };
            p.Controls.Add(txt);
            return txt;
        }

        private ComboBox OlusturComboBox(int y, Panel p, string[] items)
        {
            var cmb = new ComboBox { Location = new Point(10, y), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmb.Items.AddRange(items);
            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
            p.Controls.Add(cmb);
            return cmb;
        }

        // ====================================================
        // TAB - DAİRELER
        // ====================================================
        private DataGridView dgvDaireler;
        private TextBox txtDBlok, txtDNo, txtDm2;
        private ComboBox cmbDTip, cmbDDurum;
        private string secilenDaireId = null;

        private void YukleTab_Daireler()
        {
            var (dgv, txtAra, panelBtn, panelForm) = OlusturStandartLayout(tabDaireler);
            dgvDaireler = dgv;

            var btnEkle = OlusturButon("➕ Ekle", Color.FromArgb(39, 174, 96), 5);
            var btnGuncelle = OlusturButon("✏ Güncelle", Color.FromArgb(41, 128, 185), 110);
            var btnSil = OlusturButon("🗑 Sil", Color.FromArgb(192, 57, 43), 215);
            var btnTemizle = OlusturButon("🔄 Temizle", Color.FromArgb(127, 140, 141), 320);
            panelBtn.Controls.AddRange(new Control[] { btnEkle, btnGuncelle, btnSil, btnTemizle });

            // Form
            int y = 10;
            OlusturLabel("Blok:", y, panelForm); y += 22;
            txtDBlok = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Daire No:", y, panelForm); y += 22;
            txtDNo = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Tip:", y, panelForm); y += 22;
            cmbDTip = OlusturComboBox(y, panelForm, new[] { "1+0", "1+1", "2+1", "3+1", "4+1", "Dubleks" }); y += 34;
            OlusturLabel("m²:", y, panelForm); y += 22;
            txtDm2 = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Durum:", y, panelForm); y += 22;
            cmbDDurum = OlusturComboBox(y, panelForm, new[] { "Bos", "Dolu", "Satilik", "Kiralik" });

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    daireBL.Ekle(txtDBlok.Text.Trim(), txtDNo.Text.Trim(),
                        cmbDTip.Text, float.Parse(txtDm2.Text), cmbDDurum.Text);
                    DaireListele(); TemizleDaire();
                    MessageBox.Show("Daire eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnGuncelle.Click += (s, e) =>
            {
                if (secilenDaireId == null) { MessageBox.Show("Lütfen bir daire seçin."); return; }
                try
                {
                    daireBL.Guncelle(secilenDaireId, txtDBlok.Text.Trim(), txtDNo.Text.Trim(),
                        cmbDTip.Text, float.Parse(txtDm2.Text), cmbDDurum.Text);
                    DaireListele(); TemizleDaire();
                    MessageBox.Show("Güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnSil.Click += (s, e) =>
            {
                if (secilenDaireId == null) { MessageBox.Show("Lütfen bir daire seçin."); return; }
                if (MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try { daireBL.Sil(secilenDaireId); DaireListele(); TemizleDaire(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            btnTemizle.Click += (s, e) => TemizleDaire();

            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                secilenDaireId = row.Cells["ID"].Value?.ToString();
                txtDBlok.Text = row.Cells["Blok"].Value?.ToString();
                txtDNo.Text = row.Cells["No"].Value?.ToString();
                cmbDTip.Text = row.Cells["Tip"].Value?.ToString();
                txtDm2.Text = row.Cells["m2"].Value?.ToString();
                cmbDDurum.Text = row.Cells["Durum"].Value?.ToString();
            };

            txtAra.TextChanged += (s, e) =>
            {
                string f = txtAra.Text.Trim();
                dgv.DataSource = string.IsNullOrEmpty(f) ? daireBL.HepsiniGetir() : daireBL.Ara(f);
            };

            DaireListele();
        }

        private void DaireListele() { dgvDaireler.DataSource = daireBL.HepsiniGetir(); }
        private void TemizleDaire()
        {
            secilenDaireId = null;
            txtDBlok.Text = txtDNo.Text = txtDm2.Text = "";
            cmbDTip.SelectedIndex = 0; cmbDDurum.SelectedIndex = 0;
        }

        // ====================================================
        // TAB - KİŞİLER
        // ====================================================
        private DataGridView dgvKisiler;
        private TextBox txtKTC, txtKAd, txtKSoyad, txtKTel, txtKMail;
        private ComboBox cmbKRol;
        private string secilenKisiId = null;

        private void YukleTab_Kisiler()
        {
            var (dgv, txtAra, panelBtn, panelForm) = OlusturStandartLayout(tabKisiler);
            dgvKisiler = dgv;

            var btnEkle = OlusturButon("➕ Ekle", Color.FromArgb(39, 174, 96), 5);
            var btnGuncelle = OlusturButon("✏ Güncelle", Color.FromArgb(41, 128, 185), 110);
            var btnSil = OlusturButon("🗑 Sil", Color.FromArgb(192, 57, 43), 215);
            var btnBakiye = OlusturButon("💰 Bakiye", Color.FromArgb(142, 68, 173), 320);
            var btnTemizle = OlusturButon("🔄 Temizle", Color.FromArgb(127, 140, 141), 425);
            panelBtn.Controls.AddRange(new Control[] { btnEkle, btnGuncelle, btnSil, btnBakiye, btnTemizle });

            int y = 10;
            OlusturLabel("TC No:", y, panelForm); y += 22;
            txtKTC = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Ad:", y, panelForm); y += 22;
            txtKAd = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Soyad:", y, panelForm); y += 22;
            txtKSoyad = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Telefon:", y, panelForm); y += 22;
            txtKTel = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Mail:", y, panelForm); y += 22;
            txtKMail = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Rol:", y, panelForm); y += 22;
            cmbKRol = OlusturComboBox(y, panelForm, new[] { "Sahip", "Kiraci", "Yonetici" });

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    kisiBL.Ekle(txtKTC.Text.Trim(), txtKAd.Text.Trim(), txtKSoyad.Text.Trim(),
                        txtKTel.Text.Trim(), txtKMail.Text.Trim(), cmbKRol.Text);
                    KisiListele(); TemizleKisi();
                    MessageBox.Show("Kişi eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnGuncelle.Click += (s, e) =>
            {
                if (secilenKisiId == null) { MessageBox.Show("Lütfen bir kişi seçin."); return; }
                try
                {
                    kisiBL.Guncelle(secilenKisiId, txtKTC.Text.Trim(), txtKAd.Text.Trim(), txtKSoyad.Text.Trim(),
                        txtKTel.Text.Trim(), txtKMail.Text.Trim(), cmbKRol.Text);
                    KisiListele(); TemizleKisi();
                    MessageBox.Show("Güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnSil.Click += (s, e) =>
            {
                if (secilenKisiId == null) { MessageBox.Show("Lütfen bir kişi seçin."); return; }
                if (MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try { kisiBL.Sil(secilenKisiId); KisiListele(); TemizleKisi(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            btnBakiye.Click += (s, e) =>
            {
                if (secilenKisiId == null) { MessageBox.Show("Lütfen bir kişi seçin."); return; }
                float bakiye = kisiBL.BakiyeGetir(secilenKisiId);
                string mesaj = bakiye >= 0
                    ? $"Kişi bakiyesi: +{bakiye:F2} TL (Fazla ödeme)"
                    : $"Kişi bakiyesi: {bakiye:F2} TL (Borçlu)";
                MessageBox.Show(mesaj, "Bakiye Bilgisi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnTemizle.Click += (s, e) => TemizleKisi();

            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                secilenKisiId = row.Cells["ID"].Value?.ToString();
                txtKTC.Text = row.Cells["TCNo"].Value?.ToString();
                txtKAd.Text = row.Cells["Ad"].Value?.ToString();
                txtKSoyad.Text = row.Cells["Soyad"].Value?.ToString();
                txtKTel.Text = row.Cells["Telefon"].Value?.ToString();
                txtKMail.Text = row.Cells["Mail"].Value?.ToString();
                cmbKRol.Text = row.Cells["Rol"].Value?.ToString();
            };

            txtAra.TextChanged += (s, e) =>
            {
                string f = txtAra.Text.Trim();
                dgv.DataSource = string.IsNullOrEmpty(f) ? kisiBL.HepsiniGetir() : kisiBL.Ara(f);
            };

            KisiListele();
        }

        private void KisiListele() { dgvKisiler.DataSource = kisiBL.HepsiniGetir(); }
        private void TemizleKisi()
        {
            secilenKisiId = null;
            txtKTC.Text = txtKAd.Text = txtKSoyad.Text = txtKTel.Text = txtKMail.Text = "";
            cmbKRol.SelectedIndex = 0;
        }

        // ====================================================
        // TAB - AİDATLAR
        // ====================================================
        private DataGridView dgvAidatlar;
        private ComboBox cmbADaire, cmbAAy, cmbAYil, cmbADurum;
        private TextBox txtATutar;
        private DateTimePicker dtpASon;
        private string secilenAidatId = null;

        private void YukleTab_Aidatlar()
        {
            var (dgv, txtAra, panelBtn, panelForm) = OlusturStandartLayout(tabAidatlar);
            dgvAidatlar = dgv;

            var btnEkle = OlusturButon("➕ Ekle", Color.FromArgb(39, 174, 96), 5);
            var btnGuncelle = OlusturButon("✏ Güncelle", Color.FromArgb(41, 128, 185), 110);
            var btnSil = OlusturButon("🗑 Sil", Color.FromArgb(192, 57, 43), 215);
            var btnToplam = OlusturButon("Σ Toplam", Color.FromArgb(142, 68, 173), 320);
            var btnTemizle = OlusturButon("🔄 Temizle", Color.FromArgb(127, 140, 141), 425);
            panelBtn.Controls.AddRange(new Control[] { btnEkle, btnGuncelle, btnSil, btnToplam, btnTemizle });

            int y = 10;
            OlusturLabel("Daire:", y, panelForm); y += 22;
            cmbADaire = OlusturComboBox(y, panelForm, new string[0]); y += 34;
            OlusturLabel("Ay:", y, panelForm); y += 22;
            cmbAAy = OlusturComboBox(y, panelForm, new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" }); y += 34;
            OlusturLabel("Yıl:", y, panelForm); y += 22;
            cmbAYil = OlusturComboBox(y, panelForm, new[] { "2023", "2024", "2025", "2026", "2027" }); y += 34;
            OlusturLabel("Tutar (TL):", y, panelForm); y += 22;
            txtATutar = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Son Ödeme Tarihi:", y, panelForm); y += 22;
            dtpASon = new DateTimePicker { Location = new Point(10, y), Width = 300 };
            panelForm.Controls.Add(dtpASon); y += 34;
            OlusturLabel("Durum:", y, panelForm); y += 22;
            cmbADurum = OlusturComboBox(y, panelForm, new[] { "Odenmedi", "Kismi", "Tamamlandi" });

            // Daire combo doldur
            DaireComboDoldur(cmbADaire);

            // Refresh button to reload daires into combo
            var btnYenileD = OlusturButon("🔄 Daireleri Yükle", Color.FromArgb(22, 160, 133), 530);
            btnYenileD.Width = 140;
            panelBtn.Controls.Add(btnYenileD);
            btnYenileD.Click += (s, e) => { DaireComboDoldur(cmbADaire); };

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    var did = cmbADaire.SelectedValue?.ToString();
                    aidatBL.Ekle(did, int.Parse(cmbAAy.Text), int.Parse(cmbAYil.Text),
                        float.Parse(txtATutar.Text), dtpASon.Value, cmbADurum.Text);
                    AidatListele(); TemizleAidat();
                    MessageBox.Show("Aidat eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnGuncelle.Click += (s, e) =>
            {
                if (secilenAidatId == null) { MessageBox.Show("Lütfen bir aidat seçin."); return; }
                try
                {
                    var did = cmbADaire.SelectedValue?.ToString();
                    aidatBL.Guncelle(secilenAidatId, did, int.Parse(cmbAAy.Text), int.Parse(cmbAYil.Text),
                        float.Parse(txtATutar.Text), dtpASon.Value, cmbADurum.Text);
                    AidatListele(); TemizleAidat();
                    MessageBox.Show("Güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnSil.Click += (s, e) =>
            {
                if (secilenAidatId == null) { MessageBox.Show("Lütfen bir aidat seçin."); return; }
                if (MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try { aidatBL.Sil(secilenAidatId); AidatListele(); TemizleAidat(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            btnToplam.Click += (s, e) =>
            {
                float t = aidatBL.ToplamAl();
                MessageBox.Show($"Toplam Aidat Tutarı: {t:F2} TL", "Toplam", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnTemizle.Click += (s, e) => TemizleAidat();

            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                secilenAidatId = row.Cells["ID"].Value?.ToString();
                cmbAAy.Text = row.Cells["Ay"].Value?.ToString();
                cmbAYil.Text = row.Cells["Yil"].Value?.ToString();
                txtATutar.Text = row.Cells["Tutar"].Value?.ToString();
                cmbADurum.Text = row.Cells["Durum"].Value?.ToString();
                try
                {
                    var son = row.Cells["SonOdeme"].Value;
                    if (son != null && son != DBNull.Value)
                        dtpASon.Value = Convert.ToDateTime(son);
                }
                catch { }
            };

            txtAra.TextChanged += (s, e) =>
            {
                string f = txtAra.Text.Trim();
                dgv.DataSource = string.IsNullOrEmpty(f) ? aidatBL.HepsiniGetir() : aidatBL.Ara(f);
            };

            AidatListele();
        }

        private void AidatListele() { dgvAidatlar.DataSource = aidatBL.HepsiniGetir(); }
        private void TemizleAidat()
        {
            secilenAidatId = null;
            txtATutar.Text = "";
            cmbAAy.SelectedIndex = 0;
            // Select current year if available, otherwise last item
            string thisYear = DateTime.Now.Year.ToString();
            int yilIdx = cmbAYil.Items.IndexOf(thisYear);
            cmbAYil.SelectedIndex = yilIdx >= 0 ? yilIdx : cmbAYil.Items.Count - 1;
            cmbADurum.SelectedIndex = 0;
        }

        private void DaireComboDoldur(ComboBox cmb)
        {
            try
            {
                var dt = daireBL.HepsiniGetir();
                var view = new DataTable();
                view.Columns.Add("daire_id");
                view.Columns.Add("daire_adi");
                foreach (DataRow r in dt.Rows)
                    view.Rows.Add(r["ID"], $"Blok {r["Blok"]} No:{r["No"]}");
                cmb.DataSource = view;
                cmb.DisplayMember = "daire_adi";
                cmb.ValueMember = "daire_id";
            }
            catch { }
        }

        // ====================================================
        // TAB - ÖDEMELER
        // ====================================================
        private DataGridView dgvOdemeler;
        private ComboBox cmbOKisi, cmbOAidat, cmbOTur;
        private TextBox txtOTutar, txtOAciklama;
        private DateTimePicker dtpOTarih;
        private string secilenOdemeId = null;

        private void YukleTab_Odemeler()
        {
            var (dgv, txtAra, panelBtn, panelForm) = OlusturStandartLayout(tabOdemeler);
            dgvOdemeler = dgv;

            var btnEkle = OlusturButon("➕ Ekle", Color.FromArgb(39, 174, 96), 5);
            var btnGuncelle = OlusturButon("✏ Güncelle", Color.FromArgb(41, 128, 185), 110);
            var btnSil = OlusturButon("🗑 Sil", Color.FromArgb(192, 57, 43), 215);
            var btnToplam = OlusturButon("Σ Toplam", Color.FromArgb(142, 68, 173), 320);
            var btnTemizle = OlusturButon("🔄 Temizle", Color.FromArgb(127, 140, 141), 425);
            panelBtn.Controls.AddRange(new Control[] { btnEkle, btnGuncelle, btnSil, btnToplam, btnTemizle });

            int y = 10;
            OlusturLabel("Kişi:", y, panelForm); y += 22;
            cmbOKisi = OlusturComboBox(y, panelForm, new string[0]); y += 34;
            OlusturLabel("Aidat:", y, panelForm); y += 22;
            cmbOAidat = OlusturComboBox(y, panelForm, new string[0]); y += 34;
            OlusturLabel("Tarih:", y, panelForm); y += 22;
            dtpOTarih = new DateTimePicker { Location = new Point(10, y), Width = 300, Format = DateTimePickerFormat.Short };
            panelForm.Controls.Add(dtpOTarih); y += 34;
            OlusturLabel("Tutar (TL):", y, panelForm); y += 22;
            txtOTutar = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Ödeme Türü:", y, panelForm); y += 22;
            cmbOTur = OlusturComboBox(y, panelForm, new[] { "Nakit", "Kredi Karti", "Banka Havalesi" }); y += 34;
            OlusturLabel("Açıklama:", y, panelForm); y += 22;
            txtOAciklama = OlusturTextBox(y, panelForm);

            KisiComboDoldur(cmbOKisi);
            AidatComboDoldur(cmbOAidat);

            var btnYenileO = OlusturButon("🔄 Yenile", Color.FromArgb(22, 160, 133), 530);
            panelBtn.Controls.Add(btnYenileO);
            btnYenileO.Click += (s, e) => { KisiComboDoldur(cmbOKisi); AidatComboDoldur(cmbOAidat); };

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    odemeBL.Ekle(cmbOKisi.SelectedValue?.ToString(), cmbOAidat.SelectedValue?.ToString(),
                        dtpOTarih.Value, float.Parse(txtOTutar.Text), cmbOTur.Text, txtOAciklama.Text.Trim());
                    OdemeListele(); TemizleOdeme();
                    MessageBox.Show("Ödeme kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AidatListele(); // aidat durumu trigger ile değişmiş olabilir
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnGuncelle.Click += (s, e) =>
            {
                if (secilenOdemeId == null) { MessageBox.Show("Lütfen bir ödeme seçin."); return; }
                try
                {
                    odemeBL.Guncelle(secilenOdemeId, cmbOKisi.SelectedValue?.ToString(), cmbOAidat.SelectedValue?.ToString(),
                        dtpOTarih.Value, float.Parse(txtOTutar.Text), cmbOTur.Text, txtOAciklama.Text.Trim());
                    OdemeListele(); TemizleOdeme();
                    MessageBox.Show("Güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnSil.Click += (s, e) =>
            {
                if (secilenOdemeId == null) { MessageBox.Show("Lütfen bir ödeme seçin."); return; }
                if (MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try { odemeBL.Sil(secilenOdemeId); OdemeListele(); TemizleOdeme(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            btnToplam.Click += (s, e) =>
            {
                float t = odemeBL.ToplamAl();
                MessageBox.Show($"Toplam Ödeme Tutarı: {t:F2} TL", "Toplam", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnTemizle.Click += (s, e) => TemizleOdeme();

            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                secilenOdemeId = row.Cells["ID"].Value?.ToString();
                txtOTutar.Text = row.Cells["Tutar"].Value?.ToString();
                cmbOTur.Text = row.Cells["Tur"].Value?.ToString();
                txtOAciklama.Text = row.Cells["Aciklama"].Value?.ToString();
            };

            txtAra.TextChanged += (s, e) =>
            {
                string f = txtAra.Text.Trim();
                dgv.DataSource = string.IsNullOrEmpty(f) ? odemeBL.HepsiniGetir() : odemeBL.Ara(f);
            };

            OdemeListele();
        }

        private void OdemeListele() { dgvOdemeler.DataSource = odemeBL.HepsiniGetir(); }
        private void TemizleOdeme()
        {
            secilenOdemeId = null;
            txtOTutar.Text = txtOAciklama.Text = "";
            cmbOTur.SelectedIndex = 0;
        }

        private void KisiComboDoldur(ComboBox cmb)
        {
            try
            {
                var dt = kisiBL.HepsiniGetir();
                var view = new DataTable();
                view.Columns.Add("kisi_id");
                view.Columns.Add("kisi_adi");
                foreach (DataRow r in dt.Rows)
                    view.Rows.Add(r["ID"], $"{r["Ad"]} {r["Soyad"]}");
                cmb.DataSource = view;
                cmb.DisplayMember = "kisi_adi";
                cmb.ValueMember = "kisi_id";
            }
            catch { }
        }

        private void AidatComboDoldur(ComboBox cmb)
        {
            try
            {
                var dt = aidatBL.HepsiniGetir();
                var view = new DataTable();
                view.Columns.Add("aidat_id");
                view.Columns.Add("aidat_adi");
                foreach (DataRow r in dt.Rows)
                    view.Rows.Add(r["ID"], $"Daire: {r["DaireNo"]} - {r["Ay"]}/{r["Yil"]} ({r["Durum"]})");
                cmb.DataSource = view;
                cmb.DisplayMember = "aidat_adi";
                cmb.ValueMember = "aidat_id";
            }
            catch { }
        }

        // ====================================================
        // TAB - BAKIM TALEPLERİ
        // ====================================================
        private DataGridView dgvBakim;
        private ComboBox cmbBDaire, cmbBDurum;
        private TextBox txtBKategori, txtBAciklama;
        private DateTimePicker dtpBTarih, dtpBCozum;
        private CheckBox chkBCozum;
        private string secilenTalepId = null;

        private void YukleTab_Bakim()
        {
            var (dgv, txtAra, panelBtn, panelForm) = OlusturStandartLayout(tabBakim);
            dgvBakim = dgv;

            var btnEkle = OlusturButon("➕ Ekle", Color.FromArgb(39, 174, 96), 5);
            var btnGuncelle = OlusturButon("✏ Güncelle", Color.FromArgb(41, 128, 185), 110);
            var btnSil = OlusturButon("🗑 Sil", Color.FromArgb(192, 57, 43), 215);
            var btnTemizle = OlusturButon("🔄 Temizle", Color.FromArgb(127, 140, 141), 320);
            panelBtn.Controls.AddRange(new Control[] { btnEkle, btnGuncelle, btnSil, btnTemizle });

            int y = 10;
            OlusturLabel("Daire:", y, panelForm); y += 22;
            cmbBDaire = OlusturComboBox(y, panelForm, new string[0]); y += 34;
            OlusturLabel("Kategori:", y, panelForm); y += 22;
            txtBKategori = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Açıklama:", y, panelForm); y += 22;
            txtBAciklama = OlusturTextBox(y, panelForm); y += 34;
            OlusturLabel("Durum:", y, panelForm); y += 22;
            cmbBDurum = OlusturComboBox(y, panelForm, new[] { "Acik", "Islemde", "Tamamlandi" }); y += 34;
            OlusturLabel("Talep Tarihi:", y, panelForm); y += 22;
            dtpBTarih = new DateTimePicker { Location = new Point(10, y), Width = 300, Format = DateTimePickerFormat.Short };
            panelForm.Controls.Add(dtpBTarih); y += 34;
            chkBCozum = new CheckBox { Text = "Çözüm Tarihi Gir:", Location = new Point(10, y), AutoSize = true }; y += 24;
            panelForm.Controls.Add(chkBCozum);
            dtpBCozum = new DateTimePicker { Location = new Point(10, y), Width = 300, Format = DateTimePickerFormat.Short, Enabled = false };
            panelForm.Controls.Add(dtpBCozum);
            chkBCozum.CheckedChanged += (s, e) => dtpBCozum.Enabled = chkBCozum.Checked;

            DaireComboDoldur(cmbBDaire);

            var btnYenileB = OlusturButon("🔄 Daireleri Yükle", Color.FromArgb(22, 160, 133), 425);
            btnYenileB.Width = 140;
            panelBtn.Controls.Add(btnYenileB);
            btnYenileB.Click += (s, e) => { DaireComboDoldur(cmbBDaire); };

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    DateTime? cozum = chkBCozum.Checked ? (DateTime?)dtpBCozum.Value : null;
                    bakimBL.Ekle(cmbBDaire.SelectedValue?.ToString(), txtBKategori.Text.Trim(),
                        txtBAciklama.Text.Trim(), cmbBDurum.Text, dtpBTarih.Value, cozum);
                    BakimListele(); TemizleBakim();
                    MessageBox.Show("Talep eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnGuncelle.Click += (s, e) =>
            {
                if (secilenTalepId == null) { MessageBox.Show("Lütfen bir talep seçin."); return; }
                try
                {
                    DateTime? cozum = chkBCozum.Checked ? (DateTime?)dtpBCozum.Value : null;
                    bakimBL.Guncelle(secilenTalepId, cmbBDaire.SelectedValue?.ToString(), txtBKategori.Text.Trim(),
                        txtBAciklama.Text.Trim(), cmbBDurum.Text, dtpBTarih.Value, cozum);
                    BakimListele(); TemizleBakim();
                    MessageBox.Show("Güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnSil.Click += (s, e) =>
            {
                if (secilenTalepId == null) { MessageBox.Show("Lütfen bir talep seçin."); return; }
                if (MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try { bakimBL.Sil(secilenTalepId); BakimListele(); TemizleBakim(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            btnTemizle.Click += (s, e) => TemizleBakim();

            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                secilenTalepId = row.Cells["ID"].Value?.ToString();
                txtBKategori.Text = row.Cells["Kategori"].Value?.ToString();
                txtBAciklama.Text = row.Cells["Aciklama"].Value?.ToString();
                cmbBDurum.Text = row.Cells["Durum"].Value?.ToString();
            };

            txtAra.TextChanged += (s, e) =>
            {
                string f = txtAra.Text.Trim();
                dgv.DataSource = string.IsNullOrEmpty(f) ? bakimBL.HepsiniGetir() : bakimBL.Ara(f);
            };

            BakimListele();
        }

        private void BakimListele() { dgvBakim.DataSource = bakimBL.HepsiniGetir(); }
        private void TemizleBakim()
        {
            secilenTalepId = null;
            txtBKategori.Text = txtBAciklama.Text = "";
            cmbBDurum.SelectedIndex = 0;
            chkBCozum.Checked = false;
        }

        // ====================================================
        // TAB - DAİRE SAKİNLERİ
        // ====================================================
        private DataGridView dgvSakinler;
        private ComboBox cmbSKisi, cmbSDaire, cmbSRol;
        private DateTimePicker dtpSBaslangic;
        private string secilenSakinKisiId = null, secilenSakinDaireId = null;

        private void YukleTab_Sakinler()
        {
            var (dgv, txtAra, panelBtn, panelForm) = OlusturStandartLayout(tabSakinler);
            dgvSakinler = dgv;

            var btnEkle = OlusturButon("➕ Ekle", Color.FromArgb(39, 174, 96), 5);
            var btnGuncelle = OlusturButon("✏ Güncelle", Color.FromArgb(41, 128, 185), 110);
            var btnSil = OlusturButon("🗑 Sil", Color.FromArgb(192, 57, 43), 215);
            var btnYenile = OlusturButon("🔄 Yenile", Color.FromArgb(127, 140, 141), 320);
            panelBtn.Controls.AddRange(new Control[] { btnEkle, btnGuncelle, btnSil, btnYenile });

            int y = 10;
            OlusturLabel("Kişi:", y, panelForm); y += 22;
            cmbSKisi = OlusturComboBox(y, panelForm, new string[0]); y += 34;
            OlusturLabel("Daire:", y, panelForm); y += 22;
            cmbSDaire = OlusturComboBox(y, panelForm, new string[0]); y += 34;
            OlusturLabel("Rol:", y, panelForm); y += 22;
            cmbSRol = OlusturComboBox(y, panelForm, new[] { "Sahip", "Kiraci" }); y += 34;
            OlusturLabel("Başlangıç Tarihi:", y, panelForm); y += 22;
            dtpSBaslangic = new DateTimePicker { Location = new Point(10, y), Width = 300, Format = DateTimePickerFormat.Short };
            panelForm.Controls.Add(dtpSBaslangic);

            KisiComboDoldur(cmbSKisi);
            DaireComboDoldur(cmbSDaire);

            btnEkle.Click += (s, e) =>
            {
                try
                {
                    sakinBL.Ekle(cmbSKisi.SelectedValue?.ToString(), cmbSDaire.SelectedValue?.ToString(),
                        cmbSRol.Text, dtpSBaslangic.Value);
                    SakinListele();
                    MessageBox.Show("Sakin eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnGuncelle.Click += (s, e) =>
            {
                if (secilenSakinKisiId == null) { MessageBox.Show("Lütfen bir kayıt seçin."); return; }
                try
                {
                    sakinBL.Guncelle(secilenSakinKisiId, secilenSakinDaireId, cmbSRol.Text, dtpSBaslangic.Value);
                    SakinListele();
                    MessageBox.Show("Güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            btnSil.Click += (s, e) =>
            {
                if (secilenSakinKisiId == null) { MessageBox.Show("Lütfen bir kayıt seçin."); return; }
                if (MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try { sakinBL.Sil(secilenSakinKisiId, secilenSakinDaireId); SakinListele(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            btnYenile.Click += (s, e) => { KisiComboDoldur(cmbSKisi); DaireComboDoldur(cmbSDaire); SakinListele(); };

            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                secilenSakinKisiId = row.Cells["KisiID"].Value?.ToString();
                secilenSakinDaireId = row.Cells["DaireID"].Value?.ToString();
                cmbSRol.Text = row.Cells["Rol"].Value?.ToString();
            };

            SakinListele();
        }

        private void SakinListele() { dgvSakinler.DataSource = sakinBL.HepsiniGetir(); }
    }
}
