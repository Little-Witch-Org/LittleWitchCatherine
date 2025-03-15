using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Collider2D Border;

    private float BaseSpeed = 0.25f;
    private Rigidbody2D rbody;
    private Animator animator;
    private Vector2 movement;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(MovementCoroutine());
    }


    private IEnumerator MovementCoroutine()
    {
        while (true)
        {
            int directionX, directiony;

            if (!IsWithinBounds(transform, Border))
            {
                directionX = (int)(Border.transform.position.x - transform.position.x);
                directiony = (int)(Border.transform.position.y - transform.position.y);
            }
            else
            {
                directionX = Random.Range(-1, 2);
                directiony = Random.Range(-1, 2);
            }

            movement = new Vector2(directionX, directiony);
            rbody.MovePosition(rbody.position + movement * BaseSpeed);
            yield return new WaitForSeconds(1f);

        }
    }
    private bool IsWithinBounds(Transform target, Collider2D boundary)
    {
        Bounds bounds = boundary.bounds;
        return bounds.Contains(target.position);
    }
}
