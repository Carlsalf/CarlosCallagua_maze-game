using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasWon)
        {
            hasWon = true;
            Debug.Log("GANASTE");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.WinLevel();
            }
        }
    }
}