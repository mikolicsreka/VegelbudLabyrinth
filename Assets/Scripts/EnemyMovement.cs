using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * Az ellenségek mozgását kezeli. (forgás, előremenetel, animációk.)
 */

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] BaseHealth baseTower;
    [SerializeField] List<Waypoint> path;
    private Animator animator;
    private Waypoint endWaypoint;

    private bool reachedEnd = false;
    float enemyDizzyAnimLength;

    //mine
    Vector3[] directions = {
        Vector3.forward, //(0,10)
        Vector3.back, //(0,-10)zz
        Vector3.right, //(10,0)
        Vector3.left // (-10,0)
    };

    //Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isAnimating", true);

        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        endWaypoint = pathfinder.GetEndWaypoint();
        path = pathfinder.GetPath();
        baseTower = FindObjectOfType<BaseHealth>();
        //foreach (var item in path)
        //{
        //    print(item);
        //}

        RuntimeAnimatorController ac = animator.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == "Victory")        //If it has the same name as your clip
            {
                enemyDizzyAnimLength = ac.animationClips[i].length;

            }
        }
        StartCoroutine(FollowPath(path));
    }



    private Transform toGoTo;
    private Vector3 toGotoNext; //2nd after csekkolni h átló-e

    [SerializeField] public float moveSpeed = 50f;


    void Update()
    {
        Vector3 pos = transform.position;

        if (reachedEnd == true)
        {
            return;
        }
        else if (pos == endWaypoint.transform.position)
        {

            ReachEnd();
        }
        else
        {
            Turning();
        }


        MovingForward();
    }


    void MovingForward()
    {

        try
        {
            transform.position = Vector3.MoveTowards(transform.position, toGoTo.position, moveSpeed * Time.deltaTime);
        }
        catch (NullReferenceException ex)
        {
            //Debug.Log("EnemyMonevment, Update() " + ex.ToString() );
        }
    }

    void ReachEnd()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        reachedEnd = true;


        if (sceneName == "EndlessLevels")
        {
            //??? mivan ha eléri a végét?? despawn !
            animator.SetBool("isAtEnd", true);
            StartCoroutine(FadeAway());
        }
        else
        {
            this.gameObject.transform.LookAt(new Vector3(-baseTower.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z));

            animator.SetBool("isAnimating", false);

           

            animator.SetBool("isAttacking", true);
        }


    }

    //Endless játékmód animáció, ha az ellenség eléri a célt
    IEnumerator FadeAway()
    {
        yield return new WaitForSecondsRealtime(enemyDizzyAnimLength);
        Destroy(this.gameObject);

        InfiniteLevelController controller = FindObjectOfType<InfiniteLevelController>();
        controller.TickOneHP();
    }

    void Turning()
    {
        Vector3 pos = transform.position;

        if (pos + new Vector3(10, 0, 10) == toGotoNext && pos + new Vector3(10, 0, 0) == toGoTo.position)
        {
            StartCoroutine(TurnLeft());
        }
        else if (pos + new Vector3(10, 0, 10) == toGotoNext && pos + new Vector3(0, 0, 10) == toGoTo.position)
        {
            StartCoroutine(TurnRight());

        }
        //LEFT
        else if (pos + new Vector3(-10, 0, +10) == toGotoNext && pos + new Vector3(0, 0, 10) == toGoTo.position)
        {
            StartCoroutine(TurnLeft());
            
        }

        //LEFT

        else if (pos + new Vector3(10, 0, -10) == toGotoNext && pos + new Vector3(10, 0, 0) == toGoTo.position)
        {
            StartCoroutine(TurnRight());

        }
        else if (pos + new Vector3(10, 0, -10) == toGotoNext && pos + new Vector3(0, 0, -10) == toGoTo.position)
        {
            StartCoroutine(TurnLeft());
           
        }
        else if (pos + new Vector3(-10, 0, -10) == toGotoNext && pos + new Vector3(0, 0, -10) == toGoTo.position)
        {
            StartCoroutine(TurnRight());
        }
        else if (pos + new Vector3(-10, 0, 10) == toGotoNext && pos + new Vector3(-10, 0, 0) == toGoTo.position)
        {
            StartCoroutine(TurnRight());
        }
        else if (pos + new Vector3(-10, 0, -10) == toGotoNext && pos + new Vector3(-10, 0, 0) == toGoTo.position)
        {
            StartCoroutine(TurnLeft());
         
        }


    }

    IEnumerator FollowPath(List<Waypoint> wppath)
    {

        //wait hogy osszekapja magat
        yield return new WaitForSeconds(0f);

        for (int i = 0; i < wppath.Count; i++)
        {

            toGoTo = wppath[i].transform; //sets where the enemy should be going
            if (i < wppath.Count - 1)
            {
                toGotoNext = wppath[i + 1].transform.position;
            }


            yield return new WaitUntil(() => isAtPosition(wppath[i]) == true); //waits until isatposition = true
        }
    }

    IEnumerator TurnLeft()
    {

        yield return new WaitUntil(() => isAtPosition(toGoTo.position) == true);
        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y - 90, rot.z);

        transform.rotation = Quaternion.Euler(rot);
    }

    IEnumerator TurnRight()
    {

        yield return new WaitUntil(() => isAtPosition(toGoTo.position) == true);
        //right
        //print("Right turn");
        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 90, rot.z);
        transform.rotation = Quaternion.Euler(rot);
    }


    bool isAtPosition(Waypoint wp)
    {
        if (wp == null)
        {
            return true;
        }

        if (this.transform.position == wp.transform.position)
        {

            return true;
        }
        else
        {

            return false;
        }

    }

    bool isAtPosition(Vector3 pos)
    {
        if (this.transform.position == pos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
