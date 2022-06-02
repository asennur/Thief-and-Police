using UnityEngine;

public class BoxPosition : MonoBehaviour
{
    public GameObject player;
    private void Update()
    {
        if (player != null)
            transform.position = player.transform.position + new Vector3(0.01f, 0.4f, -0.2f);
    }
}
