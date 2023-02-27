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
        //[Given(@"the user has opened the login page")]
        //public void GivenTheUserHasOpenedTheLoginPage()
        //{
        //    _app.NavigationService.GoToLoginPage();
        //}
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



}

