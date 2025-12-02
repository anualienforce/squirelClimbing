using UnityEngine;

public class ZoomLoop : MonoBehaviour
{
    public float minScale = 0.9f;   // smallest size
    public float maxScale = 1.1f;   // biggest size
    public float speed = 1.5f;      // animation speed


    public GameObject Glow; //  drag your VFX prefab here in the inspector
    public GameObject collectVFX; //  drag your VFX prefab here in the inspector

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f); // 0→1→0
        float scale = Mathf.Lerp(minScale, maxScale, t);
        Glow.transform.localScale = new Vector3(scale, scale, scale);
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
          

            // Spawn VFX
            if (collectVFX != null)
            {
                GameObject vfx = Instantiate(collectVFX, transform.position, Quaternion.identity);
                Destroy(vfx, 2f); // optional: destroy after 2 seconds
            }
        }
    }
}
