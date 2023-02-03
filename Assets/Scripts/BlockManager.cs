using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    float prevTime;
    float fallTime = 1f;



    // Start is called before the first frame update
    void Start()
    {
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

            }

            prevTime = Time.time;
        }


        // Keyboard Input

        if (GameManager.instance.ReadIsGameOver())
            return;
        // Movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetInput(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetInput(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SetInput(Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetInput(Vector3.back);
        }

        // Rotation

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetSpeed();
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
        fallTime = 0.1f;
    }
}
