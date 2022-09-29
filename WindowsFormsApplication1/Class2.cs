using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;

using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace WindowsFormsApplication1
{
    public class MP3Player
    {
        public void Play(string FilePath)
        {
            mciSendString("close all", "", 0, 0);
            mciSendString("open " + FilePath + " alias media", "", 0, 0);
            mciSendString("play media", "", 0, 0);
        }
        public void Pause()
        {
            mciSendString("pause media", "", 0, 0);
        }
        public void Stop()
        {
            mciSendString("close media", "", 0, 0);
        }
        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        private static extern int mciSendString(
         string lpstrCommand,
         string lpstrReturnString,
         int uReturnLength,
         int hwndCallback
        );
    }
}
