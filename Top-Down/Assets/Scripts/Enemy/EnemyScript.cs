using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {

    #region Variables

    public float speed = 5f;
    float health = 3f;

    GameObject player;
    NavMeshAgent enemyAgent;

	#endregion

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyAgent = GetComponent<NavMeshAgent>();
	}
	
	void Update ()
    {
        // MOVEMENT
        #region Movement

        enemyAgent.SetDestination(player.transform.position);

        #endregion
    }

    // COLLISION CHECK
    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Player")
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

        if(health <= 0)
        {
            Destroy(gameObject);

            GameManagerScript.zombiesKilled++;

            GameManagerScript.zombieWaveCount--;
        }
    }
}
