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

namespace Pokemon
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Random rd = new Random();
        static int num;
        string[] Pokemons_Player_Location_Blacked = { "./blacked/WaterFrog.png", "./blacked/LightningRat.png", "./blacked/FireDragon.png", "./blacked/WaterTurtle.png", "./blacked/FlowerFrog.png", "./blacked/butterfly.png", "./blacked/Carby.png", "./blacked/Manula.png", "./blacked/LizardKing.png", "./blacked/Ghost.png", "./blacked/SnowyMeteor.png", };
        string[] Pokemons_Player_Location = {  "./PokemonImage/WaterFrog.png", "./PokemonImage/LightningRat.png", "./PokemonImage/FireDragon.png", "./PokemonImage/WaterTurtle.png", "./PokemonImage/FlowerFrog.png", "./PokemonImage/ButterFly.png", "./PokemonImage/Carby.png", "./PokemonImage/Manula.png", "./PokemonImage/LizardKing.png", "./PokemonImage/Ghost.png", "./PokemonImage/SnowyMeteor.png" };
        private void Form3_Load(object sender, EventArgs e)
        {
            Start();

            using (SqlConnection cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;" + "AttachDbFilename = |DataDirectory|PokemonData.mdf;" + "Integrated Security = True"))
            {
                cn.Open();
                SqlDataAdapter daPokemon = new SqlDataAdapter("SELECT [Id],[PokName] FROM Pokemon", cn);
                DataSet ds = new DataSet();
                daPokemon.Fill(ds, "我是誰");

                dataGridView1.DataSource = ds.Tables["我是誰"];

            }

            


        }
        private void Start()
        {
            
            num = rd.Next(0, 11);
            pictureBox1.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location_Blacked[num]);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            using (SqlConnection cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;" + "AttachDbFilename = |DataDirectory|PokemonData.mdf;" + "Integrated Security = True"))
            {
                cn.Open();
                SqlDataAdapter daPokemon = new SqlDataAdapter("SELECT [Id],[PokName] FROM Pokemon", cn);
                DataSet ds = new DataSet();
                daPokemon.Fill(ds, "我是誰");
                DataTable dt = ds.Tables["我是誰"];
                if (textBox1.Text == "" || Convert.ToInt32(textBox1.Text)>11 || Convert.ToInt32(textBox1.Text) <1)
                {
                    MessageBox.Show("請輸入你的答案啦幹!");
                    button1.Enabled = true;
                }
                else
                {
                    var ans = from s in dt.AsEnumerable()
                              where s.Field<int>("Id") == Convert.ToInt32(textBox1.Text)
                              select new
                              {

                                  名字 = s.Field<string>("PokName")
                              };
                    foreach (var s in ans)
                    {
                        if (s.名字 == dt.Rows[num]["PokName"].ToString())
                        {
                            MessageBox.Show("答對囉!我是" + dt.Rows[num]["PokName"].ToString());
                        }
                        else
                        {
                            MessageBox.Show("答錯囉!林北是" + dt.Rows[num]["PokName"].ToString());                         
                        }
                        button2.Enabled = true;
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location[num]);
                    }
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Start();
            textBox1.Text = "";
            textBox1.Focus();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Visible = false;
            f2.Show();
        }
    }
   
}
