using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        vel = vel.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0.0) { vel.x = -vel.x; GetComponent<SpriteRenderer>().color = Random.ColorHSV(); }
        if (1.0 < pos.x) { vel.x = -vel.x; GetComponent<SpriteRenderer>().color = Random.ColorHSV(); }
        if (pos.y< 0.0) { vel.y = -vel.y; GetComponent<SpriteRenderer>().color = Random.ColorHSV(); }
        if (1.0 < pos.y) { vel.y = -vel.y; GetComponent<SpriteRenderer>().color = Random.ColorHSV(); }

        transform.position += vel * speed * Time.deltaTime;
    }
}
