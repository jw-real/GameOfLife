using UnityEngine;

public class RowContainerActivationProbe : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"[Container] OnEnable called. activeSelf={gameObject.activeSelf}, activeInHierarchy={gameObject.activeInHierarchy}");
    }
}
