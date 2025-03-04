using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonelTakipUygulaması
{
    public partial class Personel : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source = MERT\\SQLEXPRESS; Initial Catalog = PersonelDb; Integrated Security = True; Encrypt=False");
        public Personel()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Personel_FormClosing);
        }
        private void Personel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Personel_Load(object sender, EventArgs e)
        {
            this.tbl_DepartmanTableAdapter1.Fill(this.personelVeriTabanı.Tbl_Departman);    
            Listele();
            Temizle();
            dataGridView1.ClearSelection();   
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) ||
                string.IsNullOrWhiteSpace(maskedTc.Text) ||
                (!radiobtnErkek.Checked && !radiobtnKadin.Checked) ||
                string.IsNullOrWhiteSpace(maskedDogumTarihi.Text) ||
                cbbDepartman.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(maskedCepTel.Text) ||
                string.IsNullOrWhiteSpace(txtAdres.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Ekle();
        }
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) ||
                string.IsNullOrWhiteSpace(maskedTc.Text) ||
                (!radiobtnErkek.Checked && !radiobtnKadin.Checked) ||
                string.IsNullOrWhiteSpace(maskedDogumTarihi.Text) ||
                cbbDepartman.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(maskedCepTel.Text) ||
                string.IsNullOrWhiteSpace(txtAdres.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Guncelle();
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            Sil();
        }
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                int personelId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Personel_ID"].Value);

                radiobtnErkek.Checked = false;
                radiobtnKadin.Checked = false;

                SqlCommand command = new SqlCommand("SELECT [Personel_ID], [Personel_Ad], [Personel_Soyad], [Personel_TC], " +
                    "[Personel_Cinsiyet], [Personel_DogumTarihi], [Departman_ID], [Personel_CepTel], [Personel_Adres] FROM [Tbl_Personel] " +
                    "WHERE [Personel_ID] = @id", baglanti);

                command.Parameters.AddWithValue("@id", personelId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable personelTable = new DataTable();
                adapter.Fill(personelTable);

                // Eğer personel bulunduysa
                if (personelTable.Rows.Count > 0)
                {
                    DataRow row = personelTable.Rows[0];
                    txtAd.Text = row["Personel_Ad"].ToString();
                    txtSoyad.Text = row["Personel_Soyad"].ToString();
                    maskedTc.Text = row["Personel_TC"].ToString();
                    string cinsiyet = row["Personel_Cinsiyet"].ToString();
                    if (cinsiyet == "Erkek")
                    {
                        radiobtnErkek.Checked = true;
                    }
                    else if (cinsiyet == "Kadın")
                    {
                        radiobtnKadin.Checked = true;
                    }

                    maskedDogumTarihi.Text = row["Personel_DogumTarihi"].ToString();
                    cbbDepartman.SelectedValue = row["Departman_ID"];
                    maskedCepTel.Text = row["Personel_CepTel"].ToString();
                    txtAdres.Text = row["Personel_Adres"].ToString();
                }
                else
                {
                    MessageBox.Show("Personel bulunamadı.");
                }
            }
        }

        private void Listele()
        {
            try
            {
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_Personel", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
        private void Ekle()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO Tbl_Personel (Personel_Ad,Personel_Soyad,Personel_TC,Personel_Cinsiyet,Personel_DogumTarihi,Departman_ID,Personel_CepTel,Personel_Adres)" +
                    " values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", baglanti);
                komut.Parameters.AddWithValue("@p1", txtAd.Text);
                komut.Parameters.AddWithValue("@p2", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p3", maskedTc.Text);
                string cinsiyet = "";
                if (radiobtnErkek.Checked)
                {
                    cinsiyet = radiobtnErkek.Text;
                }
                else if (radiobtnKadin.Checked)
                {
                    cinsiyet = radiobtnKadin.Text;
                }
                komut.Parameters.AddWithValue("@p4", cinsiyet);
                komut.Parameters.AddWithValue("@p5", maskedDogumTarihi.Text);
                komut.Parameters.AddWithValue("@p6", cbbDepartman.SelectedValue);
                komut.Parameters.AddWithValue("@p7", maskedCepTel.Text);
                komut.Parameters.AddWithValue("@p8", txtAdres.Text);

                komut.ExecuteNonQuery();
                MessageBox.Show("Personel Eklendi");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
                Temizle();
                Listele();
            }
        }
        private void Guncelle()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Personel_ID"].Value);
                try
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("UPDATE Tbl_Personel SET Personel_Ad=@ad, Personel_Soyad=@soyad, Personel_TC=@tc, " +
                        "Personel_Cinsiyet=@cinsiyet, Personel_DogumTarihi=@dogumTarihi, Departman_ID=@departmanid, Personel_CepTel=@cepTel, " +
                        "Personel_Adres=@adres WHERE Personel_ID=@id", baglanti);

                    DateTime dogumTarihi = DateTime.ParseExact(maskedDogumTarihi.Text, "dd.MM.yyyy", null);

                    komut.Parameters.AddWithValue("@id", id);
                    komut.Parameters.AddWithValue("@ad", txtAd.Text);
                    komut.Parameters.AddWithValue("@soyad", txtSoyad.Text);
                    komut.Parameters.AddWithValue("@tc", maskedTc.Text);
                    string cinsiyet = "";
                    if (radiobtnErkek.Checked)
                    {
                        cinsiyet = radiobtnErkek.Text;
                    }
                    else if (radiobtnKadin.Checked)
                    {
                        cinsiyet = radiobtnKadin.Text;
                    }
                    komut.Parameters.AddWithValue("@cinsiyet", cinsiyet);
                    komut.Parameters.AddWithValue("@dogumTarihi", dogumTarihi);
                    komut.Parameters.AddWithValue("@departmanid", cbbDepartman.SelectedValue);
                    komut.Parameters.AddWithValue("@cepTel", maskedCepTel.Text);
                    komut.Parameters.AddWithValue("@adres", txtAdres.Text);

                    komut.ExecuteNonQuery();
                    MessageBox.Show("Personel Güncellendi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                    Temizle();
                    Listele();
                }
            }
        }
        private void Temizle()
        {
            txtAd.Clear();
            txtSoyad.Clear();
            maskedTc.Clear();
            radiobtnErkek.Checked = false;
            radiobtnKadin.Checked = false;
            maskedDogumTarihi.Clear();
            cbbDepartman.SelectedIndex = -1;
            maskedCepTel.Clear();
            txtAdres.Clear();
        }
        private void Sil()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Personel_ID"].Value);

                try
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("DELETE FROM Tbl_Personel WHERE Personel_ID=@id", baglanti);
                    komut.Parameters.AddWithValue("@id", id);
                    komut.ExecuteNonQuery();

                    MessageBox.Show("Personel Silindi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    baglanti.Close();
                    Temizle();
                    Listele();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
