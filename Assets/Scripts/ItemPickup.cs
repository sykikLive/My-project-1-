using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : MonoBehaviour
{

    public string lvlToLoad;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.LoadScene(lvlToLoad);
        }
    }
}
