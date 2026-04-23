using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moeda"))
        {
            // Em vez de EconomyManager, chamamos o HUDManager!
            // Podes mudar o 1 para 10 se quiseres que cada moeda valha mais
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.GanharMoeda(1); 
            }

            Destroy(other.gameObject);
        }
    }
}