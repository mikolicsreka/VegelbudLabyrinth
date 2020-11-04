using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A tornyokat megvalósító osztály. A tornyok játékobjektumára kell ráhúzni.
/// </summary>
public class Tower : MonoBehaviour
{
    //parameters of each tower:
    [SerializeField] Transform objectToPan;
    [SerializeField] float attackRange;
    [SerializeField] public float attackPower ;
    [SerializeField] ParticleSystem projectileParticle;

    //state of each tower:
    Transform targetEnemy;
    //public Waypoint baseWaypoint;
    public Tile baseWaypoint;

    public int level = 1;
    public int upgradePrice ;
    public int buyPrice ;

    [SerializeField] Text levelText;
    [SerializeField] public GameObject canvas;
    bool isLevelShowing = false;

    bool updated = false;

    public int id;
    Player player;

    bool idTextSetted = false;
    [SerializeField] Text idText;

    [SerializeField] AudioClip canonSFX;

    void Start()
    {
        Reset();
        player = FindObjectOfType<Player>();
        if (canvas != null)
        {
            isLevelShowing = canvas.activeSelf;

            if (isLevelShowing)
            {
                canvas.gameObject.SetActive(false);
                SetIsLevelShowing(false);

            }
        }

    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetEnemy();


        if (targetEnemy != null && objectToPan != null)
        {
            //objectToPan.LookAt(targetEnemy);
            //LOOK AT TARGET ENEMY
            Vector3 targetPostition = new Vector3(targetEnemy.position.x,
                                                   objectToPan.position.y,
                                                   targetEnemy.position.z);
            objectToPan.LookAt(targetPostition);


            if (targetEnemy.GetComponent<EnemyDamage>().health > 0)
            {
                FireAtEnemy();
            }

        }
        else
        {
            Shoot(false);

        }

        if (updated && levelText != null)
        {
            SetLevelText();
            updated = false;
        }

        try
        {
            if (idText != null && !idTextSetted)
            {
                //1től indexelünk
                idText.text = (id + 1).ToString();
                // i 1-től megy, mert a szintek is.
                for (int i = 1; i < player.towers[Convert.ToInt32(this.gameObject.name)].level; i++)
                {
                    this.Upgrade();
                }
                idTextSetted = true;
            }
        }
        catch (Exception)
        {

            //Debug.LogError("No ID text");
        }




    }


    public void SetIsLevelShowing(bool _isLevelShowing)
    {
        isLevelShowing = _isLevelShowing;
    }

    public bool GetIsLevelShowing()
    {
        return isLevelShowing;
    }

    public void SetLevelText()
    {
        try
        {
            levelText.text = "" + "Lvl. " + level;
        }
        catch (Exception ex)
        {

        }

    }

    //ONLICK
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SceneManager.GetActiveScene().name == "EndlessLevels")
            {
                FindObjectOfType<TowerUIHandler>().SetTarget(this);

                if (FindObjectOfType<Infinite_TowerFactory>().CanBuild)
                {
                    FindObjectOfType<Infinite_TowerFactory>().SelectTile(this.baseWaypoint);
                }

            }
            else
            {
                if (!isLevelShowing)
                {
                    SetLevelText();
                    canvas.SetActive(true);
                    //canvas.gameObject.SetActive(true);
                    isLevelShowing = true;

                }
                else
                {
                    canvas.SetActive(false);
                    //canvas.gameObject.SetActive(false);
                    isLevelShowing = false;

                }
            }

        }
    }

    private void SetTargetEnemy()
    {
        var sceneEnemies = FindObjectsOfType<EnemyDamage>();
        if (sceneEnemies.Length == 0) { return; }

        Transform closestEnemy = sceneEnemies[0].transform;

        foreach (EnemyDamage testEnemy in sceneEnemies)
        {
            closestEnemy = GetClosest(closestEnemy, testEnemy.transform);
        }

        targetEnemy = closestEnemy;
    }

    private Transform GetClosest(Transform transformA, Transform transformB)
    {
        var distToA = Vector3.Distance(transform.position, transformA.position);
        var distToB = Vector3.Distance(transform.position, transformB.position);

        if (distToA < distToB)
        {
            return transformA;
        }
        return transformB;
    }

    private void FireAtEnemy()
    {

        float distanceToEnemy = Vector3.Distance(targetEnemy.transform.position, gameObject.transform.position);

        if (distanceToEnemy <= attackRange)
        {

            Shoot(true);

        }
        else
        {

            Shoot(false);
        }
    }


    private void Shoot(bool isActive)
    {
        if (!projectileParticle) return;
        var emissionModule = projectileParticle.emission;
        emissionModule.enabled = isActive;
        // todo rename this
        var components = projectileParticle.GetComponentsInChildren<ParticleSystem>();
        foreach (var item in components)
        {
            item.enableEmission = isActive;
        }


        if (isActive == true && !GetComponent<AudioSource>().isPlaying && GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().PlayOneShot(canonSFX);

        }

    }

    public void Upgrade()
    {
        level++;
        attackPower += 5.0f;
        attackRange += 5.0f;
        upgradePrice += 15;


        updated = true;
    }

    public void Reset()
    {
        level = 1;
        attackPower = 30f;
        attackRange = 40f;
        upgradePrice = 35;
        buyPrice = 30;
    }
    public int GetBuyPrice()
    {
        return buyPrice;
    }

    public int GetUpgradePrice()
    {
        return upgradePrice;
    }


}
