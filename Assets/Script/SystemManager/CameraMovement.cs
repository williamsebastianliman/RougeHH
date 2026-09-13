using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;

    public Vector3 offset = new Vector3(0, 10, -10);
    public Vector3 rotation = new Vector3(45, 45, 0);

    public float smoothSpeed = 0.125f;

    public bool isShake = false;
    public float shakeIntensity = 0.2f;

    public float clock;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        transform.rotation = Quaternion.Euler(rotation);
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPos = player.transform.position + offset;
            Vector3 calcPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);

            if (isShake)
            {
                Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
                calcPos += shakeOffset;
                if (clock <= 0)
                {
                    isShake = false;
                }
                else
                {
                    clock -= Time.deltaTime;
                }
            }

            transform.position = calcPos;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
    public void shakeForTime(float time)
    {
        clock = time;
        isShake = true;
    }
}
