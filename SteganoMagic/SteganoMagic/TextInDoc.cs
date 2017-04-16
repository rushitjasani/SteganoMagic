using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganoMagic
{
    class TextInDoc : ITextInDoc
    {
        public string Embedd(string text, string msg)
        {
            string binMsg = "";
            while (text.Contains("  "))
                text = text.Replace("  ", " ");
            text = text.Trim();

            var foundIndexes = new List<int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                    foundIndexes.Add(i);
            }

            for (int i = 0; i < msg.Length; i++)
            {
                binMsg += Convert.ToString(msg[i], 2).PadLeft(8, '0');
            }
            binMsg += "11111111";

            int ji = 0;
            for (int i = 0; i < binMsg.Length; i++)
            {
                if (binMsg[i] == '0')
                {
                    Console.Write("0");

                }
                else
                {
                    Console.Write("1");
                    text = text.Insert(foundIndexes[ji], " ");
                    for (int k = ji; k < foundIndexes.Count; k++)
                    {
                        foundIndexes[k]++;
                    }
                }
                ji++;
            }
            return text;
        }



        public string Extract(string text)
        {
            string binMsg = "";
            string msg="";

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i - 1] == ' ' && text[i] != ' ')
                    binMsg += "0";
                else if (text[i - 1] == ' ' && text[i] == ' ')
                {
                    binMsg += "1";
                    i++;
                }
            }

            for (int i = 0; i < binMsg.Length - 8; i += 8)
            {
                string tempMsg = binMsg.Substring(i,8);
                if (tempMsg.Equals("11111111"))
                {
                    break;
                }
                msg += (char)Convert.ToInt32(tempMsg,2);
            }
            return msg;
        }
    }
}
