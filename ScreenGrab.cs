using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Tesseract;
using Microsoft;

namespace WordBlitzAutoPlay
{
    class ScreenGrab
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void Capture (){

            Thread.Sleep(5000);
            Rectangle boundsTemp = Screen.GetBounds(Point.Empty);
            Rectangle bounds = new Rectangle(Cursor.Position.X, Cursor.Position.Y, (int)(boundsTemp.Width * 0.26), (int)(boundsTemp.Height * 0.87f));
            Point topLeft = new Point(Cursor.Position.X, Cursor.Position.Y);
            Console.WriteLine(bounds);
            Bitmap bitmap = new Bitmap(bounds.Size.Width, bounds.Height);

            Graphics g = Graphics.FromImage(bitmap);
                
            g.CopyFromScreen(topLeft, Point.Empty, bounds.Size);
            bitmap.Save("screencapture.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            TesseractEngine tEngine = new TesseractEngine("eng.Traineddata", "eng", EngineMode.Default);
            tEngine.Process(bitmap);
           
        }

        public void BruteCapture()
        {
            Random rn = new Random();
            Thread.Sleep(5000);
            Rectangle bounds = new Rectangle(Cursor.Position.X, Cursor.Position.Y, 422, 422);
            Point topLeft = new Point(Cursor.Position.X, Cursor.Position.Y);
            Console.WriteLine(bounds);
            Bitmap bitmap = new Bitmap(bounds.Size.Width, bounds.Height);

            Graphics g = Graphics.FromImage(bitmap);

            g.CopyFromScreen(topLeft, Point.Empty, bounds.Size);
            bitmap.Save("screencapture.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            MouseManager mm = new MouseManager();

            Point newPoint = new Point(0, 0);

            Point[,] allPoints = new Point[4,4];

            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    allPoints[i, j] = new Point(topLeft.X + (97 / 2) + (110*i), topLeft.Y + (97 / 2) + (110 * j));
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int l = 0; l < 25; l++) {
                        int pastNumberi = i;
                        int pastNumberj = j;
                        mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                        mm.LeftDown();

                        for (int x = 0; x < 4; x++)
                        {
                            Point[] pastPoints = new Point[5];
                            pastPoints[0] = allPoints[i, j];

                            pastNumberi += (rn.Next(1, 1000) % 3) - 1;
                            pastNumberj += (rn.Next(1, 1000) % 3) - 1;
                            int checker = 1;
                            while (checker > 0)
                            {
                                pastNumberi += (rn.Next(1, 1000) % 3) - 1;
                                pastNumberj += (rn.Next(1, 1000) % 3) - 1;
                                foreach (Point p in pastPoints)
                                {
                                    if (p == allPoints[IsWithin(pastNumberi), IsWithin(pastNumberj)])
                                    {
                                        checker++;
                                        break;
                                    }
                                    else
                                    {
                                        checker = 0;
                                    }
                                }
                            }
                            pastPoints[x + 1] = allPoints[IsWithin(pastNumberi), IsWithin(pastNumberj)];
                            Thread.Sleep(10);
                            mm.MoveCursor(pastPoints[x + 1].X, pastPoints[x + 1].Y);
                        }
                        Thread.Sleep(10);
                        mm.LeftUp();
                    }

                    for (int l = 0; l < 25; l++)
                    {
                        int pastNumberi = i;
                        int pastNumberj = j;

                        mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                        mm.LeftDown();
                        for (int x = 0; x < 7; x++)
                        {
                            Point[] pastPoints = new Point[8];
                            pastPoints[0] = allPoints[i, j];

                            pastNumberi += (rn.Next(1, 1000) % 3) - 1;
                            pastNumberj += (rn.Next(1, 1000) % 3) - 1;
                            int checker = 1;
                            while (checker > 0)
                            {
                                pastNumberi += (rn.Next(1, 1000) % 3) - 1;
                                pastNumberj += (rn.Next(1, 1000) % 3) - 1;
                                foreach (Point p in pastPoints)
                                {
                                    if (p == allPoints[IsWithin(pastNumberi), IsWithin(pastNumberj)])
                                    {
                                        checker++;
                                        break;
                                    }
                                    else
                                    {
                                        checker = 0;
                                    }
                                }
                            }
                            pastPoints[x + 1] = allPoints[IsWithin(pastNumberi), IsWithin(pastNumberj)];
                            Thread.Sleep(10);
                            mm.MoveCursor(pastPoints[x + 1].X, pastPoints[x + 1].Y);
                        }
                        Thread.Sleep(10);
                        mm.LeftUp();
                    }

                    for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++) {
                                //0 -1
                                mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                                Thread.Sleep(10);
                                mm.LeftDown();
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[IsWithin(i + x), IsWithin(j + y)].X, allPoints[IsWithin(i + x), IsWithin(j + y)].Y);
                                Thread.Sleep(10);
                                mm.LeftUp();
                            }
                        }
                        
                    if (i == 0)
                    {
                        for (int x = 2; x < 4; x++)
                        {

                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            int y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i + y, j].X, allPoints[i + y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, IsWithin(j + y)].X, allPoints[i, IsWithin(j + y)].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i+y, IsWithin(j + y)].X, allPoints[i+y, IsWithin(j + y)].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross up
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i + y, j].X, allPoints[i + y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i + y), IsWithin(j + 1)].X, allPoints[IsWithin(i + y), IsWithin(j + 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross down
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i + y, j].X, allPoints[i + y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i + y), IsWithin(j - 1)].X, allPoints[IsWithin(i + y), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4waytry 1
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i + y, j].X, allPoints[i + y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i + y), IsWithin(j - 1)].X, allPoints[IsWithin(i + y), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i + y + 1), IsWithin(j - 1)].X, allPoints[IsWithin(i + y + 1), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4waytry 2
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i + y, j].X, allPoints[i + y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i + y), IsWithin(j - 1)].X, allPoints[IsWithin(i + y), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i + y - 1), IsWithin(j - 1)].X, allPoints[IsWithin(i + y - 1), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                        }
                    }

                    if (i == 3)
                    {
                        for (int x = 2; x < 4; x++)
                        {
                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            int y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i - y, j].X, allPoints[i - y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, IsWithin(j - y)].X, allPoints[i, IsWithin(j - y)].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //cross
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i -y, IsWithin(j - y)].X, allPoints[i-y, IsWithin(j - y)].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross up
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i - y, j].X, allPoints[i - y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - y), IsWithin(j + 1)].X, allPoints[IsWithin(i - y), IsWithin(j + 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross down
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i - y, j].X, allPoints[i - y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - y), IsWithin(j - 1)].X, allPoints[IsWithin(i - y), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4way 1
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i - y, j].X, allPoints[i - y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - y), IsWithin(j - 1)].X, allPoints[IsWithin(i - y), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - y + 1), IsWithin(j - 1)].X, allPoints[IsWithin(i - y + 1), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4way 1
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i - y, j].X, allPoints[i - y, j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - y), IsWithin(j - 1)].X, allPoints[IsWithin(i - y), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - y - 1), IsWithin(j - 1)].X, allPoints[IsWithin(i - y - 1), IsWithin(j - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                        }
                    }

                    if (j == 0)
                    {
                        for (int x = 2; x < 4; x++)
                        {
                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            int y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j + y].X, allPoints[i, j + y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[IsWithin(i + y), j].X, allPoints[IsWithin(i + y), j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //cross
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[IsWithin(i +y), j+y].X, allPoints[IsWithin(i+y), j + y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross up
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j + y].X, allPoints[i, j + y].Y);
                                y++;
                            }
                            Thread.Sleep(10);

                            mm.MoveCursor(allPoints[IsWithin(i + 1), IsWithin(j + y)].X, allPoints[IsWithin(i + 1), IsWithin(j + y)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross down
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j + y].X, allPoints[i, j + y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j + y)].X, allPoints[IsWithin(i - 1), IsWithin(j + y)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4way1
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j + y].X, allPoints[i, j + y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j + y)].X, allPoints[IsWithin(i - 1), IsWithin(j + y)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j + y + 1)].X, allPoints[IsWithin(i - 1), IsWithin(j + y + 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4way2
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j + y].X, allPoints[i, j + y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j + y)].X, allPoints[IsWithin(i - 1), IsWithin(j + y)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j + y - 1)].X, allPoints[IsWithin(i - 1), IsWithin(j + y - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();
                        }
                    }
                    if (j == 3)
                    {
                        for (int x = 2; x < 4; x++)
                        {

                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            int y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j - y].X, allPoints[i, j - y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //cross
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[IsWithin(i - y), j -y].X, allPoints[IsWithin(i - y), j -y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Straight
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[IsWithin(i - y), j].X, allPoints[IsWithin(i - y), j].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross up
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j - y].X, allPoints[i, j - y].Y);
                                y++;
                            }
                            Thread.Sleep(10);

                            mm.MoveCursor(allPoints[IsWithin(i + 1), IsWithin(j - y)].X, allPoints[IsWithin(i + 1), IsWithin(j - y)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //Cross down
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j - y].X, allPoints[i, j - y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j - y)].X, allPoints[IsWithin(i - 1), IsWithin(j - y)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4way1
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j - y].X, allPoints[i, j - y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j - y)].X, allPoints[IsWithin(i - 1), IsWithin(j - y)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j - y + 1)].X, allPoints[IsWithin(i - 1), IsWithin(j - y + 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                            //4way2
                            mm.MoveCursor(allPoints[i, j].X, allPoints[i, j].Y);
                            Thread.Sleep(10);
                            mm.LeftDown();
                            y = 1;
                            while (y - 1 != x)
                            {
                                Thread.Sleep(10);
                                mm.MoveCursor(allPoints[i, j - y].X, allPoints[i, j - y].Y);
                                y++;
                            }
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j - y)].X, allPoints[IsWithin(i - 1), IsWithin(j - y)].Y);
                            Thread.Sleep(10);
                            mm.MoveCursor(allPoints[IsWithin(i - 1), IsWithin(j - y - 1)].X, allPoints[IsWithin(i - 1), IsWithin(j - y - 1)].Y);
                            Thread.Sleep(10);
                            mm.LeftUp();

                        }
                    }
                }
            }
            mm.MoveCursor(allPoints[0,0].X, allPoints[0, 0].Y);
            Thread.Sleep(10);
            mm.LeftDown();
            Thread.Sleep(10);
            mm.MoveCursor(allPoints[1, 0].X, allPoints[1, 0].Y);
            Thread.Sleep(10);
            mm.LeftUp();
        }

        private class MouseManager
        {
            public void MoveCursor(int x, int y)
            {
                Point p = new Point(x, y);

                Cursor.Position = p;
            }

            public void LeftDown()
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            }
            public void LeftUp()
            {
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        public static int IsWithin(int value)
        {
            if(value < 0)
            {
                return 0;
            }
            if(value > 3)
            {
                return 3;
            }
            return value;
            
        }

    }
}
