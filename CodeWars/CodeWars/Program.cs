using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeWars.Junior_level;
using CodeWars.Noob_level;

namespace CodeWars
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task01 t1 =new Task01();
            //Console.WriteLine(t1.Check(0,0,0));

            //Task02 t2=new Task02();
            //string str = "1 2 3 5 g 72 1 3 2 -3";
            //Console.WriteLine(t2.GetNum(str));

            //Task03 t3 = new Task03();
            //Console.WriteLine(t3.TripleDouble(451992777, 41177722899));

            //Task04 t4 =new Task04();
            //Console.WriteLine(t4.FindShort("turns out random test cases are easier than writing out basic ones"));

            //Task05 t5 = new Task05();
            //Console.WriteLine(t5.Averages(new[] { 1, 3, 5, -5 }));

            //Task06 t6 =new Task06();
            //Console.WriteLine(t6.seriesSum(6));

            //string[] a1 = new string[] { "arp", "live", "strong","","11111","l","harp" };
            //string[] a2 = new string[] { "lively", "alive", "harp", "sharp", "armstrong" };
            //string[] r = new string[] { "arp", "live", "strong" };
            //Task07 t7 =new Task07();
            //var t =t7.inArray(a1,a2);
            //t?.ToList().ForEach(X=>Console.WriteLine(X));


            //Task08 t8 =new Task08();
            //Console.WriteLine(t8.ReverseWords("world! hello 111"));
          
            //Task09 t9 = new Task09();
            //Console.WriteLine(t9.Solution("abcdef"));
         
            //Task10 t10 = new Task10();
            //Console.WriteLine(t10.GetReadableTime(1000));

            JunTasks jun = new JunTasks();

            //jun.sqInRect(6,3).ForEach(X=>Console.WriteLine(X));

            //Console.WriteLine(jun.palindromeChainLength(89));
           
            Console.WriteLine(jun.IsBalanced("(Sensei [says) no!]", "()[]"));



            Console.ReadKey();
        }
    }
}
