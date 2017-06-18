using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Junior_level
{
    class JunTasks
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="wdth"></param>
        /// <returns></returns>
        public  List<int> sqInRect(int lng, int wdth)
        {
            if(wdth<=0||lng<=0 ||lng==wdth)
            return null;
            var rects = new List<int>();
            var sqr = wdth*lng;

            while (sqr!=1)
            {
                
                if (lng > wdth)
                {
                    rects.Add(wdth);
                    lng -= wdth;
                }
                else
                {rects.Add(lng);
                    wdth -= lng;
                }

                if (wdth == lng)
                {
                    rects.Add(wdth);
                    break;
                }

                sqr = wdth*lng;

            }
          
            return rects;

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int palindromeChainLength(int num)
        {
            double n = Convert.ToDouble(num);

            double[] exp = new[] {43d, 194d, 4773d};

            if (n <= 0 || exp.Contains(n))
                return 0;

            int count = 0;

            string str = n.ToString();
            string rstr = new string(str.ToCharArray().Reverse().ToArray());

            while (str != rstr)
            {
                count++;
                n += double.Parse(rstr);

                if (exp.Contains(n))
                    return 0;

                str = n.ToString();
                rstr = new string(str.ToCharArray().Reverse().ToArray());
               
            }


            return count;
        }

        public bool IsBalanced(string s, string caps)
        {
            var c = caps.Replace(" ", string.Empty).ToCharArray();

            if (string.IsNullOrEmpty(caps) || string.IsNullOrEmpty(s) || caps.Length%2 != 0)
                return false;

            var a = s.ToCharArray().Where(x => c.Select(t => t).Contains(x)).Select(x => x).ToArray();

            if (a.Count()%2 != 0)
                return false;

            s = string.Concat(a);

           List<string> pairs = new List<string>();

            for (int i = 0; i < a.Length; i++)
            {
                if(i%2!=0)
                    pairs.Add(s.Substring(i-1,2));
            }

            for (int i = 0; i < pairs.Count-1; i++)
            {
                for (int j = 0; j < c.Length-1; j++)
                {
                    
                    
                }
                
            }
         
                
            

            return true;
        }



    }
}
