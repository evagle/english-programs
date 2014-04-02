using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrtTimeModify.src
{
    class ITime
    {
        public int intTime;
        public string millSecond;
        public string strTime;
        public string originTime;
        public ITime clone()
        {
            ITime t = new ITime(this.strTime);
            return t;
        }
        
        public ITime(string time)
        {
            this.strTime = time;
            this.originTime = time;
            this.strTimetoIntTime();
        }
        public void strTimetoIntTime(){
            string[] parts = this.strTime.Split(new char[]{':',','});
            if (parts.Length == 4)
            {
                this.intTime = Convert.ToInt32(parts[0]) * 3600 + Convert.ToInt32(parts[1]) * 60 + Convert.ToInt32(parts[2]);
                this.millSecond = parts[3];
            }
        }
        public void setMilliSec(String milli) {
            millSecond = milli;
            intToString();
        }
        public ITime amend(int val){
            intTime = intTime + val <= 0 ? 0 : intTime + val;
            intToString();
            return this;
        }
        public void intToString(){
            strTime = "";
            strTime += (intTime / 3600).ToString("00") + ":" + ((intTime % 3600) / 60).ToString("00") +":"+ (intTime % 3600 % 60).ToString("00");
            strTime += "," + millSecond;
        }
        public double sub(ITime otherTime) {
            double res = intTime - otherTime.intTime;
            res += (double.Parse(millSecond) - double.Parse(otherTime.millSecond)) / 1000;
            return res;
        }

    }
}
