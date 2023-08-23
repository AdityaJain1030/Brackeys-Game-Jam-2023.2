using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BaseAnimation : MonoBehaviour
{
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected Material[] materials;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    public virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
