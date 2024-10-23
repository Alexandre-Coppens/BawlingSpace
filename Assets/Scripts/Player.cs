using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] Vector3 collisionSize;
    [SerializeField] float testDistance;

    [Header("Spaceship")]
    [SerializeField] float speed = 2f;
    [SerializeField] Vector3 velocity;
    [SerializeField] bool canShoot = true;
    [SerializeField] GameObject bulletPREFAB;

    [Header("Asteroids")]
    [SerializeField] GameObject asteroidFolder;
    [SerializeField] float asteroidsRadius;

    [Header ("Debug")]
    [SerializeField] Vector2 debugMousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mouse();
        Inputs();
        transform.position += velocity * Time.deltaTime;
        CheckViewportBoundaries();
    }

    private void Mouse()
    {
        Vector2 worldPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePos = Camera.main.WorldToViewportPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        float opposite = (worldPos.y - mousePos.y);
        float adjacent = (worldPos.x - mousePos.x);
        float alpha = Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, alpha + 90);
    }

    private void Inputs()
    {
        if (Input.GetKey(KeyCode.W)) { velocity += transform.up * speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.S)) { velocity -= transform.up * speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.A)) { velocity -= transform.right * speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.D)) { velocity += transform.right * speed * Time.deltaTime; }
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot) { StartCoroutine(Shoot()); }
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
        Gizmos.DrawWireSphere(transform.position, testDistance);
    }
}
