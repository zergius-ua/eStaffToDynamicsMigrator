using Xunit;

namespace Migrator
{
    public class TestMigrator
    {
        [Fact]
        public void TestToEntity()
        {
            var c = new Candidate()
            {
                Address = "Address line",
                Email = "email@my.org.com",
                FirstName = "FirstNameValue",
                LastName = "LastNameValue",
                MobilePhone = "+1 234 567-89-00",
                WorkPhone = "+98 765 432-10-12",
                BirthDate = "1995-07-08"
            };
            var e = c.ToEntity();
            Assert.Equal("Address line", e.Attributes["address2_composite"]);
        }

        [Fact]
        private void CheckRange()
        {
            var max = 300;
            var i = 230;
            var res = max % i;
        }
    }
}