using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPos;
    float maxDist;

    [SerializeField] float radius = 0.1f;
    [SerializeField] float speed = 1.2f;
    [SerializeField] GameObject asteroidFolder;

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
    }

    private void CheckAsteroids()
    {
        foreach(Transform asteroid in asteroidFolder.GetComponentsInChildren<Transform>())
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}