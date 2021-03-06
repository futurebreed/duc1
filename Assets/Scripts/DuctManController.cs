﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[Serializable]
public enum PlayerRunwayPosition
{
    Left = 0,
    Center,
    Right
}

[Serializable]
public class StatTracker
{
    public int cracksMissed;
    public int cracksFilled;
    public int ventsHit;
    public int ductTapeCollected;

    public void StackTracker()
    {
        cracksMissed = 0;
        cracksFilled = 0;
        ventsHit = 0;
        ductTapeCollected = 0;
    }

    public void StackTracker(int missed, int filled, int vents, int fans, int tape)
    {
        cracksMissed = missed;
        cracksFilled = filled;
        ventsHit = vents;
        ductTapeCollected = tape;
    }
}

public class DuctManController : MonoBehaviour
{
    public static DuctManController instance;
    public static AudioSource audioSourceRef;

    public float ForwardVelocity = 1f;
    public float CameraDistance = 3f;
    public Camera MainCameraReference;
    public int PlayerPosition = 0;
    public Transform DuctTapeOuterTransform;
    public float TapeRotationalVelocityZ = 1f;
    public float changeLaneDelay = 2.0f;
    public float changeLaneSpeed = 0.1f;
    public Animator animator;
    public StatTracker stats = new StatTracker();
    public AudioSource audioSource;

    private Transform cameraTransform = null; 
    private Transform playerTransform = null;
    private float playerPositionX = 0f;
    private float playerPositionY = 0f;
    private Vector3 playerRotation = Vector3.zero;
    private int lastPosition = 0;
    private float timeSinceLaneChange = 2f;
    [SerializeField]
    private LayTape layTape;
    public bool isTaping = false;
    public bool isDashing = false;

    private readonly Vector3 downRotation = Vector3.zero;
    private readonly Vector3 leftRotation = new Vector3(0, 0, -90);
    private readonly Vector3 upRotation = new Vector3(0, 0, -180);
    private readonly Vector3 rightRotation = new Vector3(0, 0, 90);

    // Start is called before the first frame update
    void Start()
    {
        audioSourceRef = audioSource;
        instance = this;
        animator = GetComponent<Animator>();

        cameraTransform = MainCameraReference.transform;
        playerTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 60;

        timeSinceLaneChange += Time.deltaTime;

        switch (PlayerPosition)
        {
            case 0:
                playerPositionX = 0f;
                playerPositionY = 0f;
                playerRotation = downRotation;
                break;
            case 1:
                playerPositionX = -2f;
                playerPositionY = 0f;
                playerRotation = downRotation;
                break;
            case 2:
                playerPositionX = -4f;
                playerPositionY = 0f;
                playerRotation = downRotation;
                break;
            case 3:
                playerPositionX = -6f;
                playerPositionY = 0f;
                playerRotation = downRotation;
                break;
            case 4:
                playerPositionX = -7.55f;
                playerPositionY = 1f;
                playerRotation = leftRotation;
                break;
            case 5:
                playerPositionX = -7.55f;
                playerPositionY = 3f;
                playerRotation = leftRotation;
                break;
            case 6:
                playerPositionX = -7.55f;
                playerPositionY = 4.5f;
                playerRotation = leftRotation;
                break;
            case 7:
                playerPositionX = -7.55f;
                playerPositionY = 6.25f;
                playerRotation = leftRotation;
                break;
            case 8:
                playerPositionX = -7.55f;
                playerPositionY = 7f;
                playerRotation = leftRotation;
                break;
            case 9:
                playerPositionX = -4.5f;
                playerPositionY = 9f;
                playerRotation = upRotation;
                break;
            case 10:
                playerPositionX = -2.25f;
                playerPositionY = 9f;
                playerRotation = upRotation;
                break;
            case 11:
                playerPositionX = 0f;
                playerPositionY = 9f;
                playerRotation = upRotation;
                break;
            case 12:
                playerPositionX = 2.25f;
                playerPositionY = 9f;
                playerRotation = upRotation;
                break;
            case 13:
                playerPositionX = 4.5f;
                playerPositionY = 9f;
                playerRotation = upRotation;
                break;
            case 14:
                playerPositionX = 6.25f;
                playerPositionY = 9f;
                playerRotation = upRotation;
                break;
            case 15:
                playerPositionX = 7.5f;
                playerPositionY = 7.5f;
                playerRotation = rightRotation;
                break;
            case 16:
                playerPositionX = 7.5f;
                playerPositionY = 6.5f;
                playerRotation = rightRotation;
                break;
            case 17:
                playerPositionX = 7.5f;
                playerPositionY = 4.5f;
                playerRotation = rightRotation;
                break;
            case 18:
                playerPositionX = 7.5f;
                playerPositionY = 1.25f;
                playerRotation = rightRotation;
                break;
            case 19:
                playerPositionX = 6.25f;
                playerPositionY = 0f;
                playerRotation = downRotation;
                break;
            case 20:
                playerPositionX = 4.75f;
                playerPositionY = 0f;
                playerRotation = downRotation;
                break;
            case 21:
                playerPositionX = 2.5f;
                playerPositionY = 0f;
                playerRotation = downRotation;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(playerPositionX, playerPositionY, -6.066026f), changeLaneSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(playerRotation), changeLaneSpeed);
        //Quaternion.Euler(playerRotation)

        animator.SetBool("Shitting", isTaping);

        if(isTaping)
        {
            LengthOfTape.rollFillCurrent -= 0.5f;
        }

        //if (LengthOfTape.rollFillCurrent <= 0)
        //    SceneManager.LoadScene(sceneToLoadOnDeath);

        #region Old system
        //if (lastPosition != PlayerPosition)
        //{
        //    lastPosition = PlayerPosition;
        //    //switch (PlayerPosition)
        //    //{
        //    //    case PlayerRunwayPosition.Left:
        //    //        playerPositionX = -2.25f;
        //    //        break;
        //    //    case PlayerRunwayPosition.Center:
        //    //        playerPositionX = 0f;
        //    //        break;
        //    //    case PlayerRunwayPosition.Right:
        //    //        playerPositionX = 2.25f;
        //    //        break;
        //    //}
        //}

        //if (playerTransform.position.x != playerPositionX)
        //{
        //    playerTransform.position = new Vector3(playerPositionX, playerTransform.position.y, playerTransform.position.z);
        //}

        //// update player & camera translation
        //playerTransform.position += new Vector3(0.0f, 0.0f, ForwardVelocity);
        //cameraTransform.position = new Vector3(playerPositionX, cameraTransform.position.y, playerTransform.position.z - CameraDistance);

        ////todo: this could be a function of player movement
        //DuctTapeOuterTransform.Rotate(new Vector3(0f, 0f, -TapeRotationalVelocityZ));

        #endregion
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var directionalMovement = context.ReadValue<Vector2>();
            var movementX = directionalMovement.x;

            if (timeSinceLaneChange >= changeLaneDelay)
            {
                if (movementX > 0)
                {
                    changeLaneDelay = 0f;
                    PlayerPosition--;
                    animator.SetTrigger("MoveLeft");
                }
                else if (movementX < 0)
                {
                    changeLaneDelay = 0f;
                    PlayerPosition++;
                    animator.SetTrigger("MoveRight");
                }
            }

            if (PlayerPosition > 21)
            {
                PlayerPosition = 0;
            }
            else if (PlayerPosition < 0)
            {
                PlayerPosition = 21;
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && !GameManager.instance.gameOver)
        {
            layTape.EnableTape();
            isTaping = true;
        }
        if (context.canceled)
        {
            layTape.DisableTape();
            isTaping = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !GameManager.instance.gameOver)
        {
            isDashing = true;
        }
        if (context.canceled)
        {
            isDashing = false;
        }
    }

    public void OnQuitGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Crack" && isTaping)
        //{
            //GameManager.instance.AddPoints(100);
            //AudioManager.instance.PlaySound("FillCrack");

            //Animator anim = other.gameObject.GetComponent<Animator>();

            //if(anim)
            //{
            //    anim.SetTrigger("Tape");
            //}
        //}
        //else 
        if (other.tag == "Damagable")
        {
            stats.ventsHit++;

            LengthOfTape.rollFillCurrent -= 25f;
            AudioManager.instance.PlaySound("TakeDamage");
            GameManager.instance.MakeAnnouncement(Color.red, "Lost tape!");
            GameManager.instance.FlashScreen(Color.red);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Crack" && isTaping)
        {
            other.tag = "Untagged";

            GameManager.instance.AddPoints(100);
            AudioManager.instance.PlaySound("FillCrack");
            GameManager.instance.MakeAnnouncement(Color.yellow, "Nice patchwork!");
            GameManager.instance.FlashScreen(Color.yellow);

            Animator anim = other.gameObject.GetComponent<Animator>();

            stats.cracksFilled++;

            if (anim)
            {
                anim.SetTrigger("Tape");
            }
        }
    }
}