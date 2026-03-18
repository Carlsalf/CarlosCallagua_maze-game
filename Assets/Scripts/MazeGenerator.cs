using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator Instance;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject exitPrefab;
    public GameObject ball;

    [Header("Tamaño base")]
    public int baseWidth = 10;
    public int baseHeight = 10;

    [Header("Tamaño actual")]
    public int width;
    public int height;

    [Header("Dificultad")]
    [Range(0.1f, 0.9f)]
    public float wallChance = 0.3f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            ApplyDifficulty(GameManager.Instance.currentLevel);
        }
        else
        {
            width = baseWidth;
            height = baseHeight;
            wallChance = 0.3f;
        }

        GenerateMaze();
    }

    public void ApplyDifficulty(int level)
    {
        width = baseWidth + (level - 1) * 2;
        height = baseHeight + (level - 1) * 2;

        wallChance = Mathf.Clamp(0.30f + (level - 1) * 0.05f, 0.30f, 0.55f);

        Debug.Log("Nivel: " + level + " -> Maze: " + width + "x" + height + " | wallChance: " + wallChance);
    }

    private void GenerateMaze()
    {
        ClearPreviousMaze();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                CreateFloor(x, z);

                bool isBorder = x == 0 || z == 0 || x == width - 1 || z == height - 1;
                bool isStart = (x == 1 && z == 1);
                bool isExit = (x == width - 2 && z == height - 2);

                if (isBorder && !isStart && !isExit)
                {
                    CreateWall(x, z);
                    continue;
                }

                if (!isStart && !isExit && Random.value < wallChance)
                {
                    CreateWall(x, z);
                }
            }
        }

        EnsureBasicPath();
        CreateExit();
        ResetBall();
    }

    private void ClearPreviousMaze()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void CreateFloor(int x, int z)
    {
        GameObject floor = Instantiate(
            floorPrefab,
            new Vector3(x, 0f, z),
            Quaternion.identity
        );

        floor.transform.SetParent(transform);
    }

    private void CreateWall(int x, int z)
    {
        GameObject wall = Instantiate(
            wallPrefab,
            new Vector3(x, 0.5f, z),
            Quaternion.identity
        );

        wall.transform.SetParent(transform);
    }

    private void CreateExit()
    {
        GameObject exitObj = Instantiate(
            exitPrefab,
            new Vector3(width - 2, 0.15f, height - 2),
            Quaternion.identity
        );

        exitObj.transform.SetParent(transform);
    }

    private void ResetBall()
    {
        if (ball != null)
        {
            ball.transform.position = new Vector3(1f, 1f, 1f);

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void EnsureBasicPath()
    {
        // Limpiar zona inicial
        ClearCell(1, 1);
        ClearCell(2, 1);
        ClearCell(3, 1);
        ClearCell(4, 1);

        // Limpiar zona final
        ClearCell(width - 2, height - 2);
        ClearCell(width - 3, height - 2);
        ClearCell(width - 2, height - 3);

        // Camino horizontal inicial
        for (int x = 1; x <= Mathf.Min(4, width - 2); x++)
        {
            ClearCell(x, 1);
        }

        // Camino vertical cerca de la salida
        for (int z = Mathf.Max(1, height - 4); z < height - 1; z++)
        {
            ClearCell(width - 2, z);
        }
    }

    private void ClearCell(int x, int z)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            Vector3 p = child.position;

            bool sameX = Mathf.RoundToInt(p.x) == x;
            bool sameZ = Mathf.RoundToInt(p.z) == z;
            bool looksLikeWall = child.position.y > 0.1f;

            if (sameX && sameZ && looksLikeWall)
            {
                Destroy(child.gameObject);
            }
        }
    }
}