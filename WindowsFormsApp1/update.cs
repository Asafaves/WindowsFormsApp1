using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class update : Form
    {
        public update()
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
        Boolean kontrol = false;
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "resim aç";
            openFileDialog1.Filter = "Jpeg Dosyası(*.jpg)|*.jpg|Gif Dosyası(*.gif)|*.gif|Png Dosyası(*.png)|*.png|Tif Dosyası(*.tif)|*.tif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                resimpath = openFileDialog1.FileName.ToString();
                kontrol = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\deneme.mdf;Integrated Security=True");
            baglanti.Open();
            if (kontrol == false)
            {
                SqlCommand düzenle = new SqlCommand("update icerik set ad=@ad,soyad=@soyad,tc=@tc,dogumyeri=@dogumyeri,dogumyili=@dogumyili where ID=@kimlik", baglanti);
                düzenle.Parameters.Add("@ad", textBox1.Text);
                düzenle.Parameters.Add("@soyad", textBox2.Text);
                düzenle.Parameters.Add("@tc", textBox3.Text);
                düzenle.Parameters.Add("@dogumyeri", textBox4.Text);
                düzenle.Parameters.Add("@dogumyili", textBox5.Text);
                düzenle.Parameters.Add("@kimlik", Program.Düzenlenecek_ID);
                düzenle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("resimsiz düzenleme başarılı");
                icerik frm = new icerik();
                frm.tiklama_kontrol = false;
                frm.Show();
                this.Close();
            }
            else
            {//resimli
                FileStream fs = new FileStream(resimpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] resim = br.ReadBytes((int)fs.Length);
                SqlCommand düzenle = new SqlCommand("update icerik set ad=@ad,soyad=@soyad,tc=@tc,dogumyeri=@dogumyeri,dogumyili=@dogumyili,resim=@image where ID=@kimlik", baglanti);
                düzenle.Parameters.Add("@ad", textBox1.Text);
                düzenle.Parameters.Add("@soyad", textBox2.Text);
                düzenle.Parameters.Add("@tc", textBox3.Text);
                düzenle.Parameters.Add("@dogumyeri", textBox4.Text);
                düzenle.Parameters.Add("@dogumyili", textBox5.Text);
                düzenle.Parameters.Add("@image", SqlDbType.Image, resim.Length).Value = resim;
                düzenle.Parameters.Add("@kimlik", Program.Düzenlenecek_ID);
                düzenle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("resimli düzenleme başarılı");
                icerik frm = new icerik();
                frm.tiklama_kontrol = false;
                frm.Show();
                this.Close();
            }
        }
    }
}
