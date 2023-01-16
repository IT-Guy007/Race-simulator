using Controller;
using Model;

namespace ControllerTest;

[TestFixture]
internal class RaceTest {
    
    [SetUp]
    public void SetUp() {
        Data.Initialize();
        Data.NextRace();
    }

    [Test]
    public void TrackNotNull() {
        Assert.NotNull(Data.currentRace);
    }
    
}