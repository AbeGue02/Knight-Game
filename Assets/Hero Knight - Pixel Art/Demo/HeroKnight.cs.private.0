using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f; //Speed of movement
    [SerializeField] float      m_jumpForce = 7.5f; //Jump height
    [SerializeField] float      m_rollForce = 6.0f; //Distance of roll?
    [SerializeField] bool       m_noBlood = false; //No blood mode
    [SerializeField] GameObject m_slideDust; //Object instantiated when rolling

    private Animator            m_animator; //Gets animator component
    private Rigidbody2D         m_body2d; //Gets RigidBody component

    //These four are targeting children GameObjects that have hitboxes to check whether
    //The character is touching a wall or is on the ground at any given time.
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;

    private bool  m_isWallSliding = false; //Tells whether character is sliding down a wall
    private bool  m_grounded = false; //Checks if character is on the ground
    private bool  m_rolling = false; //Checks if character is rolling
    private int m_facingDirection = 1; //1 for right, -1 for left
    private int m_currentAttack = 0; //Current attack in 3-step attack sequence
    private float m_timeSinceAttack = 0.0f; //Time since last attack
    private float m_delayToIdle = 0.0f; //Decides when to start idling
    private float m_rollDuration = 1.0f; //8.0f / 14.0f; // How long the roll lasts
    private float m_rollCurrentTime; // I think this is a timer to decide how long the roll lasts
    public float attackSpeed = 0.25f;
    private bool isBlocking = false;

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        //The code does this to later check if is too late for doing the next attack in the sequence

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;
        // I think the code does this to later check how long you've been rolling

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration) {
            m_rolling = false;
            m_rollCurrentTime = 0f;
        }
        // This takes you out of the rolling animation if you go past the set time

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0) {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }  else if (inputX < 0) {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling && !isBlocking)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        // This just seems to calculate your speed as long as you are not rolling

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);
        //FIX THIS TO FIX WEIRD ANIMATION ERROR WHEN CHANGING DIRECTION ON WALL

        //Death
        // if (Input.GetKeyDown("e") && !m_rolling)
        // {
        //     m_animator.SetBool("noBlood", m_noBlood);
        //     m_animator.SetTrigger("Death");
        // }
        // This can be deactivated out of testing
            
        //Hurt
        // else if (Input.GetKeyDown("q") && !m_rolling)
        //     m_animator.SetTrigger("Hurt");
        // This can also be deactivated out of testing

        //Attack
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > attackSpeed && !m_rolling && !isBlocking)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 0.8f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }
        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling && m_grounded)
        {
            isBlocking = true;
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
            m_body2d.velocity = new Vector2(0, 0);
        }

        else if (Input.GetMouseButtonUp(1)) {
            isBlocking = false;
            m_animator.SetBool("IdleBlock", false);
        }

        // Roll
        if (Input.GetKeyDown("left shift") && m_grounded && !m_rolling && !m_isWallSliding && !isBlocking)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            float rollingVelocity = m_facingDirection * m_rollForce;
            m_body2d.velocity = new Vector2(rollingVelocity, m_body2d.velocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling && !isBlocking)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && !isBlocking && !m_rolling)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
