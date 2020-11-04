using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * Az ellenségek sebzését, halálát és a hozzá tartozó dolgokat, animációkat kezeli.
 */
public class EnemyDamage : MonoBehaviour
{
    private Animator animator;
    private bool damageTaken = false;
    private bool isAlive = true;

    [SerializeField] float startHealth = 100;
    int scorePerEnemy = 12;

    public float health;


    [SerializeField] ParticleSystem damageTakenParticlePrefab;
    [SerializeField] AudioClip deathSFX;

    public Image healthBar;

    Player player;

    InfiniteLevelController infiniteLevelController;
    float enemyDieAnimLength;
    float enemyTakeDamageAnimLength;


    private EnemySpawner enemySpawner;
    private int SpawnerID;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        health = startHealth;
        player = FindObjectOfType<Player>(); //így hogy csak 1 db van belole

        enemySpawner = FindObjectOfType<EnemySpawner>();
        try
        {
            var infiniteLevelC = FindObjectOfType<InfiniteLevelController>();
            if (infiniteLevelC != null)
            {
                infiniteLevelController = infiniteLevelC;
            }
        }
        catch (Exception ex)
        {
            //NULLEXCEPTION
        }


        RuntimeAnimatorController ac = animator.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == "Die")        //If it has the same name as your clip
            {
                enemyDieAnimLength = ac.animationClips[i].length;

            }
            if (ac.animationClips[i].name == "GetHit")
            {
                enemyTakeDamageAnimLength = ac.animationClips[i].length;

            }
        }


    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public float GetStartHealth()
    {
        return startHealth;
    }

    // this gets called in the beginning when it is created by the spawner script
    void setName(int sName)
    {
        SpawnerID = sName;
    }

    void Update()
    {
        if (damageTaken == true)
        {
            animator.SetBool("damageTaken", false);
            damageTaken = false;
        }

        RotateHealthBar();
    }

    void RotateHealthBar()
    {
        var target = FindObjectOfType<Camera>();
        //var lookPos = target.transform.position - healthBar.transform.position;
        //lookPos.y = 0;
        //var rotation = Quaternion.LookRotation(lookPos);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

        Vector3 targetPostition = new Vector3(target.transform.position.x,
                                       healthBar.transform.parent.position.y,
                                       target.transform.position.z);
        healthBar.transform.parent.LookAt(targetPostition);
    }
    // Update is called once per frame
    private void OnParticleCollision(GameObject other)
    {
        
        StartCoroutine(ProcessHit(other.GetComponentInParent<Tower>().attackPower));
    }


    public void KillEnemy()
    {

        this.gameObject.GetComponent<EnemyMovement>().moveSpeed = 0f;
        AddScore();
        if (enemySpawner != null)
        {
            enemySpawner.BroadcastMessage("killEnemy", SpawnerID);
        }
        
        DestroyImmediate(gameObject);
    }

    private void AddScore()
    {
        if (player != null)
        {
            player.AddScore(scorePerEnemy);
        }


        if (infiniteLevelController != null)
        {

            infiniteLevelController.AddScorePerEnemy();
        }
    }

    IEnumerator PrepareToKillEnemy()
    {
        animator.SetBool("isAlive", false);

        if (deathSFX != null && isAlive) //todo check that its play once
        {
            GetComponent<AudioSource>().PlayOneShot(deathSFX);
            isAlive = false;
        }



        yield return new WaitForSecondsRealtime(enemyDieAnimLength);
        
        KillEnemy();
    }


    IEnumerator ProcessHit(float damage)
    {

        health -= damage;


        healthBar.fillAmount = health / startHealth;


        if (health <= 0)
        {

            StartCoroutine(PrepareToKillEnemy());
            yield break;
        }


        var vfx = Instantiate(damageTakenParticlePrefab, transform.position, Quaternion.identity);
        vfx.Play();

        animator.SetBool("damageTaken", true);

        // print("Current HP: " + hitPoints);
        yield return new WaitForSecondsRealtime(enemyTakeDamageAnimLength);

        Destroy(vfx.gameObject);



        damageTaken = true;
    }



}
