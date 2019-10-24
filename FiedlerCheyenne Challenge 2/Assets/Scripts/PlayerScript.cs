using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    private int scoreCount = 0;
    private int lives = 3;

    public float speed;

    public Text score;
    public Text winText;
    public Text livesText;
    public Text credits;

    public AudioSource musicSource;
    public AudioClip winMusic;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim =GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreCount.ToString();
        livesText.text = "Lives: " + lives.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
    }

    //Animations
    //0 = idle
    //1 = run
    //2 = jump
    void Update()
    {
        //D Key
        if(Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D)){
            anim.SetInteger("State", 0);
        }

        //W Key
        //Faces opposite direction when idling?
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
            Flip();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
            Flip();
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }

        
    }

    void Flip()
    {
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Regular Coins
        if (collision.collider.tag == "Coin")
        {
            scoreCount += 1;
            score.text = "Score: " + scoreCount.ToString();
            Destroy(collision.collider.gameObject);

            //Stage 2 Transform
            //For future reference, if this keeps executing and snapping object to this location, try nesting it in a different command
            if (scoreCount == 4)
            {
                transform.position = new Vector2(147.38f, 7.18f);
            }
        }

        //Reset Lives for Stage 2
        if (scoreCount == 4)
        {
            lives = 3;
            livesText.text = "Lives: " + lives.ToString();
        }

        //Enemy Coins
        if (collision.collider.tag == "Enemy")
        {
            lives -= 1;
            livesText.text = "Lives: " + lives.ToString();
            Destroy(collision.collider.gameObject);
        }
        
        //Victory and Music!
        //Destroys Enemies when Winning
        if (scoreCount == 8)
        {
            winText.text = "YOU WIN! GAME CREATED BY CHEYENNE FIEDLER.";
            credits.text = "Win SFX is Realization by Jason Dagenet. BG Music is Snowfall by Joseph Gilbert/Kistol";
            musicSource.clip = winMusic;
            musicSource.Play();
            Destroy(GameObject.FindWithTag("Enemy"));
        }

        //DISHONOR.
        if (lives < 1)
        {
            winText.text = "You lose :(";
            anim.SetInteger("State", 3);
            Destroy(GameObject.FindWithTag("Coin"));
            Destroy(this);
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                anim.SetInteger("State", 1);
            }
        }
    }

}