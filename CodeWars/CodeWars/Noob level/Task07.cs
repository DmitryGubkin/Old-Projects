using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    class Task07
    {
        public  string[] inArray(string[] array1, string[] array2)
        {
            if (array1 == null || array2 == null)
                return null;
            var words = new List<string>();
            
            foreach (var word1 in array1)
            {
                if(string.IsNullOrEmpty(word1) || words.Contains(word1))
                    continue;

                foreach (var word2 in array2)
                {
                    if (word2.Contains(word1) && !string.IsNullOrEmpty(word2))

                    {
                        words.Add(word1);
                        break;
                    }
                    
                }
            }
            words.Sort();
            return words.ToArray();


        }
    }
}
