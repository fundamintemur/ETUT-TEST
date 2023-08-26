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

namespace Etüt_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection bgl = new SqlConnection(@"Data Source=LENOVO\SQLEXPRESS;Initial Catalog=EtutTest;Integrated Security=True");

        void DersListesi()
        {

            //1.YOLL
            //bgl.Open();
            //SqlCommand komut = new SqlCommand("select DERSAD from TBLDERSLER", bgl);
            //SqlDataReader dr = komut.ExecuteReader();
            //while (dr.Read())
            //{
            //    CmbDers.Items.Add(dr[0]);
            //}
            //bgl.Close();
            //2.YOL
            SqlDataAdapter da = new SqlDataAdapter("select*from TBLDERSLER", bgl);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbDers.ValueMember = "DERSID";
            CmbDers.DisplayMember = "DERSAD";
            CmbDers.DataSource = dt;



        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DersListesi();
            etutListesi();
        }

        private void CmbDers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da2 = new SqlDataAdapter("select * from TBLOGRETMENLER where BRANSID=" + CmbDers.SelectedValue, bgl);
            DataTable dt2 = new DataTable();
            
            da2.Fill(dt2);
            CmbOgretmen.ValueMember = "OGRTID";
            CmbOgretmen.DisplayMember = "AD";
            CmbOgretmen.DataSource = dt2;


            //bgl.Open();
            //CmbOgretmen.Items.Clear();//cmbdoktor içindeki verileri temizlemek.Yoksa bütün veriler alt alta tekrarlanıyor.
            //SqlCommand komut3 = new SqlCommand("select AD,SOYAD from TBLOGRETMENLER where BRANSID=@p1", bgl);
            //komut3.Parameters.AddWithValue("@p1", CmbDers.SelectedValue);//where yazdığımız şartı aldık.
            //SqlDataReader dr3 = komut3.ExecuteReader();//verileri listemedem okumak için

            //while (dr3.Read())///while koşuluyla verilerin doğru okunmasını sağlıyoruz.
            //{
            //    CmbOgretmen.Items.Add(dr3[0] + " " + dr3[1]);
            //}
            //bgl.Close();
        }

        private void BtnEtutOlustur_Click(object sender, EventArgs e)
        {
            bgl.Open();
            SqlCommand komut = new SqlCommand("insert into TBLETUT (DERSID,OGRETMENID,TARIH,SAAT) values(@p1,@p2,@p3,@p4)", bgl);
            komut.Parameters.AddWithValue("@p1",CmbDers.SelectedValue);
            komut.Parameters.AddWithValue("@p2",CmbOgretmen.SelectedValue);
            komut.Parameters.AddWithValue("@p3",MskTarih.Text);
            komut.Parameters.AddWithValue("@p4",MskSaat.Text);
            komut.ExecuteNonQuery();
            bgl.Close();
            MessageBox.Show("İŞLEM KAYDEDİLDİ");

        }

        //Etüt Listesi
        void etutListesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("execute etut", bgl);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            textBox2.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();

        }

        private void BtnEtütDetay_Click(object sender, EventArgs e)
        {
            bgl.Open();
            SqlCommand komut = new SqlCommand("Update TBLETUT set OGRENCIID=@p1,DURUM=@p2 where ID=@p3", bgl);
            komut.Parameters.AddWithValue("@p1",textBox1.Text);
            komut.Parameters.AddWithValue("@p2","True");
            komut.Parameters.AddWithValue("@p3",textBox2.Text);
            komut.ExecuteNonQuery();
            bgl.Close();
            MessageBox.Show("Bilgiler Güncellendi");
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void BTNFOTOYUKLE_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }
    }
}
