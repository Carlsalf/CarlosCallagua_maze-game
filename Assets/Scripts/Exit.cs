using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject winText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GANASTE");

            if (winText != null)
            {
                winText.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }
}