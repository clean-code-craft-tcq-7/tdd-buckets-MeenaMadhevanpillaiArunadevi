using System;
using Xunit;
namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            TDD_ChargeRangeCalculator rangeDetector = new TDD_ChargeRangeCalculator();
            Assert.True(rangeDetector.getChargingSession_Ranges(new int[]{4, 5}).Contains("4-5, 2"));

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
    }
}
