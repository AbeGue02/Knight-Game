using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{

    public float health = 100;
    public float speed = 2f;
    private GameObject target; // Sets the current target to the nearest player character
    private Rigidbody2D body2d; //Rigidbody component

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player"); //Assigns nearest player as target
        body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target.transform.position.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        } else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

            body2d.velocity = new Vector2(
            ((target.transform.position.x - transform.position.x) * speed / Time.deltaTime) / 100,
            (((target.transform.position.y - transform.position.y) * speed / Time.deltaTime) / 100) + 0.4f
        );
    }
}
