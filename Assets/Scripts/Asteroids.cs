using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    public int currentSize = 3;
    [SerializeField] float speed = 1f;
    public float radius = 3f;
    [SerializeField] Vector3 velocity;

    [SerializeField] GameObject medium;
    [SerializeField] GameObject small;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0,0, Random.Range(0,361));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        CheckViewportBoundaries();
    }

    void CheckViewportBoundaries()
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

    public void Destroy()
    {
        if(currentSize == 3) 
        { 
            GameObject child = Instantiate(medium, transform.position, Quaternion.identity);
            child.transform.parent = transform.parent;
            child = Instantiate(medium, transform.position, Quaternion.identity);
            child.transform.parent = transform.parent;
        }
        if(currentSize == 2) 
        { 
            GameObject child = Instantiate(small, transform.position, Quaternion.identity);
            child.transform.parent = transform.parent;
            child = Instantiate(small, transform.position, Quaternion.identity);
            child.transform.parent = transform.parent;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
