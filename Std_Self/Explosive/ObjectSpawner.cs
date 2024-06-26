using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Vector2 minPoint = new(-14f, -14f);
    [SerializeField] private Vector2 maxPoint = new(14f, 14f);

    private void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            SpawnObject(0);
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObject(1);
        }
    }

    void SpawnObject(int index)
    {
        float x = Random.Range(minPoint.x, maxPoint.x);
        float y = 10f;
        float z = Random.Range(minPoint.y, maxPoint.y);
        Color color = Random.ColorHSV();

        GameObject clone = Instantiate(objects[index], new Vector3(x,y,z),Quaternion.identity);

        clone.GetComponent<MeshRenderer>().material.color = color;

        if(clone.TryGetComponent<TrailRenderer>(out var trail))
        {
            trail.startColor = color;
            trail.endColor = color;
        }
    }
}
