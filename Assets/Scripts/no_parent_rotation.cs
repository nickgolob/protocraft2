using UnityEngine;

public class NoParentRotation : MonoBehaviour {
    // Update is called once per frame
    private void LateUpdate() {
        transform.rotation = Quaternion.Euler(
            0.0f, 0.0f, transform.parent.rotation.z * -1.0f);
    }
}