using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    public LineRenderer FloorLine;
    [SerializeField]private string[] _colliderTags;
    [SerializeField]private Transform _lineBottom;
    [SerializeField]private string _floorColliderTag;
    private Ray _ray;
    private RaycastHit _hit;
    private Vector3 _hitPoint;
    protected Vector3 hitPoint=> _hitPoint;
    private bool _isHitCollider;
    protected bool isHitCollider=> _isHitCollider;
    private int _index = 0;
    public LayerMask Ignorelayer;
    public void UpdateRaycast()
    {

        UpdateBasicLineDetector(); 
        if (isHitCollider) SetFloorLine(hitPoint, Color.green);
        else if (RayHitCollision(_floorColliderTag))
            SetFloorLine(hitPoint, Color.red);
    }

    protected bool RayHitCollision(string colliderTag)
    {
        if((_hit.collider != null) && (_hit.collider.CompareTag(colliderTag)))
        {
            _hitPoint = _hit.point; 
            return true;
        }
        return false; 
    }

     protected void UpdateBasicLineDetector()
    {
        _isHitCollider = false;
        _ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(_ray, out _hit,Mathf.Infinity,Ignorelayer))
        {
            if (_hit.collider != null)
            {
                for (_index = 0; _index < _colliderTags.Length; _index++)
                {
                    if (_hit.collider.CompareTag(_colliderTags[_index]))
                    {
                        _hitPoint = _hit.point;
                        _isHitCollider = true;
                    }
                }
            }
        }
    }

    private void SetFloorLine(Vector3 hitPoint, Color colour)
    {
        FloorLine.startColor = colour;
        FloorLine.endColor = colour;
        FloorLine.SetPosition(0, transform.position);
        FloorLine.SetPosition(1, hitPoint);
        _lineBottom.position = hitPoint;
    }
}
