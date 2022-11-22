using NUnit.Framework;
using System;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ReservationTests
    {
        //name_scenario_expectedBehavior
        [Test] 
        public void CanBeCancelledBy_UserIsAdmin_ReturnsTrue()  
        {
            //arrange
            var reservation = new Reservation();

            //act
            var result = reservation.CanBeCancelledBy(new User { IsAdmin = true });

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanBeCancelledBy_SameUserCancellingTheReservation_ReturnsTrue()
        {
            var user = new User();
            var reservation = new Reservation { MadeBy = user };

            var result = reservation.CanBeCancelledBy(user);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanBeCancelledBy_AnotherUserCancellingTheReservation_ReturnsFalse()
        {
            var user = new User();
            var reservation = new Reservation { MadeBy = user };

            var result = reservation.CanBeCancelledBy(new User());

            Assert.IsFalse(result);
        }
    }
}
