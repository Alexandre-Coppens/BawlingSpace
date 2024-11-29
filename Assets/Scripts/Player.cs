using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] private Vector3 collisionSize;
    [SerializeField] private float radius;

    [Header("Spaceship")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private GameObject bulletPREFAB;
    private Vector2 oldMousePos = Vector2.zero;

    [Header("Asteroids")]
    [SerializeField] private GameObject asteroidFolder;
    [SerializeField] private float asteroidsRadius;
    Transform[] asteroids;

    [Header ("Debug")]
    [SerializeField] Vector2 debugMousePos;
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameSystem.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameSystem == null) { gameSystem = GameSystem.instance; }
        CheckAsteroids();
        Mouse();
        Inputs();
        transform.position += velocity * Time.deltaTime;
        CheckViewportBoundaries();
    }

    private void Mouse()
    {
        Vector2 worldPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePos = Camera.main.WorldToViewportPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if(mousePos != oldMousePos)
        {
            float opposite = (worldPos.y - mousePos.y);
            float adjacent = (worldPos.x - mousePos.x);
            float alpha = Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, alpha + 90);
            oldMousePos = mousePos;
        }
    }

    private void Inputs()
    {
        if (Input.GetKey(KeyCode.W)) { velocity += transform.up * speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.S)) { velocity = Vector3.MoveTowards(velocity, Vector3.zero, Time.deltaTime * speed); }
        //if (Input.GetKey(KeyCode.A)) { velocity -= transform.right * speed * Time.deltaTime; }
        //if (Input.GetKey(KeyCode.D)) { velocity += transform.right * speed * Time.deltaTime; }
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot) { StartCoroutine(Shoot()); }
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }

    private void CheckViewportBoundaries()
    {
        Camera cam = Camera.main;

        Vector3 pos = cam.WorldToViewportPoint(transform.position);

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        if (pos.x < 0.0) { transform.position += new Vector3(width, 0); }
        if (1.0 < pos.x) { transform.position -= new Vector3(width, 0); }
        if (pos.y < 0.0) { transform.position += new Vector3(0, height); }
        if (1.0 < pos.y) { transform.position -= new Vector3(0, height); }
    }

    private void CheckAsteroids()
    {
        asteroids = asteroidFolder.GetComponentsInChildren<Transform>();
        if (asteroids.Length <= 1 && finished == false) 
        {
            finished = true;
            gameSystem.WonGame();
        }
        foreach (Transform asteroid in asteroids)
        {
            Asteroids astScript = asteroid.GetComponent<Asteroids>();
            if (astScript != null)
            {
                if (Vector3.Distance(asteroid.position, transform.position) < astScript.radius + radius)
                {
                    gameSystem.GameOver();
                }
            }
            Debug.Log(asteroids.Length);
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        GameObject bullet = Instantiate(bulletPREFAB, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().asteroidFolder = asteroidFolder;
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, collisionSize);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
