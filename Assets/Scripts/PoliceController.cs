using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Random = UnityEngine.Random;

public class PoliceController : MonoBehaviour
{
    public static bool Stopper = true;
    [SerializeField] private NavMeshAgent agent;
    private int _x, _y, _z;
    private Vector3 _target;
    private Coroutine _waitCoroutine;
    
    public void Start()
    {
        Stopper = true;
        _waitCoroutine = StartCoroutine (Count_Coroutine());
        RandomPos();
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,0.5f,0), transform.forward, out hit, 5) && hit.collider.gameObject.name == "Thief" && hit.collider.gameObject.GetComponent<BoxCollider>().enabled)
        {
            Stopper = false;
            agent.SetDestination(hit.collider.gameObject.transform.position + new Vector3(1, 0, 1));
        }
        _target = new Vector3(_x, transform.position.y, _z);
        if (Stopper)
            agent.SetDestination(_target);
        if (_waitCoroutine == null)
            _waitCoroutine = StartCoroutine (Count_Coroutine());
    }
    private IEnumerator Count_Coroutine () 
    {
        if (Mathf.Abs(transform.position.x - _x) <= 1 && Mathf.Abs(transform.position.z - _z) <= 1)
            RandomPos();
        else
        {
            yield return new WaitForSeconds(3f);
            _waitCoroutine = null;
            RandomPos();
        }
        yield return null;
    }
    
    public void RandomPos()
    {
        _x = Random.Range(-6, 6);
        _z = Random.Range(-6, 6);
    }
}
