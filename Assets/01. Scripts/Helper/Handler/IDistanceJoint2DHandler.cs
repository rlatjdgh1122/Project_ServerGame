using UnityEngine;

public interface IDistanceJoint2DHandler
{
    public void SetDistance(float distance);
    public void SetTarget(Rigidbody2D target);
    public void SetDisable();
}