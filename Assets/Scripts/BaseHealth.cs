using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * A player bázisának életerejét irányítja, illetve a játék elvesztését triggereli.
 * 
 */
public class BaseHealth : MonoBehaviour
{

    [SerializeField] float startHealth = 100;
    float health;
    [SerializeField] int healthDecrease = 10;
    [SerializeField] Text healthText;
    [Range(1f,10f)]
    [SerializeField] float secondsToTickHP = 2f;

    [SerializeField] GameObject LostMenu;

    public Image healthBar;

    [SerializeField] AudioClip lostSFX;

    bool isGameLost = false;
    void Start()
    {
        healthText.text = startHealth.ToString();
        health = startHealth;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (health >= healthDecrease)
        {
            StartCoroutine(TickHealth(other));
        }
       

    }

    IEnumerator TickHealth(Collider enemy)
    {
 
            while (!enemy.GetComponent<BoxCollider>().Equals(null) && enemy.gameObject != null && health >= healthDecrease)
            {
                health -= healthDecrease;
                healthBar.fillAmount = health / startHealth;

                healthText.text = health.ToString();
                yield return new WaitForSecondsRealtime(secondsToTickHP);
            }
        

        
    }

    void Lost()
    {
        if (lostSFX != null)
        {
            GetComponent<AudioSource>().PlayOneShot(lostSFX);
        }

        LostMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if(health <= 0 && !isGameLost)
        {
            isGameLost = true;
            Lost();
        }
    }
}
