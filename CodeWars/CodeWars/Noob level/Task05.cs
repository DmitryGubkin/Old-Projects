using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    //Write a method, that gets an array of integer-numbers and return an array of the averages of each integer-number and his follower, if there is one.
    class Task05
    {
        public  double[] Averages(int[] numbers)
        {
            if (numbers == null||numbers.Length<2)
                return new double [0];

            double[] arr = new double[numbers.Length-1];

            for (int i = 0; i < numbers.Length; i++)
            {
                if(i!=numbers.Length-1)
                arr[i] = ((double)numbers[i] + (double)numbers[i + 1])/2d;
            }
            return arr;
        }
    }
}
