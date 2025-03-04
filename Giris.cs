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
    public partial class Giris : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source = MERT\\SQLEXPRESS; Initial Catalog = PersonelDb; Integrated Security = True; Encrypt=False");

        public Giris()
        {
            InitializeComponent();
        }
        private void btnGiris_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            baglanti.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Tbl_Giris", baglanti);
            SqlDataReader dr = cmd.ExecuteReader();
            bool isThere = false;

            while (dr.Read())
            {
                if (username == dr["username"].ToString().TrimEnd() && password == dr["password"].ToString().TrimEnd())
                {
                    isThere = true;
                    break;
                }
                else
                {
                    isThere = false;
                }
            }
            baglanti.Close();

            if (isThere)
            {
                MessageBox.Show("Başarıyla Giriş yaptınız");
                Personel personel = new Personel();
                personel.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Giriş yapamadınız!");
            }
        }
    }
}
