using UnityEngine;

namespace AstronautPlayer
{

    public class PlanetGravityField : MonoBehaviour
    {
        public Transform gravityCenter; //Point the player is pulled toward

        void Start()
        {
            if (gravityCenter == null)
                gravityCenter = transform.parent != null ? transform.parent : transform;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<AstronautPlayerPlanet>();
            if (player != null) player.SetGravitySource(gravityCenter);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var player = other.GetComponent<AstronautPlayerPlanet>();
            if (player != null) player.ClearGravitySource(gravityCenter);
        }
    }
}
