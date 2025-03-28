using Unity.Cinemachine;
using UnityEngine;

public class CameraPositionFreeze : MonoBehaviour
{
    private bool cameraFrozen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (cameraFrozen) return;

        if (other.CompareTag("Player"))
        {
            CinemachineBrain brain = Camera.main?.GetComponent<CinemachineBrain>();
            if (brain != null && brain.OutputCamera != null)
            {
                var activeCamera = brain.ActiveVirtualCamera as CinemachineCamera;

                if (activeCamera != null && activeCamera.Follow != null)
                {
                    activeCamera.Follow = null;
                    
                    activeCamera.LookAt = null;

                    cameraFrozen = true;
                }
            }
        }
    }
}
