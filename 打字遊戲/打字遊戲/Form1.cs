using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 打字遊戲
{
    public partial class Form1 : Form
    {
        Random rd = new Random(); //宣告亂數
        Random rdcol = new Random(); //宣告顏色亂數
        Color[] col = new Color[3]; //三種顏色
        FileInfo fi;  //宣告建檔
        StreamWriter sw; //寫檔

        int score; //分數
        int tmp; //排序暫存
        int Num = 1; //幾人
        int clik = 0, cliktrue = 0; //按錯及沒按錯
        int[,] ch = new int[3, 4]; //宣告陣列

        public Form1()
        {
            InitializeComponent(); //初始化

            label6.Visible = false;  //排行榜隱藏
            textBox1.Visible = false; 

            pictureBox2.Visible = false; //字母隱藏
            label2.Visible = false; //圖片隱藏

            pictureBox3.Visible = false;
            label3.Visible = false; //圖片隱藏

            pictureBox4.Visible = false; //字母隱藏
            label4.Visible = false; //圖片隱藏

            col[0] = Color.White; //陣列第一個白色
            col[1] = Color.Gold; //陣列第二個金色
            col[2] = Color.GreenYellow; //陣列第三個黃綠色
        }

        void reset() //重設
        {
            for (int i = 0; i < 3; i++)
            {
                ch[i, 0] = rd.Next(33, 127);
                ch[i, 1] = rd.Next(50, 90);
                ch[i, 2] = 0; //有沒有打到
            }

            label7.Text = "0";
            score = 0;
            progressBar1.Value = 100;
            progressBar2.Value = 100;
        }

        void ASCII()
        {
            if (clik > 3)
            {
                return;
            }

            ch[clik, 0] = rd.Next(33, 127);
            ch[clik, 1] = rd.Next(50, 90);
            ch[clik, 2] = 0; //有沒有打到

            if (clik == 0)
            {
                pictureBox2.Top = ch[0, 1];
                label2.Text = ((char)ch[0, 0]).ToString();
            }

            if (clik == 1)
            {
                pictureBox3.Top = ch[1, 1];
                label3.Text = ((char)ch[1, 0]).ToString();
            }

            if (clik == 2)
            {
                pictureBox4.Top = ch[2, 1];
                label4.Text = ((char)ch[2, 0]).ToString();
            }
        }

        void start()
        {
            for (int i = 0; i < 3; i++)
            {
                clik = i;
                ASCII();
            }

            pictureBox2.Visible = true;
            label2.Visible = true;

            pictureBox3.Visible = true;
            label3.Visible = true;

            pictureBox4.Visible = true;
            label4.Visible = true;

            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            timer4.Enabled = true;

            button4.Visible = false;
            label6.Visible = false;
            textBox1.Visible = false;
            reset();          
        }

        void PHOTO()
        {
            pictureBox1.Image = Image.FromFile("gamephoto\\background.jpg");

            TP(label6);
            TP(label1);
            TP(label5);
            TP(label7);
            TP(label9);

            TP2(pictureBox2,label2);
            TP2(pictureBox3,label3);
            TP2(pictureBox4,label4);
        }

        void save()
        {
            fi = new FileInfo("score.txt");
            sw = fi.CreateText();

            for (int i = 0; i < ch[i,3]; i++)
            {
                sw.WriteLine(ch[i, 3].ToString());
            }

            sw.Flush();
            sw.Close();
        }

        void TP(Label a)
        {
            a.BackColor = Color.Transparent;
            pictureBox1.Controls.Add(a);
        }

        void TP2(PictureBox c, Label b)
        {
            c.BackColor = Color.Transparent;
            pictureBox1.Controls.Add(c);
            b.BackColor = Color.Transparent;
            b.Location = new Point(16, 10);
            c.Controls.Add(b);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PHOTO();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            start();
            button1.Visible = false;
        }   

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 0)
            {
                gameover();
            }

            if(progressBar2.Value - 1 > 0)
            {
                progressBar2.Value -= 1;
            }
            else
            {
                gameover();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            txt(pictureBox2, label2);
        }

        private void Timer3_Tick(object sender, EventArgs e)
        {
            txt(pictureBox3, label3);
        }

        private void Timer4_Tick(object sender, EventArgs e)
        {
            txt(pictureBox4, label4);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        aaaa(label2, i,e.KeyChar.ToString());
                        break;

                    case 1:
                        aaaa(label3, i, e.KeyChar.ToString());
                        break;

                    case 2:
                        aaaa(label4, i, e.KeyChar.ToString());
                        break;
                }
            }

            if (cliktrue == 0)
            {
                MISS(clik);

                if (progressBar1.Value <= 0)
                {
                    gameover();
                    reset();
                    return;
                }
            }
            cliktrue = 0;
        }

        void txt(PictureBox p, Label l)
        {
            if (p.Top + 5 > this.ClientSize.Height - 40) //失誤
            {
                clik = 0;
                progressBar2.Value = 0;
                gameover();
                save();
            }
            else
            {
                p.Top += 5;
            }
        }

        void aaaa(Label aaaaa, int i,string aaaaaa)
        {
            if (aaaaaa == aaaaa.Text)
            {
                MissOrNot(i);
                aaaaa.ForeColor = col[rdcol.Next(0, 3)];
                cliktrue ++;
            }
        }

        void gameover() //結束遊戲
        {
            ch[Num, 3] = score;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (ch[i, 3] > ch[j, 3])
                    {
                        tmp = ch[i, 3];
                        ch[i, 3] = ch[j, 3];
                        ch[j, 3] = tmp;
                    }
                }
            }

            textBox1.Clear();
            for (int i = 0; i < Num; i++)
            {
                textBox1.AppendText("玩家 " + (i + 1).ToString() + "：" + ch[i, 3].ToString() + "分\r\n");
            }
            Num++;

            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;

            label6.Visible = true;         
            button1.Visible = true;
            textBox1.Visible = true;
            MessageBox.Show("GameOver");           
            return;
        }

        private void Button2_Click(object sender, EventArgs e) //暫停
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            button2.Visible = false;
            button4.Visible = true;
        }

        private void Button4_Click(object sender, EventArgs e) //繼續
        {
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            timer4.Enabled = true;
            button2.Visible = true;
            button4.Visible = false;
        }

        void MISS(int i) //按錯
        {
            score -= 5;
            ch[i, 2] = 0;
            label7.Text = score.ToString();

            if (progressBar2.Value - 3 > 0)
            {
                progressBar2.Value -= 3;
            }
            else
            {
                progressBar2.Value = 0;
                gameover();
            }
        }

        void MissOrNot(int i) //有按對功能
        {
            clik = i;
            score += 20;
            ch[i, 2] = 1;
            label7.Text = score.ToString();

            if (progressBar1.Value - 3 > 0)
            {
                progressBar1.Value -= 3;
            }
            else
            {
                progressBar1.Value = 0;
                gameover();
            }
            ASCII();
        }
    }
}
