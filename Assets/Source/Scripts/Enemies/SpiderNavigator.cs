using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SpiderNavigator : MonoBehaviour
{
    private const float AtPointDistance = 2f;

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _ignoringLayers;

    [SerializeField] private float _normalSpeed = 3.5f;
    [SerializeField] private float _fleeSpeed = 10f;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform[] _points;

    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float _attackDistance = 4f;
    private IPlayerTransform _player;

    [Inject]
    public void Construct(IPlayerTransform player)
    {
        _player = player;
    }

    public IEnumerator Attack()
    {
        yield return null;
        Debug.Log("Player atacked");
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = _player.Player.position - _rayOrigin.position;
        Ray ray = new Ray(_rayOrigin.position, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, _ignoringLayers))
        {
            if ((_playerLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.Log("all good");
                return true;
            }
            else
            {
                Debug.Log($"collider: {hit.collider.gameObject.name} {hit.collider.transform.parent.name}");
            }
        }
        Debug.Log("Miss");

        return false;
    }

    public void MoveToPlayer()
    {
        _agent.SetDestination(_player.Player.position);
    }

    public bool IsCloseToPoint()
    {
        return _agent.pathPending == false && _agent.remainingDistance <= AtPointDistance;
    }

    public bool AtAttackDistance()
    {
        return _agent.pathPending == false && _agent.remainingDistance <= _attackDistance;
    }

    public void Flee()
    {
        _agent.speed = _fleeSpeed;
        SetDestination();
    }

    public void MoveToRandomPosition()
    {
        _agent.speed = _normalSpeed;
        SetDestination();
    }

    public void Stop()
    {
        _agent.isStopped = true;
    }

    [Button]
    public void TrySetDestination(Transform transform)
    {
        Debug.Log(_agent.SetDestination(transform.position));
    }

    private void SetDestination()
    {
        Vector3 position = _points[UnityEngine.Random.Range(0, _points.Length)].transform.position;

        if (_agent.SetDestination(position) == false)
            Debug.Log("Can't set destination");
    }
}


