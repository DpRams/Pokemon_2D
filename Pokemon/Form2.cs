using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pokemon
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string[] Player = { "./Player/李.png", "./Player/郭.png", "./Player/陳.png" };
        string[] Author = { "CBF106008 李冠穎", "CBF106002 郭丞哲", "CBF106005 陳家銘" };
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Visible = false;
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Visible == true)
            { textBox1.Visible = false; }
            else
            {
                textBox1.Visible = true;
                textBox1.Text = "神奇寶貝聯盟大賽冠軍，是所有神奇寶貝訓練家的夢想巔峰"+Environment.NewLine+Environment.NewLine + "來自真新鎮的訓練家小智，歷經長達20年的奮鬥" + Environment.NewLine + Environment.NewLine  + "最後仍然飲恨在冠軍戰中敗下陣來" + Environment.NewLine + Environment.NewLine + "然而這份意志將由同為訓練家的瑟蕾娜繼續傳承下去" + Environment.NewLine + Environment.NewLine + "在挑戰名為最高殿堂的聯盟大賽前" + Environment.NewLine+Environment.NewLine + "她決定先從各地道館開始挑戰以壯大自身實力…";

            }  
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            num++;
            if (num == 3)
            {
                num = 0;
            }
            pictureBox1.Image = Image.FromFile(Application.StartupPath + Player[num]);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            label2.Text = Author[num];

        }
        static int num = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Visible == true)
            {
                pictureBox1.Visible = false;
                label2.Visible = false; 
            }
            else
            {
                pictureBox1.Visible = true;
                label2.Visible = true;
                label2.Text = Author[num];
            }
            
            pictureBox1.Image = Image.FromFile(Application.StartupPath + Player[num]);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            
            label2.Text = Author[num];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            this.Visible = false;
            f3.Show();
        }
    }
}
