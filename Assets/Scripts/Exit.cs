using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject winPanel;

    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasWon)
        {
            hasWon = true;

            Debug.Log("GANASTE");

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }
}