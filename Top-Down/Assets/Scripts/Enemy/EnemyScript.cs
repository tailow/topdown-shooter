using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    #region Variables

    public float minSpeed = 3f;
    public float maxSpeed = 6f;

    float speed;
    public float health = 3f;

    GameObject player;
    NavMeshAgent enemyAgent;

    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyAgent = GetComponent<NavMeshAgent>();

        speed = Random.Range(minSpeed, maxSpeed);
        enemyAgent.speed = speed;
    }

    void Update()
    {
        // MOVEMENT
        #region Movement

        enemyAgent.SetDestination(player.transform.position);

        #endregion
    }

    // COLLISION CHECK
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            StartCoroutine("EnemyHitCoroutine");
        }
    }

    // ENEMY STOPPING AFTER HIT
    IEnumerator EnemyHitCoroutine()
    {
        enemyAgent.isStopped = true;

        yield return new WaitForSeconds(1f);

        enemyAgent.isStopped = false;
    }

    // LOSING HEALTH
    void LoseHealth()
    {
        health--;

        if (health <= 0)
        {
            Destroy(gameObject);

            GameManagerScript.zombiesKilled++;

            GameManagerScript.zombieWaveCount--;
        }
    }
}
