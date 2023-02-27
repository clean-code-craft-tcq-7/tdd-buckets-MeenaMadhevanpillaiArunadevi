using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestProject1
{
    public class TDD_TestDataDriven_12BitADC
    {
        //3. Data-Driven UT - for 12 Bit ADC Ignore Error Readings
        [Theory, ClassData(typeof(A2D12Bit_IgnoreErrorReadingsInput))]
        public void DataDrivenTest_12BitADC_IgnoreErrorReadings(int[] inputdata, int[] expectedOutput)
        {
            ADC AmpsConverter = new ADC(12, 0, 10);
            var actualResult = AmpsConverter.ignoreErrorReadings(inputdata);
            Assert.Equal(expectedOutput, actualResult);

        }
        //4. Data-Driven UT - for 12 Bit ADC Readings to Amps Conversion
        [Theory, ClassData(typeof(A2D12Bit_ConvertToAmpsInput))]
        public void DataDrivenTest_12BitADC_ConvertToAmps(int[] inputdata, int[] expectedOutput)
        {
            ADC AmpsConverter = new ADC(12, 0, 10);
            var actualResult = AmpsConverter.convertReadingsToAmps_fromADC(inputdata);
            Assert.Equal(expectedOutput, actualResult);

        }

        //5. Data-Driven UT - Integration:  12 Bit ADC Readings to Amps Conversion -> input to Range detection
        [Theory, ClassData(typeof(ADC12BitToAmpsConverter_IntegrationToRangeDetector))]
        public void DataDrivenTest_12BitADCAmpsConverter_IntegrationToRangeDetector(int[] inputdata, string expectedOutput)
        {
            ADC AmpsConverter = new ADC(12, 0, 10);
            var inputdata_forChargeRangeDetector = AmpsConverter.convertReadingsToAmps_fromADC(inputdata);
            TDD_ChargeRangeCalculator rangeDetector = new TDD_ChargeRangeCalculator();
            var actualResult = rangeDetector.getChargingSession_Ranges(inputdata_forChargeRangeDetector);
            Assert.Equal(expectedOutput, actualResult);

        }

    }


    public class A2D12Bit_IgnoreErrorReadingsInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //>=1 range with unique values (sorted/unsorted) - use case 
            //sorted
            yield return new object[] { new int[] { 0, 1, 4096, 4095, 4094 }, new int[] { 0, 1, 4094 } };

            yield return new object[] { new int[] { 0, 1, 1111, 3000, 5040, 4092 }, new int[] { 0, 1, 1111, 3000, 4092 } };
            //unsorted
            yield return new object[] { new int[] { 5000, 4000, 3950, 3, 1, 2, 9910 }, new int[] { 4000, 3950, 3, 1, 2 } };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class A2D12Bit_ConvertToAmpsInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //>=1 range with unique values (sorted/unsorted) - use case 
            //sorted
            yield return new object[] { new int[] { 0, 1, 4094 }, new int[] { 0, 0, 10 } };
            //sorted ; with error readings
            yield return new object[] { new int[] { 0, 1, 1111, 3000, 3040, 4095 }, new int[] { 0, 0, 3, 7, 7 } };
            //unsorted
            yield return new object[] { new int[] { 5000, 4000, 3950, 4097, 3, 1, 2, 1000 }, new int[] { 10, 10, 0, 0, 0, 2 } };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ADC12BitToAmpsConverter_IntegrationToRangeDetector : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //>=1 range with unique values (sorted/unsorted) - use case 
            //sorted
            yield return new object[] { new int[] { 0, 1, 4094 }, "0, 2\r\n10, 1\r\n" }; // op of adc and ip of range detector :  new int[] { 0, 0, 10 } ,
            //sorted ; with error readings
            yield return new object[] { new int[] { 0, 1, 1111, 3000, 3040, 4095 }, "0, 2\r\n3, 1\r\n7, 2\r\n" }; // new int[] { 0, 0, 3, 7, 7 },
            //unsorted
            yield return new object[] { new int[] { 5000, 4000, 3950, 1200, 4097, 3, 900, 1, 2, 1000 }, "0, 3\r\n2-3, 3\r\n10, 2\r\n" }; // new int[] { 10,3, 10, 0,2, 0, 0, 2 },

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
