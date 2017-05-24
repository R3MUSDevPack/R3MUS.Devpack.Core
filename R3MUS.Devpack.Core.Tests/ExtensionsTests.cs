using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ploeh.AutoFixture;
using R3MUS.Devpack.Core.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MUS.Devpack.Core.Tests
{
    [TestClass]
    public class ExtensionsTests : TestBase
    {
        [TestMethod]
        public void SetProperties_ValidObjectMatchSetsValues()
        {
            //  Arrange
            var sourceObject = Fixture.Create<TestObject>();
            var destObject = new TestObject();

            //  Act
            destObject.SetProperties(sourceObject);

            //  Assert
            destObject.ShouldBeEquivalentTo(sourceObject);
        }

        [TestMethod]
        public void SetProperties_InvalidObjectMatchThrowsException()
        {
            //  Arrange
            var sourceObject = Fixture.Create<InvalidTestObject>();
            var destObject = new TestObject();

            //  Act
            Action actual = () => destObject.SetProperties(sourceObject);

            //  Assert
            actual.ShouldThrow<Exception>().And.Message.Should().Be("Type mismatch");
        }

        [TestMethod]
        public void TryCast_SuccessfulCast()
        {
            //  Arrange
            var sourceObject = (JObject)JsonConvert.SerializeObject(Fixture.Create<TestObject>()).Deserialize(typeof(object));
            var destObject = new TestObject();

            //  Act
            sourceObject.TryCast<TestObject>(destObject);

            //  Assert
            destObject.ShouldBeEquivalentTo(sourceObject);
        }
    }
}
