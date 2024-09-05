using UnityEngine;

public interface ISpringJoint2DHandler
{
    public void SetDistance(float distance);
    public void SetTarget(Rigidbody2D target);
    public void SetDisable();
}