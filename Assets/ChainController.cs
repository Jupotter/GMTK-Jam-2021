using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[ExecuteInEditMode]
public class ChainController : MonoBehaviour
{
    public int AngleLimit = 30;

    public Rigidbody2D  FirstLink;
    public HingeJoint2D LastLink;

    public HingeJoint2D LinkPrefab;

    public float LinkSize;

    private List<HingeJoint2D> _chainLinks = new List<HingeJoint2D>();

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        UpdateConfig();
    }

    [Button]
    private void Setup()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Vector2 path     = LastLink.transform.position - FirstLink.transform.position;

        Debug.Log(path);

        var     distance = path.magnitude - LinkSize;

        var direction = path.normalized;

        var previousLink   = FirstLink;
        var placedDistance = LinkSize;
        do
        {
            var newLink = Instantiate(LinkPrefab, this.transform);
            newLink.transform.position = FirstLink.position + direction * placedDistance;

            newLink.connectedBody = previousLink;

            _chainLinks.Add(newLink);
            previousLink   =  newLink.GetComponent<Rigidbody2D>();
            placedDistance += LinkSize;
        } while (placedDistance <= distance);

        LastLink.connectedBody = previousLink;
    }

    [Button]
    public void UpdateConfig()
    {
        SetAngle(AngleLimit);
    }

    public void SetAngle(int angle)
    {
        AngleLimit = angle;
        foreach (var link in _chainLinks)
        {
            link.limits = new JointAngleLimits2D
                          {
                              max = angle,
                              min = -angle,
                          };
        }
    }
}
