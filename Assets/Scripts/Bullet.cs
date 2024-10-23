using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPos;
    float maxDist;

    [SerializeField] float radius = 0.1f;
    [SerializeField] float speed = 1.2f;
    public GameObject asteroidFolder;
    [SerializeField] Transform[] asteroids;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        maxDist = width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        if(Vector3.Distance(transform.position, startPos) >= maxDist) { Destroy(gameObject); }
        CheckAsteroids();
    }

    private void CheckAsteroids()
    {
        asteroids = asteroidFolder.GetComponentsInChildren<Transform>();
        foreach (Transform asteroid in asteroids)
        {
            Asteroids astScript = asteroid.GetComponent<Asteroids>();
            if(astScript != null) 
            {
                if (Vector3.Distance(asteroid.position, transform.position) < astScript.radius + radius)
                {
                    astScript.Destroy();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
