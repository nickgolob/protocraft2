using Unity.Mathematics;
using UnityEngine;

public class HealthMeter : MonoBehaviour {
    public void OnHealthChange(float health_ratio) {
        // x traverses between 0 and -1
        transform.localPosition = new Vector3(health_ratio / 2 - 0.5f, 0, 0);
        transform.localScale = new Vector3(health_ratio, 1, 1);

        var full = Color.green;
        var empty = Color.red;

        GetComponent<SpriteRenderer>().color =
            new Color(
                math.min(2 - 2 * health_ratio, 1),
                math.min(health_ratio * 2, 1),
                0, 1
            );
        // new Color(
        //     (float)Math.Cos(health_ratio * Math.PI / 2),
        //     (float)Math.Sin(health_ratio * Math.PI / 2), 0,
        //     1);
        // Color.LerpUnclamped(empty, full, health_ratio);
    }
}