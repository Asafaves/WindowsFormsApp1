using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ekle : Form
    {
        public ekle()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("çıkmak istediğineze emin misiniz?", "çıkış işlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                icerik frm = new icerik();
                frm.Show();
                this.Visible = false;
            }
        }
        string resimpath;
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "resim aç";
            openFileDialog1.Filter = "Jpeg Dosyası(*.jpg)|*.jpg|Gif Dosyası(*.gif)|*.gif|Png Dosyası(*.png)|*.png|Tif Dosyası(*.tif)|*.tif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                resimpath = openFileDialog1.FileName.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("resim seçiniz");
            }
            else
            {
                FileStream fs = new FileStream(resimpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] resim = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                //TC KONTROL İŞLEMİ BAŞLIYOR
                SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select * from icerik where tc=@tc", baglanti);
                komut.Parameters.AddWithValue("@tc", textBox3.Text);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("bu tc kauıtlıdır", "hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dr.Close();
                    SqlCommand kmt = new SqlCommand("insert into icerik(ad,soyad,tc,dogumyeri,dogumyili,resim) Values (@ad,@soyad,@tc,@dogumyeri,@dogumyili,@image)", baglanti);
                    kmt.Parameters.Add("@ad", textBox1.Text);
                    kmt.Parameters.Add("@soyad", textBox2.Text);
                    kmt.Parameters.Add("@tc", textBox3.Text);
                    kmt.Parameters.Add("@dogumyeri", textBox4.Text);
                    kmt.Parameters.Add("@dogumyili", textBox5.Text);
                    kmt.Parameters.Add("@image", SqlDbType.Image, resim.Length).Value = resim;
                    kmt.ExecuteNonQuery();
                    MessageBox.Show("veri tabanına kayıt yapıldı.");
                    baglanti.Close();


                    icerik frm = new icerik();
                    frm.Show();
                    this.Visible = false;
                }
            }
        }
    }
}
