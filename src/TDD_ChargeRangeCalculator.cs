using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject1
{
    class TDD_ChargeRangeCalculator
    {
        public string getChargingSession_Ranges(int[] chargeRange)
        {
            string chargingSession_RangeReading = string.Empty;
            //if (chargeRange != null) //if {} or {3}
            //{
                int[] sortedValues = sortTheValues(chargeRange);
                int startValue = sortedValues[0];
                int endValue = sortedValues[0];
                int readingCount = 1;

                for (int i = 1; i < sortedValues.Length; i++) //{4,5}   {3,3,4}
                {
                        if ((sortedValues[i] == sortedValues[i - 1]) || (sortedValues[i] == sortedValues[i - 1] + 1)) //in range
                        {
                            endValue = sortedValues[i];
                            readingCount += 1;
                        }
                        else //range completed ; next range begins
                        {
                            chargingSession_RangeReading = string.Concat(chargingSession_RangeReading, FormatRangesWithReadings(startValue, endValue, readingCount));

                            startValue = sortedValues[i];
                            endValue = sortedValues[i];
                            readingCount = 1;
                        }
                }
                chargingSession_RangeReading = string.Concat(chargingSession_RangeReading, FormatRangesWithReadings(startValue, endValue, readingCount));
               
           // }

            return chargingSession_RangeReading;
        }

        public string FormatRangesWithReadings(int sValue, int eValue, int count)
        {
            string RangeList = string.Empty;
            if(sValue != eValue)
            {
                RangeList = sValue + "-" + eValue + ", " + count + System.Environment.NewLine;
            }
            else
            {
                RangeList = sValue + ", " + count + System.Environment.NewLine;
            }
            return RangeList;
        }

        public int[] sortTheValues(int[] chargeRange)
        {            
            Array.Sort(chargeRange);
            return chargeRange;
        }
    }
}
