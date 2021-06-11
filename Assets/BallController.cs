using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class BallController : MonoBehaviour
{
    public float MoveForce = 2f;

    public bool InitialUp;

    public Rigidbody2D Ball1;
    public Rigidbody2D Ball2;
    public GameObject  Chain;

    private Rigidbody2D[] _chainLinks;
    private Rigidbody2D   _controlled;
    private Rigidbody2D   _fixed;

    [ShowNonSerializedField] private Vector2 _force   = Vector2.zero;
    [ShowNonSerializedField] private float   _gravity = 1;

    // Start is called before the first frame update
    private void Start()
    {
        Assert.IsNotNull(Ball1);
        Assert.IsNotNull(Ball2);

        _chainLinks = Chain.GetComponentsInChildren<Rigidbody2D>();
        _controlled = Ball2;
        _fixed      = Ball1;

        if (InitialUp)
            _gravity = -1;

        SwitchControlled();
    }

    // Update is called once per frame
    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var normal     = _controlled.position - _fixed.position;

        normal.Normalize();

        var input = new Vector2(horizontal, vertical);

        _force = (new Vector2(-normal.y, normal.x) * (horizontal * _gravity) + (normal * vertical)).normalized * MoveForce;
        //_force = input * MoveForce * Vector2.right;
        _force = input * MoveForce;

        Debug.DrawRay(_controlled.position, _force.normalized, Color.red);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gravity *= -1;

            SwitchControlled();
        }
    }

    private void SwitchControlled()
    {
        var temp = _controlled;
        _controlled = _fixed;
        _fixed = temp;

        _fixed.constraints = RigidbodyConstraints2D.FreezeAll;
        _controlled.constraints  = RigidbodyConstraints2D.None;
        _controlled.gravityScale = _gravity;

        foreach (var link in _chainLinks)
        {
            link.gravityScale = _gravity;
        }
    }

    private void FixedUpdate()
    {
        _controlled.AddForce(_force);
    }
}
