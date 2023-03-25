// A script that animates a sprite renderer
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    // The sprite renderer component of the object
    private SpriteRenderer spriteRenderer;
    // The time it takes for each animation frame to change
    public float animationTime = 0.25f;

    // The current animation frame
    private int animationFrame;

    // The sprite to use when the object is idle
    public Sprite idleSprites;

    // The sprites to use for animation
    public Sprite[] animationSprites;

    // Whether to loop the animation
    public bool loop = true;

    // Whether the object is idle
    public bool idle = true;

    // Whether the animation is currently playing
    public bool animating { get; set; }

    // Whether to flip the sprite renderer's X-axis
    public bool flipX { get; set; }

    // Initialize the sprite renderer component
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Enable the sprite renderer when the object is enabled
    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    // Disable the sprite renderer when the object is disabled
    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    // Start the animation loop
    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    // Advance to the next animation frame
    private void NextFrame()
    {
        // Increment the animation frame
        animationFrame++;

        // Loop the animation if necessary
        if (loop && animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        // Set the sprite to the idle sprite if the object is idle
        if (idle)
        {
            spriteRenderer.sprite = idleSprites;
        }
        // Set the sprite to the current animation frame if there are animation sprites
        else if (animationFrame >= 0 && animationFrame < animationSprites.Length)
        {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }


        // Flip the sprite renderer's X-axis if necessary
        spriteRenderer.flipX = flipX;
    }

    // Set whether the animation is playing
    public void SetAnimating(bool value)
    {
        animating = value;
    }

    // Stop the animation
    public void Stop()
    {
        animating = false;
    }

}