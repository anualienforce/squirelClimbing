using UnityEngine;

public class CloudAnimation : MonoBehaviour
{

    //public GameObject cloudsPrefab;

    public Transform player;
    public Transform allCloud;
    [Header("cloudA ")]
    public Transform cloudA;
    public Vector3 cloudApos1;
    public Vector3 cloudApos2;
    [Header("cloudB ")]
    public Transform cloudB;

    public Vector3 cloudBpos1;
    public Vector3 cloudBpos2;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        float t = Mathf.PingPong(Time.time * 0.2f, 1f);

        // Cloud A move on X only
        float newAx = Mathf.Lerp(cloudApos1.x, cloudApos2.x, t);
        cloudA.position = new Vector3(newAx, cloudA.position.y, cloudA.position.z);

        // Cloud B move on X only
        float newBx = Mathf.Lerp(cloudBpos1.x, cloudBpos2.x, t);
        cloudB.position = new Vector3(newBx, cloudB.position.y, cloudB.position.z);
    }


}
