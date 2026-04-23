using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    void Update()
    {
        // Adicionámos o "Space.World" no final!
        transform.Rotate(0, 100 * Time.deltaTime, 0, Space.World);
    }
}
