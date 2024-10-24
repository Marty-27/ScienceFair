using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    private int size;
    public Material[] wallAliveMaterials;
    public Material deadMaterial;
    public float updateInterval = 0.5f;
    public bool start = false;

    private Dictionary<string, bool[,]> cellStatesDict = new Dictionary<string, bool[,]>();
    private Dictionary<string, GameObject> walls = new Dictionary<string, GameObject>();
    private bool[,] cellStates;
    private float timer;
    private AudioSource audioSource;

    private void Start()
    {
        if(GameManager.Instance != null)
        {
            this.size = GameManager.Instance.WorldSize;
        }else
        {
            this.size = 10;
        }
        this.audioSource = this.GetComponent<AudioSource>();
        this.cellStates = new bool[this.size, this.size];
        this.timer = 0;
        string[] wallNames = { "Front wall", "Back wall", "Right wall", "Left wall", "Top wall", "Floor wall" };
        foreach (string wallName in wallNames)
        {
            bool[,] states = new bool[this.size, this.size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    states[x, y] = UnityEngine.Random.value > 0.9f;
                }
            }
            this.cellStatesDict[wallName] = states;
        }
        this.UpdateCellStates();
        this.CreateRoom();
    }

    private void Update()
    {
        if (this.start)
        {
            this.timer += Time.deltaTime;

            // Handle case where updateInterval is less than 1
            if (this.updateInterval < 1.0f)
            {
                if (this.timer >= 0.5f)
                {
                    this.timer = 0;
                    this.audioSource.volume = 0.1f; // Adjust the volume if needed
                    this.audioSource.Play();
       
                }
                this.UpdateCellStates();
                this.CreateRoom();
            }
            else if (this.timer >= this.updateInterval)
            {
                this.timer = 0;

                if (this.updateInterval >= 2.0f)
                {
                    this.audioSource.volume = 0.5f;
                }
                else if (this.updateInterval >= 1.0f)
                {
                    this.audioSource.volume = 0.25f;
                }

                this.audioSource.Play();
                this.UpdateCellStates();
                this.CreateRoom();
            }
        }
    }

    private void CreateRoom()
    {
        string[] wallNames = { "Front wall", "Back wall", "Right wall", "Left wall", "Top wall", "Floor wall" };
        if (walls.Count == 0)
        {
            // Create walls if they don't exist
            walls["Front wall"] = CreateWall(cellStatesDict["Front wall"], new Vector3(0, this.size / 2, this.size / 2), Vector3.zero, "Front wall", wallAliveMaterials[0]);
            walls["Back wall"] = CreateWall(cellStatesDict["Back wall"], new Vector3(0, this.size / 2, -this.size / 2), new Vector3(0, 180, 0), "Back wall", wallAliveMaterials[1]);
            walls["Right wall"] = CreateWall(cellStatesDict["Right wall"], new Vector3(this.size / 2, this.size / 2, 0), new Vector3(0, 90, 0), "Right wall", wallAliveMaterials[2]);
            walls["Left wall"] = CreateWall(cellStatesDict["Left wall"], new Vector3(-this.size / 2, this.size / 2, 0), new Vector3(0, -90, 0), "Left wall", wallAliveMaterials[3]);
            walls["Top wall"] = CreateWall(cellStatesDict["Top wall"], new Vector3(0, this.size, 0), new Vector3(-90, 0, 0), "Top wall", wallAliveMaterials[4]);
            walls["Floor wall"] = CreateWall(cellStatesDict["Floor wall"], new Vector3(0, 0, 0), new Vector3(90, 0, 0), "Floor wall", wallAliveMaterials[5]);
        }
        else
        {
            // Update existing walls
            foreach (var wallName in wallNames)
            {
                if (walls.ContainsKey(wallName))
                {
                    UpdateWallMesh(walls[wallName], cellStatesDict[wallName], wallAliveMaterials[Array.IndexOf(walls.Keys.ToArray(), wallName)]);
                }
            }
        }
    }

    private GameObject CreateWall(bool[,] states, Vector3 position, Vector3 rotation, string name, Material aliveMaterial)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> aliveTriangles = new List<int>();
        List<int> deadTriangles = new List<int>();

        int quadCount = 0;
        List<CombineInstance> combine = new List<CombineInstance>();

        for (int x = 0; x < this.size; x++)
        {
            for (int y = 0; y < this.size; y++)
            {

                Vector3 quadPos = new Vector3(x - size / 2 + 0.5f, y - size / 2 + 0.5f, 0);

                vertices.Add(quadPos + new Vector3(-0.5f, -0.5f, 0)); // Bottom-left
                vertices.Add(quadPos + new Vector3(0.5f, -0.5f, 0));  // Bottom-right
                vertices.Add(quadPos + new Vector3(-0.5f, 0.5f, 0));  // Top-left
                vertices.Add(quadPos + new Vector3(0.5f, 0.5f, 0));   // Top-right

                if (states[x, y]) // Alive cell
                {
                    aliveTriangles.Add(quadCount * 4 + 0);
                    aliveTriangles.Add(quadCount * 4 + 2);
                    aliveTriangles.Add(quadCount * 4 + 1);
                    aliveTriangles.Add(quadCount * 4 + 2);
                    aliveTriangles.Add(quadCount * 4 + 3);
                    aliveTriangles.Add(quadCount * 4 + 1);
                }
                else // Dead cell
                {
                    deadTriangles.Add(quadCount * 4 + 0);
                    deadTriangles.Add(quadCount * 4 + 2);
                    deadTriangles.Add(quadCount * 4 + 1);
                    deadTriangles.Add(quadCount * 4 + 2);
                    deadTriangles.Add(quadCount * 4 + 3);
                    deadTriangles.Add(quadCount * 4 + 1);
                }

                quadCount++;

            }
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.subMeshCount = 2;
        mesh.SetTriangles(aliveTriangles.ToArray(), 0);
        mesh.SetTriangles(deadTriangles.ToArray(), 1);
        mesh.RecalculateNormals();

        GameObject wall = new GameObject(name);
        wall.transform.SetParent(transform);
        wall.transform.position = position;
        wall.transform.eulerAngles = rotation;
        wall.tag = "Wall";

        MeshFilter mf = wall.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = wall.AddComponent<MeshRenderer>();
        mr.materials = new Material[] { aliveMaterial, deadMaterial };

        MeshCollider mc = wall.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;

        WallCellInfo cellInfo = wall.AddComponent<WallCellInfo>();
        cellInfo.Initialize(size, rotation, name);

        return wall;
    }

    private void UpdateCellStates()
    {
        foreach (var key in cellStatesDict.Keys.ToList())
        {
            bool[,] states = cellStatesDict[key];
            bool[,] newStates = new bool[this.size, this.size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int aliveNeighbors = this.CountAliveNeighbors(states, x, y);
                    if (states[x, y])
                    {
                        newStates[x, y] = aliveNeighbors == 2 || aliveNeighbors == 3;
                    }
                    else
                    {
                        newStates[x, y] = aliveNeighbors == 3;
                    }
                }
            }
            cellStatesDict[key] = newStates;
        }
    }

    private int CountAliveNeighbors(bool[,] states, int x, int y)
    {
        int count = 0;

        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
                if (nx == x && ny == y)
                {
                    continue;
                }

                if (nx >= 0 && nx < size && ny >= 0 && ny < size)
                {
                    if (states[nx, ny])
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    private void UpdateWallMesh(GameObject wall, bool[,] states, Material aliveMaterial) 
    {
        // Regenerate the mesh data
        List<Vector3> vertices = new List<Vector3>();
        List<int> aliveTriangles = new List<int>();
        List<int> deadTriangles = new List<int>();

        int quadCount = 0;

        for (int x = 0; x < this.size; x++)
        {
            for (int y = 0; y < this.size; y++)
            {
                Vector3 quadPos = new Vector3(x - size / 2 + 0.5f, y - size / 2 + 0.5f, 0);

                // Add vertices for each cell
                vertices.Add(quadPos + new Vector3(-0.5f, -0.5f, 0)); // Bottom-left
                vertices.Add(quadPos + new Vector3(0.5f, -0.5f, 0));  // Bottom-right
                vertices.Add(quadPos + new Vector3(-0.5f, 0.5f, 0));  // Top-left
                vertices.Add(quadPos + new Vector3(0.5f, 0.5f, 0));   // Top-right

                // Assign triangles to alive or dead submesh
                if (states[x, y]) // Alive cell
                {
                    aliveTriangles.Add(quadCount * 4 + 0);
                    aliveTriangles.Add(quadCount * 4 + 2);
                    aliveTriangles.Add(quadCount * 4 + 1);
                    aliveTriangles.Add(quadCount * 4 + 2);
                    aliveTriangles.Add(quadCount * 4 + 3);
                    aliveTriangles.Add(quadCount * 4 + 1);
                }
                else // Dead cell
                {
                    deadTriangles.Add(quadCount * 4 + 0);
                    deadTriangles.Add(quadCount * 4 + 2);
                    deadTriangles.Add(quadCount * 4 + 1);
                    deadTriangles.Add(quadCount * 4 + 2);
                    deadTriangles.Add(quadCount * 4 + 3);
                    deadTriangles.Add(quadCount * 4 + 1);
                }

                quadCount++;
            }
        }

        Mesh mesh = wall.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.subMeshCount = 2;
        mesh.SetTriangles(aliveTriangles.ToArray(), 0);
        mesh.SetTriangles(deadTriangles.ToArray(), 1);
        mesh.RecalculateNormals();

        // Update the materials if needed
        MeshRenderer mr = wall.GetComponent<MeshRenderer>();
        mr.materials = new Material[] { aliveMaterial, deadMaterial };

        // Update the collider
        MeshCollider mc = wall.GetComponent<MeshCollider>();
        mc.sharedMesh = mesh;
    }

    public void SetCellState(string wallName, int x, int y, bool state)
    {
        if (cellStatesDict.ContainsKey(wallName))
        {
            bool[,] states = cellStatesDict[wallName];
            if (x >= 0 && x < size && y >= 0 && y < size)
            {
                states[x, y] = state;
                // Update the wall immediately
                UpdateWallMesh(walls[wallName], states, wallAliveMaterials[Array.IndexOf(walls.Keys.ToArray(), wallName)]);
            }
        }
    }
}
