using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    internal class Task10
    {
        public string GetReadableTime(int seconds)
        {
            if (seconds <= 0)
                return "00:00:00";

            if (seconds > 359999)
                seconds = 359999;

            int hours = 0;
            int mins = 0;

            hours = (int) Math.Floor(((decimal) seconds/3600m));
            seconds -= 3600*hours;

            mins = (int) Math.Floor(((decimal) seconds/60m));
            seconds -= mins*60;

            string[] time = new string[] {hours.ToString(), mins.ToString(), seconds.ToString()};

            for (int i = 0; i < time.Length; i++)
            {
                if (time[i].Length < 2)
                {
                    time[i] = time[i].Insert(0, "0");
                }
            }
            return $"{time[0]}:{time[1]}:{time[2]}";
        }

    }
}

