using UnityEngine;
using LootLocker.Requests;
using System.Collections;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent playerConnected;
    private IEnumerator Start()
    {
        bool connected = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("Starting LootLocker session");
                return;
            }
            Debug.Log("Sucessfully LootLocker session");
            connected = true;
        });
        yield return new WaitUntil(() => connected);
        playerConnected.Invoke();
    }
}
