using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isSlowed;
    public bool isBlockByMelee;
    public bool canBlock=true;
    private Transform targetPosition;
    private Vector3 direction;
    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    public float moveSpeed = 5f;
    public float attackRange = 0.5f;

    public Animator animator;

    // Định nghĩa delegate
    public delegate void EnemyDestroyedHandler();

    // Định nghĩa sự kiện sử dụng delegate
    public event EnemyDestroyedHandler OnEnemyDestroyed;

    void Start()
    {
    }

    public void SetWaypoints(List<Transform> waypoints)
    {
        this.waypoints = waypoints;

        if (waypoints.Count > 0)
        {
            currentWaypointIndex = 0;
            targetPosition = waypoints[currentWaypointIndex];
            direction = (targetPosition.position - transform.position).normalized;
            SetMovementAnimation(); // Gọi hàm để đặt animation lần đầu
        }
        else
        {
            targetPosition = GameObject.FindWithTag("Castle").transform;
            direction = (targetPosition.position - transform.position).normalized;
            SetMovementAnimation(); // Gọi hàm để đặt animation lần đầu
        }
    }

    void Update()
    {
        if (targetPosition != null && !isBlockByMelee)
        {
            MoveTowardsTarget();
        }
    }

    void OnDestroy()
    {
        // Kích hoạt sự kiện khi kẻ địch bị tiêu diệt
        OnEnemyDestroyed?.Invoke();
    }

    void MoveTowardsTarget()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        Vector3 directionToTarget = targetPosition.position - transform.position;
        if (directionToTarget.magnitude <= moveSpeed * Time.deltaTime)
        {
            ReachTarget();
        }
    }

    void SetMovementAnimation()
    {
        if (animator == null) return; // Kiểm tra xem animator có tồn tại không

        if (currentWaypointIndex < waypoints.Count - 1)
        {
            Transform currentWaypoint = waypoints[currentWaypointIndex];
            Transform nextWaypoint = waypoints[currentWaypointIndex + 1];

            if (Mathf.Abs(currentWaypoint.position.x - nextWaypoint.position.x) > Mathf.Abs(currentWaypoint.position.y - nextWaypoint.position.y))
            {
                if (currentWaypoint.position.x > nextWaypoint.position.x)
                {
                    animator.SetTrigger("MoveLeft");
                }
                else
                {
                    animator.SetTrigger("MoveRight");
                }
            }
            else
            {
                if (currentWaypoint.position.y > nextWaypoint.position.y)
                {
                    animator.SetTrigger("MoveDown");
                }
                else
                {
                    animator.SetTrigger("MoveUp");
                }
            }
        }
    }

    void ReachTarget()
    {
        if (currentWaypointIndex < waypoints.Count - 1)
        {
            currentWaypointIndex++;
            targetPosition = waypoints[currentWaypointIndex];
            direction = (targetPosition.position - transform.position).normalized;
            SetMovementAnimation(); // Gọi hàm để đặt animation mỗi khi qua 1 waypoint
        }
        else
        {
            targetPosition = GameObject.FindWithTag("Castle").transform;
            direction = (targetPosition.position - transform.position).normalized;
            SetMovementAnimation(); // Gọi hàm để đặt animation khi đến điểm cuối
        }
    }

    public void SlowDown(float slowAmount)
    {
        if (!isSlowed)
        {
            moveSpeed -= slowAmount;
            isSlowed = true;
            Debug.Log("Enemy Slowed: " + moveSpeed);
        }
    }

    public void ResetSpeed(float originalSpeed)
    {
        if (isSlowed)
        {
            moveSpeed = originalSpeed;
            isSlowed = false;
            Debug.Log("Enemy Speed Restored: " + moveSpeed);
        }
    }

    public void stopWalking()
    {
        isBlockByMelee = true;
    }

    public void setIsAttacked(bool value)
    {
        isBlockByMelee = value;
    }
    public void ResetBlockStatus()
    {
        isBlockByMelee = false;
    }
}
