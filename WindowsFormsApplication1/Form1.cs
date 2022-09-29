using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Gma.UserActivityMonitor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int start = 0;
        int start2 = 0;
        int item = 0;
        int env2 = 0;
        int env = 0;
        int enving = 0;
        int map1girl = 0;
        int map2boy = 0;
        int min = 0;
        int menu;
        int menu2x;
        int menu2y;
        int talking = 0;
        string[] itemarr;
        public string[] pokarr;
        KeyEventArgs keyboardtmp;
        bool IsADown = false;
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(int hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int screensize);
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        private static IntPtr HWND_TOP = IntPtr.Zero;
        private const int SWP_SHOWWINDOW = 64;
        static Label[] labx = new Label[15];

        public Bitmap TakeScreenshot(int x, int y, Bitmap bmpx, int cutx1, int cutx2, int cuty1, int cuty2)
        {
            Bitmap destBitmap = new Bitmap(bmpx.Width, bmpx.Height);
            // destBitmap 為你的目的圖檔，長、寬為原圖1/3 
            Rectangle destRect = new Rectangle(0, 0, destBitmap.Width / cutx1, destBitmap.Height / cuty1); // 你的輸出範圍
            Rectangle srcRect = new Rectangle(x, y, bmpx.Width / cutx1, bmpx.Height / cuty2); // 你的原圖剪裁區域
            Graphics.FromImage(destBitmap).DrawImage(bmpx, destRect, srcRect, GraphicsUnit.Pixel);
            return destBitmap;
        }

        public Class1 game;
        public Form1()
        {

            InitializeComponent();

        }
        public void AlphaBlend(Bitmap source, byte alpha, Color backColor)
        {
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    Color sc = source.GetPixel(x, y);
                    byte R = (byte)(sc.R * alpha / 255 + backColor.R * (255 - alpha) / 255);
                    byte G = (byte)(sc.G * alpha / 255 + backColor.G * (255 - alpha) / 255);
                    byte B = (byte)(sc.B * alpha / 255 + backColor.B * (255 - alpha) / 255);
                    byte A = (byte)(sc.A * alpha / 255 + backColor.A * (255 - alpha) / 255);
                    source.SetPixel(x, y, Color.FromArgb(A, R, G, B));
                }
            }
        }

        Bitmap rainDrops;
        Timer timer = new Timer();
        byte xl = 255;


        private void Form1_Load(object sender, EventArgs e)
        {

            // timer.Interval = 1000;
            // timer.Enabled = true;
            // timer.Tick += delegate { Invalidate(); };
            labx[0] = item0;
            labx[1] = item1;
            labx[2] = item2;
            labx[3] = item3;
            labx[4] = item4;
            labx[5] = item5;
            labx[6] = item6;
            labx[7] = item7;
            labx[8] = item8;
            labx[9] = item9;
            labx[10] = item10;
            labx[11] = item11;
            labx[12] = item12;
            itemarr = new string[12];
            pokarr = new string[20];



            timer2.Enabled = true;
            // this.WindowState = FormWindowState.Normal;
            //  this.FormBorderStyle = FormBorderStyle.None;
            //  SetWindowPos((int)this.Handle, HWND_TOP, 0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), SWP_SHOWWINDOW);
            // this.ControlBox = true;
            timer2.Enabled = true;

        }
        private int checkface()
        {
            if (game.msy < game.ply)
                return 1;
            else if (game.msy > game.ply)
                return 2;
            else if (game.msx < game.plx)
                return 3;
            else if (game.msx > game.plx)
                return 4;
            else { return 0; }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            keyboardtmp = e;
            if (start == 0 && start2 == 0 && e.KeyCode == Keys.Z)
            {
                timer5.Enabled = true; back.URL = ("sound/menu.wav");
                back.Ctlcontrols.play();
            }
            if (start == 1)
            {
                if (enving == 0)
                {
                    game.handlekeypress(e);
                }

                //  if(e.KeyCode==Keys.Z)
                //    {
                if (e.KeyCode == Keys.X && talking == 0)
                {
                    label5.Text = "MAP:" + game.now_map;

                    if (pictureBox3.Visible == true)
                    {

                        enving = 0;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;


                        pictureBox6.Visible = false;
                        pictureBox7.Visible = false;
                        pictureBox8.Visible = false;
                        pictureBox9.Visible = false;
                        pictureBox10.Visible = false;

                        label7.Visible = false;
                        label8.Visible = false;
                        label10.Visible = false;
                        label11.Visible = false;
                        label12.Visible = false;

                        itemx.Visible = false;
                        label3.Visible = false;
                        label4.Visible = false;
                        label5.Visible = false;
                        pictureBox5.Visible = false;
                        for (int i = 0; i < 13; i++)
                            labx[i].Visible = false;

                        menu = 0;
                    }
                    else
                    {
                        menu2y = pictureBox6.Location.Y;
                        pictureBox4.Location = new Point(517, 43);
                        enving = 1;
                        pictureBox3.Visible = true;
                        pictureBox4.Visible = true;

                        label3.Visible = true;
                        label4.Visible = true;
                        label5.Visible = true;

                    }
                }
                if (e.KeyCode == Keys.Down && pictureBox3.Visible == true)
                {
                    if (menu == 2 && menu2y + 116 <= pictureBox10.Location.Y)
                        menu2y += 116;
                    if (menu == 0 && pictureBox4.Location.Y + 56 <= label4.Location.Y + 30)
                        pictureBox4.Location = new Point(pictureBox4.Location.X, pictureBox4.Location.Y + 56);
                    if (menu == 1 && pictureBox4.Location.Y + 93 <= labx[9].Location.Y + 30)
                        pictureBox4.Location = new Point(pictureBox4.Location.X, pictureBox4.Location.Y + 93);
                    for (int i = 0; i < 11; i++)
                    {
                        if (labx[i].Location.X == pictureBox4.Location.X + 20 && labx[i].Location.Y == pictureBox4.Location.Y - 5)
                        {
                            min = i;

                            check_item();
                        }
                    }



                }

                if (e.KeyCode == Keys.Up && pictureBox3.Visible == true)
                {
                    if (menu == 2 && menu2y - 116 >= pictureBox6.Location.Y)
                        menu2y -= 116;

                    if (menu == 0 && pictureBox4.Location.Y - 56 >= label3.Location.Y - 30)
                        pictureBox4.Location = new Point(pictureBox4.Location.X, pictureBox4.Location.Y - 56);
                    if (menu == 1 && pictureBox4.Location.Y - 93 >= labx[0].Location.Y - 30)
                        pictureBox4.Location = new Point(pictureBox4.Location.X, pictureBox4.Location.Y - 93);
                    for (int i = 0; i < 11; i++)
                    {
                        if (labx[i].Location.X == pictureBox4.Location.X + 20 && labx[i].Location.Y == pictureBox4.Location.Y - 5)
                        {
                            min = i;

                            check_item();
                        }
                    }


                }
                if (e.KeyCode == Keys.Left && itemx.Visible == true)
                {


                    if (menu == 1 && pictureBox4.Location.X - 141 >= labx[0].Location.X - 30)
                        pictureBox4.Location = new Point(pictureBox4.Location.X - 141, pictureBox4.Location.Y);
                    for (int i = 0; i < 11; i++)
                    {
                        if (labx[i].Location.X == pictureBox4.Location.X + 20 && labx[i].Location.Y == pictureBox4.Location.Y - 5)
                        {
                            min = i;

                            check_item();
                        }
                    }


                }
                if (e.KeyCode == Keys.Right && itemx.Visible == true)
                {


                    if (menu == 1 && pictureBox4.Location.X + 141 <= labx[2].Location.X + 30)

                        pictureBox4.Location = new Point(pictureBox4.Location.X + 141, pictureBox4.Location.Y);
                    for (int i = 0; i < 11; i++)
                    {
                        if (labx[i].Location.X == pictureBox4.Location.X + 20 && labx[i].Location.Y == pictureBox4.Location.Y - 5)
                        {
                            min = i;

                            check_item();
                        }
                    }


                }
                if (e.KeyCode == Keys.Z && pictureBox3.Visible == true && pictureBox4.Location.Y > label3.Location.Y && label3.Location.Y + label3.Size.Height > pictureBox4.Location.Y && menu == 0)
                {

                    talking = 0;
                    check_item();
                    menu = 1;
                    itemx.Visible = true;
                    for (int i = 0; i < 13; i++)
                        labx[i].Visible = true;
                    pictureBox4.Location = new Point(labx[0].Location.X - 20, labx[0].Location.Y + 5);

                }
                if (e.KeyCode == Keys.Z && pictureBox3.Visible == true && pictureBox4.Location.Y > label3.Location.Y && label4.Location.Y + label4.Size.Height > pictureBox4.Location.Y && menu == 0)
                {
                    check_pok();
                    menu2y = pictureBox6.Location.Y + Convert.ToInt32(pokarr[0]) * 116;
                    talking = 0;

                    menu = 2;

                    pictureBox5.Visible = true;

                    pictureBox6.Visible = true;
                    pictureBox7.Visible = true;
                    pictureBox8.Visible = true;
                    pictureBox9.Visible = true;
                    pictureBox10.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true;
                    label10.Visible = true;
                    label11.Visible = true;
                    label12.Visible = true;
                }

                if (e.KeyCode == Keys.Z && pictureBox3.Visible == false)
                {

                    if (game.now_map == "Test2.txt" && game.max_monster[0] == 1)
                    {
                        talking = 1;
                        game.talkface(checkface(), 0);
                        talk(map2boy);
                        map2boy++;

                        if (map2boy == 3 && env2 == 0)
                        {
                            label9.Text = "統神哥哥:再來阿";
                            talk(map2boy);

                            if (game.max_monster[0] == 1)
                            {

                                back3.URL = ("再來阿.mp3");
                                back3.Ctlcontrols.play();

                                wmp.Ctlcontrols.stop();
                                game.shock();
                                game.View(true);
                                game.ms1();
                                env2 = 1;
                                map2boy = 0;
                                talk(map2boy);
                            }
                        }
                        else if (map2boy == 1 && env2 == 0)
                        {

                            label9.Text = "統神哥哥:想幹架嗎??";
                            talk(map2boy);
                            map2boy++;
                        }
                        else if (map2boy == 3 && env2 == 1)
                        {
                            label9.Text = "統神哥哥:外掛狗";

                            talk(map2boy);
                            map2boy++;
                        }
                        else if (map2boy == 1 && env2 == 1)
                        {

                            label9.Text = "統神哥哥:笑笑不多說";
                            talk(map2boy);
                            map2boy++;

                        }

                        else { map2boy = 0; }

                    }

                    /////////////////////////////////////////////////
                    if (game.now_map == "Test2.txt" && game.max_monster[1] == 1)
                    {
                        talking = 1;
                        game.talkface(checkface(), 1);
                        talk(map1girl);
                        map1girl++;
                        if (map1girl == 5)
                        {

                            label9.Text = "你失去了哈密瓜";
                            talk(map1girl);
                            map1girl++;
                            delate_item(1);
                        }
                        else if (map1girl == 3)
                        {

                            label9.Text = "統表:你他媽有被揍過?無差別偷你哈密瓜";
                            talk(map1girl);
                            map1girl++;

                        }
                        else if (map1girl == 1)
                        {

                            label9.Text = "統表:我哥哥在隔壁別來煩我,滾";
                            talk(map1girl);
                            map1girl++;

                        }

                        else { map1girl = 0; }

                    }

                    /////////////////////////////////////////////////
                    if (game.now_map == "Test.txt" && game.max_monster[1] == 1)
                    {
                        talking = 1;
                        game.talkface(checkface(), 1);
                        talk(map1girl);
                        map1girl++;

                        if (map1girl == 7 && env == 0)
                        {

                            get_item(1);

                            back2.URL = ("item.mp3");
                            back2.Ctlcontrols.play();

                            label9.Text = "獲得哈密瓜.";
                            talk(map1girl);
                            map1girl++;
                            env = 1;

                        }
                        else if (map1girl == 5 && env == 0)
                        {

                            label9.Text = "小女孩:這是我撿到的一顆東西.";
                            talk(map1girl);
                            map1girl++;

                        }

                        else if (map1girl == 3)
                        {

                            label9.Text = "小女孩:嚇得我,趕快跑回家.";
                            talk(map1girl);
                            map1girl++;

                        }
                        else if (map1girl == 1)
                        {
                            label9.Text = "小女孩:我上次在後山看到一到閃光";

                            talk(map1girl);
                            map1girl++;
                        }

                        else { map1girl = 0; }
                    }
                    /////////////////////////////////////////////////
                    if (game.now_map == "Test.txt" && game.max_monster[0] == 1)
                    {
                        talking = 1;

                        game.talkface(checkface(), 0);
                        talk(map2boy);
                        map2boy++;

                        if (map2boy == 3)
                        {

                            label9.Text = "統神哥哥:你也想去試試看?";

                            talk(map2boy);
                            map2boy++;


                        }
                        else if (map2boy == 1)
                        {

                            label9.Text = "統神哥哥:我弟弟在野區找人單挑";
                            talk(map2boy);
                            map2boy++;
                        }

                        else { map2boy = 0; }
                    }

                }
            }
        }
        private void check_item()
        {
            StreamReader sr = new StreamReader(@"item.txt");
            string a;
            //===逐行讀取，直到檔尾===
            item = 0;
            while (!sr.EndOfStream)
            {
                a = sr.ReadLine();
                string[] strArray = a.Split(',');
                for (int i = 0; i < strArray.Length; i++)        //透過迴圈將陣列值取出 也可用foreach
                {
                    if (strArray[i].ToString() == "1")
                    {


                        labx[item].Text = "哈密瓜"; itemarr[item] = strArray[i].ToString();
                    }

                    else { labx[item].Text = strArray[i].ToString(); itemarr[item] = strArray[i].ToString(); }
                    item++;
                }
            }

            if (min >= 0)
                if (itemarr[min] == "1")
                    labx[12].Text = "有一點哈味不過還能吃.";
                else
                    labx[12].Text = labx[min].Text;



            sr.Close();
        }
        int zero;
        string all_item;
        private void delate_item(int item)
        {

            for (int i = 0; i < 12; i++)
            {
                all_item += itemarr[i];
                if ((i + 1) % 3 == 0)
                {
                    all_item += "\r\n";
                }
                else
                    all_item += ",";
            }
            all_item = all_item.Replace(item.ToString(), "-");
            StreamWriter sw = new StreamWriter(@"item.txt", false);
            //第二個參數設定為true表示不覆蓋原本的內容，把新內容直接添加進去
            sw.Write(all_item);
            sw.Flush();
            sw.Close();
            all_item = null;
        }
        private void get_item(int item)
        {

            for (int i = 0; i < 12; i++)
            {

                if (itemarr[i] == "-")
                { zero = i; break; }
            }
            if (item == 1)
                itemarr[zero] = "1";
            for (int i = 0; i < 12; i++)
            {
                all_item += itemarr[i];

                if ((i + 1) % 3 == 0)
                {
                    all_item += "\r\n";
                }

                else
                    all_item += ",";
            }

            StreamWriter sw = new StreamWriter(@"item.txt", false);
            //第二個參數設定為true表示不覆蓋原本的內容，把新內容直接添加進去
            sw.Write(all_item);
            sw.Flush();
            sw.Close();
            all_item = null;

        }
        int pok_length;
        string all_pok;
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
        private void set_pok()
        {

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
        private void talk(int tmp)
        {

            if (tmp == 0 || tmp % 2 == 0)
            {
                pictureBox2.Visible = false;
                label9.Visible = false;
                enving = 0;
                talking = 0;
            }
            else if (tmp % 2 == 1)
            {

                pictureBox2.Visible = true;
                label9.Visible = true;
                enving = 1;
                talking = 1;
            }


        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Bitmap srcBitmap = new Bitmap("pok/" + "player2.png");// bitmap 為你的原圖
            Bitmap destBitmap = new Bitmap(srcBitmap.Width, srcBitmap.Height);
            // destBitmap 為你的目的圖檔，長、寬為原圖1/3 
            Rectangle destRect = new Rectangle(0, 0, destBitmap.Width / 3, destBitmap.Height / 4); // 你的輸出範圍
            Rectangle srcRect = new Rectangle(43, 0, srcBitmap.Width / 3, srcBitmap.Height / 4); // 你的原圖剪裁區域
            Graphics.FromImage(destBitmap).DrawImage(srcBitmap, destRect
            , srcRect, GraphicsUnit.Pixel);
            pictureBox1.Image = destBitmap;
        }


        private void timer2_Tick(object sender, EventArgs e)
        {

            if (start == 1)
            {
                if (game.fight == 1)
                {
                    wmp.Ctlcontrols.pause();
                    if (game.atack.over2() == 1)
                    {
                        game.View(false);
                        wmp.Ctlcontrols.play();
                        game.fight = 0;
                    }
                }
                if (game.changemaptest == 1)
                {

                    if (game.now_map == "Test.txt")
                    {
                        wmp.URL = "sound/theme.mp3";
                        wmp.Ctlcontrols.play();
                        timer5.Enabled = true;
                        game.changemaptest = 0;


                    }
                    if (game.now_map == "Test2.txt")
                    {
                        wmp.URL = "sound/101.mp3";
                        wmp.Ctlcontrols.play();
                        timer5.Enabled = true;
                        game.changemaptest = 0;
                    }
                    if (game.now_map == "Test3.txt")
                    {
                        wmp.URL = "sound/well.mp3";
                        wmp.Ctlcontrols.play();
                        timer5.Enabled = true;
                        game.changemaptest = 0;
                    }
                }
                label2.Text = game.INL.ToString() + "\r" + game.plx.ToString() + ":" + game.ply.ToString() + "\r" + game.msx + ":" + game.msy + "\n" + game.face.ToString() + "\n" + game.pb1x.ToString();
            }
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            if (start == 1)
                if (game.now_map == "Test.txt" && enving == 0)
                {

                    if (game.times < 6)
                    {
                        if (game.npcmove2(3, 0, 1) == 1)
                            game.times++;

                    }
                    if (game.times >= 6 && game.times < 12)
                    {

                        if (game.npcmove(1, 0, 1) == 1)
                            game.times++;

                    }
                    if (game.times >= 12 && game.times < 18)
                    {

                        if (game.npcmove2(4, 0, 1) == 1)
                            game.times++;
                    }
                    if (game.times >= 18 && game.times < 24)
                    {

                        if (game.npcmove(2, 0, 1) == 1)
                            game.times++;
                    }
                    if (game.times >= 24)
                    { game.times = 0; }
                }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (game.loding == 1)
                {

                    ProcessMutilKey(keyboardtmp.KeyCode, true);
                }
            }
            catch
            { }
        }
        private void ProcessMutilKey(Keys MutilKey, bool DownOrUp)
        {
            if (talking == 0)
            {
                // 當觸發 A, Ctrl, Alt 鍵時，若為 KeyDown 則把該所屬的旗標設為 true，
                // 反之，若為 KeyUp 時則把該所屬的旗標設為 false
                if (MutilKey == Keys.Up)
                    IsADown = DownOrUp;
                if (MutilKey == Keys.Down)
                    IsADown = DownOrUp;
                if (MutilKey == Keys.Right)
                    IsADown = DownOrUp;
                if (MutilKey == Keys.Left)
                    IsADown = DownOrUp;
                if (IsADown == true)
                {

                    IsADown = false;
                    game.handlekeypress2(keyboardtmp);
                }
            }
        }


        private void pictureBox2_VisibleChanged(object sender, EventArgs e)
        {
            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();
        }

        private void pictureBox3_VisibleChanged(object sender, EventArgs e)
        {
            back.URL = ("sound/menu2.wav");
            back.Ctlcontrols.play();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();
        }

        private void pictureBox4_Move(object sender, EventArgs e)
        {

            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();
        }


        private void pictureBox5_VisibleChanged(object sender, EventArgs e)
        {
            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();

        }



        private void back2_StatusChange(object sender, EventArgs e)
        {
            if ((int)back2.playState == 1)//如果播放状态等于停止
            {
                wmp.Ctlcontrols.play();
                //这里写你的处理代码
            }
            if (back2.playState == WMPLib.WMPPlayState.wmppsPlaying)//如果播放状态等于停止
            {
                wmp.Ctlcontrols.stop();
                //这里写你的处理代码
            }
        }

        private void wmp_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 1)
                this.wmp.Ctlcontrols.play();
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

        private void timer5_Tick(object sender, EventArgs e)
        {
            if (start2 >= 5 && start == 0)
            {

                timer5.Dispose();

                start2 = 0;
                start = 1;
                game = new Class1(this);
                label6.Dispose();

            }

            if (start2 % 2 == 0 && start == 0)
                label6.ForeColor = Color.White;
            else if (start2 % 2 == 1)
                label6.ForeColor = Color.Black;

            start2 += 1;

        }

        private void pictureBox5_VisibleChanged_1(object sender, EventArgs e)
        {
            back.URL = ("sound/menu2.wav");
            back.Ctlcontrols.play();
        }
        Bitmap bmp2;
        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            check_pok(); if (pokarr[2] != "-1")
            {
                bmp2 = new Bitmap("pok/" + pokarr[2] + ".png");
                label7.Text = pokarr[3];
                e.Graphics.DrawImage(TakeScreenshot(0, 0, bmp2, 1, 1, 1, 1, 0.7), 40, 0);

            }


            PictureBox pb = (PictureBox)sender;
            if (menu2y == pb.Location.Y)
            {
                pokarr[0] = "0";
                set_pok();
                e.Graphics.DrawRectangle(new Pen(Color.Red, 15), 0, 0, pb.Width, pb.Height);
                bmp2 = new Bitmap("pok/" + pokarr[2] + ".png");
                pictureBox5.Refresh();
            }

        }

        private void pictureBox7_Paint(object sender, PaintEventArgs e)
        {
            check_pok(); if (pokarr[4] != "-1")
            {
                bmp2 = new Bitmap("pok/" + pokarr[4] + ".png");
                label8.Text = pokarr[5];
                e.Graphics.DrawImage(TakeScreenshot(0, 0, bmp2, 1, 1, 1, 1, 0.7), 40, 0);
            }



            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();
            PictureBox pb = (PictureBox)sender;
            if (menu2y == pb.Location.Y)
            {
                pokarr[0] = "1";
                set_pok();
                e.Graphics.DrawRectangle(new Pen(Color.Red, 15), 0, 0, pb.Width, pb.Height);
                bmp2 = new Bitmap("pok/" + pokarr[4] + ".png");
                pictureBox5.Refresh();
            }
        }

        private void pictureBox8_Paint(object sender, PaintEventArgs e)
        {
            check_pok(); if (pokarr[6] != "-1")
            {
                bmp2 = new Bitmap("pok/" + pokarr[6] + ".png");
                label10.Text = pokarr[7];
                e.Graphics.DrawImage(TakeScreenshot(0, 0, bmp2, 1, 1, 1, 1, 0.7), 40, 0);
            }



            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();
            PictureBox pb = (PictureBox)sender;
            if (menu2y == pb.Location.Y)
            {
                pokarr[0] = "2";
                set_pok();
                e.Graphics.DrawRectangle(new Pen(Color.Red, 15), 0, 0, pb.Width, pb.Height);
                bmp2 = new Bitmap("pok/" + pokarr[6] + ".png");
                pictureBox5.Refresh();
            }
        }

        private void pictureBox9_Paint(object sender, PaintEventArgs e)
        {
            check_pok(); if (pokarr[8] != "-1")
            {
                bmp2 = new Bitmap("pok/" + pokarr[8] + ".png");
                label11.Text = pokarr[9];
                e.Graphics.DrawImage(TakeScreenshot(0, 0, bmp2, 1, 1, 1, 1, 0.7), 40, 0);
            }



            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();
            PictureBox pb = (PictureBox)sender;
            if (menu2y == pb.Location.Y)
            {
                pokarr[0] = "3";
                set_pok();
                e.Graphics.DrawRectangle(new Pen(Color.Red, 15), 0, 0, pb.Width, pb.Height);
                bmp2 = new Bitmap("pok/" + pokarr[8] + ".png");
                pictureBox5.Refresh();
            }
        }

        private void pictureBox10_Paint(object sender, PaintEventArgs e)
        {
            check_pok(); if (pokarr[10] != "-1")
            {
                bmp2 = new Bitmap("pok/" + pokarr[10] + ".png");
                label12.Text = pokarr[11];
                e.Graphics.DrawImage(TakeScreenshot(0, 0, bmp2, 1, 1, 1, 1, 0.7), 40, 0);

            }



            back.URL = ("sound/menu.wav");
            back.Ctlcontrols.play();
            PictureBox pb = (PictureBox)sender;
            if (menu2y == pb.Location.Y)
            {
                pokarr[0] = "4";
                set_pok();
                e.Graphics.DrawRectangle(new Pen(Color.Red, 15), 0, 0, pb.Width, pb.Height);
                bmp2 = new Bitmap("pok/" + pokarr[10] + ".png");
                pictureBox5.Refresh();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void timer4_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawImage(TakeScreenshot(0, 0, bmp2, 1, 1, 1, 1, 2), 20, 0);

        }
    }
}

