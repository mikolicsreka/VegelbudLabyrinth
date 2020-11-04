using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// A játékoshoz tartozó dolgok tesztelése.
/// </summary>
namespace Tests
{
    public class PlayerTest
    {

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.

        [UnityTest]
        public IEnumerator PlayerStartingTowerTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            Player player = Player.FindObjectOfType<Player>();
            Assert.AreEqual(player.startingTowerNum, player.numOfTowers);
            Assert.IsNotNull(player.towers);
        }

        [Test]
        public void PlayerAddScoreTest()
        {
            Player player = new Player();
            int score = player.score;
            int adding = 33;
            player.AddScore(adding);
            Assert.AreEqual((adding+score) ,player.score);

        }

        [Test]
        public void IsPlayerSingletonTest()
        {
            Player player1 = Player.Instance;
            Player player2 = Player.Instance;
            int adding = 33;
            player1.AddScore(adding);
            Assert.AreEqual(player1, player2);

        }

        [UnityTest]
        public IEnumerator PlayerAddTowerTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);

            Player player = Player.FindObjectOfType<Player>();

            int numTowers = player.numOfTowers;
            int lenghtOfTowerList = player.towers.Count;
            int adding = 3;

            for (int i = 0; i < adding; i++)
            {
                player.AddTower();
            }


            int sum1 = numTowers + adding;
            int sum2 = lenghtOfTowerList + adding;


            Assert.IsTrue(sum1 == player.numOfTowers);
            Assert.IsTrue(sum2 == player.towers.Count);

        }

        [UnityTest]
        public IEnumerator GetScoreFromEnemyTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);
            var enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
            float timeTilWave = enemySpawner.GetWaveTimer();

            Player player = Player.FindObjectOfType<Player>();
            float score = player.score;

            yield return new WaitForSecondsRealtime(timeTilWave+4);

            var enemies = EnemyDamage.FindObjectsOfType<EnemyDamage>();
            foreach (var item in enemies)
            {
                item.KillEnemy();
            }
            Assert.IsTrue(score < player.score);
        }

        [UnityTest]
        public IEnumerator GetDamageFromEnemyTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(1);
            var enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
            float timeTilWave = enemySpawner.GetWaveTimer();

            var health = BaseHealth.FindObjectOfType<BaseHealth>().healthBar.fillAmount; //1
            Debug.Log("Health: " + health);
            yield return new WaitForSecondsRealtime(timeTilWave);
            EnemyMovement enemy = EnemyMovement.FindObjectOfType<EnemyMovement>();

            yield return new WaitUntil(() => enemy.transform.position == Pathfinder.FindObjectOfType<Pathfinder>().endWaypoint.transform.position);
            yield return new WaitForSecondsRealtime(5);


            Assert.IsTrue(BaseHealth.FindObjectOfType<BaseHealth>().healthBar.fillAmount < health);
        }

        [UnityTest]
        public IEnumerator IsDontDestroyOnLoadTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            Player player = Player.FindObjectOfType<Player>();
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(2);


            Assert.AreEqual(player, Player.FindObjectOfType<Player>());
        }




    }
}
