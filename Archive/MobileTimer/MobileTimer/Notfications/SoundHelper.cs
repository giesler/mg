using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MobileTimer
{
    class SoundHelper
    {
        public static void PlaySound(string fileName)
        {
            WCE_PlaySound(fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_FILENAME));
        }

        [DllImport("CoreDll.dll", EntryPoint = "PlaySound", SetLastError = true)]
        private extern static int WCE_PlaySound(string szSound, IntPtr hMod, int flags);

        private enum Flags
        {
            SND_SYNC = 0x0000, /* play synchronously (default) */
            SND_ASYNC = 0x0001, /* play asynchronously */
            SND_NODEFAULT = 0x0002, /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004, /* pszSound points to a memory file */
            SND_LOOP = 0x0008, /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010, /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000, /* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004 /* name is resource name or atom */
        }

        public enum Volumes : int
        {
            OFF = 0,
            LOW = 858993459,
            NORMAL = 1717986918,
            MEDIUM = -1717986919,
            HIGH = -858993460,
            VERY_HIGH = -1
        }

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int waveOutSetVolume(IntPtr device, int volume);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int waveOutGetVolume(IntPtr device, ref int volume);

        public static Volumes Volume
        {
            get
            {
                int v = (int)0;

                waveOutGetVolume(IntPtr.Zero, ref v);

                switch (v)
                {
                    case (int)Volumes.OFF: return Volumes.OFF;
                    case (int)Volumes.LOW: return Volumes.LOW;
                    case (int)Volumes.NORMAL: return Volumes.NORMAL;
                    case (int)Volumes.MEDIUM: return Volumes.MEDIUM;
                    case (int)Volumes.HIGH: return Volumes.HIGH;
                    case (int)Volumes.VERY_HIGH: return Volumes.VERY_HIGH;
                    default: return Volumes.OFF;
                }
            }

            set
            {
                waveOutSetVolume(IntPtr.Zero, (int)value);
            }
        }
    }
}