using UnityEngine;
using TMPro; // Required for TextMeshPro

public class FloatingWord : MonoBehaviour
{
    public float floatSpeed = 0.5f;
    public float lifetime = 2.5f;
    
    private TextMeshPro textMesh;
    private float timer = 0f;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        

        transform.position += new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));


        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime, Space.World);

        // Constantly look at the main camera so it is never backwards
        if (Camera.main != null)
        {

            Vector3 targetPostition = new Vector3(Camera.main.transform.position.x, 
                                        transform.position.y, 
                                        Camera.main.transform.position.z);
            transform.LookAt(targetPostition);
            

            transform.Rotate(0, 180, 0); 
        }

        timer += Time.deltaTime;
        if (textMesh != null)
        {
            Color c = textMesh.color;
            c.a = 1f - (timer / lifetime); 
            textMesh.color = c;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}