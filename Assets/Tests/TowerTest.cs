using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// A tornyok működésének tesztelése.
/// </summary>
namespace Tests
{
    public class TowerTest
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TowerPlacementTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);

            var tiles = GameObject.FindObjectsOfType<Tile>();
            var towerFactory = GameObject.FindObjectOfType<TowerFactory>();
            Tile tile = tiles[0];
            towerFactory.AddTower(tile);

            Assert.IsNotNull(Tower.FindObjectOfType<Tower>());
        }

        [Test]
        public void Tower_Damage()
        {
            Tower tower = new Tower();
            tower.Reset();
            Assert.IsTrue(tower.attackPower > 0.0f);
        }

        [Test]
        public void Tower_Range()
        {
            Tower tower = new Tower();

            tower.Reset();
   
            Assert.IsTrue(tower.GetAttackRange() > 0.0f);
        }

        [Test]
        public void Tower_UpgradedDamage()
        {
            Tower tower = new Tower();
            tower.Reset();
            float basicDamage = tower.attackPower;
            tower.Upgrade();

            Assert.IsTrue(tower.attackPower >  basicDamage);
        }

        [Test]
        public void Tower_UpgradedRange()
        {
            Tower tower = new Tower();
            tower.Reset();
            float basicRange = tower.GetAttackRange();
            tower.Upgrade();
            Assert.IsTrue(tower.GetAttackRange() > basicRange);
        }

        [Test]
        public void Tower_UpgradedPrice()
        {
            Tower tower = new Tower();
            tower.Reset();
            float basicUpgradePrice = tower.upgradePrice;
            tower.Upgrade();

            Assert.IsTrue(tower.upgradePrice > basicUpgradePrice);
        }

        [Test]
        public void Tower_UpgradedLevel()
        {
            Tower tower = new Tower();
            tower.Reset();
            float lvl = tower.level;
            tower.Upgrade();

            Assert.IsTrue(tower.level > lvl);
        }

        [Test]
        public void IsTowerFactorySingleton()
        {
            TowerFactory towerFactory1 = TowerFactory.Instance;
            TowerFactory towerFactory2 = TowerFactory.Instance;

            towerFactory1.gameObject.name = "different";
            Assert.AreEqual(towerFactory1,towerFactory2);

        }
    }
}
