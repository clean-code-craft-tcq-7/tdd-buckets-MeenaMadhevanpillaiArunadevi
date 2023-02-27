using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject1
{
    public class ADC
    {
        public int inputBit;
        public int readingMin;
        public int readingMax;
        public int currentMax;
        public int currentMin; 
        //public List<int> readingsList;

        public ADC(int inputBit,int currentMin, int currentMax)
        {
            this.inputBit = inputBit;
            this.currentMax = currentMax;
            this.currentMin = currentMin;
            findReadingMin();
            findReadingMax();
        }

        public void findReadingMax()
        {
            this.readingMax = (int)(Math.Pow(2, inputBit) - 2);
        }

        public void findReadingMin()
        {
            this.readingMin = (int)(Math.Pow(2, 0) - 1);
        }

        public int[] ignoreErrorReadings(int[] readings)
        {
           
            foreach (var reading in readings)
            {
                if(reading < readingMin || reading > readingMax)
                {
                    readings = readings.Where(val => val != reading).ToArray();
                }
            }
            return readings;
        }


        public int[] convertReadingsToAmps_fromADC(int[] readings)
        {
            int value;
            var Amps = new List<int>();
            foreach(var reading in ignoreErrorReadings(readings))
            {
                value = reading;
                if(currentMin < 0) // for 10 bit compatibility , -15A to 15A ; not 0 to 15A ; (to get -15A to 15A ;convert 0 to 1023 to -1023 to 1023)
                {
                    value = (reading - readingMax/2) * 2;
                }
                Amps.Add(getAmpsFromADCReading(value));
               
            }
            return Amps.ToArray();
        }

        public int getAmpsFromADCReading(int value) //compatible for 12/10 bit
        {
            int Amps = (int)Math.Round((decimal)(value * currentMax) / readingMax);
            return Math.Abs(Amps);
        }
    }
}
