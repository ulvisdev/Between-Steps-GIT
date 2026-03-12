using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    public Transform leftcheck;
    public Transform rightcheck;
    public LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        float ledgePushAmount = 0.005f;

        // // LEFT OUTER
        // Vector2 lroLEFT = left_outer != null ? (Vector2)left_outer.position : (Vector2)transform.position + groundCheckOffset;
        // RaycastHit2D hit_leftO = Physics2D.Raycast(lroLEFT, Vector2.up, groundCheckDistance, groundLayer);
        // bool hitLouter = hit_leftO.collider != null;

        // // LEFT INNER
        // Vector2 lriLEFT = left_inner != null ? (Vector2)left_inner.position : (Vector2)transform.position + groundCheckOffset;
        // RaycastHit2D hit_leftI = Physics2D.Raycast(lriLEFT, Vector2.up, groundCheckDistance, groundLayer);
        // bool hitLinner = hit_leftI.collider != null;

        // // RIGHT OUTER
        // Vector2 lroRIGHT = right_outer != null ? (Vector2)right_outer.position : (Vector2)transform.position + groundCheckOffset;
        // RaycastHit2D hit_rightO = Physics2D.Raycast(lroRIGHT, Vector2.up, groundCheckDistance, groundLayer);
        // bool hitRouter = hit_rightO.collider != null;

        // // RIGHT INNER
        // Vector2 lriRIGHT = right_inner != null ? (Vector2)right_inner.position : (Vector2)transform.position + groundCheckOffset;
        // RaycastHit2D hit_rightI = Physics2D.Raycast(lriRIGHT, Vector2.up, groundCheckDistance, groundLayer);
        // bool hitRinner = hit_rightI.collider != null;

        bool hitLeftCheck = Physics2D.OverlapBox(leftcheck.position, new Vector2(0.25f, 0.25f), 0f, groundLayer);
        bool hitRightCheck = Physics2D.OverlapBox(rightcheck.position, new Vector2(0.25f, 0.25f), 0f, groundLayer);

        // Push away from the ledge
        // if (rb.linearVelocity.y > 0f)
        // {
        //     if (hitLeft)
        //         rb.position += Vector2.right * ledgePushAmount;

        //     if (hitRight)
        //         rb.position += Vector2.left * ledgePushAmount;
        // }

        if (rb.linearVelocity.y > 0f)
        {
            if (playerController.IsFacingRight())
            {
                if (hitLeftCheck)
                    rb.position += Vector2.right * ledgePushAmount;

                if (hitRightCheck)
                    rb.position += Vector2.left * ledgePushAmount;
            }
            else
            {
                if (hitLeftCheck)
                    rb.position += Vector2.left * ledgePushAmount;

                if (hitRightCheck)
                    rb.position += Vector2.right * ledgePushAmount;
            }
        }
    }
}
