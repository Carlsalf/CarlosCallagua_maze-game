using UnityEngine;

public class Trap : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("CAISTE EN TRAMPA");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseLife();
            }
        }
    }
}