using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWall : MonoBehaviour
{
    [Header("Check wall")]
    public float rayHeight;
    public float checkWallDistance;

    public bool CheckWallObstacle(int direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, rayHeight, 0), transform.right, checkWallDistance * direction, LayerMask.GetMask("Ground"));

        Ray2D ray = new Ray2D(transform.position + new Vector3(0, rayHeight, 0), transform.right);
        Debug.DrawRay(ray.origin, ray.direction * checkWallDistance * (direction), Color.red);

        //Debug.Log(hit.collider != null);
        if (hit.collider != null)
            return true;

        return false;
    }
}
