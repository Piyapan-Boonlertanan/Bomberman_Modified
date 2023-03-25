using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JoystickAnimateV2 : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDirectionChanged))]
    private Vector2 direction = Vector2.down;

    public float speed = 5f;

    public DynamicJoystick Joystick;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;

    private AnimatedSpriteRenderer activeSpriteRenderer;
    public AnimatedSpriteRenderer spriteRendererDeath;
    public GameOver GameOver;
    private GameObject Setting;

    private void Awake()
    {
        Joystick = GameObject.FindWithTag("Joystick").GetComponent<DynamicJoystick>();
        // Set the active sprite renderer to the spriteRendererDown variable
        activeSpriteRenderer = spriteRendererDown;
        Setting = GameObject.Find("Setting");
    }

    private void Update()
    {
        if (this.isLocalPlayer)
        {
            // Read input from the joystick
            float horizontalInput = Joystick.Horizontal;
            float verticalInput = Joystick.Vertical;

            // Update the direction based on the joystick input
            Vector2 inputDirection = new Vector2(horizontalInput, verticalInput);
            if (inputDirection.magnitude > 0)
            {
                // Check if the input direction is a diagonal direction
                if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) > 0)
                {
                    // Choose one of the two cardinal directions based on the input direction
                    if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
                    {
                        direction = horizontalInput > 0 ? Vector2.right : Vector2.left;
                    }
                    else
                    {
                        direction = verticalInput > 0 ? Vector2.up : Vector2.down;
                    }
                }
                else
                {
                    direction = inputDirection.normalized;
                }
            }
            else
            {
                direction = Vector2.zero;
            }

            UpdateDirection(direction);
        }
    }

    private void UpdateDirection(Vector2 newDirection)
    {
        direction = newDirection;
        CmdUpdateDirection(newDirection);
    }

    [Command]
    private void CmdUpdateDirection(Vector2 newDirection)
    {
        direction = newDirection;
        RpcUpdateDirection(newDirection);
    }

    [ClientRpc]
    private void RpcUpdateDirection(Vector2 newDirection)
    {
        // Set the active sprite renderer based on the current direction
        if (newDirection == Vector2.up)
        {
            SetActiveSpriteRenderer(spriteRendererUp, false);
        }
        else if (newDirection == Vector2.down)
        {
            SetActiveSpriteRenderer(spriteRendererDown, false);
        }
        else if (newDirection == Vector2.left)
        {
            SetActiveSpriteRenderer(spriteRendererLeft, false);
        }
        else if (newDirection == Vector2.right)
        {
            SetActiveSpriteRenderer(spriteRendererRight, false);
        }
        else
        {
            SetActiveSpriteRenderer(activeSpriteRenderer, false);
        }
    }

    private void SetActiveSpriteRenderer(AnimatedSpriteRenderer newSpriteRenderer, bool flipX)
    {
        if (activeSpriteRenderer != null)
        {
            activeSpriteRenderer.Stop();
            activeSpriteRenderer.enabled = false;
        }
        newSpriteRenderer.enabled = true;
        activeSpriteRenderer = newSpriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
        activeSpriteRenderer.flipX = flipX;
        activeSpriteRenderer.SetAnimating(true);
    }

    private void OnDirectionChanged(Vector2 oldDirection, Vector2 newDirection)
    {
        if (newDirection == Vector2.up)
        {
            SetActiveSpriteRenderer(spriteRendererUp, false);
        }
        else if (newDirection == Vector2.down)
        {
            SetActiveSpriteRenderer(spriteRendererDown, false);
        }
        else if (newDirection == Vector2.left)
        {
            SetActiveSpriteRenderer(spriteRendererLeft, false);
        }
        else if (newDirection == Vector2.right)
        {
            SetActiveSpriteRenderer(spriteRendererRight, false);
        }
        else
        {
            SetActiveSpriteRenderer(activeSpriteRenderer, false);
        }
    }


    private void FixedUpdate()
    {
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + translation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
    }
    [ClientRpc]
    void RpcOnDeathSequenceEnded()
    {
        OnDeathSequenceEnded();
    }

    [Command]
    void CmdOnDeathSequenceEnded()
    {
        RpcOnDeathSequenceEnded();
    }
    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombControllerUI>().enabled = false;

        if (isServer)
        {
            // If running on the server, call the Rpc method to execute the death animation on all clients
            RpcPlayDeathAnimation();
        }
        else
        {
            // If running on a client, call the Cmd method to execute the death animation on the server
            CmdPlayDeathAnimation();
        }
    }
    [Command]
    private void CmdPlayDeathAnimation()
    {
        RpcPlayDeathAnimation();
    }

    [ClientRpc]
    private void RpcPlayDeathAnimation()
    {
        // Play the death animation on all clients
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        // Disable the player object after the death animation has finished playing
        Invoke(nameof(CmdOnDeathSequenceEnded), 1.25f);
    }


}