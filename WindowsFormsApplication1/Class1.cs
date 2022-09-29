using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using WMPLib;
using System.IO;

namespace WindowsFormsApplication1
{
    public class Class1
    {
        System.Windows.Forms.Timer aTimer;
        public MP3Player backer = new MP3Player();
        public int loding = 0;
        public int touch;
        public int plx = 560;
        public int ply = 240;
        public int pb1x = 0;
        public int pb1y = 0;
        public int orplx = 240;
        public int orply = 240;
        public int msx;
        public int msy;
        public int wallx;
        public int wally;
        public int u, p, r, f;
        public string backroundsound;
        public string size;
        public int ms1_ev = 0;
        public int ms2_ev = 0;
        public int map1_ev = 0;
        public int train;
        public int face;
        public int face_back;
        public int touch2;
        public int touch3;
        public int[] max_map;
        public int[] max_monster;
        public int[] max_wall;
        public string now_map = "Test";
        public int beforex;
        public int beforey;
        public int INL;
        public int times;
        public byte lightblack = 255;
        public int lightblack2 = 255;
        public int map_loading = 0;
        int z;
        int y;
        public int nowid;
        public int changemaptest = 0;
        public int changemaping = 0;
        public int changemaping2 = 0;
        System.Timers.Timer timersTimer = new System.Timers.Timer();
        System.Timers.Timer timersTimer2 = new System.Timers.Timer();

        /// <summary> ///////////////////////////////////////////////////////
        Bitmap bmplayer = new Bitmap("pok/player.png");
        Bitmap bmplayer2 = new Bitmap("pok/2.png");
        Bitmap bmptmp;
        Bitmap bmpman = new Bitmap("pok/man.png");
        Bitmap bmpgirl = new Bitmap("pok/girl.png");
        Bitmap bmp2 = new Bitmap("pok/MAP2.jpg");

        Form gameform;
        worldmap worldmap;
        Playerparty playerparty;
        PictureBox worldmapspritepb;
        worldmapmonster[] map;
        worldmapmonster[] monster;
        worldmapmonster[] wall;
        KeyEventArgs tmpe;
        public int fight = 0;
        public CombatGUI atack;

        Image img;
        Graphics device;

        /// <切割圖片走圖>///////////////////////////////////////////////////////
        ///         public static Bitmap Resize(Bitmap originImage, Double times)
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

        public Class1(Form form)
        {

            //////////////////////////////////////////////////////////
            map = new worldmapmonster[10];
            monster = new worldmapmonster[10];

            Bitmap bmp = new Bitmap("pok/player.png");
            gameform = form;
            gameform.Width = 700;
            gameform.Height = 700;
            gameform.BackColor = Color.White;
            gameform.BackgroundImage = new Bitmap("pok/MAP.jpg");
            worldmap = new worldmap(gameform);


            playerparty = new Playerparty(new Point(plx, ply), TakeScreenshot(0, 0, bmp, 4, 4, 4, 4, 2), 1);
            worldmapspritepb = new PictureBox();
            worldmapspritepb.Width = gameform.Width;
            worldmapspritepb.Height = gameform.Height;
            worldmapspritepb.BackColor = Color.Transparent;
            worldmapspritepb.Parent = gameform;


            loding = 1;



            changemap("Test.txt");

            ///////////////////////////////////////////////////////////

        }
        [DllImport("user32.dll")]//取设备场景   
        private static extern IntPtr GetDC(IntPtr hwnd);//返回设备场景句柄   
        [DllImport("gdi32.dll")]//取指定点颜色   
        private static extern int GetPixel(IntPtr hdc, Point p);
        int tmpxx = 0;
        /*  private void TimerEventProcessor2(object myObject, EventArgs myEventArgs)
        /  {
              Point p = new Point(0,0);//取置顶点坐标   
              IntPtr hdc = GetDC(new IntPtr(0));//取到设备场景(0就是全屏的设备场景)   
              int c = GetPixel(hdc, p);//取指定点颜色   
              int r = (c & 0xFF);//转换R   
              int g = (c & 0xFF00) / 256;//转换G   
              int b = (c & 0xFF0000) / 65536;//转换B   
              if (r == 255 && g == 255 && b == 255)
              {
                  gameform.Refresh();

                  changemaping2 = 1;
                  changemaping = 0;

                  draw();
              }
              timersTimer2.Start();
          }*/
        /* private void TimerEventProcessor(object myObject, EventArgs myEventArgs)
         {


             if (lightblack2 <= 0)
             {

                 a = new Bitmap("cl2.jpg");
                 playerparty.partsprite.draw2(device, plx, ply);
                 lightblack2 = lightblack = 255;
                 worldmapspritepb.Image = TakeScreenshot(0, 0, a, 1, 1, 1, 1,360);
                 playerparty.partsprite.draw2(device, plx, ply);

                 gameform.Refresh();


                 timersTimer2.Start();
             }
             else if(lightblack2>0)

             timersTimer.Start();

             lightblack2 -= 5;
             AlphaBlend(a, lightblack -=1, Color.Black);
             worldmapspritepb.Image = TakeScreenshot(0, 0, a, 1, 1, 1, 1, 360);
         }*/

        public void talkface(int kind, int x)
        {

            if (nowid == 2)
                bmptmp = bmpman;
            if (nowid == 3)
                bmptmp = bmpgirl;
            monster[x].look(kind, monster[x].id);
            monster[x].draw(device);
            draw();

        }
        public int msty(int x)
        {
            return monster[x].location.Y;
        }
        public int npcmove(int kind, int x, int times)
        {

            if (nowid == 2)
                bmptmp = bmpman;
            if (nowid == 3)
                bmptmp = bmpgirl;

            max_monster[x] = updata(playerparty.partsprite.UPdata(), monster[x].UPdata(), monster[x].location.X, monster[x].location.Y, plx, ply);

            if (kind == 2 && monster[x].location.Y - 40 != ply || monster[x].location.X != plx)
            {

                monster[x].move(kind, monster[x].id, times, max_monster[x]);

                msy = monster[x].location.Y;
                monster[x].draw(device);
                draw();
                return 1;
            }
            else if (kind == 1 && monster[x].location.Y + 40 != ply || monster[x].location.X != plx)
            {
                monster[x].move(kind, monster[x].id, times, max_monster[x]);

                msy = monster[x].location.Y;
                monster[x].draw(device);
                draw();
                return 1;

            }

            else

                return 0;

        }
        public int npcmove2(int kind, int x, int times)
        {

            if (nowid == 2)
                bmptmp = bmpman;
            if (nowid == 3)
                bmptmp = bmpgirl;

            max_monster[x] = updata(playerparty.partsprite.UPdata(), monster[x].UPdata(), monster[x].location.X, monster[x].location.Y, plx, ply);
            if (kind == 3 && monster[x].location.X - 40 != plx || monster[x].location.Y != ply)
            {

                monster[x].move(kind, monster[x].id, times, max_monster[x]);

                msx = monster[x].location.X;

                monster[x].draw(device);
                worldmapspritepb.Image = img;
                draw();
                return 1;
            }
            else if (kind == 4 && monster[x].location.X + 40 != plx || monster[x].location.Y != ply)
            {

                monster[x].move(kind, monster[x].id, times, max_monster[x]);
                msx = monster[x].location.X;
                monster[x].draw(device);
                worldmapspritepb.Image = img;
                draw();
                return 1;
            }
            else
                return 0;

        }
        public void mapadd()
        {

            z = 0;
            y = 0;

            for (int i = 0; i < 50; i++)
                for (int j = 0; j < 41; j++)
                {
                    if (worldmap.maparr[i, j] == 2)
                    {
                        monster[z] = new worldmapmonster(new Point(i * 40, j * 40), TakeScreenshot(20, 64, bmpman, 3, 3, 4, 4, 2), 2);
                        z++;
                    }
                    if (worldmap.maparr[i, j] == 5)
                    {
                        monster[z] = new worldmapmonster(new Point(i * 40, j * 40), TakeScreenshot(20, 64, bmpgirl, 3, 3, 4, 4, 2), 3);
                        z++;
                    }
                }

            max_monster = new int[z];
            try
            {
                timersTimer.Start();
            }
            catch { timersTimer.Stop(); }
        }

        ///數據更新///////////////////////////////////////////////////////////
        public int updata(Rectangle player, Rectangle mon, int targetx, int targety, int mex, int mey)
        {

            bool overlapped = player.IntersectsWith(mon);
            if (overlapped == true)
            {
                plx = mex;
                ply = mey;
                msx = targetx;
                msy = targety;
                if (face == 4 & (msx == plx - 40) & (msx < plx) & (msy == ply))
                    return 1;
                else if (face == 1 & (msx == plx + 40) & (msx > plx) & (msy == ply))
                    return 1;
                else if (face == 2 & (msy == ply - 40) & (msy < ply) & (msx == plx))
                    return 1;
                else if (face == 3 & (msy == ply + 40) & (msy > ply) & (msx == plx))
                    return 1;
                else if ((msy != ply) & (msx != plx))
                    return 0;
                else
                    return 0;
            }
            else
            {
                plx = mex;
                ply = mey;
                return 0;
            }


        }

        public bool walltest()
        {

            if (((plx / 40) <= 50) && (worldmap.maparr[(plx / 40) + 1, ply / 40] == 5 || worldmap.maparr[(plx / 40) + 1, ply / 40] == 6 || /*worldmap.maparr[(plx / 40) + 1, ply / 40] == 2||*/ worldmap.maparr[(plx / 40) + 1, ply / 40] == 3 || worldmap.maparr[(plx / 40) + 1, ply / 40] == 1) && face == 1)
                return false;
            if (((plx / 40) - 1 >= 0) && (worldmap.maparr[(plx / 40) - 1, ply / 40] == 5 || worldmap.maparr[(plx / 40) - 1, ply / 40] == 6 ||/* worldmap.maparr[(plx / 40) - 1, ply / 40] == 2|| */worldmap.maparr[(plx / 40) - 1, ply / 40] == 3 || worldmap.maparr[(plx / 40) - 1, ply / 40] == 1) && face == 4)
                return false;
            if (((ply / 40) - 1 >= 0) && (worldmap.maparr[(plx / 40), ply / 40 - 1] == 5 || worldmap.maparr[(plx / 40), ply / 40 - 1] == 6 || /*worldmap.maparr[(plx / 40), (ply / 40) - 1] == 2||*/ worldmap.maparr[(plx / 40), (ply / 40) - 1] == 3 || worldmap.maparr[(plx / 40), (ply / 40) - 1] == 1) && face == 2)
                return false;
            if ((ply / 40) <= 40 && (worldmap.maparr[(plx / 40), ply / 40 + 1] == 5 || worldmap.maparr[(plx / 40), ply / 40 + 1] == 6 || /*worldmap.maparr[(plx / 40), (ply / 40) + 1] == 2|| */worldmap.maparr[(plx / 40), (ply / 40) + 1] == 3 || worldmap.maparr[(plx / 40), (ply / 40) + 1] == 1) && face == 3)

                return false;

            else
                return true;
        }
        public int monst()
        {
            Random crandom = new Random(Guid.NewGuid().GetHashCode());
            int crit = crandom.Next(1, 100);

            if (worldmap.maparr[(plx / 40), (ply / 40) + 1] == 7)
            {

                if (crit > 95)

                    return 1;


                else
                    return 0;
            }
            else
                return 0;
        }
        public bool walltest2(int tmp)
        {
            if (((plx / 40) + 1 <= 50) && (worldmap.maparr[(plx / 40) + 1, ply / 40] == tmp && face == 1))
                return false;
            if (((plx / 40) - 1 >= 0) && (worldmap.maparr[(plx / 40) - 1, ply / 40] == tmp && face == 4))
                return false;
            if (((ply / 40) - 1 >= 0) && (worldmap.maparr[(plx / 40), (ply / 40) - 1] == tmp && face == 2))
                return false;
            if ((ply / 40) + 1 <= 40 && (worldmap.maparr[(plx / 40), (ply / 40) + 1] == tmp && face == 3))
                return false;
            else
                return true;

        }

        /// <shark視窗震動>
        public void shock()
        {
            Point now_p = gameform.Location;
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Point new_p = new Point(now_p.X + r.Next(-10, 10), now_p.Y + r.Next(-10, 10)); //新的位置
                gameform.Location = new_p;
                Thread t1 = new Thread(MyBackgroundTask2);
                t1.Start();
                t1.Join();
                gameform.Location = now_p; //還原位置

            }
        }
        /// <summary>
        /// monst事件
        /// </summary>
        /// 
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
            bmplayer = new Bitmap("pok/" + Convert.ToString(Convert.ToInt32(pokarr[((Convert.ToInt32(pokarr[0]) + 1) * 2)]) + 10) + ".png");
            sr.Close();

        }
        public void ms1()
        {
            gameform.Hide();
            atack = new CombatGUI();
            check_pok();
            atack.f1 = TakeScreenshot(0, 0, bmplayer, 1, 1, 1, 1, 2);
            bmplayer2 = new Bitmap("pok/20.png");
            atack.now_monname = "20";
            atack.f2 = TakeScreenshot(0, 0, bmplayer2, 1, 1, 1, 1, 2);
            atack.train = 1;
            atack.Show();
            fight = 1;
            ms1_ev = 1;
            atack.train = 1;
            touch2 = 0;
        }
        public void ms2()
        {
            gameform.Hide();
            atack = new CombatGUI();
            check_pok();
            atack.f1 = TakeScreenshot(0, 0, bmplayer, 1, 1, 1, 1, 2);
            Random crandom = new Random(Guid.NewGuid().GetHashCode());
            int crit = crandom.Next(0, 10);

            bmplayer2 = new Bitmap("pok/" + Convert.ToString(crit) + ".png");
            atack.now_monname = Convert.ToString(crit);
            atack.bmplayer3 = bmplayer2;
            atack.f2 = TakeScreenshot(0, 0, atack.bmplayer3, 1, 1, 1, 1, 2);

            atack.Show();
            fight = 1;

            atack.train = 0;
            touch2 = 0;
        }
        public void ms3()
        {
            gameform.Hide();
            atack = new CombatGUI();
            check_pok();
            atack.f1 = TakeScreenshot(0, 0, bmplayer, 1, 1, 1, 1, 2);
            bmplayer2 = new Bitmap("pok/011.png");
            atack.now_monname = "011";
            atack.f2 = TakeScreenshot(0, 0, bmplayer2, 1, 1, 1, 1, 2);
            atack.train = 1;
            atack.Show();
            fight = 1;
            ms2_ev = 1;
            atack.train = 1;
            touch2 = 0;
        }
        public void map1()
        {
            if (now_map == "Test.txt")
            {
                beforex = plx;
                beforey = ply;
                plx = 80;
                ply = 120;

                changemap("Test2.txt");
            }
            else if (now_map == "Test2.txt")
            {
                plx = beforex;
                ply = beforey;
                changemap("Test.txt");
            }
        }
        public void map2()
        {
            if (now_map == "Test.txt")
            {
                beforex = plx;
                beforey = ply;
                plx = 960;
                ply = 400;

                changemap("Test3.txt");
            }
            else if (now_map == "Test3.txt")
            {
                plx = beforex;
                ply = beforey;
                changemap("Test.txt");
            }
        }
        public void playernewpoint(int x, int y)
        {
            playerparty.partsprite.location = new Point(x, y);

        }
        public void handlekeypress(KeyEventArgs e)
        {

            tmpe = e;


            //   Thread t1 = new Thread(MyBackgroundTask);
            //   t1.Start();

            for (int i = 0; i < z; i++)
            {
                if (plx + 40 == monster[i].location.X || plx - 40 == monster[i].location.X || ply + 40 == monster[i].location.Y || ply - 40 == monster[i].location.Y)
                    max_monster[i] = updata(playerparty.partsprite.UPdata(), monster[i].UPdata(), monster[i].location.X, monster[i].location.Y, plx, ply);
            }
            if (e.KeyCode == Keys.Right)
            {

                face = 1;
                if ((plx + 40 != msx || ply != msy) && walltest())
                {


                    plx += 40; playerparty.partsprite.move(plx, ply, e, 1, 1);


                    //       worldmapspritepb.Location = new Point(worldmapspritepb.Location.X - 50, worldmapspritepb.Location.Y);
                    if (monst() == 1)
                    { ms2(); }
                }

                else
                {
                    if (walltest() == false)
                        backer.Play("sound/wall.wav"); handlekeypress2(e);
                }
            }
            if (e.KeyCode == Keys.Left)
            {

                face = 4;
                if ((plx - 40 != msx || ply != msy) && walltest())
                {


                    plx -= 40; playerparty.partsprite.move(plx, ply, e, 1, 4);
                    //     worldmapspritepb.Location = new Point(worldmapspritepb.Location.X + 50, worldmapspritepb.Location.Y);
                    if (monst() == 1)
                    { ms2(); }
                }
                else
                {
                    if (walltest() == false)
                        backer.Play("sound/wall.wav"); handlekeypress2(e);
                }
            }
            if (e.KeyCode == Keys.Up)
            {

                face = 2;
                if ((plx != msx || ply - 40 != msy) && walltest())
                {

                    ply -= 40;
                    playerparty.partsprite.move(plx, ply, e, 1, 2);
                    //    worldmapspritepb.Location = new Point(worldmapspritepb.Location.X , worldmapspritepb.Location.Y+50);
                    if (monst() == 1)
                    { ms2(); }
                }
                else
                {
                    if (walltest() == false)
                        backer.Play("sound/wall.wav"); handlekeypress2(e);
                }
            }
            if (e.KeyCode == Keys.Down)
            {

                face = 3;
                if ((plx != msx || ply + 40 != msy) && walltest())
                {

                    ply += 40; playerparty.partsprite.move(plx, ply, e, 1, 3);
                    //     worldmapspritepb.Location = new Point(worldmapspritepb.Location.X, worldmapspritepb.Location.Y -50);
                    if (monst() == 1)
                    { ms2(); }
                }
                else
                {
                    if (walltest() == false)
                        backer.Play("sound/wall.wav"); handlekeypress2(e);
                }

            }

            if (walltest2(4) == false)
                map1();
            if (walltest2(8) == false)
                map2();



            draw();

            //   t1.Join();
        }
        public void handlekeypress2(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Right)
            { playerparty.partsprite.move(plx, ply, e, 1, 5); }
            else if (e.KeyCode == Keys.Up)
            { playerparty.partsprite.move(plx, ply, e, 1, 6); }
            else if (e.KeyCode == Keys.Left)
            { playerparty.partsprite.move(plx, ply, e, 1, 7); }
            else if (e.KeyCode == Keys.Down)
            { playerparty.partsprite.move(plx, ply, e, 1, 8); }
            draw();
        }

        /// < MyBackgroundTask延遲函數>
        void MyBackgroundTask()
        {


            lightblack2 = lightblack = 255;


            changemaping = 0;
            changemaping2 = 1;
            if (changemaping2 == 1)
            {
                timersTimer.Stop();
                draw();
                changemaping2 = 1;

            }
        }
        void MyBackgroundTask2()
        {
            Thread.Sleep(10);
        }
        /// <View設定隱藏顯示>
        public void View(bool sw)
        {
            if (sw == true)
                gameform.Hide();
            else
                gameform.Show();
        }
        public void changemap(string mapname)
        {
            backer.Play("sound/changemap.wav");

            now_map = mapname;

            worldmap.inmap(mapname);
            mapadd();
            changemaptest = 1;
            times = 0;
            changemaping = 0;

        }

        private Image CutImage(Image SourceImage, Point StartPoint, Rectangle CutArea)
        {
            try
            {
                Bitmap NewBitmap = new Bitmap(CutArea.Width, CutArea.Height);
                Graphics tmpGraph = Graphics.FromImage(NewBitmap);
                tmpGraph.DrawImage(SourceImage, CutArea, StartPoint.X, StartPoint.Y, CutArea.Width, CutArea.Height, GraphicsUnit.Pixel);
                tmpGraph.Dispose();
                return NewBitmap;
            }
            catch { return null; }
        }

        public void draw()
        {

            img = new Bitmap(400 + plx, 400 + ply);


            device = Graphics.FromImage(img);

            for (int i = 0; i < z; i++)
            {
                monster[i].draw(device);
            }

            for (int i = 0; i < y; i++)
            {
                wall[i].draw(device);
            }

            worldmap.drawmap(device, plx / 40, ply / 40);


            playerparty.partsprite.draw2(device, plx, ply);
            worldmap.drawmap2(device);


            img = CutImage(img, new Point(plx - 320, ply - 280), new Rectangle(0, 0, 800, 800));
            worldmapspritepb.Image = img;

            gameform.Refresh();


            GC.Collect();//清除new物件 重要會吃記憶體

        }
    }
    class worldmap
    {
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

        public worldmap(Form form)
        {
        }
        public float[,] maparr;
        public int tmpx;
        public int tmpy;
        public MP3Player backer = new MP3Player();
        public int loading = 0;
        public int loading2 = 0;
        public void inmap(string mapname)
        {
            int txtLength = 0, i = 0;
            string s1;
            FileStream fs = new FileStream(mapname, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.Default);

            /* 取得行數 */
            s1 = sr.ReadLine();
            while (s1 != null)
            {
                txtLength++;
                s1 = sr.ReadLine();
            }
            fs.Position = 0;

            /* 取得資料並放入陣列 */
            string[] s2 = new string[2];
            float[,] txtValue = new float[50, 41];
            maparr = new float[50, 41];

            s1 = sr.ReadLine();
            while (s1 != null)
            {
                s2 = s1.Split(' ');
                for (int j = 0; j < 50; j++)
                    txtValue[j, i] = float.Parse(s2[j]);
                i++;
                s1 = sr.ReadLine();
            }

            for (int x = 0; x < 50; x++)
                for (int y = 0; y < 41; y++)
                {
                    maparr[x, y] = txtValue[x, y];
                }
            sr.Close();
            fs.Close();

        }

        public void drawmap(Graphics device, int plx, int ply)
        {

            Bitmap bmp2 = new Bitmap("pok/MAP.jpg");
            Bitmap bmp3 = new Bitmap("pok/MAP2.jpg");
            Bitmap bmp4 = new Bitmap("pok/MAP5.png");
            Bitmap bmp5 = new Bitmap("pok/MAP6.png");
            Bitmap bmp6 = new Bitmap("pok/cl.jpg");
            Bitmap bmp7 = new Bitmap("pok/pokemcen.png");
            try
            {
                loading = 0;

                // txtValue陣列是從記事本取得的二維陣列，也就是您要的結果
                /////////////////////
                //繪畫網格方便設定物件
                for (int x = plx - 9; x < plx + 9; x++)
                {
                    for (int y = ply - 9; y < ply + 9; y++)
                    {
                        if (x < 0 || y < 0 || x >= 50 || y >= 40)
                            continue;
                        if (maparr[x, y] == 7)
                        {
                            if (x == plx && y == ply + 1 && maparr[plx, ply + 1] == 7)
                            {
                                device.DrawImage(TakeScreenshot(0, 0, bmp5, 1, 1, 1, 1, 1), x * 40, y * 40);

                            }
                            else
                                device.DrawImage(TakeScreenshot(0, 0, bmp4, 1, 1, 1, 1, 1), x * 40, y * 40);
                        }

                        if (maparr[x, y] == 1)
                        {

                            device.DrawImage(TakeScreenshot(0, 0, bmp6, 1, 1, 1, 1, 2), x * 40, y * 40);

                        }
                        if (maparr[x, y] == 6)
                        {
                            device.DrawImage(TakeScreenshot(0, 0, bmp7, 1, 1, 1, 1, 1.2), x * 40, y * 40);

                        }

                        if (maparr[x, y] == 3)
                        {
                            device.DrawImage(TakeScreenshot(0, 0, bmp3, 1, 1, 1, 1, 2), x * 40, y * 40);
                        }
                        if (y > 1)
                            if (maparr[x, y + 1] == 3 && maparr[x, y - 1] == 3)
                            {
                                tmpx = plx; tmpy = ply + 1;
                                continue;
                            }

                    }

                    //  Pen pen = new Pen(Color.Green);


                }
                loading = 1;
            }
            catch
            { }

        }
        public void drawmap2(Graphics device)
        {
            loading = 0;
          
                Bitmap bmp3 = new Bitmap("pok/MAP2.jpg");
                for (int i = tmpy; i < tmpy+10; i++)
                {
                if(i<40)
                    if (maparr[tmpx, i] == 3)
                    {
                        device.DrawImage(TakeScreenshot(0, 0, bmp3, 1, 1, 1, 1, 2), tmpx * 40, i * 40);

                    }
                }
            
   
            loading = 1;

        }
        public void delatemap()
        {
            maparr = null;
        }
    }

}

class worldmapsprite
{
    Bitmap bmpman = new Bitmap("pok/man.png");
    Bitmap bmpgirl = new Bitmap("pok/girl.png");
    Bitmap bmpt;
    Bitmap bmptmp;
    Random crandom = new Random(Guid.NewGuid().GetHashCode());
    public PictureBox pb1 = new PictureBox();
    Bitmap bmp = new Bitmap("pok/player.png");
    public Point location;
    public Image image;
    int plx = 0;
    int ply = 0;
    int rl = 0;
    int rl2 = 0;
    int race;
    int height;
    int width;
    public int id;
    int back;
    System.Timers.Timer tmr = new System.Timers.Timer(200);
    int cutx = 0, cuty = 0, orx = 0, ory = 0, chx = 0, chy = 0;

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


    public Rectangle UPdata()
    {//33 48

        Rectangle p1 = pb1.ClientRectangle;
        pb1.Location = new Point(plx, ply);
        p1.Offset(pb1.Location);
        p1.Location = new Point(location.X, location.Y);

        return p1;
    }
    public void UPdata2(int plxx, int plyy)
    {//33 48
        plx = plxx;
        ply = plyy;
    }
    void MyBackgroundTask()
    {

        Thread.Sleep(20);
    }

    public worldmapsprite(Point location, Image image, int id)
    {
        pb1.Size = new Size(80, 80);
        this.location = location;
        this.image = image;
        this.id = id;
        width = image.Width;
        height = image.Height;
        tmr.Start();
    }
    void UP()
    {//48 64 16
     //player 

        if (race == 1) { cutx = 48; cuty = 0; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; }
        if (rl == 1)
        {
            image = TakeScreenshot(cutx * 0, cuty, bmpt, orx, ory, chx, chy, 2);
        }
        else if (rl == 0) { image = TakeScreenshot(cutx * 1, cuty, bmpt, orx, ory, chx, chy, 2); }


    }
    void DOWN()
    {

        if (race == 1) { cutx = 48; cuty = 64; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; }
        if (rl == 1)
        {
            image = TakeScreenshot(cutx * 0, cuty, bmpt, orx, ory, chx, chy, 2);
        }
        else if (rl == 0) { image = TakeScreenshot(cutx * 1, cuty, bmpt, orx, ory, chx, chy, 2); }
    }
    void RIGHT()
    {
        if (race == 1) { cutx = 48; cuty = 16; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; }
        if (rl == 1)
        {
            image = TakeScreenshot(cutx * 0, cuty * 2, bmpt, orx, ory, chx, chy, 2);
        }
        else if (rl == 0) { image = TakeScreenshot(cutx * 1, cuty * 2, bmpt, orx, ory, chx, chy, 2); }

    }
    void LEFT()
    {
        if (race == 1) { cutx = 48; cuty = 16; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; }
        if (rl == 1)
        {
            image = TakeScreenshot(cutx * 0, cuty * 6, bmpt, orx, ory, chx, chy, 2);
        }
        else if (rl == 0) { image = TakeScreenshot(cutx * 1, cuty * 6, bmpt, orx, ory, chx, chy, 2); }

    }
    public void look(int face, int id)
    {
        if (id == 2)
            bmptmp = bmpman;

        else if (id == 3)
            bmptmp = bmpgirl;

        if (face == 2)
            image = TakeScreenshot(20, 1, bmptmp, 3, 3, 4, 4, 2);
        else if (face == 3)
            image = TakeScreenshot(21, 32, bmptmp, 3, 3, 4, 4, 2);
        else if (face == 1)
            image = TakeScreenshot(20, 64, bmptmp, 3, 3, 4, 4, 2);
        else if (face == 4)
            image = TakeScreenshot(21, 96, bmptmp, 3, 3, 4, 4, 2);
    }
    public void move(int face, int id, int times, int touch)
    {
        if (rl2 == 0) { rl2 = 1; }
        else if (rl2 == 1)
        { rl2 = 0; }
        if (id == 2)
            bmptmp = bmpman;

        else if (id == 3)
            bmptmp = bmpgirl;


        if (face == 1)
        {
            location.Y += 40;
            if (rl2 == 0)
                image = TakeScreenshot(1, 64, bmptmp, 3, 3, 4, 4, 2);
            else if (rl2 == 1)
            {
                image = TakeScreenshot(45, 64, bmptmp, 3, 3, 4, 4, 2);

            }


        }

        else if (face == 2)
        {

            location.Y -= 40;
            if (rl2 == 0)
                image = TakeScreenshot(1, 1, bmptmp, 3, 3, 4, 4, 2);
            else if (rl2 == 1)
            {
                image = TakeScreenshot(45, 1, bmptmp, 3, 3, 4, 4, 2);

            }



        }
        else if (face == 3)
        {
            location.X -= 40;
            if (rl2 == 0)

                image = TakeScreenshot(1, 96, bmptmp, 3, 3, 4, 4, 2);
            else if (rl2 == 1)
            {
                image = TakeScreenshot(45, 96, bmptmp, 3, 3, 4, 4, 2);
            }



        }
        else if (face == 4)

        {
            location.X += 40;
            if (rl2 == 0)
                image = TakeScreenshot(1, 32, bmptmp, 3, 3, 4, 4, 2);


            else if (rl2 == 1)
            {
                image = TakeScreenshot(45, 32, bmptmp, 3, 3, 4, 4, 2);
            }



        }



    }

    public void move(int x, int y, KeyEventArgs e, int inrace, int face)
    {
        GC.Collect();//清除new物件 重要會吃記憶體

        race = inrace;

        if (e.KeyCode == Keys.Up)
        {
            UP();
            if (rl == 0) { rl = 1; }
            else if (rl == 1)
            { rl = 0; }


        }
        else if (e.KeyCode == Keys.Down)
        {
            DOWN();
            if (rl == 1) { rl = 0; }
            else if (rl == 0)
            { rl = 1; }


        }
        else if (e.KeyCode == Keys.Left)
        {
            LEFT();
            if (rl == 0) { rl = 1; }
            else if (rl == 1)
            { rl = 0; }
        }
        else if (e.KeyCode == Keys.Right)
        {
            RIGHT();
            if (rl == 0) { rl = 1; }
            else if (rl == 1)
            { rl = 0; }

        }

        back = face;
        if (back == 5)
        { cutx = 48; cuty = 16; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; image = TakeScreenshot(20, cuty * 2, bmpt, orx, ory, chx, chy, 2); }
        else if (back == 6)
        { cutx = 48; cuty = 0; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; image = TakeScreenshot(20, cuty, bmpt, orx, ory, chx, chy, 2); }
        else if (back == 7)
        { cutx = 48; cuty = 16; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; image = TakeScreenshot(20, cuty * 6, bmpt, orx, ory, chx, chy, 2); }
        else if (back == 8)
        { cutx = 48; cuty = 16; orx = 3; ory = 3; chy = 4; chx = 4; bmpt = bmp; bmpt = bmp; image = TakeScreenshot(20, cuty * 4, bmpt, orx, ory, chx, chy, 2); }
        pb1.Location = new Point(x, y);
        location.X = x;
        location.Y = y;
    }

    public void draw(Graphics device)
    {
        try
        {

            device.DrawImage(image, location);
        }
        catch { }
    }
    public void draw2(Graphics device, int plx, int ply)
    {


            device.DrawImage(image, plx, ply);
   
    }


    public PictureBoxSizeMode StretchImage { get; set; }
}

class worldmapmonster : worldmapsprite
{
    public bool isstatic;
    public worldmapmonster(Point location, Image image, int id)
        : base(location, image, id)
    {
        isstatic = true;
    }
}

class Playerparty
{
    public worldmapsprite partsprite;
    public Playerparty(Point location, Image image, int id)
    {
        partsprite = new worldmapsprite(location, image, id);
    }

}


