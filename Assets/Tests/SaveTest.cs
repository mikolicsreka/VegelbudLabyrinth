using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// A mentés helyességének tesztelése.
/// </summary>
namespace Tests
{
    public class SaveTest
    {


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator IsDataCorrectTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);
            
            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(4);
            Player player = Player.FindObjectOfType<Player>();
            int level = player.level;
            int score = player.score;
            string scene = player.scene;
            int numTowers = player.numOfTowers;
            List<Tower> towers = player.towers;


            player.SavePlayer();
            player.LoadPlayer();


            Assert.AreEqual(level, player.level);
            Assert.AreEqual(score, player.score);
            Assert.AreEqual(scene, player.scene);


            Assert.AreEqual(numTowers, player.numOfTowers);


            for (int i = 0; i < numTowers; i++)
            {
                Assert.AreEqual(towers[i], player.towers[i]);
            }

        }

        [UnityTest]
        public IEnumerator IsLoadedTest()
        {
            SceneManager.LoadScene("StartMenu");
            yield return new WaitForSecondsRealtime(4);

            SceneManager.LoadScene("Level_1");
            yield return new WaitForSecondsRealtime(2);
            Player player = Player.FindObjectOfType<Player>();

            int level = player.level;
            int score = player.score;
            string scene = player.scene;
            int numTowers = player.numOfTowers;
            List<Tower> towers = player.towers;

            player.SavePlayer();
            player.LoadPlayer();

            Assert.AreEqual(level, player.level);
            Assert.AreEqual(score, player.score);
            Assert.AreEqual(scene, player.scene);
            Assert.AreEqual(numTowers, player.numOfTowers);
            for (int i = 0; i < numTowers; i++)
            {
                Assert.AreEqual(towers[i], player.towers[i]);
            }


            //change up data
            SceneManager.LoadScene("Level_3");
            yield return new WaitForSecondsRealtime(2);
            player = Player.FindObjectOfType<Player>();
            player.AddScore(10);
            player.AddTower();
            player.scene = "Different";
            
            //check its not equal


            Assert.AreNotEqual(level, player.level);
            Assert.AreNotEqual(score, player.score);
            Assert.AreNotEqual(numTowers, player.numOfTowers);

            //load back and check its equal
            player.LoadPlayer();

            Assert.AreEqual(level, player.level);
            Assert.AreEqual(score, player.score);
            Assert.AreEqual(scene, player.scene);
            Assert.AreEqual(numTowers, player.numOfTowers);
            for (int i = 0; i < numTowers; i++)
            {
                Assert.AreEqual(towers[i], player.towers[i]);
            }






            yield return null;
        }


    }
}
