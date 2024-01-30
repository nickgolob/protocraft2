using UnityEngine;

public class player_controlled : MonoBehaviour {
    private readonly float _speed = 3;

    private void Update() {
        var delta = new Vector3(
            Input.GetAxisRaw("Horizontal")
            , Input.GetAxisRaw("Vertical"), 0).normalized;
        transform.position += delta * _speed * Time.deltaTime;
    }
}