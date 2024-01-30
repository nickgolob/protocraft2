using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Fighter : MonoBehaviour {
    public GameObject sword;
    public HealthMeter health_meter;

    public readonly float speed = 3;

    private string debug_name;
    private const int kMaxHealth = 10;
    private int health = kMaxHealth;

    private void Start() {
        debug_name = name; // TODO try and get parent.
    }

    private void Update() {
        var opponent = FindOpponent();
        var opponent_position = opponent.transform.position;

        // Rotate to look at opponent
        var target_vector = opponent_position - transform.position;
        var target = Quaternion.LookRotation(
            //target_vector, 
            new Vector3(0, 0, 1), target_vector);
        transform.rotation = target;

        var ideal_enemy_position = target_vector.normalized * SwordReach() +
                                   transform.position;
        Vector3 enemy_hitbox_distance = opponent.GetComponent<Collider2D>()
            .ClosestPoint(transform.position);
        var walk_direction = enemy_hitbox_distance - ideal_enemy_position;

        // Debug.Log(
        //     debug_name + " position: " + transform.position.ToShortString()
        //     + " target_vector " + target_vector.ToShortString()
        //     + " ideal_enemy_position: " + ideal_enemy_position.ToShortString()
        //     + " enemy_hitbox_distance " +
        //     enemy_hitbox_distance.ToShortString());

        if (name == "A") { return; }

        // Step towards opponent
        transform.position +=
            walk_direction.normalized * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == sword) {
            Physics2D.IgnoreCollision(
                collision.collider, GetComponent<Collider2D>());
        }
        Debug.Log(debug_name + " collided");
        health -= 1;
        health_meter.OnHealthChange((float)health / kMaxHealth);
        if (health <= 0) { Destroy(gameObject); }
    }

    private GameObject FindOpponent() {
        var opponents = new SortedDictionary<float, GameObject>();
        foreach (var obj in
                 GameObject.FindGameObjectsWithTag("fighter")) {
            if (obj != this.GameObject()) {
                opponents[
                        (obj.transform.position - transform.position)
                        .magnitude] =
                    obj;
            }
        }
        return opponents.First().Value;
    }

    private float SwordReach() {
        var ideal_distance = sword.GetComponent<Collider2D>();
        var positionToCollider = sword.transform.position - transform.position;
        var otherSide = sword.transform.position + positionToCollider;
        Vector3 farthestPoint = sword.GetComponent<Collider2D>()
            .ClosestPoint(otherSide);
        return (farthestPoint - transform.position).magnitude;
    }
}