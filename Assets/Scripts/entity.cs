using UnityEditor;
using UnityEngine;

public class entity : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("collided");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private void Update() {
        ClampToScreen2();
    }

    private void ClampToScreen2() {
        // https://www.reddit.com/r/Unity2D/comments/2jwp0j/im_confused_on_how_i_keep_a_sprite_on_screen/
        var objectWidth =
            transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        var objectHeight =
            transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        var screenMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        var screenMax = Camera.main.ViewportToWorldPoint(Vector2.one);
        var viewPos = transform.position;
        viewPos.x = Mathf.Clamp(
            viewPos.x, screenMin.x + objectWidth, screenMax.x - objectWidth);
        viewPos.y = Mathf.Clamp(
            viewPos.y, screenMin.y + objectHeight, screenMax.y - objectHeight);
        transform.position = viewPos;
    }

    private void ClampToScreen() {
        // deprecated. Doesnt consider size of sprite.oiu
        var camera_bottom_left = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var camera_top_right = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Camera.main.pixelWidth, Camera.main.pixelHeight));
        var camera_rect = new Rect(
            camera_bottom_left.x,
            camera_bottom_left.y,
            camera_top_right.x - camera_bottom_left.x,
            camera_top_right.y - camera_bottom_left.y);
        transform.position = new Vector3(
            Mathf.Clamp(
                transform.position.x, camera_rect.xMin, camera_rect.xMax),
            Mathf.Clamp(
                transform.position.y, camera_rect.yMin, camera_rect.yMax),
            transform.position.z);
    }
}