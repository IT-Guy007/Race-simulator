using Controller;
using Model;

namespace ControllerTest;

public class TrackTest
{

    [SetUp]
    public void SetUp()
    {
        Data.Initialize();
        Data.NextRace();
    }

    [Test]
    public void HasNoNullValues() {
        foreach (Track track in Data.currentCompetition.Tracks) {
            if(track.laps == null) {
                Assert.Fail("Track " + track.name + " has null laps");
            }
            if(track.name == null) {
                Assert.Fail("Track " + track.name + " has null name");
            }
            if(track.Sections == null) {
                Assert.Fail("Track " + track.name + " has null sections");
            }
            if(track.startDirection == null) {
                Assert.Fail("Track " + track.name + " has null startDirection");
            }
            if(track.backgroundColorMaui == null) {
                Assert.Fail("Track " + track.name + " has null backgroundColorMaui");
            }
            if(track.backgroundColorSpectre == null) {
                Assert.Fail("Track " + track.name + " has null backgroundColorSpectre");
            }
            if(track.startX == null) {
                Assert.Fail("Track " + track.name + " has null startX");
            }
            if(track.startY == null) {
                Assert.Fail("Track " + track.name + " has null startY");
            }
            
        }
    }

}
