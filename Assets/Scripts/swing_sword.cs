using System;
using UnityEngine;

public class SwingSword : MonoBehaviour {
    private readonly TimeSpan attack_animation_period =
        TimeSpan.FromSeconds(0.5);

    private readonly float attack_animation_start_angle = -45;
    private readonly float attack_animation_end_angle = 45;
    private readonly float distance_from_parent = 0.7f;

    private readonly TimeSpan collision_shock =
        TimeSpan.FromSeconds(0.5);

    private bool collided;
    private float collision_time;

    private void Update() {
        // if (collided) {
        //     collided = false;
        //     // reset attack animation.
        // }
        // if (collision_time + collision_shock.TotalSeconds > Time.time) {
        //     return;
        // }

        // Update rotation / position
        var animation_proportion =
            // Base on the last collision_time
            (float)((Time.time - collision_time) %
                    attack_animation_period.TotalSeconds /
                    attack_animation_period.TotalSeconds);
        var angle =
            (attack_animation_end_angle - attack_animation_start_angle) *
            animation_proportion + attack_animation_start_angle;
        var angle_quaternion = Quaternion.Euler(0, 0, angle);
        transform.localRotation = angle_quaternion;
        transform.localPosition = angle_quaternion *
                                  new Vector3(0, distance_from_parent, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Debug.Log("sword collided");
        collided = true;
        collision_time = Time.time;
    }
}