using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputs : MonoBehaviour
{
    public static ButtonInputs instance;

    public GameObject[] rotateCanvases;
    public GameObject moveCanvas;

    private GameObject activeBlock;
    BlockManager activeTBlock;

    public bool isMoveOn = true;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SetInputs();
    }

    void RepositionToActiveBlock()
    {
        if (activeBlock != null)
        {
            transform.position = activeBlock.transform.position;
        }
    }

    public void SetActiveBlock(GameObject block, BlockManager tBlock)
    {
        activeBlock = block;
        activeTBlock = tBlock;
    }

    // Update is called once per frame
    void Update()
    {
        RepositionToActiveBlock();
    }

    public void MoveBlock(string direction)
    {
        if(activeBlock != null)
        {
            if(direction == "left")
            {
                activeTBlock.SetInput(Vector3.left);
            }

            if (direction == "right")
            {
                activeTBlock.SetInput(Vector3.right);
            }

            if (direction == "forward")
            {
                activeTBlock.SetInput(Vector3.forward);
            }

            if (direction == "back")
            {
                activeTBlock.SetInput(Vector3.back);
            }

        }
    }

    public void RotateBlock(string rotation)
    {
        if(activeBlock != null && Input.GetKey(KeyCode.Space) == false)
        {
            //X Rotation
            if(rotation == "posX")
            {
                activeTBlock.SetRotationInput(new Vector3(90, 0, 0));
                
            }
            if (rotation == "negX")
            {
                activeTBlock.SetRotationInput(new Vector3(-90, 0, 0));
                
            }
            if (rotation == "posY")
            {
                
                activeTBlock.SetRotationInput(new Vector3(0, 90, 0));
            }
            if (rotation == "negY")
            {
                
                activeTBlock.SetRotationInput(new Vector3(0, -90, 0));
            }
            if (rotation == "posZ")
            {
                
                activeTBlock.SetRotationInput(new Vector3(0, 0, 90));
            }
            if (rotation == "negZ")
            {
                
                activeTBlock.SetRotationInput(new Vector3(0, 0, -90));
            }
        }
    }

    public void SwitchInputs()
    {
        isMoveOn = !isMoveOn;
        SetInputs();
    }
    void SetInputs()
    {
        moveCanvas.SetActive(isMoveOn);
        foreach (GameObject c in rotateCanvases)
        {
            c.SetActive(!isMoveOn);
        }
    }

    public void SetHighSpeed()
    {
        activeTBlock.SetSpeed();
        activeTBlock.SetDropScore();
    }
}
