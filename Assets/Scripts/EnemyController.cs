using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    Rigidbody2D rb2d;
    float timer;
    int direction = 1;
    bool broken = true;
    Animator animator;
    public ParticleSystem smokeEffect;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!broken){
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0){
            direction = -direction;
            timer = changeTime;
        }
        Vector2 position = rb2d.position;

        if(vertical){
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
        } else {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + Time.deltaTime * speed * direction;
        }

        rb2d.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D other){
        RubyController player = other.gameObject.GetComponent<RubyController>();
        if(player != null){
            player.changeHealth(-1);
        }
    }
    public void Fix(){
        broken = false;
        rb2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
}
