using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject exitPrefab;
    public GameObject ball;

    public int width = 10;
    public int height = 10;

    private void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // limpiar clones previos
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // SIEMPRE crear suelo
                GameObject floor = Instantiate(floorPrefab, new Vector3(x, 0f, z), Quaternion.identity);
                floor.transform.SetParent(transform);

                bool isBorder = x == 0 || z == 0 || x == width - 1 || z == height - 1;
                bool isStart = (x == 1 && z == 1);
                bool isExit = (x == width - 2 && z == height - 2);

                if (!isStart && !isExit)
                {
                    if (isBorder || Random.value > 0.7f)
                    {
                        GameObject wall = Instantiate(wallPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
                        wall.transform.SetParent(transform);
                    }
                }
            }
        }

        GameObject exitObj = Instantiate(exitPrefab, new Vector3(width - 2, 0.15f, height - 2), Quaternion.identity);
        exitObj.transform.SetParent(transform);

        // colocar bola en una celda segura
        if (ball != null)
        {
            ball.transform.position = new Vector3(1f, 1f, 1f);
        }
    }
}