using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static Playfield instance;

    public int gridSizeX, gridSizeY, gridSizeZ;
    [Header("Block")]
    public GameObject[] blockList;
    public GameObject[] ghostList;

    [Header("Playfield Visuals")]
    public GameObject bottomPlane;
    public GameObject N, S, W, E;

    int randomIndex;
    
    public Transform[,,] theGrid;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        CalculatePreview();
        SpawnNewBlock();
    }

    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
    }

    public bool CheckInsideGrid(Vector3 pos)
    {

        return ((int)pos.x >= 0 && (int)pos.x < gridSizeX && 
                (int)pos.z >= 0 && (int)pos.z < gridSizeZ &&
                (int)pos.y >=0);
    }

    public void UpdateGrid(BlockManager block)
    {
        // Delete posible parent objects
        for (int x =0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if(theGrid[x,y,z] != null)
                    {
                        if (theGrid[x, y, z].parent == block.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }
                }
            }
        }

        // Fill in all child objects
        foreach (Transform child in block.transform)
        {
            Vector3 pos = Round(child.position);
            if (pos.y < gridSizeY)
            {
                theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }

    public Transform GetTransformOnGridPos(Vector3 pos)
    {
        if(pos.y > gridSizeY - 1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }

    }
    public void SpawnNewBlock ()
    {
        Vector3 spawnPoint = new Vector3((int)(transform.position.x + (float)gridSizeX / 2), (int)transform.position.y + gridSizeY, (int)(transform.position.z + (float)gridSizeZ / 2));
        


        // Spawn the block
        GameObject newBlock = Instantiate(blockList[randomIndex], spawnPoint, Quaternion.identity) as GameObject;
        // Ghost
        GameObject newGhost = Instantiate(ghostList[randomIndex], spawnPoint, Quaternion.identity) as GameObject;

        newGhost.GetComponent<GhostBlock>().SetParent(newBlock);

        CalculatePreview();
        Previewer.instance.ShowPreview(randomIndex);
    }

    public void CalculatePreview()
    {
        randomIndex = Random.Range(0, blockList.Length);
    }

    public void DeleteLayer()
    {
        int layersCleared = 0;
        for (int y = gridSizeY-1; y >= 0; y--)
        {
            // Check full layer
            if (CheckFullLayer(y))
            {
                layersCleared++;
                // Delete all blocks
                DeleteLayerAt(y);
                // Move remaining blocks by 1
                MoveAllLayerDown(y);
            }
        }
        
        if (layersCleared > 0)
        {
            GameManager.instance.LayersCleared(layersCleared);
        }
    }

    bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Destroy(theGrid[x, y, z].gameObject);
                theGrid[x, y, z] = null;
            }
        }
    }

    void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            MoveOneLayerDown(i);
        }
    }

    void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z] != null)
                {
                    theGrid[x, y - 1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y - 1, z].position += Vector3.down;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (bottomPlane != null)
        {
            // Resize Bottom Plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeZ / 10);
            bottomPlane.transform.localScale = scaler;

            // Reposition Bottom Plane
            bottomPlane.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2, transform.position.y, transform.position.z + (float)gridSizeZ / 2);

            // Retile Material  
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }

        if (N != null)
        {
            // Resize Bottom Plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            N.transform.localScale = scaler;

            // Reposition Bottom Plane
            N.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2, transform.position.y + (float)gridSizeY / 2, transform.position.z + gridSizeZ);

            // Retile Material  
            N.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (S != null)
        {
            // Resize Bottom Plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            S.transform.localScale = scaler;

            // Reposition Bottom Plane
            S.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2, transform.position.y + (float)gridSizeY / 2, transform.position.z);

            // Retile Material  
            //S.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (E != null)
        {
            // Resize Plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            E.transform.localScale = scaler;

            // Reposition Plane
            E.transform.position = new Vector3(transform.position.x + gridSizeX, transform.position.y + (float)gridSizeY / 2, transform.position.z + (float)gridSizeZ / 2);

            // Retile Material  
            E.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

        if (W != null)
        {
            // Resize Plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            W.transform.localScale = scaler;

            // Reposition Plane
            W.transform.position = new Vector3(transform.position.x, transform.position.y + (float)gridSizeY / 2, transform.position.z + (float)gridSizeZ / 2);

            // Retile Material  
            //W.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }
    }

}
