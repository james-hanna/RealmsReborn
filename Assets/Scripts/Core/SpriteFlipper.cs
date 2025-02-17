using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipper : MonoBehaviour
{
    // Update the sprite's direction based on horizontal velocity.
    // If horizontalVelocity is > 0, the sprite faces right; if < 0, it faces left.
    public void UpdateDirection(float horizontalVelocity)
    {
        if (horizontalVelocity > 0f)
        {
            FaceRight();
        }
        else if (horizontalVelocity < 0f)
        {
            FaceLeft();
        }
        // If horizontalVelocity is 0, do nothing.
    }

    // Explicitly set the sprite to face right.
    public void FaceRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    // Explicitly set the sprite to face left.
    public void FaceLeft()
    {
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
