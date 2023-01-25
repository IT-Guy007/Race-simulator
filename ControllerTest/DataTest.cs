using Controller;
using Model;

namespace ControllerTest;

[TestFixture]
internal class DataTest {
    
    [SetUp]
    public void SetUp() {
        Data.Initialize();
        Data.NextRace();
    }

    [Test]
    public void DriversTest() {
        Assert.That(Data.CurrentCompetition.Participants.Count, Is.EqualTo(8));
    }

    [Test]
    public void TrackTest() {
        Assert.That(Data.CurrentCompetition.Tracks.Count, Is.EqualTo(3));
    }
    
    //Check if each track has a starting grid
    [Test]
    public void TrackTestCheckIfHasStartingGrid() {
        Queue<bool> hasStartingGrid = new Queue<bool>();
        foreach (var track in Data.CurrentCompetition.Tracks) {
            foreach (Section section in track.Sections) {
                if (section.SectionType == SectionTypes.StartGrid) {
                    hasStartingGrid.Enqueue(true);
                    break;
                }
            }
        }

        for (int i = 0; i < hasStartingGrid.Count; i++) {
            if (!hasStartingGrid.Dequeue()) {
                Assert.Fail();
                break;
            }
                
        }
        Assert.Pass();
    }
    
    [Test]
    public void TrackTestCheckIfHasFinish() {
        Queue<bool> hasFinish = new Queue<bool>();
        foreach (var track in Data.CurrentCompetition.Tracks) {
            foreach (Section section in track.Sections) {
                if (section.SectionType == SectionTypes.Finish) {
                    hasFinish.Enqueue(true);
                    break;
                }
            }
        }

        for (int i = 0; i < hasFinish.Count; i++) {
            if (!hasFinish.Dequeue()) {
                Assert.Fail();
                break;
            }
                
        }
        Assert.Pass();
    }
    
    [Test]
    public void SectionShouldBeInRace() {
        Section firstValue = Data.CurrentCompetition.Tracks.First().Sections.First?.Value;
        if (firstValue != null) {
            Section section = firstValue;
            SectionData result = Data.CurrentRace.GetSectionData(section);
            
            Assert.IsInstanceOf<SectionData>(result);
            Assert.IsNotNull(result);
        } else {
            Assert.Fail();
        }
    }

}