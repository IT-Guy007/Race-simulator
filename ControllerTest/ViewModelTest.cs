using Controller;

namespace ControllerTest;

[TestFixture]
public class ViewModelTest {
    private RaceStatisticsViewModel RaceStatisticsViewModel;
    
    [SetUp]
    public void SetUp() {
        Data.Initialize();
        Data.NextRace();
        RaceStatisticsViewModel = new RaceStatisticsViewModel();
    }
    
    [Test]
    public void DriverNamesTest() {
        Assert.AreEqual(8, RaceStatisticsViewModel.DriverNames.Count);
    }

    [Test]
    public void DriverStatusTest() {
        Assert.AreEqual(8, RaceStatisticsViewModel.DriverNames.Count);
        foreach (var status in RaceStatisticsViewModel.DriverStatus) {
            Assert.IsNotNull(status);
        }
    }

    [Test]
    public void EquipmentSpeedTest() {
        Assert.AreEqual(8, RaceStatisticsViewModel.EquipmentSpeed.Count);
    }
    
    [Test]
    public void EquipmentQualityTest() {
        Assert.AreEqual(8, RaceStatisticsViewModel.EquipmentQuality.Count);
    }
    
    [Test]
    public void EquipmentPerformanceTest() {
        Assert.AreEqual(8, RaceStatisticsViewModel.EquipmentPerformance.Count);

    }

}