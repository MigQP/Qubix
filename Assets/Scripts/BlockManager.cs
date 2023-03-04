using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public AudioClip moveSound;
    public AudioClip dropSound;
    public AudioClip rotateSound;
    private AudioSource movePiece;
    public Material emissionMaterial;
    public Color emissionColor;

    float prevTime;
    float fallTime = 1f;
    public float moveSpeed = 1f;
    private Camera mainCamera;

    bool canIncrease;

    int maxDescentScore = 50;

    // Start is called before the first frame update
    private void Awake()
    {
        if (!CheckValidMove())
        {
            GameManager.instance.SetIsGameOver();
        }
    }

    void Start()
    {
        movePiece = GameObject.FindGameObjectWithTag("MovePiece").GetComponent<AudioSource>();
        mainCamera = Camera.main;
        ButtonInputs.instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.instance.ReadFallSpeed();
        if(!CheckValidMove())
        {
            GameManager.instance.SetIsGameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - prevTime > fallTime)
        {
            transform.position += Vector3.down;

            //prevTime = Time.time;

            if (!CheckValidMove())
            {
                transform.position += Vector3.up;
                // Delete layer if possible
                Playfield.instance.DeleteLayer();
                enabled = false;
                // Create New Block
                if (!GameManager.instance.ReadIsGameOver())
                {

                    Playfield.instance.SpawnNewBlock();
                }
                
            }
            else
            {
                // Update the grid 
                Playfield.instance.UpdateGrid(this);
                if (canIncrease)
                {    
                    GameManager.instance.SetScore(1);
                }

                maxDescentScore = maxDescentScore - 5;
            }

            prevTime = Time.time;
        }

        // Keyboard Input

        if (GameManager.instance.ReadIsGameOver())
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraUp = mainCamera.transform.up;
        Vector3 cameraRight = mainCamera.transform.right;

        // Calculate the left vector by taking the cross product of the camera's forward and up vectors
        //Vector3 cameraLeft = Vector3.Cross(cameraUp, cameraForward).normalized;
        Vector3 projectedForward = Vector3.ProjectOnPlane(cameraForward, Vector3.up).normalized;
        Vector3 cameraLeft = Vector3.Cross(cameraUp, projectedForward).normalized;


        // Movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            //SetInput(Vector3.left);
            // Calculate the movement direction vector by multiplying the camera's left vector by the movement input value
            Vector3 moveDirection = cameraLeft * horizontal + cameraUp * vertical;

            moveDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up);

            moveDirection = new Vector3(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y), Mathf.Round(moveDirection.z));

            SetInputRelativeToCamera(moveDirection);
            movePiece.PlayOneShot(moveSound);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //SetInput(Vector3.right);
            Vector3 moveDirection = cameraLeft * horizontal + projectedForward * vertical;

            // Clamp the movement direction vector to be either purely horizontal or purely vertical
            Vector3 cameraForwardPlane = Vector3.Cross(cameraLeft, Vector3.up).normalized;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, cameraForwardPlane);
            moveDirection.y = 0f;
            moveDirection = new Vector3(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y), Mathf.Round(moveDirection.z));

            SetInputRelativeToCamera(moveDirection);
            movePiece.PlayOneShot(moveSound);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //SetInput(Vector3.forward);

            Vector3 moveDirection = cameraForward * horizontal + projectedForward * vertical;

            // Clamp the movement direction vector to be either purely horizontal or purely vertical
            Vector3 cameraForwardPlane = Vector3.Cross(cameraForward, Vector3.up).normalized;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, cameraForwardPlane);
            //moveDirection.y = 0f;
            moveDirection = new Vector3(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y), Mathf.Round(moveDirection.z));

            SetInputRelativeToCamera(moveDirection);
            movePiece.PlayOneShot(moveSound);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //SetInput(Vector3.back);
            Vector3 moveDirection = cameraForward * horizontal + projectedForward * vertical;

            // Clamp the movement direction vector to be either purely horizontal or purely vertical
            Vector3 cameraForwardPlane = Vector3.Cross(cameraForward, Vector3.up).normalized;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, cameraForwardPlane);
            //moveDirection.y = 0f;
            moveDirection = new Vector3(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y), Mathf.Round(moveDirection.z));

            SetInputRelativeToCamera(moveDirection);
            movePiece.PlayOneShot(moveSound);
        }

        if (Input.GetKey(KeyCode.R))
        {
            fallTime = fallTime * .99f;
        }

        if (Input.GetKeyDown(KeyCode.R) && !GameManager.instance.ReadIsGameOver())
        {
            canIncrease = true;

        }
        if (Input.GetKeyUp(KeyCode.R) || GameManager.instance.ReadIsGameOver())
        {
            canIncrease = false;
            fallTime = GameManager.instance.ReadFallSpeed();
        }

        if (Input.GetKeyUp(KeyCode.R) && Input.GetKeyUp(KeyCode.Space))
        {
            fallTime = GameManager.instance.ReadFallSpeed();
        }

        // Rotation

        /*

        if (Input.GetKeyDown(KeyCode.Z))
        {

            SetRotationInput(new Vector3(0, 90, 0));
            movePiece.PlayOneShot(rotateSound);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {

            //transform.rotation *= Quaternion.AngleAxis(90f, new Vector3(cameraUp.x, 0, 0));
            //SetRotationInput(new Vector3(-90, 0, 0));
            //transform.Rotate(cameraUp, rotationSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.forward, 90f);
            SetRotationInput(new Vector3(90, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {

            SetRotationInput(new Vector3(0, 0, 90));

        }

        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetRotationInput(new Vector3(90,0,0));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetRotationInput(new Vector3(-90, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetRotationInput(new Vector3(0, 0, 90));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetRotationInput(new Vector3(0, 0, -90));
        }

        */
        

        // Land

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetSpeed();
            //LandImmediately();
            //GameManager.instance.SetScore(50);
            GameManager.instance.SetScore(maxDescentScore);
            movePiece.PlayOneShot(dropSound);
        }
    }

    public void SetInputRelativeToCamera(Vector3 direction)
    {
        transform.position += direction;
        if (!CheckValidMove())
        {
            transform.position -= direction;
        }

        else
        {
            Playfield.instance.UpdateGrid(this);
        }
    }

    public void SetInput(Vector3 direction)
    {
        transform.position += direction;
        if (!CheckValidMove())
        {
            transform.position -= direction;
        }

        else
        {
            Playfield.instance.UpdateGrid(this);
        }
    }

    public void SetRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);
        if(!CheckValidMove())
        {
            transform.Rotate(-rotation, Space.World);
        }
        else
        {
            Playfield.instance.UpdateGrid(this);
        }
    }

    bool CheckValidMove()
    {
        foreach(Transform child in transform)
        {
            Vector3 pos = Playfield.instance.Round(child.position);
            if (!Playfield.instance.CheckInsideGrid(pos))
            {
                return false;
            }
        }

        foreach(Transform child in transform)
        {
            Vector3 pos = Playfield.instance.Round(child.position);
            Transform t = Playfield.instance.GetTransformOnGridPos(pos);
            if(t != null && t.parent != transform)
            {
                return false;
            }
        }
        return true;
    }


    public void SetSpeed()
    {
        fallTime = 0.005f;
    }

    public void SetSpeed1()
    {
        fallTime = GameManager.instance.ReadFallSpeed() * .95f;
    }

    public void LandImmediately()
    {
        // Move the piece down until it hits the bottom or another piece
        while (CheckValidMove())
        {
            transform.position += Vector3.down;
        

        }

        transform.position += Vector3.up; // Move the piece back up one unit

        // Delete layer if possible
        //Playfield.instance.DeleteLayer();
        //Playfield.instance.UpdateGrid(this);
        //enabled = false;
        
        
        // Create new block
        if (!GameManager.instance.ReadIsGameOver())
        {
            //Playfield.instance.UpdateGrid(this);
            //Playfield.instance.DeleteLayer();
            //Playfield.instance.UpdateGrid(this);
            //Playfield.instance.SpawnNewBlock();
        }

    }
    
}
