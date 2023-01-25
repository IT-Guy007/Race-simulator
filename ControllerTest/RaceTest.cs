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
        Assert.NotNull(Data.CurrentRace);
    }
    
    [Test]
    public void CheckRandomEquipment() {
        foreach (IParticipant driver in Data.CurrentCompetition.Participants) {
            if (driver.Equipment.Performance == 80 && driver.Equipment.Speed == 80 && driver.Equipment.Quality == 80) {
                Assert.Fail();
            }
            if(driver.Equipment.IsBroken) {
                Assert.Fail();
            }
        }
    }
}