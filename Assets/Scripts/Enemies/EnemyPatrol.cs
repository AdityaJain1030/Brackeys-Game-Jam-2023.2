using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    
    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement paramteres")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;
    // Start is called before the first frame update
    
    void Awake() {
        initScale = enemy.localScale;
    }

    private void OnDisable() 
    {
        anim.SetBool("moving", false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft) 
        {
            if (enemy.position.x >= leftEdge.position.x)
                moveInDirection(-1);
            else
            {
                // change direction
                directionChange();
            }
        } else 
        {
            if (enemy.position.x <= rightEdge.position.x) 
                moveInDirection(1);
            else 
            {
                directionChange();
            }
        }
    }
    private void moveInDirection(int _direction) {
        idleTimer = 0;
        anim.SetBool("moving", true);
        // Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        // Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }

    private void directionChange() 
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }
}
