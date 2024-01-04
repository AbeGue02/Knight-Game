using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{

    public float health = 100;
    public float speed = 2f;
    public float attackInterval = 1; //Seconds between each attack
    private GameObject target; // Sets the current target to the nearest player character
    private Rigidbody2D body2d; //Rigidbody component
    private Animator animator;

    private float attackCooldownTimer = 1;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player"); //Assigns nearest player as target
        body2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetTrigger("Flight");

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (attackCooldownTimer <= 0)
            {
                animator.SetTrigger("Attack1");
                attackCooldownTimer = attackInterval;
            }
            else
            {
                attackCooldownTimer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            attackCooldownTimer = attackInterval;
        }
    }
}
