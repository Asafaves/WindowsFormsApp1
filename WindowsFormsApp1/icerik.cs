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
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace WindowsFormsApp1
{
    public partial class icerik : Form
    {
        public icerik()
        {
            InitializeComponent();
        }

        void göster()
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter vericek = new SqlDataAdapter("select * from icerik order by ad", baglanti);
            DataSet ds = new DataSet();
            vericek.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("çıkmak istediğinize emin misiniz?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void icerik_Load(object sender, EventArgs e)
        {
            göster();
        }
        public string secilikayitno;
        public int secilikayit;
        public Boolean tiklama_kontrol = false;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tiklama_kontrol = true;
            secilikayit = dataGridView1.SelectedCells[0].RowIndex;
            secilikayitno = dataGridView1.Rows[secilikayit].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.Rows[secilikayit].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[secilikayit].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[secilikayit].Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.Rows[secilikayit].Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.Rows[secilikayit].Cells[5].Value.ToString();

            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from icerik where ID=@kimlik", baglanti);
            komut.Parameters.AddWithValue("@kimlik", dataGridView1.Rows[secilikayit].Cells[0].Value);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                if (dr[6].ToString() == "")
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    byte[] imgdata = (byte[])dataGridView1.Rows[secilikayit].Cells[6].Value;
                    MemoryStream ms = new MemoryStream(imgdata);
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            komut.Dispose();
            baglanti.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ekle frm = new ekle();
            frm.Show();
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int secilikayit = dataGridView1.SelectedCells[0].RowIndex;
            secilikayitno = dataGridView1.Rows[secilikayit].Cells[0].Value.ToString();
            DialogResult onay = MessageBox.Show(secilikayitno + "nolu kaydı silmekistediğinize emin misiniz?", "silne işlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
                baglanti.Open();
                SqlCommand komut = new SqlCommand("delete from icerik where ID=@kimlik", baglanti);
                komut.Parameters.AddWithValue("@kimlik", dataGridView1.Rows[secilikayit].Cells[0].Value);
                komut.ExecuteNonQuery();
                MessageBox.Show("s,lme işlemi başarıyla tamamlandı");
                baglanti.Close();
                göster();
                pictureBox1.Image = null;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tiklama_kontrol == false)
            {
                MessageBox.Show("lütfen kayı seçiniz");
            }
            else
            {
                update frm = new update();
                frm.textBox1.Text = textBox1.Text;
                frm.textBox2.Text = textBox2.Text;
                frm.textBox3.Text = textBox3.Text;
                frm.textBox4.Text = textBox4.Text;
                frm.textBox5.Text = textBox5.Text;
                //resim
                frm.pictureBox1.Image = pictureBox1.Image;
                Program.Düzenlenecek_ID = dataGridView1.Rows[secilikayit].Cells[0].Value.ToString();
                frm.Show();
                this.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter arama = new SqlDataAdapter("select * from icerik where ad like '" + textBox6.Text + "'order by ad", baglanti);

            DataSet ds = new DataSet();
            arama.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
            baglanti.Open();
            SqlDataAdapter arama = new SqlDataAdapter("select * from icerik where ad like '" + textBox6.Text + "%'order by ad", baglanti);

            DataSet ds = new DataSet();
            arama.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglanti.Close();
        }
    }
}
