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

namespace Pokemon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        string[] Backgrounds_Location = { "./background/beach.jpg", "./background/hill.jpg", "./background/cave.jpg", "./background/stadium.jpg", "./background/grave.jpg", "./background/elsa.jpg" };

        string[] Special_Effect_Location = { "./SpecialEffect/normal.jpg", "./SpecialEffect/normal2.jpg" };

        string[] Pokemons_Player_Location = { "./PokemonImage/PokemonBall.png", "./PokemonImage/WaterFrog.png", "./PokemonImage/LightningRat.png", "./PokemonImage/FireDragon.png",  "./PokemonImage/WaterTurtle.png", "./PokemonImage/FlowerFrog.png" };
        string[] Pokemons_Player_Name = { "寶貝球", "蚊香泳士", "雷丘", "噴火龍", "水箭龜", "妙蛙花" };

        string[] Pokemons_Enemy_Location = { "./PokemonImage/ButterFly.png", "./PokemonImage/Carby.png", "./PokemonImage/Manula.png", "./PokemonImage/LizardKing.png", "./PokemonImage/Ghost.png","./PokemonImage/SnowyMeteor.png" };
        string[] Pokemons_Enemy_Name = { "巴大蝶", "卡比獸", "瑪狃拉", "蜥蜴王", "耿鬼", "雪童子" };

        string[] Player = { "./Player/Player1.png", "./Player/Enemy1.png", "./Player/Enemy2.png", "./Player/Enemy3.png", "./Player/Enemy4.png" };

        string[] Skill_Enemy = { "Skill1", "Skill2", "Skill3", "Skill4" };
        string[] Skill_Player = { "Skill1", "Skill2", "Skill3", "Skill4" };
        string PokHP;
        Random rand = new Random();
        static int pokemon_enemy_skill_num = 0,Level =5;
        

        Dictionary<string, int> Pokemon_HP = new Dictionary<string, int>();

        DataTable dt_SkillDamage = new DataTable();
        DataTable dt_Pokemon = new DataTable();
        DataSet ds_SkillDamage = new DataSet();
        DataSet dsPokemon = new DataSet();

        static int pic5_X; 

        private void Form1_Load(object sender, EventArgs e)
        {

            //1.圖片設定 pb1>背景,pb2,3>人物,pb4,5>寶可夢,pb6>寶貝球,pb7,8>攻擊特效

            pictureBox1.Image = Image.FromFile(Application.StartupPath + Backgrounds_Location[Level-5]);

            pictureBox2.Image = Image.FromFile(Application.StartupPath + Player[0]);
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;

            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;

            pictureBox3.Image = Image.FromFile(Application.StartupPath + Player[4]);
            pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Parent = pictureBox1;

            pictureBox7.Parent = pictureBox1;

            pic5_X = pictureBox5.Location.X;
            //pictureBox7.Image = Image.FromFile(Application.StartupPath + Special_Effect_Location[0]);
            
            //pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox7.BackColor = Color.Transparent;
            //pictureBox7.Parent = pictureBox1;
            //pictureBox7.Parent = pictureBox5;

           

            //5.用ComboBox 轉換寶可夢
            using (SqlConnection cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;" + "AttachDbFilename = |DataDirectory|PokemonData.mdf;" + "Integrated Security = True"))
            {
                SqlDataAdapter daPokemon = new SqlDataAdapter("SELECT * FROM Pokemon", cn);

                daPokemon.Fill(dsPokemon, "Pokemon");
                dt_Pokemon = dsPokemon.Tables["Pokemon"];
                
                for (int i = 0; i < 5; i++)
                {
                    
                    comboBox1.Items.Add(dt_Pokemon.Rows[i]["PokName"].ToString());
                }
      

            }
            //字典的建立
            for (int i = 0; i < dt_Pokemon.Rows.Count; i++)
            {
                Pokemon_HP.Add(dt_Pokemon.Rows[i]["PokName"].ToString(), Convert.ToInt32(dt_Pokemon.Rows[i]["HP"]));
            }

           
            //查詢字典內所有item
            foreach (KeyValuePair<string, int> item in Pokemon_HP)
            {
                textBox2.Text += string.Format("{0}:{1}\r", item.Key, item.Value);
                
            }
            

        }
        

        static int count_changePokemon = 0;  //寶可夢更換次數變數

        //選擇我方寶可夢的方法
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) 
        {
            

            if (comboBox1.Text == "(陣亡)")
            {      
                timer5.Enabled = false;
                sk1.Enabled = false;
                sk2.Enabled = false;
                sk3.Enabled = false;
                sk4.Enabled = false;
                

            }
            
            else
            {
                //3.血量設定
                progressBar1.Maximum = Convert.ToInt32(dt_Pokemon.Rows[comboBox1.SelectedIndex]["HP"].ToString());
                progressBar1.Minimum = 0;
                progressBar1.Value = Pokemon_HP[dt_Pokemon.Rows[comboBox1.SelectedIndex]["PokName"].ToString()];
                progressBar2.Maximum = Convert.ToInt32(dt_Pokemon.Rows[Level]["HP"].ToString());
                progressBar2.Minimum = 0;
                progressBar2.Value = Pokemon_HP[Pokemons_Enemy_Name[Level - 5]];
                comboBox1.Enabled = false;

                sk1.Enabled = true;
                sk2.Enabled = true;
                sk3.Enabled = true;
                sk4.Enabled = true;

                if (count_changePokemon > 0)    //戰鬥中更換寶可夢的回合我方不得攻擊
                {

                    Enemy_Attack();             //輪到敵方攻擊
                    sk1.Enabled = false;
                    sk2.Enabled = false;
                    sk3.Enabled = false;
                    sk4.Enabled = false;

                }
                count_changePokemon += 1;

                if (comboBox1.Text != "(陣亡)")
                {
                    pictureBox6.Visible = false;
                    progressBar1.Visible = true;
                    pictureBox4.Visible = true;


                   
                    label1.Visible = true;
                    label1.Text = string.Format("{0}/{1}", Pokemon_HP[dt_Pokemon.Rows[comboBox1.SelectedIndex]["PokName"].ToString()], dt_Pokemon.Rows[comboBox1.SelectedIndex]["HP"].ToString());
                    label2.Visible = true;
                    label2.Text = string.Format("{0}/{1}", Pokemon_HP[dt_Pokemon.Rows[Level]["PokName"].ToString()], dt_Pokemon.Rows[Level]["HP"].ToString()); //Level = 5
                }
                    
                
                pictureBox4.BackColor = Color.Transparent;
                pictureBox4.Parent = pictureBox1;
                pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            if (comboBox1.SelectedIndex == 0) //蚊香泳士
            {
                pictureBox4.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location[1]); //WaterFrog   
                sk1.Text = dt_Pokemon.Rows[0]["Skill1"].ToString();
                sk2.Text = dt_Pokemon.Rows[0]["Skill2"].ToString();
                sk3.Text = dt_Pokemon.Rows[0]["Skill3"].ToString();
                sk4.Text = dt_Pokemon.Rows[0]["Skill4"].ToString();


            }
            else if (comboBox1.SelectedIndex == 1) //雷丘
            {
                pictureBox4.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location[2]); //LightningRat 
                sk1.Text = dt_Pokemon.Rows[1]["Skill1"].ToString();
                sk2.Text = dt_Pokemon.Rows[1]["Skill2"].ToString();
                sk3.Text = dt_Pokemon.Rows[1]["Skill3"].ToString();
                sk4.Text = dt_Pokemon.Rows[1]["Skill4"].ToString();
            }
            else if (comboBox1.SelectedIndex == 2) //噴火龍
            {
                pictureBox4.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location[3]); //FireDragon
                sk1.Text = dt_Pokemon.Rows[2]["Skill1"].ToString();
                sk2.Text = dt_Pokemon.Rows[2]["Skill2"].ToString();
                sk3.Text = dt_Pokemon.Rows[2]["Skill3"].ToString();
                sk4.Text = dt_Pokemon.Rows[2]["Skill4"].ToString();
            }
            else if (comboBox1.SelectedIndex == 3) //水箭龜
            {
                pictureBox4.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location[4]); //WaterTurtle
                sk1.Text = dt_Pokemon.Rows[3]["Skill1"].ToString();
                sk2.Text = dt_Pokemon.Rows[3]["Skill2"].ToString();
                sk3.Text = dt_Pokemon.Rows[3]["Skill3"].ToString();
                sk4.Text = dt_Pokemon.Rows[3]["Skill4"].ToString();
            }
            else if (comboBox1.SelectedIndex == 4) //妙蛙花
            {
                pictureBox4.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location[5]); //FlowerFrog
                sk1.Text = dt_Pokemon.Rows[4]["Skill1"].ToString();
                sk2.Text = dt_Pokemon.Rows[4]["Skill2"].ToString();
                sk3.Text = dt_Pokemon.Rows[4]["Skill3"].ToString();
                sk4.Text = dt_Pokemon.Rows[4]["Skill4"].ToString();
            }



        }
        static bool gameover;
        //玩家寶可夢陣亡的方法
        private void Player_Defeated()
        {
            //我方全數陣亡的判斷
            if ( timer8.Enabled == false && Pokemon_HP[Pokemons_Player_Name[1]] <= 0 && Pokemon_HP[Pokemons_Player_Name[2]] <= 0 && Pokemon_HP[Pokemons_Player_Name[3]] <= 0 && Pokemon_HP[Pokemons_Player_Name[4]] <= 0 && Pokemon_HP[Pokemons_Player_Name[5]] <= 0)
            {
                gameover = true;

                if (gameover == true )
                {
                    timer9.Enabled = true;
                    timer9.Interval = 100;
                }

                int SelectedIndex_combox = 0;
                SelectedIndex_combox = comboBox1.SelectedIndex;
                comboBox1.Items.Insert(comboBox1.SelectedIndex, "(陣亡)");
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
                comboBox1.SelectedIndex = SelectedIndex_combox;
                comboBox1.Enabled = false;

                textBox1.Text = "菜逼八，我看你各位不適合當寶可夢大師阿!"+Environment.NewLine+"還是趕快轉職ㄅ";

                sk1.Visible = false;
                sk2.Visible = false;
                sk3.Visible = false;
                sk4.Visible = false;

                pictureBox7.Visible = true;

            }
            else
            {
                int SelectedIndex_combox = 0;
                SelectedIndex_combox = comboBox1.SelectedIndex;
                comboBox1.Items.Insert(comboBox1.SelectedIndex, "(陣亡)");
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex );
                comboBox1.SelectedIndex = SelectedIndex_combox;

                label1.Text = "";
                progressBar1.Visible = false;

            }
           

        }

        //敵人血量歸零後的方法
        private void Enemy_Defeated()
        {
            Level += 1;
            if (Level >= 11) //打敗所有敵人的判斷
            {
                pictureBox5.Visible = false;
                pictureBox3.Visible = true;
                progressBar2.Visible = false;
                label2.Text = "";


                pictureBox2.Visible = true;
                pictureBox4.Visible = false;
                progressBar1.Visible = false;
                label1.Text = "";

                sk1.Visible = false;
                sk2.Visible = false;
                sk3.Visible = false;
                sk4.Visible = false;

                textBox1.Text = "恭喜你打敗道館館主!獲得道館徽章!";
                pictureBox7.Visible = true;

                gameover = true;

                if (gameover == true)
                {
                    timer9.Enabled = true;
                    timer9.Interval = 100;
                }

            }
            else
            {
                pictureBox5.Image = Image.FromFile(Application.StartupPath + Pokemons_Enemy_Location[Level - 5]);
                label2.Text = string.Format("{0}/{1}", Pokemon_HP[dt_Pokemon.Rows[Level]["PokName"].ToString()], dt_Pokemon.Rows[Level]["HP"].ToString());
                progressBar2.Maximum = Pokemon_HP[Pokemons_Enemy_Name[Level - 5]];
                progressBar2.Value = Pokemon_HP[Pokemons_Enemy_Name[Level - 5]];
                pictureBox1.Image = Image.FromFile(Application.StartupPath + Backgrounds_Location[Level - 5]);
            }
  
            
        }

        //進入戰鬥的方法
        private void button1_Click(object sender, EventArgs e)
        {
            sk1.Visible = true;
            sk2.Visible = true;
            sk3.Visible = true;
            sk4.Visible = true;
            sk1.Enabled = false;
            sk2.Enabled = false;
            sk3.Enabled = false;
            sk4.Enabled = false;
            comboBox1.Enabled = true;

            button1.Enabled = false;

            textBox1.Text = "決鬥!";

            
            progressBar2.Visible = true;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox6.Visible = true;

            
            pictureBox5.Visible = true;

            pictureBox6.Image = Image.FromFile(Application.StartupPath + Pokemons_Player_Location[0]);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.BackColor = Color.Transparent;
            pictureBox6.Parent = pictureBox1;

            

            
            pictureBox5.Image = Image.FromFile(Application.StartupPath + Pokemons_Enemy_Location[Level-5]); //Level++ 則變更圖片
            
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            
            pictureBox5.BackColor = Color.Transparent;
            pictureBox5.Parent = pictureBox1;

        }

        //再玩一次
        private void button3_Click(object sender, EventArgs e)
        {
            Level = 5;
            pictureBox1.Image = Image.FromFile(Application.StartupPath + Backgrounds_Location[Level - 5]);
            button1.Enabled = true;
            //5.用ComboBox 轉換寶可夢
            button3.Visible = false;
            count_changePokemon = 0;

            pictureBox5.Location = new Point(pic5_X, 160); //不知為何timer8執行不完全，導致picturebox5無法回到原本位置，故加上此行程式碼

            using (SqlConnection cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;" + "AttachDbFilename = |DataDirectory|PokemonData.mdf;" + "Integrated Security = True"))
            {
                SqlDataAdapter daPokemon = new SqlDataAdapter("SELECT * FROM Pokemon", cn);

                daPokemon.Fill(dsPokemon, "Pokemon");
                dt_Pokemon = dsPokemon.Tables["Pokemon"];

                comboBox1.Items.Clear();
                for (int i = 0; i < 5; i++)
                {

                    comboBox1.Items.Add(dt_Pokemon.Rows[i]["PokName"].ToString());
                }

                
                
                for (int i = 0; i < dt_Pokemon.Rows.Count; i++)
                {
                    Pokemon_HP[dt_Pokemon.Rows[i]["PokName"].ToString()] =  Convert.ToInt32(dt_Pokemon.Rows[i]["HP"]);
                }

            }

            //foreach (KeyValuePair<string, int> item in Pokemon_HP)
            //{
            //    Pokemon_HP.Remove(item.Key);

            //}
            
            //Pokemon_HP.Clear();
            //字典的建立
            


        }

        //玩家 Click Button 所啟用的方法
        private void sk1_Click(object sender, EventArgs e1)
        {

            textBox1.Text = string.Format("我方的{0}使出{1}", comboBox1.Text, sk1.Text);
            Player_Attack(0);



        }
        private void sk2_Click(object sender, EventArgs e2)
        {

            textBox1.Text = string.Format("我方的{0}使出{1}", comboBox1.Text, sk2.Text);
            Player_Attack(1);

        }

        private void sk3_Click(object sender, EventArgs e3)
        {

            textBox1.Text = string.Format("我方的{0}使出{1}", comboBox1.Text, sk3.Text);
            Player_Attack(2);

        }

        private void sk4_Click(object sender, EventArgs e4)
        {

            textBox1.Text = string.Format("我方的{0}使出{1}", comboBox1.Text, sk4.Text);
            Player_Attack(3);

        }


        static int attack = 0;
        //玩家與敵人在進行戰鬥時的血量變動方法
        private int HP_Search(string Pokemon_Name, string Skill_Attacked) //被攻擊的寶可夢,被攻擊的招式
        {
            //用於儲存技能傷害

            //Pokemon_HP.Add(dt.Rows[],) //首先建立一個辭典，放入所有寶可夢的HP資料


            using (SqlConnection cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;" + "AttachDbFilename = |DataDirectory|PokemonData.mdf;" + "Integrated Security = True"))
            {

                SqlDataAdapter da_SkillDamage = new SqlDataAdapter("SELECT  [SkName],[Damage] FROM Skill", cn);
                da_SkillDamage.Fill(ds_SkillDamage, "Damage");
                dt_SkillDamage = ds_SkillDamage.Tables["Damage"];

                DataRow[] dr = dt_SkillDamage.Select(string.Format("SkName = '{0}'",Skill_Attacked)); //Q
                         
                attack = Convert.ToInt32(dr[0][1].ToString());
                textBox1.Text += string.Format(" 造成{0}點的傷害",dr[0][1].ToString());
                Pokemon_HP[Pokemon_Name] = Pokemon_HP[Pokemon_Name] -attack;

                
                return Pokemon_HP[Pokemon_Name];

            }

        }
        

       


        //我方攻擊敵方血量減少
        private void Player_Attack(int num_Skill_Player)
        {
            timer1.Enabled = true;
            timer1.Interval = 25;
            timer2.Enabled = true;
            timer2.Interval = 50;

            sk1.Enabled = false;
            sk2.Enabled = false;
            sk3.Enabled = false;
            sk4.Enabled = false;

            comboBox1.Enabled = false;



            using (SqlConnection cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;" + "AttachDbFilename = |DataDirectory|PokemonData.mdf;" + "Integrated Security = True"))
            {

                cn.Open();  

                SqlDataAdapter daPokemon = new SqlDataAdapter("SELECT * FROM Pokemon", cn);

                daPokemon.Fill(dsPokemon, "Pokemon");
                dt_Pokemon = dsPokemon.Tables["Pokemon"];


                SqlCommand cmd_Hp = new SqlCommand(); //預存程序GetPokemonHP
                cmd_Hp.Connection = cn;
                cmd_Hp.CommandText = "GetPokemonHP";
                cmd_Hp.CommandType = CommandType.StoredProcedure;
                cmd_Hp.Parameters.Add(new SqlParameter("@PokName", SqlDbType.NVarChar));
                cmd_Hp.Parameters["@PokName"].Value = dt_Pokemon.Rows[Level]["PokName"];

                SqlDataReader dr1 = cmd_Hp.ExecuteReader();

                if (dr1.Read())
                {
                    PokHP = dr1[0].ToString();
                }


                cn.Close();
                cn.Open();

                //Skill_Player = Skill1~Skill4 
                //output_Skill_Player為我方使用的技能名稱

                SqlDataAdapter da_SkillDamage = new SqlDataAdapter("SELECT  [SkName],[Damage] FROM Skill", cn);
                da_SkillDamage.Fill(ds_SkillDamage, "Damage");
                dt_SkillDamage = ds_SkillDamage.Tables["Damage"];

                output_Skill_Player = dt_Pokemon.Rows[comboBox1.SelectedIndex][Skill_Player[num_Skill_Player]].ToString();

                //我方攻擊陣亡?
                if (comboBox1.Text == "(陣亡)")
                {
                    textBox1.Text = "此角色已陣亡";
                }
                else
                {
                    label2.Text = string.Format("{0}/{1}", HP_Search(Pokemons_Enemy_Name[Level - 5]/*敵方名稱*/, output_Skill_Player/*我方技能名稱*/).ToString(), PokHP);
                }
                //敵人陣亡
                if (Pokemon_HP[Pokemons_Enemy_Name[Level-5]] < 0)
                {
                    progressBar2.Value =0;
                }
                else
                {
                    progressBar2.Value -= attack;
                }
                
                

            }

        }
        static string output_Skill_Player;//output_Skill_Player為我方使用的技能名稱
        static string pokemon_enemy_skill_name;

        //敵方攻擊我方血量減少
        private void Enemy_Attack()
        {
            pokemon_enemy_skill_num = rand.Next(0, 4);


            
            timer5.Enabled = true;
            timer5.Interval = 25;
            timer6.Enabled = true;
            timer6.Interval = 50;

            sk1.Enabled = false;
            sk2.Enabled = false;
            sk3.Enabled = false;
            sk4.Enabled = false;

            if (timer8.Enabled == false) //玩家死亡後寶貝球圖片才出現
            {
                using (SqlConnection cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;" + "AttachDbFilename = |DataDirectory|PokemonData.mdf;" + "Integrated Security = True"))
                {
                    pokemon_enemy_skill_name = dt_Pokemon.Rows[Level][Skill_Enemy[pokemon_enemy_skill_num]].ToString(); //Level = 5
                    textBox1.Text = string.Format("敵方的{0}對你使出了{1}", Pokemons_Enemy_Name[Level - 5], pokemon_enemy_skill_name);


                    cn.Open();
                    SqlCommand cmd_Hp = new SqlCommand();
                    cmd_Hp.Connection = cn;
                    cmd_Hp.CommandText = "GetPokemonHP";
                    cmd_Hp.CommandType = CommandType.StoredProcedure;
                    cmd_Hp.Parameters.Add(new SqlParameter("@PokName", SqlDbType.NVarChar));
                    cmd_Hp.Parameters["@PokName"].Value = comboBox1.Text;

                    SqlDataReader dr1 = cmd_Hp.ExecuteReader();

                    if (dr1.Read())
                    {
                        PokHP = dr1[0].ToString();
                    }

                }

                if (comboBox1.Text != "(陣亡)")
                {
                    label1.Text = HP_Search(comboBox1.Text/*我方寶可夢名稱*/, dt_Pokemon.Rows[Level][string.Format("Skill{0}", pokemon_enemy_skill_num + 1)].ToString()) + "/" + PokHP;

                    if (Pokemon_HP[comboBox1.Text] <= 0) //dt_Pokemon.Rows[comboBox1.SelectedIndex]["PokName"].ToString()
                    {
                        Player_Defeated();
                        progressBar1.Value = 0;
                    }
                    else
                    {
                        progressBar1.Value -= attack;
                    }
                }
                else
                {
                    textBox1.Text = Environment.NewLine + "此角色已陣亡";
                }
            }
        }

        double count = 0; //用於動畫特效的變數


        //Timer1~4 為玩家攻擊特效
        private void timer1_Tick(object sender, EventArgs e)  //我方蓄力
        {
            pictureBox4.Left -= 15;

            if (count >= 5)
            {
                count = 0;
                timer1.Enabled = false;

            }
            else
            {
                count += 1.5;
            }

        }

        private void timer2_Tick(object sender, EventArgs e)  //我方蓄力
        {
            pictureBox4.Left += 30;
            if (count >= 13.5)
            {
                count = 0;
                timer2.Enabled = false;

            }
            else
            {
                count += 1.5;
            }
            if (timer2.Enabled == false)
            {
                timer3.Enabled = true;
                timer3.Interval = 50;

            }
        }

        private void timer3_Tick(object sender, EventArgs e)  //我方攻擊
        {
            pictureBox4.Left -= 30;
            pictureBox5.Left += 5;
            if (count >= 13.5)
            {
                count = 0;
                timer3.Enabled = false;
            }
            else
            {
                count += 1.5;
            }
            if (timer3.Enabled == false)
            {
                timer4.Enabled = true;
                timer4.Interval = 100;

            }
        }

        private void timer4_Tick(object sender, EventArgs e)  //我方攻擊
        {

            pictureBox5.Left -= 5;
            if (count >= 13.5)
            {
                count = 0;
                timer4.Enabled = false;
                sk1.Enabled = true;
                sk2.Enabled = true;
                sk3.Enabled = true;
                sk4.Enabled = true;
            }
            else
            {
                count += 1.5;
            }
            if (timer4.Enabled == false)
            {

                //敵方若血量歸零則換角色
                if (Pokemon_HP[Pokemons_Enemy_Name[Level - 5]] <= 0)
                {
                    Enemy_Defeated();


                }
                else
                {
                    Enemy_Attack(); //我方攻擊結束輪到敵人攻擊
                }

            }

        }


        //Timer5~8為敵人攻擊特效 
        private void timer5_Tick(object sender, EventArgs e)  //敵方蓄力
        {
            
            pictureBox5.Left += 15;

            if (count >= 5)
            {
                count = 0;
                timer5.Enabled = false;

            }
            else
            {
                count += 1.5;
            }
        }

        private void timer6_Tick(object sender, EventArgs e)  //敵方蓄力
        {
            pictureBox5.Left -= 30;
            if (count >= 9)
            {
                count = 0;
                timer6.Enabled = false;

            }
            else
            {
                count += 1;
            }
            if (timer6.Enabled == false)
            {
                timer7.Enabled = true;
                timer7.Interval = 50;

            }
        }

        private void timer7_Tick(object sender, EventArgs e) //敵方攻擊
        {
            pictureBox5.Left -= 9;
            pictureBox4.Left -= 6;
            if (count >= 9)
            {
                count = 0;
                timer7.Enabled = false;
            }
            else
            {
                count += 1;
            }
            if (timer7.Enabled == false)
            {
                timer8.Enabled = true;
                timer8.Interval = 100;

            }
        }

        

        private void timer8_Tick(object sender, EventArgs e)  //敵方攻擊
        {
            
            pictureBox5.Left += 39;
            pictureBox4.Left += 6;

            

            if (count >= 9)
            {
                count = 0;
                timer8.Enabled = false;

                if (comboBox1.Text == "(陣亡)")
                {

                    textBox1.Text += Environment.NewLine + "此角色已陣亡";

                    sk1.Enabled = false;
                    sk2.Enabled = false;
                    sk3.Enabled = false;
                    sk4.Enabled = false;

                    label1.Text = "";
                    progressBar1.Visible = false;
                    pictureBox4.Visible = false;
                    pictureBox6.Visible = true;

                }
 

                if (pictureBox6.Visible == false)
                {
                    sk1.Enabled = true;
                    sk2.Enabled = true;
                    sk3.Enabled = true;
                    sk4.Enabled = true;
                }
                

                
                if (timer8.Enabled == false)
                {
                    comboBox1.Enabled = true; //敵方攻擊結束才可更換寶可夢
                    
                }

            }
            else
            {
                count += 1;

                if (comboBox1.Text == "(陣亡)")
                {
                    sk1.Enabled = false;
                    sk2.Enabled = false;
                    sk3.Enabled = false;
                    sk4.Enabled = false;
                }
                
            }

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Close();
            f2.Show();
        }

        private void timer9_Tick(object sender, EventArgs e)
        {
            if (count >= 32)
            {
                
                timer9.Enabled = false;
                button3.Visible = true;
                count = 0;
                
            }

            else if (count >= 24)
            {
                count +=1;

                pictureBox2.Visible = true;
                pictureBox3.Visible = true;
                 
            }
            else if (count >= 12)
            {
                pictureBox5.Visible = false;
                pictureBox4.Visible = false;
                pictureBox6.Visible = false;

                progressBar1.Visible = false;
                progressBar2.Visible = false;

                label2.Text = "";
                label1.Text = "";
                count += 1;
            }
            
            else
            {
                count += 1;
            }

          
        }


    }

    
}
