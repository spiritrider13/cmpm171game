using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleToPlayer : MonoBehaviour
{   

    public Transform player;
    private Vector3 targetPos;
    private Vector3 targetDir;

    private SpriteRenderer sprite_renderer;
    private Animator sprite_animator;

    public float angle;
    // Start is called before the first frame update

    public int last_index;

    void Start()
    {
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite_animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {   

        targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        targetDir = targetPos - transform.position;

        angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        last_index = getIndex(angle);

        sprite_animator.SetFloat("facing", last_index);
        
    }

    private int getIndex(float angle){

        if(angle < 45.0f && angle > -45.0f){
            //forward
            return 0;
        }
        if(angle >= 45.0f && angle < 135.0f){
            //right
            return 1;
        }
        if(angle <= -45.0f && angle > -135.0f){
            //left
            return 2;
        }
        //back
        return 3;
        
    }
}
