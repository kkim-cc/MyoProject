using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pd_myo
{
    class MData
    {
        public int[] data = new int[8];
        public string strTime;

        public MData()
        {
            strTime = "";
            for (int i = 0; i < 8; i++)
			{
			    data[i] = 0;
			}
        }
        public int GetData(int index)
        {
            int nData = data[index];
            return nData;
        }
    }
}
