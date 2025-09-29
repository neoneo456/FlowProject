using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float minX = -23.64803f;         
    public float maxX = 31.41f; 

    void LateUpdate()
    {
        // คำนวณตำแหน่งเป้าหมาย
        Vector3 targetPosition = player.position + offset;

        // จำกัดการเคลื่อนที่ในแนว X
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

        // คำนวณตำแหน่งที่ขยับช้า
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
