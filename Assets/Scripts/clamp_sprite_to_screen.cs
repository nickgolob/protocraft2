using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clamp_sprite_to_screen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        var objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        var screenMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
        var screenMax = Camera.main.ViewportToWorldPoint(Vector2.one);
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenMin.x + objectWidth, screenMax.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenMin.y + objectHeight, screenMax.y - objectHeight);
        transform.position = viewPos;
    }
}
