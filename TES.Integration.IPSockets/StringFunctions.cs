using System;
using System.Collections.Generic;
using System.Text;

namespace TES.Integration.IPSockets
{
    public class StringFunctions
    {
        public Dictionary<int, string> NvPToDictionary(string v_inputtext, string v_lineseperator, string v_valueseperator)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            string[] strArray = v_inputtext.Split(v_lineseperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in strArray)
            {
                int key = 0;
                string[] strArray2 = str.Split(v_valueseperator.ToCharArray());
                key = int.Parse(strArray2[0]);
                dictionary.Add(key, strArray2[1]);
            }
            return dictionary;
        }
    }
}
}
