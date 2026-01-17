using UnityEngine;
using Unity.Netcode;

namespace Gear.Camera
{
    /// <summary>
    /// Simple camera follow script that smoothly follows the local player.
    /// Automatically finds the player with IsOwner = true.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [Header("Camera Settings")]
        [Tooltip("Camera offset from player (X=right/left, Y=up/down, Z=forward/back)")]
        [SerializeField] private Vector3 offset = new Vector3(1.2f, 2f, -2f);
        [SerializeField] private float smoothSpeed = 5f;
        [Tooltip("Height offset for camera look-at target")]
        [SerializeField] private float lookAtHeight = 2f;

        private Transform target;

        private void LateUpdate()
        {
            // Find the local player if we don't have a target yet
            if (target == null)
            {
                FindLocalPlayer();
                return;
            }

            // Smoothly follow the target
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            // Look at the player
            Vector3 lookAtPosition = target.position + Vector3.up * lookAtHeight;
            transform.LookAt(lookAtPosition);
        }

        /// <summary>
        /// Finds the local player (the one owned by this client).
        /// Called every frame until a player is found.
        /// </summary>
        private void FindLocalPlayer()
        {
            // Find all NetworkBehaviour objects
            var networkObjects = FindObjectsByType<NetworkBehaviour>(FindObjectsSortMode.None);
            
            foreach (var netObj in networkObjects)
            {
                // Check if this is the local player
                if (netObj.IsOwner && netObj.CompareTag("Player"))
                {
                    target = netObj.transform;
                    break;
                }
            }
        }
    }
}
