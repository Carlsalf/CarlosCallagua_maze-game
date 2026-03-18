using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int points = 10;
    public bool rotateObject = true;
    public float rotationSpeed = 90f;

    private void Update()
    {
        if (rotateObject)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreSystem.Instance != null)
            {
                ScoreSystem.Instance.AddScore(points);
            }

            Destroy(gameObject);
        }
    }
}