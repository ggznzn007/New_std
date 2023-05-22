using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private GameObject uMarker;
    private NavMeshAgent nAgent;

    private void Awake()
    {
        nAgent = GetComponent<NavMeshAgent>();
    }

    public void SelectUnit()
    {
        uMarker.SetActive(true);
    }

    public void DeslectUnit()
    {
        uMarker.SetActive(false);
    }

    public void MoveTo(Vector3 goal)
    {
        nAgent.SetDestination(goal);
    }
}
