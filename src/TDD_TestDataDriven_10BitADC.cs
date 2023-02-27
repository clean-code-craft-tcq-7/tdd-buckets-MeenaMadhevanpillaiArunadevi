using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestProject1
{
    public class TDD_TestDataDriven_10BitADC
    {


        //6. Data-Driven UT - for 10 Bit ADC Readings to Amps Conversion
        [Theory, ClassData(typeof(A2D10Bit_ConvertToAmpsInput))]
        public void DataDrivenTest_10BitADC_ConvertToAmps(int[] inputdata, int[] expectedOutput)
        {
            ADC AmpsConverter = new ADC(10, -15, 15);
            var actualResult = AmpsConverter.convertReadingsToAmps_fromADC(inputdata);
            Assert.Equal(expectedOutput, actualResult);

        }


    }

    public class A2D10Bit_ConvertToAmpsInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //>=1 range with unique values (sorted/unsorted) - use case 
            //sorted
            yield return new object[] { new int[] { 0, 1, 1111, 4094, 1022, 511, 1024 }, new int[] { 15, 15, 15, 0 } };

            //unsorted
            yield return new object[] { new int[] { 511, 5000, 4000, 3950, 4097, 3, 1, 2, 1000, 1024, 900, 100 }, new int[] { 0, 15, 15, 15, 14, 11, 12 } };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
