using System;
using Model;

namespace ControllerTest {

    [TestFixture]
    public class Model_Competition_NextTrackShould {

        private Competition? _competition;

        [SetUp]
        public void setUp() {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull() {
            var result = _competition.NextTrack();
            Assert.IsNull(result);

        }
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack() {
            _competition.Tracks.Enqueue(new Track());
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue() {
            _competition.Tracks.Enqueue(new Track());
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        public void NextTrack_TwoInQueue_ReturnNextTrack() {
            _competition.Tracks.Enqueue(new Track());
            _competition.Tracks.Enqueue(new Track());
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }
    }
}