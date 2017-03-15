using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace R3MUS.Devpack.Core.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        protected IFixture Fixture;

        public void TestInitialise()
        {
            Fixture = new Fixture();
        }
    }
}
