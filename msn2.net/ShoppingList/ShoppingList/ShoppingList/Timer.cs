using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace mn2.net.ShoppingList
{
    class PerfTimer
    {
        [DllImport("coredll.dll")]
        extern static int QueryPerformanceCounter(ref long perfCounter);

        [DllImport("coredll.dll")]
        extern static int QueryPerformanceFrequency(ref long frequency);

        static private Int64 m_frequency;
        private Int64 m_start;
        private Int64 m_lastCheck;

        // Static constructor to initialize frequency.
        static PerfTimer()
        {
            if (QueryPerformanceFrequency(ref m_frequency) == 0)
            {
                throw new ApplicationException();
            }
            // Convert to ms.
            m_frequency /= 1000;
        }

        public void Start()
        {
            if (QueryPerformanceCounter(ref m_start) == 0)
            {
                throw new ApplicationException();
            }

            m_lastCheck = m_start;
        }

        public Int64 ElapsedTime()
        {
            Int64 current = 0;

            if (QueryPerformanceCounter(ref current) == 0)
            {
                throw new ApplicationException();
            }

            Int64 elapsed = (current - m_lastCheck) / m_frequency;
            m_lastCheck = current;
            
            return elapsed;
        }

        public Int64 Stop()
        {
            Int64 stop = 0;
            if (QueryPerformanceCounter(ref stop) == 0)
            {
                throw new ApplicationException();
            }
            return (stop - m_start) / m_frequency;
        }
    }
}
