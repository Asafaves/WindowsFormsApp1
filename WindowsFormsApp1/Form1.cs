using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("çıkmak istediğinize emin misiniz?", "çıkış işlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti =new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * from kullanici where kullaniciadi=@kad and parola=@parola", baglanti);
            komut.Parameters.AddWithValue("@kad", textBox1.Text);
            komut.Parameters.AddWithValue("@parola", textBox2.Text);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                icerik frm = new icerik();
                frm.Show();
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("yanlış kullanıcı adı veya parolası", "hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
