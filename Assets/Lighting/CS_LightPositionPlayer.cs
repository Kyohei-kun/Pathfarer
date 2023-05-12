using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LightPositionPlayer : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] bool update;
    Transform _transform;
    Transform player;

    private void Start()
    {
        _transform = transform;
        player = _transform.parent;
    }

    private void Update()
    {
        if (update)
        {
            _transform.position = player.position + direction;
        }
    }
}
