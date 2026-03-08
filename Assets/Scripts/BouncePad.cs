using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 20.0f;

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<AstronautPlayer.AstronautPlayer>();
            player.Bounce(bounceForce);
        }
    }
}
