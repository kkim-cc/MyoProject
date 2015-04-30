using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace EMGClass
{

    public class EMGData
    {
        public int[] emg = new int[9];
        public string buffer;

        public int line;


        public EMGData()
        {
            getFile();
            
        }

        public void getFile()
        {
            string path = Directory.GetCurrentDirectory();
            string inputFile = "fly by night.txt";

            try
            {
                buffer = System.IO.File.ReadAllText(path + "\\" + inputFile);
            }
            catch
            {                
            }

        }
        public void GetData(string oneLine)
        {
            string line_data;
            line_data = oneLine.Replace(" ", "");
            string[] data = line_data.Split(',');

            if (data.Count() != 9)
                return;

            for (int i = 0; i < 8; i++)
            {
                emg[i] = Int32.Parse(data[i]);
            }
            /*
            string getDigit = "^[^\\d]*(\\d+)";
            
            for (int i = 0; i < 9; i++)
            {
                try
                {
                    if (Regex.Match(oneLine, getDigit).Success)
                    {
                        emg[i] = Convert.ToInt32(Regex.Match(oneLine, getDigit).Value);
                        oneLine = Regex.Replace(oneLine, getDigit, string.Empty);
                        oneLine = Regex.Replace(oneLine, ",", string.Empty);
                    }
                }
                catch
                {
                }
            }
            */
        }

        public void nextLine()
        {
            string getDigit = "^[^\\d]*(\\d+)";

            for (int i = 0; i < 9; i++)
            {
                try
                {
                    if (Regex.Match(buffer, getDigit).Success)
                    {
                        emg[i] = Convert.ToInt32(Regex.Match(buffer, getDigit).Value);
                        buffer = Regex.Replace(buffer, getDigit, string.Empty);
                        buffer = Regex.Replace(buffer, ",", string.Empty);
                        //Console.WriteLine(buffer);                        
                    }
                }
                catch
                {
                }
            }

        }
    }
}
