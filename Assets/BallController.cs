using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class BallController : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("Active");

    public float InactiveGravity = 5f;
    public float InactiveDrag    = 5f;
    public float MoveForce       = 2f;

    public bool InitialUp;

    public Rigidbody2D Ball1;
    public Rigidbody2D Ball2;

    private ChainController _chain;

    private Rigidbody2D _controlled;
    private Rigidbody2D _fixed;

    [ShowNonSerializedField] private Vector2 _force   = Vector2.zero;
    [ShowNonSerializedField] private float   _gravity = 1;

    private AudioSource _audioSource;
    public  AudioClip   SwapSound;

    // Start is called before the first frame update
    private void Start()
    {
        Assert.IsNotNull(Ball1);
        Assert.IsNotNull(Ball2);

        _audioSource = GetComponent<AudioSource>();
        _chain       = GetComponentInChildren<ChainController>();
        _controlled  = Ball2;
        _fixed       = Ball1;

        if (InitialUp)
        {
            _gravity = -1;
        }

        SwitchControlled();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale < 1)
            return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical   = Input.GetAxis("Vertical");
        var normal     = _controlled.position - _fixed.position;

        normal.Normalize();

        var input = new Vector2(horizontal, vertical);

        _force = (new Vector2(-normal.y, normal.x) * (horizontal * _gravity) + normal * vertical).normalized *
                 MoveForce;
        //_force = input * MoveForce * Vector2.right;
        _force = input * MoveForce;

        Debug.DrawRay(_controlled.position, _force.normalized, Color.red);

        if (Input.GetButtonDown("Swap"))
        {
            _gravity *= -1;

            SwitchControlled();
        }
    }

    private void FixedUpdate()
    {
        _controlled.AddForce(_force);
    }

    private void SwitchControlled()
    {
        var temp = _controlled;
        _controlled = _fixed;
        _fixed      = temp;

        //_fixed.constraints = RigidbodyConstraints2D.FreezePositionX;
        _fixed.gravityScale *= InactiveGravity;
        _fixed.drag         =  InactiveDrag;
        _fixed.GetComponent<Animator>().SetBool(Active, false);
        _controlled.gravityScale = _gravity;
        _controlled.drag         = 0;
        _controlled.GetComponent<Animator>().SetBool(Active, true);

        _chain.SetGravity(_gravity);

        _audioSource.PlayOneShot(SwapSound);
    }
}
