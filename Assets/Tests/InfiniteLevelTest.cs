using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// A végtelen játékmód egyedi elemeinek tesztelése.
/// </summary>
namespace Tests
{
    public class InfiniteLevelTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IsLevelControllerSingleton()
        {
            InfiniteLevelController levelController1 = InfiniteLevelController.Instance;
            InfiniteLevelController levelController2 = InfiniteLevelController.Instance;
            levelController1.score = 22;
            Assert.AreEqual(levelController1,levelController2);
        
        }

        [Test]
        public void IsEnemySpawnerSingleton()
        {
            Infinite_EnemySpawner enemySpawner1 = Infinite_EnemySpawner.Instance;
            Infinite_EnemySpawner enemySpawner2 = Infinite_EnemySpawner.Instance;
            enemySpawner1.enemyPrefab = null;
            Assert.AreEqual(enemySpawner1, enemySpawner2);

        }

        [Test]
        public void IsTowerFactorySingleton()
        {
            Infinite_TowerFactory towerFactory1 = Infinite_TowerFactory.Instance;
            Infinite_TowerFactory towerFactory2 = Infinite_TowerFactory.Instance;
            towerFactory2.blueTurretPrefab = null;
            Assert.AreEqual(towerFactory1, towerFactory2);

        }

        [Test]
        public void MazeGeneratorSingleton()
        {
            MazeGenerator mazeGenerator1 = MazeGenerator.Instance;
            MazeGenerator mazeGenerator2 = MazeGenerator.Instance;
            mazeGenerator1.tileMaterial = null;
            Assert.AreEqual(mazeGenerator1, mazeGenerator1);

        }

        
        [UnityTest]
        public IEnumerator IsEnemySpawningTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);

            SceneManager.LoadScene("EndlessLevels");
            yield return new WaitForSecondsRealtime(4);
            var enemies = EnemyMovement.FindObjectsOfType<EnemyMovement>();

            Assert.IsNotNull(enemies);

        }


        [UnityTest]
        public IEnumerator AreTheMazesDifferent()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);

            SceneManager.LoadScene("EndlessLevels");
            yield return new WaitForSecondsRealtime(4);
            var mazeGenerator = MazeGenerator.FindObjectOfType<MazeGenerator>();
            var maze1 = mazeGenerator.GetMaze();
            mazeGenerator.CreateMazeWithPathfinder();
            var maze2 = mazeGenerator.GetMaze();


            Assert.AreNotEqual(maze1,maze2);

        }

        [UnityTest]
        public IEnumerator IsEnemyMovingTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);

            SceneManager.LoadScene("EndlessLevels");
            yield return new WaitForSecondsRealtime(4);
            var enemies = EnemyMovement.FindObjectsOfType<EnemyMovement>();
            Infinite_EnemySpawner enemySpawner = Infinite_EnemySpawner.FindObjectOfType<Infinite_EnemySpawner>();


            Assert.IsNotNull(enemies);
            Assert.AreNotEqual(enemySpawner.transform.position, enemies[0]);
            

        }

        [UnityTest]
        public IEnumerator IsTowerShooting()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);

            SceneManager.LoadScene("EndlessLevels");
            yield return new WaitForSecondsRealtime(0.2f);
            var enemies = EnemyDamage.FindObjectsOfType<EnemyDamage>();
            var tiles = Tile.FindObjectsOfType<Tile>();
            var enemy = enemies[0];

            Infinite_TowerFactory towerFactory = Infinite_TowerFactory.FindObjectOfType<Infinite_TowerFactory>();
            towerFactory.SelectTurretToBuild(Infinite_Shop.FindObjectOfType<Infinite_Shop>().blueTower);
            for (int i = tiles.Length-1; i > tiles.Length-3; i--)
            {
                towerFactory.InstantiateNewTower(tiles[i]);
            }
            yield return new WaitForSecondsRealtime(3);

            Assert.IsTrue(enemy.GetStartHealth() > enemy.health);


        }



        [UnityTest]
        public IEnumerator IsMazeGeneratedTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);

            SceneManager.LoadScene("EndlessLevels");
            yield return new WaitForSecondsRealtime(4);
            var mazeTiles = Tile.FindObjectsOfType<Tile>();
            var mazeWaypoints = Waypoint.FindObjectsOfType<Waypoint>();

            Assert.IsNotNull(mazeTiles);
            Assert.IsNotNull(mazeWaypoints);

        }

        [UnityTest]
        public IEnumerator EnemyReachEndTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);

            SceneManager.LoadScene("EndlessLevels");
            yield return new WaitForSecondsRealtime(4);
            var enemies = EnemyMovement.FindObjectsOfType<EnemyMovement>();
            var endwaypoint = Pathfinder.FindObjectOfType<Pathfinder>().GetEndWaypoint();
            var enemy = enemies[0].gameObject;
            Assert.IsNotNull(enemy);
            InfiniteLevelController infiniteLevelController = InfiniteLevelController.FindObjectOfType<InfiniteLevelController>();
            int hp = infiniteLevelController.GetHP();
            yield return new WaitUntil(() => enemy.transform.position == endwaypoint.transform.position);
            yield return new WaitForSecondsRealtime(2);
            Assert.IsTrue(hp > infiniteLevelController.GetHP());
        }


    }
}
