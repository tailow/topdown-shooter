using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerScript : MonoBehaviour
{

    #region Variables

    [Header("Speed and Time Between Shots")]
    public float speed = 7f;
    public float fireSpeed;

    [Space(10)]

    public static float health = 3;

    public GameObject playerModel;
    public GameObject barrel;

    public GameObject bulletWallHit;
    public GameObject bulletEnemyHit;

    public LayerMask floorLayerMask;

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject deathScreen;
    public GameObject UICanvas;

    GameObject fadeManager;

    public AudioSource shotAudio;

    bool isDead = false;

    LineRenderer shot;
    Light shotLight;

    GameManagerScript gameManager;

    public PostProcessingProfile ppProfile;

    #endregion

    void Start()
    {
        fadeManager = GameObject.Find("FadeManager");

        shot = barrel.GetComponent<LineRenderer>();
        shot.enabled = false;

        shotLight = GameObject.Find("ShotLight").GetComponent<Light>();
        shotLight.enabled = false;

        health = 3;

        isDead = false;
    }

    void FixedUpdate()
    {
        // MOVEMENT
        #region Movement

        if (isDead)
            return;

        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 normalized = dir.normalized;
        Vector3 velocity = normalized * speed / 70;

        GetComponent<Rigidbody>().MovePosition(transform.position + velocity);
        #endregion
    }

    void Update()
    {
        //ROTATION
        #region Rotation

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayerMask) && !pauseMenu.activeInHierarchy && !GameManagerScript.controllerConnected && !isDead && !settingsMenu.activeInHierarchy)
        {
            Vector3 lookDir = new Vector3(hit.point.x, hit.point.y, hit.point.z); ;
            lookDir.y = 1.87f;

            playerModel.transform.LookAt(lookDir);

            playerModel.transform.eulerAngles = new Vector3(0, playerModel.transform.eulerAngles.y, 0);

            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        #endregion 

        // SHOOTING
        #region Shooting

        if (Input.GetButtonDown("Fire1") && !pauseMenu.activeInHierarchy && !settingsMenu.activeInHierarchy && !GameManagerScript.controllerConnected && !isDead)
        {
            StartCoroutine("ShotCoroutine");
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine("ShotCoroutine");

            shot.enabled = false;
            shotLight.enabled = false;
        }
        #endregion
    }

    #region Shooting2

    IEnumerator ShotCoroutine()
    {
        for (int i = 0; i < 100000; i++)
        {
            Vector3 forward = barrel.transform.TransformDirection(Vector3.forward);

            RaycastHit hit;
            shot.SetPosition(0, barrel.transform.position);

            if (Physics.Raycast(barrel.transform.position, forward, out hit, 100))
            {
                bulletHit(hit.point, hit.distance, hit.collider);
            }

            else
            {
                shot.SetPosition(0, barrel.transform.position);
                shot.SetPosition(1, barrel.transform.TransformPoint(Vector3.forward * 100));
            }

            shot.enabled = true;
            shotLight.enabled = true;
            shotAudio.Play();

            yield return new WaitForSeconds(0.02f);

            shot.enabled = false;
            shotLight.enabled = false;

            yield return new WaitForSeconds(fireSpeed);
        }
    }

    void bulletHit(Vector3 hitPoint, float distance, Collider coll)
    {
        // SET RAY LENGTH
        shot.SetPosition(0, barrel.transform.position);
        shot.SetPosition(1, hitPoint);

        // CHECK HIT OBJECT
        if (coll.tag == "Enemy")
        {
            coll.gameObject.SendMessage("LoseHealth");

            var enemyHit = Instantiate(bulletEnemyHit, hitPoint, Quaternion.identity);

            enemyHit.transform.parent = GameObject.Find("Particles").transform;
        }

        else
        {
            var wallHit = Instantiate(bulletWallHit, hitPoint, Quaternion.identity);

            wallHit.transform.parent = GameObject.Find("Particles").transform;
        }
    }
    #endregion

    // COLLISION
    #region Collision

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Enemy" && !isDead)
        {
            LoseHealth();
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Healthpack")
        {
            GainHealth();

            Destroy(coll.gameObject);
        }
    }

    void LoseHealth()
    {
        health--;
        GetComponent<AudioSource>().Play();

        if (health == 0)
        {
            StartCoroutine("DeathCoroutine");
        }
    }

    void GainHealth()
    {
        if (health < 3)
        {
            health++;
        }

        // Healthpack sound
    }

    #endregion

    IEnumerator DeathCoroutine()
    {
        float fadeTime = 2f;
        float t = 0;

        isDead = true;

        while (true)
        {
            if (t < 1f)
            {
                Time.timeScale = Mathf.Lerp(1f, 0.5f, t);
                t += Time.deltaTime / fadeTime;
            }

            else
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        GameObject.Find("GameManager").SendMessage("SetTexts");

        ppProfile.depthOfField.enabled = true;
        deathScreen.SetActive(true);
        UICanvas.SetActive(false);

        Time.timeScale = 0;
    }
}
