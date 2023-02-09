using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //1.Basic Unit Testing
            TDD_ChargeRangeCalculator rangeDetector = new TDD_ChargeRangeCalculator();
            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 4, 5 }).Contains("4-5, 2"));

            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 3 }).Contains("3, 2"));

            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 3, 4, 5 }).Contains("3-5, 4"));

            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3 }).Contains("3, 1"));

            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 3, 5, 4, 10, 11, 12 }).Contains("3-5, 4"));
            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 3, 5, 4, 10, 11, 12 }).Contains("10-12, 3"));

            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 5 }).Contains("3, 1"));
            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 5 }).Contains("5, 1"));

            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 4, 5, 10 }).Contains("3-5, 3"));
            Assert.True(rangeDetector.getChargingSession_Ranges(new int[] { 3, 4, 5, 10 }).Contains("10, 1"));
        }

        //2. Data-Driven Unit Testing - for same use case
        [Theory,ClassData(typeof(DataDrivenTest_Input))]
        public void DataDrivenTest_RangeWithUniqueValues(int[] inputdata, string expectedOutput)
        {
            TDD_ChargeRangeCalculator rangeDetector = new TDD_ChargeRangeCalculator();
            var actualResult = rangeDetector.getChargingSession_Ranges(inputdata);
            Assert.Equal(expectedOutput, actualResult);

        }
        
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

        //6. Data-Driven UT - for 10 Bit ADC Readings to Amps Conversion
        [Theory, ClassData(typeof(A2D10Bit_ConvertToAmpsInput))]
        public void DataDrivenTest_10BitADC_ConvertToAmps(int[] inputdata, int[] expectedOutput)
        {
            ADC AmpsConverter = new ADC(10, -15, 15);
            var actualResult = AmpsConverter.convertReadingsToAmps_fromADC(inputdata);
            Assert.Equal(expectedOutput, actualResult);

        }
    }
    public class DataDrivenTest_Input : IEnumerable<object[]>
    {
           
            public IEnumerator<object[]> GetEnumerator()
            {
                //>=1 range with unique values (sorted/unsorted) - use case 
                //sorted
                yield return new object[] { new int[] {3,5,7}, "3, 1\r\n5, 1\r\n7, 1\r\n" };
                //unsorted
                yield return new object[] { new int[] { 7, 6, 4, 5, 12, 15, 14 }, "4-7, 4\r\n12, 1\r\n14-15, 2\r\n" };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
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
            yield return new object[] { new int[] { 0, 1, 4094 }, new int[]{ 0, 0, 10} };
            //sorted ; with error readings
            yield return new object[] { new int[] { 0, 1, 1111, 3000, 3040, 4095 }, new int[] { 0, 0, 3, 7, 7 } };
            //unsorted
            yield return new object[] { new int[] { 5000, 4000, 3950, 4097, 3, 1, 2, 1000 }, new int[] { 10, 10, 0, 0, 0, 2} };
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

