using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class BallController : MonoBehaviour
{
    public float MoveForce = 2f;

    public Rigidbody2D Ball1;
    public Rigidbody2D Ball2;

    private Rigidbody2D _controlled;
    
    [ShowNonSerializedField]
    private Vector2 _force = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(Ball1);
        Assert.IsNotNull(Ball2);

        Ball1.constraints = RigidbodyConstraints2D.FreezeAll;
        _controlled       = Ball2;
    }

    // Update is called once per frame
    void Update()
    {
        var input = Input.GetAxis("Horizontal");
        _force = input * MoveForce * Vector2.right;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controlled.constraints = RigidbodyConstraints2D.FreezeAll;
            _controlled             = _controlled == Ball1 ? Ball2 : Ball1;
            _controlled.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void FixedUpdate()
    {
        _controlled.AddForce(_force);
    }
}
