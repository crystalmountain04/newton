using newtonTests.Helper;
using System.Timers;

namespace newtonTests
{
    public class UtilityTests
    {
        [Fact]
        public void TestTimer()
        {
            // arrange
            var aTimer = new TestTimer();
            var aHasReceived = false;
            ElapsedEventArgs aEventArgs = null;
            aTimer.Elapsed += (sender, e) => { aHasReceived = true; aEventArgs = e; };
            ElapsedEventArgs aTestEventArgs = new EventArgs() as ElapsedEventArgs;

            // act
            aTimer.RaiseElapsed(aTestEventArgs);

            // assert
            Assert.True(aHasReceived);
            Assert.Equal(aEventArgs, aTestEventArgs);
        }
    }
}