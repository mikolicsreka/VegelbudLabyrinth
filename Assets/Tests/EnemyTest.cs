using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Az ellenségek tesztelése.
/// </summary>
/// 
namespace Tests
{
    public class EnemyTest
    {
        [UnityTest]
        public IEnumerator EnemySpawningTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);
            var enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
            float timeTilWave = enemySpawner.GetWaveTimer();
            yield return new WaitForSecondsRealtime(timeTilWave + 1);

            var enemies = GameObject.FindObjectsOfType<EnemyMovement>();
            Assert.IsNotNull(enemies);


        }

        [UnityTest]
        public IEnumerator EnemyMovingTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);
            var enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
            float timeTilWave = enemySpawner.GetWaveTimer();
            yield return new WaitForSecondsRealtime(timeTilWave);
            Assert.IsTrue(GameObject.FindObjectOfType<EnemyMovement>().transform.position != enemySpawner.transform.position);
        }

        [UnityTest]
        public IEnumerator EnemyGetDamageTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);
            var enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
            float timeTilWave = enemySpawner.GetWaveTimer();

            var tiles = GameObject.FindObjectsOfType<Tile>();
            var towerFactory = GameObject.FindObjectOfType<TowerFactory>();
            Tile tile = tiles[0];
            foreach (var item in tiles)
            {
                if (item.name == "0,1")
                {
                    tile = item;
                    break;
                }
            }
            towerFactory.AddTower(tile);
            yield return new WaitForSecondsRealtime(timeTilWave + 1);


            yield return new WaitForSecondsRealtime(2);
            EnemyDamage enemy = EnemyDamage.FindObjectsOfType<EnemyDamage>()[0];

            Assert.IsTrue(enemy.health < enemy.GetStartHealth());
        }


        [UnityTest]
        public IEnumerator EnemyDeathTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);
            var enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
            float timeTilWave = enemySpawner.GetWaveTimer();

            var tiles = GameObject.FindObjectsOfType<Tile>();
            var towerFactory = GameObject.FindObjectOfType<TowerFactory>();
            Tile tile = tiles[0];
            foreach (var item in tiles)
            {
                if (item.name == "0,1")
                {
                    tile = item;
                    break;
                }
            }
            towerFactory.AddTower(tile);
            yield return new WaitForSecondsRealtime(1);
            var tower = Tower.FindObjectOfType<Tower>();
            tower.attackPower = 101;

            yield return new WaitForSecondsRealtime(timeTilWave + 1);


            yield return new WaitForSecondsRealtime(1);
            EnemyDamage enemy = EnemyDamage.FindObjectsOfType<EnemyDamage>()[0];

            Assert.IsTrue(enemy.health <= 0);
            Assert.IsTrue(enemy.IsAlive() == false);
        }

        [Test]
        public void IsEnemySpawnerSingleton()
        {
            EnemySpawner enemySpawner1 = EnemySpawner.Instance;
            EnemySpawner enemySpawner2 = EnemySpawner.Instance;

            enemySpawner1.enemyLevel = EnemySpawner.EnemyLevels.Hard;
            enemySpawner2.BossEnemy = null;
            enemySpawner1.gameObject.name = "asd";

            Assert.AreEqual(enemySpawner1,enemySpawner2);
        
        }



    }
}
