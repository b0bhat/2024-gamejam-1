using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    #region Singleton
    public static CameraController instance;
    void Awake() {
        instance = this;
    }
    #endregion

    public Transform target;
    public float smoothSpeed = 0.3f;
    public Vector3 offset;
    public Vector3 smoothedPosition;
    void Start() {
        target = Player.instance.gameObject.transform;
    }

    void FixedUpdate() {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void ShakeCamera(float duration, float magnitude) {
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude) {
        Vector3 orignalPosition = smoothedPosition;
        float elapsed = 0f;
        
        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(orignalPosition.x + x, orignalPosition.y + y, offset.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }
}
