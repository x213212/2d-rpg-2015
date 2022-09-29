using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using WMPLib;
using System.IO;
namespace WindowsFormsApplication1
{
    public partial class CombatGUI : Form
    {
        int Poison = 3;
        MP3Player back = new MP3Player();
        public Bitmap bmplayer3 = new Bitmap("pok/2.png");
        public Bitmap bmplayer4 = new Bitmap("pok/1.png");
        public string now_monname;
        int catching;
        public int train = 0;
        int enhp = (2000 / 200);
        int plhp = (2000 / 200);
        int plattack_now = 0;
        int plattack = 0;
        int enattack_now = 0;
        int enattack = 0;
        public int time;
        public int win;
        public int outside;
        public double tmpx;
        public double tmpy;
        public int enytmpx;
        public int enytmpy;
        int catcho;
        int sleeptime = 0;
        int sh1;
        int sh2;
        public int over = 0;
        int locked = 0;
        public int over2()
        {
            return over;
        }
        void MyBackgroundTask()
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;

            Thread.Sleep(20000);

            Environment.Exit(Environment.ExitCode);
        }
        public string backroundsound;

        public Bitmap f1, f2;

        public CombatGUI()
        {

            InitializeComponent();

        }


        private static Bitmap Process(Bitmap originImage, int oriwidth, int oriheight, int width, int height)
        {
            Bitmap resizedbitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(resizedbitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(originImage, new Rectangle(0, 0, width, height), new Rectangle(0, 0, oriwidth, oriheight), GraphicsUnit.Pixel);
            return resizedbitmap;
        }
        public static Bitmap Resize(Bitmap originImage, Double times)
        {
            int width = Convert.ToInt32(originImage.Width * times);
            int height = Convert.ToInt32(originImage.Height * times);

            return Process(originImage, originImage.Width, originImage.Height, width, height);
        }
        public Bitmap TakeScreenshot(int y, int x, Bitmap bmpx, int cutx1, int cutx2, int cuty1, int cuty2, double multiple)
        {
            Bitmap destBitmap = new Bitmap(bmpx.Width, bmpx.Height);
            Rectangle destRect = new Rectangle(0, 0, destBitmap.Width / cutx1, destBitmap.Height / cuty1); // 你的輸出範圍
            Rectangle srcRect = new Rectangle(y, x, bmpx.Width / cutx1, bmpx.Height / cuty2); // 你的原圖剪裁區域
            Graphics.FromImage(destBitmap).DrawImage(bmpx, destRect, srcRect, GraphicsUnit.Pixel);
            return Resize(destBitmap, multiple);
        }
        private void CombatGUI_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Z:
              
                    if (locked == 1 && catching == 0)
                    {

                        change_if();
                   


                    }



                    break;

                    /*     case Keys.Down:
                             radioButton1.Focus();
                             radioButton5.Focus();
                             break;
                         case Keys.Up:
                             radioButton4.Focus();
                             radioButton8.Focus();
                             break;*/
            }
        }

        void MyBackgroundTask2()
        {
            Thread.Sleep(sleeptime);
        }

        private void CombatGUI_Load(object sender, EventArgs e)
        {

            // 初始化画板
            Bitmap image = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            // 获取背景层
            Bitmap bg = (Bitmap)pictureBox1.BackgroundImage;
            Bitmap bmplayer2 = new Bitmap("pok/ball.png");
            // 初始化画布
            Bitmap canvas = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);

            // 初始化图形面板
            Graphics g = Graphics.FromImage(image);
            Graphics gb = Graphics.FromImage(canvas);
            pictureBox2.BackgroundImage = canvas; // 设置为背景层
            pictureBox2.CreateGraphics().DrawImage(canvas, 0, 0);
            bg = new Bitmap("pok/back.png");
   
            DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer3, 1, 1, 1, 1, 2), EnemyPB1.Location.X, EnemyPB1.Location.Y, 1, 1);

            this.Focus();

            wmp2.URL = "sound/battle.mp3";
            wmp2.Ctlcontrols.play();


            PartyPB1.Image = f1;
            EnemyPB1.Image = f2;
            enytmpx = EnemyPB1.Location.X;
            enytmpy = EnemyPB1.Location.Y;

            //  ModifyProgressBarColor.SetState(progressBar1, 3);
            //        ModifyProgressBarColor.SetState(progressBar2, 3);
            //   ModifyProgressBarColor.SetState(progressBar3, 1);
            //  ModifyProgressBarColor.SetState(progressBar4, 1);

            enemyAttackTimer.Enabled = true;
            Playerattacttimer.Enabled = true;
            if (train == 1)
                radioButton2.Enabled = false;
            else
                radioButton2.Enabled = true;
            locked = 1;
            label4.Text = "what to do now?";
            check_pok();
        }


        private void enemyAttackTimer_Tick_1(object sender, EventArgs e)
        {



        }



        private void Playerattacttimer_Tick(object sender, EventArgs e)
        {


        }



       void change_if()
        {
            Random crandom = new Random(Guid.NewGuid().GetHashCode());
            int crit = crandom.Next(1, 10);
            int crit2 = crandom.Next(1, 100);

            if (radioButton1.Checked == true)
            {
                label4.Visible = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;

                radioButton5.Visible = true;
                radioButton6.Visible = true;
                radioButton7.Visible = true;
                radioButton8.Visible = true;
                locked = 0;
 

            }
            else if (radioButton5.Checked == true || radioButton6.Checked == true)
            {


                ///////////////////////////////////////////////////////////////////
                if (crit < 2)
                {
                    wmp3.URL = "sound/lit.wav";
                    label4.Text = "Player attack!";
                }
                if (crit >= 2 && crit < 8)
                {
                    wmp3.URL = "sound/knf.wav";
                    label4.Text = "Player attack!";
                }
                if (crit >= 9)
                {
                    wmp3.URL = "sound/cr.wav";
                    label4.Text = "Player Crit attack!";
                }
                if (crit >= 6)
                    if (radioButton6.Checked == true)
                    {
                        wmp3.URL = "sound/cr.wav";
                        timer3.Enabled = false;
                    }

                wmp3.Ctlcontrols.play();

                attack(enhp, crit, 1);
                timer7.Enabled = true;
                ///////////////////////////////////////////////////////////////////
                //   cmbSkill.Items.Add("Player attack!" ;
                label4.Visible = true;


                radioButton5.Visible = false;
                radioButton6.Visible = false;
                radioButton7.Visible = false;
                radioButton8.Visible = false;

                locked = 0;
            }
            else if (radioButton2.Checked == true)
            {
                enemyAttackTimer.Enabled = false;
                Playerattacttimer.Enabled = false;


                label4.Text = "丟出大師球";
                this.Refresh();
                pokeball();
            }
            else if (radioButton4.Checked == true)
            {
                enemyAttackTimer.Enabled = false;
                Playerattacttimer.Enabled = false;
                if (crit2 > 30)
                {
                    stop();
                    outside = 1;
                    wmp3.URL = "sound/out.wav";
                    wmp3.Ctlcontrols.play();

                    label4.Text = "逃跑成功!!";
                   
                }
                else
                {
                    enemyAttackTimer.Enabled = true;
                    Playerattacttimer.Enabled = true;
                    wmp3.URL = "sound/out.wav";
                    wmp3.Ctlcontrols.play();

                    label4.Text = "逃跑失敗!!";
                    enyattack();

                }
            }

       
        }





        private void wmp_StatusChange(object sender, EventArgs e)
        {
            /*     if ((int)wmp.playState == 1)//如果播放状态等于停止
                 {
                     if (start == 0)
                     {
                         enemyAttackTimer.Enabled = true;
                         Playerattacttimer.Enabled = true;
                         wmp2.URL = "battle.mp3";
                         wmp2.Ctlcontrols.play(); //这里写你的处理代码
                         start = 1;
                     }
                 }*/
        }

        void pokeball()
        {

            // 初始化画板
            Bitmap image = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            MP3Player mp3 = new MP3Player();
            // 获取背景层
            Bitmap bg = (Bitmap)pictureBox1.BackgroundImage;
            Bitmap bmplayer2 = new Bitmap("pok/ball.png");

            // 初始化画布
            Bitmap canvas = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);

            // 初始化图形面板
            Graphics g = Graphics.FromImage(image);
            Graphics gb = Graphics.FromImage(canvas);
            catching = 1;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            pictureBox2.BackgroundImage = canvas; // 设置为背景层
            pictureBox2.CreateGraphics().DrawImage(canvas, 0, 0);
            bg = new Bitmap("pok/back.png");
            double t;
            int xxx = 0;
            back.Play("sound/throw.wav");

            for (t = -5; t < 2; t = t + 0.2)
            {

                double x = xx(2.5 * t, 500);
                double y = yy(3 * t, 0);

                DRAWIMAGEXX(gb, TakeScreenshot(0, xxx, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(x), Convert.ToInt32(y), 1, 1);

                if (y < 300 && x > 450)
                {
                    tmpx = x + 10;
                    tmpy = y;
                    mp3.Play("sound/inside2.wav");

                    break;
                }
                if (xxx <= 320)
                    xxx += 40;
                else
                    xxx = 0;
                label3.Text = Convert.ToInt32(x).ToString() + ":" + Convert.ToInt32(y).ToString();
            }

            DRAWIMAGEXX(gb, TakeScreenshot(0, 400, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);

            for (t = 7; t >= 6; t = t - 0.2)

            {
                sleeptime = 5;
                Thread t1 = new Thread(MyBackgroundTask2);
                t1.Start();
                t1.Join();
                double x = xx(2.5 * t, tmpx);
                double y = yy(0.2 * t, tmpy);

                if (t == 6)
                {
                    tmpx = x;
                    tmpy = y;
                }

            }



            DRAWIMAGEXX(gb, TakeScreenshot(0, 400, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);



            EnemyPB1.SendToBack();
            mp3.Play("sound/inside.wav");

            for (double i = 2; i >= 1; i -= 0.1)
            {

                sleeptime = 1;
                Thread t1 = new Thread(MyBackgroundTask2);
                t1.Start();
                t1.Join();
                DRAWIMAGEXX(gb, TakeScreenshot(0, 400, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 0, 0);
                DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer3, 1, 1, 1, 1, i), EnemyPB1.Location.X, EnemyPB1.Location.Y, 1, 1);

                if (EnemyPB1.Location.X >= pictureBox2.Location.X + 50)
                    EnemyPB1.Location = new Point(EnemyPB1.Location.X - 2, EnemyPB1.Location.Y - 1);
                else
                    EnemyPB1.Image = null;

            }

            DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);
            back.Play("sound/ballund.wav");

            for (t = 0; t < 5; t = t + 0.4)
            {


                double x = tmpx;
                double y = yy(1.5 * t, 0);
                if (y >= 200)
                {
                    tmpy = y;
                    break;
                }
                DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);
                tmpy = y;
            }

            for (t = 5; t >= 0; t = t - 0.2)
            {

                double x = tmpx;
                double y = yy(t, 0);
                if (y <= 100)
                {
                    tmpy = 100;
                    break;
                }
                DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);
                tmpy = y;
            }
            back.Play("sound/ballund.wav");
            for (t = 0; t < 5; t = t + 0.4)
            {
                double x = tmpx;
                double y = yy(1.5 * t, tmpy);
                if (y >= 200)
                {
                    break;
                }
                DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);
                tmpy = y;
            }

            int pok = 440;
            for (int j = 1; j <= 6; j++)
            {
                if (j % 2 == 0) pok = 440; else pok = 560;
                for (int i = 1; i <= 29; i += 2)
                {
                    label3.Text = i.ToString();
                    if (i == 1)
                        mp3.Play("sound/ball.wav");
                    sleeptime = 1;
                    Thread t1 = new Thread(MyBackgroundTask2);
                    t1.Start();
                    t1.Join();
                    bmplayer2 = new Bitmap("pok/ball.png");
                    DRAWIMAGEXX(gb, TakeScreenshot(0, pok, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);

                    if (i % 9 == 0)
                        pok += 40;
                }
                DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);


            }

            mp3.Play("sound/get.mp3");
            DRAWIMAGEXX(gb, TakeScreenshot(0, 0, bmplayer2, 17, 17, 17, 17, 1.5), Convert.ToInt32(tmpx), Convert.ToInt32(tmpy), 1, 1);
            EnemyPB1.Location = new Point(enytmpx, enytmpy);

            label4.Text = "抓到了!!";

            wmp2.URL = "sound/win2.mp3";
            wmp2.Ctlcontrols.play();
            if(pokarr[11]=="-1")
            get_pok();
            catcho = 1;
            stop();
            gb.Clear(Color.Transparent);
            catching = 0;
            radioButton1.Enabled = true;
            if (train == 1)
                radioButton2.Enabled = false;
            else
                radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            radioButton4.Enabled = true;
        }
        public void DRAWIMAGEXX(Graphics gb, Bitmap image, double x, double y, int refresh, int clear)
        {
            gb.DrawImage(image, Convert.ToInt32(x), Convert.ToInt32(y)); // 先绘制背景层
            if (refresh == 1)
                pictureBox2.Refresh();
            if (clear == 1)
                gb.Clear(Color.Transparent);
            GC.Collect();

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            back.Play("sound/menu.wav");
        }
        void stop()
        {
            enemyAttackTimer.Enabled = false;
            Playerattacttimer.Enabled = false;

        }
        void start()
        {
            enemyAttackTimer.Enabled = true;
            Playerattacttimer.Enabled = true;

        }




        private double xx(double t, double basex)
        {
            double v0 = 30;//水平初速度
            return (v0 * t) + basex;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            back.Play("sound/menu.wav");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            back.Play("sound/menu.wav");
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

            if (catcho == 1 || win == 1 || outside == 1 || over == 1)
            {

                over = 1; this.Close();

            }
            back.Play("sound/menu.wav");
        }

        private void timer7_Tick(object sender, EventArgs e)
        {

            sh1 += 1;
            if (EnemyPB1.Visible == true)
                EnemyPB1.Visible = false;
            else
                EnemyPB1.Visible = true;
            if (sh1 >= 4)
            {
                EnemyPB1.Visible = true;
                timer7.Enabled = false;
                locked = 0;
                sh1 = 0;
            }
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            sh2 += 1;
            if (PartyPB1.Visible == true)
                PartyPB1.Visible = false;
            else
                PartyPB1.Visible = true;

            if (sh2 >= 4)
            {
                PartyPB1.Visible = true;
                timer8.Enabled = false;
           
                sh2 = 0;
               
            
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox5.Size = new Size(pictureBox5.Size.Width + 5, pictureBox5.Size.Height);
        }
        public void attack(int basic, int sum, int change)
        {
            plattack = basic * sum;
            plattack_now = pictureBox5.Size.Width - plattack;
            if (change == 1)
                timer9.Enabled = true;
            timer9.Enabled = true;
        }
        public void attack2(int basic, int sum)
        {
            enattack = basic * sum;
            enattack_now = pictureBox7.Size.Width - enattack;
            timer10.Enabled = true;
        }
        void enyattack()
        {
            Random crandom = new Random(Guid.NewGuid().GetHashCode());
            int crit = crandom.Next(1, 10);

            if (crit < 2)
            {
                wmp3.URL = "sound/lit.wav";
                label4.Text = "enermy Attack!!";
            }
            if (crit > 2 && crit < 9)
            {
                wmp3.URL = "sound/knf.wav";
                label4.Text = "enermy Attack!!";


            }
            if (crit >= 9)
            {

                label4.Text = "enermy Attack!!";

                wmp3.URL = "sound/cr.wav";


            }

            wmp3.Ctlcontrols.play();

            attack2(plhp, crit);
            timer8.Enabled = true;
           

        }
        private void timer9_Tick(object sender, EventArgs e)
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;

            this.Refresh();
            if ((pictureBox5.Size.Width) <= 150 && (pictureBox5.Size.Width) >= 50)
                pictureBox5.BackColor = Color.Orange;
            else if ((pictureBox5.Size.Width) < 50)
                pictureBox5.BackColor = Color.Red;

            if ((pictureBox5.Size.Width - 1) != plattack_now)
                pictureBox5.Size = new Size(pictureBox5.Size.Width - 1, pictureBox5.Size.Height);
            else
            {
           
                start();
                plattack = 0;
                plattack_now = 0;
                timer9.Enabled = false;
                enyattack();
           
         
            }
            if (pictureBox5.Size.Width <= 0)
            {
                label4.Text = "Youw Win!!";
                win = 1;
                wmp2.URL = "sound/win.mp3";
                wmp2.Ctlcontrols.play();
                timer9.Enabled = false;
                wmp5.Ctlcontrols.stop();
                timer3.Enabled = false;
                timer4.Enabled = false;
                timer5.Enabled = false;
                timer6.Enabled = false;
                radioButton1.Enabled = true;
                if (train == 1)
                    radioButton2.Enabled = false;
                else
                    radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
                stop();
            }

        }

        private void timer10_Tick(object sender, EventArgs e)
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;

            this.Refresh();
            if ((pictureBox7.Size.Width) > 150)
            {
                wmp2.Ctlcontrols.play();

                wmp5.Ctlcontrols.pause();

                pictureBox7.BackColor = Color.MediumSeaGreen;
            }
            else if ((pictureBox7.Size.Width) <= 150 && (pictureBox7.Size.Width) >= 50)
            {
                wmp2.Ctlcontrols.play();

                wmp5.Ctlcontrols.pause();

                pictureBox7.BackColor = Color.Orange;
            }
            else if ((pictureBox7.Size.Width) < 50)
            {

                wmp2.Ctlcontrols.pause();
                if (wmp5.URL != "sound/lowhp.mp3")

                    wmp5.URL = "sound/lowhp.mp3";
                wmp5.Ctlcontrols.play();

                pictureBox7.BackColor = Color.Red;

            }

            if ((pictureBox7.Size.Width - 1) != enattack_now)
                pictureBox7.Size = new Size(pictureBox7.Size.Width - 1, pictureBox7.Size.Height);
            else
            {
                label4.Text = "what to do now?";
                radioButton1.Enabled = true;
                if (train == 1)
                    radioButton2.Enabled = false;
                else
                    radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
                
         
           
                stop();

                start();
                enattack = 0;
                enattack_now = 0;
                timer10.Enabled = false;
                radioButton5.Checked =false;
            }
            if (pictureBox7.Size.Width <= 0)
            {
                pictureBox1.BringToFront();
                pictureBox1.ImageLocation = "pok/gameover.png";
                enemyAttackTimer.Enabled = false;

                wmp2.Ctlcontrols.stop();
                pictureBox1.Visible = true;
                wmp.Ctlcontrols.stop();
                wmp2.Ctlcontrols.stop();
                wmp.URL = "sound/gameover.mid";
                wmp.Ctlcontrols.play();
                Thread t1 = new Thread(MyBackgroundTask);
                t1.Start();

                timer10.Enabled = false;
                wmp5.Ctlcontrols.stop();
                stop();
            }




        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

            back.Play("sound/menu.wav");
            locked = 1;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {


            back.Play("sound/menu.wav");
            locked = 1;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

            back.Play("sound/menu.wav");
            locked = 1;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {


            back.Play("sound/menu.wav");
            locked = 1;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {


            Poison--;
            pictureBox11.BackColor = Color.Purple;
            attack(enhp, 1, 0);
            if (Poison <= 0)
            {
                pictureBox11.BackColor = Color.White;
                timer3.Enabled = false;
                Poison = 3;
            }
        }

        private void wmp_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 1)
                this.wmp.Ctlcontrols.play();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
     
        }

        private double yy(double t, double basey)
        {
            double g = 9.8;//重力加速度
            return (0.5 * g * t * t) + basey;
        }

        int pok_length;
        string all_pok;
        int item;
        string[] pokarr = new string[12];
        private void check_pok()
        {
            StreamReader sr = new StreamReader(@"pok.txt");
            string a;
            //===逐行讀取，直到檔尾===
            item = 0;
            while (!sr.EndOfStream)
            {
                a = sr.ReadLine();
                string[] strArray = a.Split(',');
                for (int i = 0; i < strArray.Length; i++)        //透過迴圈將陣列值取出 也可用foreach
                {
                    pokarr[item] = strArray[i].ToString();
                    item++;
                }

                pok_length = strArray.Length;
            }

            sr.Close();

        }
        private void get_pok()
        {
            for (int i= 0; i <14; i++)
            {
                if (pokarr[i] == "-1")
                {
                    item = i;
                    break;
                }

            }
            pokarr[item] = now_monname;
            pokarr[item+1] = "1000";
            for (int i = 0; i < 12; i++)
            {
                all_pok += pokarr[i];
                if ((i + 1) % 2 == 0)
                {
                    all_pok += "\r\n";
                }
                else
                    all_pok += ",";
            }

            StreamWriter sw = new StreamWriter(@"pok.txt", false);
            //第二個參數設定為true表示不覆蓋原本的內容，把新內容直接添加進去
            sw.Write(all_pok);
            sw.Flush();
            sw.Close();
            all_pok = null;

        }
    }
    /*
        public static class ModifyProgressBarColor
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
            static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
            public static void SetState(this ProgressBar pBar, int state)
            {
                SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
            }
        }*/

}

