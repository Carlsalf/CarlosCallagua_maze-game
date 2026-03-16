using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject exitPrefab;

    public int width = 10;
    public int height = 10;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Crear suelo
                Instantiate(floorPrefab, new Vector3(x, 0, z), Quaternion.identity);

                // Crear paredes en bordes
                if (x == 0 || z == 0 || x == width - 1 || z == height - 1)
                {
                    Instantiate(wallPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
                }
                else
                {
                    // Paredes aleatorias internas
                    if (Random.value > 0.7f)
                    {
                        Instantiate(wallPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
                    }
                }
            }
        }

        // Crear salida (Exit)
        Instantiate(exitPrefab, new Vector3(width - 2, 0.5f, height - 2), Quaternion.identity);
    }
}