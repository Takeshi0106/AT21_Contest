using UnityEngine;

public class Particle : MonoBehaviour
{
    float freams = 0.0f;
    [Header("パーティクルの時間")]
    [SerializeField] float hitTime = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        freams++;

        if(freams > hitTime)
        {
            Destroy(this.gameObject);
        }
    }
}
