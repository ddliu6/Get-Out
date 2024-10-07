using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    public Animator animator;
    public GameObject key, door;
    public AudioClip collectItem, openDoor, gotHit;
    private AudioSource source;

    float horizontalMove = 0f;
    bool jump = false, crouch = false;
    static List<bool> gotKey = new List<bool>() { false, false, false, false };
    static int keyCount = 0;
    static bool isGoal = false;

    public float moveSpeed = 40f;
    Transform leftPoint, rightPoint;
    Vector3 localScale;
    bool moveR = true;
    Rigidbody2D rb;

    //player hp
    public int maxtHP = 10;
    private int currentHP;
    public HealthBar healthBar;
    float timer = 0, check;

    int nextScene = 0;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        /*if(isGoal && (SceneManager.GetActiveScene().buildIndex == 3))
            Instantiate(door, new Vector3(7.13f, 3.78f, 0), Quaternion.identity);*/

        if (!gotKey[SceneManager.GetActiveScene().buildIndex - 1])
        {
            DontDestroyOnLoad(key);
        }
        else
        {
            Destroy(key);
        }
    }
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        leftPoint = GameObject.Find("LeftPoint").GetComponent<Transform>();
        rightPoint = GameObject.Find("RightPoint").GetComponent<Transform>();

        //hp
        currentHP = maxtHP;
        healthBar.SetHealth(maxtHP);

        check = timer;
    }

    void Update()
    {
        //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(moveSpeed));
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (keyCount == 3 && (SceneManager.GetActiveScene().buildIndex == 3) && !isGoal)
        {
            Instantiate(door, new Vector3(7.13f, 3.78f, 0), Quaternion.identity);
            isGoal = true;
        }

        if (Input.GetButtonDown("Crouch"))
            crouch = true;
        else if(Input.GetButtonUp("Crouch"))
            crouch = false;

        timer += Time.deltaTime;
        
        if (Mathf.Round(timer) - check == 2)
        {
            check = Mathf.Round(timer);
            TakeDamage(1);
        }

        if(currentHP == 0)
        {
            Invoke("Restart", 1f);
            animator.enabled = false;
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            enabled = false;

            Enemy e1 = GameObject.Find("Enemy 1").GetComponent<Enemy>();
            Enemy e2 = GameObject.Find("Enemy 2").GetComponent<Enemy>();
            e1.Freeze();
            e2.Freeze();
        }
    }

    void FixedUpdate()
    {
        if (transform.position.x > rightPoint.position.x)
        {
            healthBar.Flip();
            moveR = false;
        }
        if (transform.position.x < leftPoint.position.x)
        {
            healthBar.Flip();
            moveR = true;
        }
        if (moveR)
            moveRight();
        else
            moveLeft();

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void Restart()
    {
        //reset data
        gotKey[2] = false;
        isGoal = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void addHp()
    {
        if (currentHP < maxtHP)
            currentHP ++;
        else
            currentHP = maxtHP;
        healthBar.SetHealth(currentHP);
    }

    public void TakeDamage(int damage)
    {
        if(currentHP > 0)
            currentHP -= damage;
        
        healthBar.SetHealth(currentHP);
    }

    void moveRight()
    {
        moveR = true;
        localScale.x = 1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }
    void moveLeft()
    {
        moveR = false;
        localScale.x = -1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "Key")
        {
            Debug.Log("Got Key!");
            gotKey[SceneManager.GetActiveScene().buildIndex-1] = true;
            source.PlayOneShot(collectItem);
            Destroy(key);
        }
        else if (collision.collider.name == "Goal" && gotKey[2])
        {
            Debug.Log("Goal!");
            animator.enabled = false;
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            enabled = false;

            Enemy e1 = GameObject.Find("Enemy 1").GetComponent<Enemy>();
            Enemy e2 = GameObject.Find("Enemy 2").GetComponent<Enemy>();
            e1.Freeze();
            e2.Freeze();

            nextScene = 5;
            source.PlayOneShot(openDoor);

            //reset data
            gotKey[2] = false;
            isGoal = false;
            StartCoroutine(LoadLevelAfterDelay(2));
        }
        else if (collision.collider.tag =="Enemy")
        {
            TakeDamage(1);
            source.PlayOneShot(gotHit);
        }
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene);
    }

    public void loadScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void loadScene2()
    {
        SceneManager.LoadScene(2);
    }
    public void loadScene3()
    {
        SceneManager.LoadScene(3);
    }
}
