using NUnit.Framework;
using UnityEngine;

namespace Modules.Planets
{
    public sealed class PlanetTest
    {
        private MoneyAdapterMock _moneyAdapter;
        private PlanetConfig _config;
        private Planet _planet;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _config = PlanetConfig.New(new PlanetConfig.CreateArgs
            {
                name = "Test",
                maxLevel = 10,
                startIncome = 10,
                endIncome = 100,
                incomeStep = 10,
                incomeDuration = 5,
                unlockPrice = 200,
                upgradePrice = 20
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ScriptableObject.DestroyImmediate(_config);
        }

        [SetUp]
        public void Setup()
        {
            _moneyAdapter = new MoneyAdapterMock();
            _planet = new Planet(_config, _moneyAdapter);
        }

        [Test]
        public void Instantiate()
        {
            //Arrange:
            _moneyAdapter.Money = 1000;
            
            //Assert:
            Assert.AreEqual(_planet.Name, "Test");
            Assert.IsTrue(_planet.CanUnlock);
            Assert.IsFalse(_planet.CanUpgrade);
            Assert.IsFalse(_planet.IsUnlocked);

            Assert.AreEqual(0, _planet.Level);
            Assert.AreEqual(10, _planet.MaxLevel);
            Assert.AreEqual(1, _planet.NextLevel);
            Assert.IsFalse(_planet.IsMaxLevel);

            Assert.AreEqual(200, _planet.Price);
            
            Assert.Zero(_planet.MinuteIncome);
        }

        [Test]
        public void Unlock()
        {
            bool unlockEvent = false;
            
            //Arrange:
            _moneyAdapter.Money = 1000;
            _planet.OnUnlocked += () => unlockEvent = true; 

            //Act:
            bool success = _planet.Unlock();
            
            //Assert:
            Assert.IsTrue(success);
            Assert.IsTrue(unlockEvent);
            
            Assert.AreEqual(1, _planet.Level);
            Assert.AreEqual(2, _planet.NextLevel);
            Assert.IsFalse(_planet.IsMaxLevel);

            Assert.IsTrue(_planet.CanUpgrade);
            Assert.AreEqual(40, _planet.Price);
        }
    }
}