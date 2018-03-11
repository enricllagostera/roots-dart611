using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    private Vector3 _root;
    [SerializeField] private float _angle;
    [SerializeField] private float _radius;
    [SerializeField] private AnimationCurve _heightCurve;
    public float groundRadius;
    public float targetAngle;
    public float progress;
    public float maxSpeed = 10f;
    public float minRadius = 3f;

    private Transform _visual;
    [SerializeField] private float _smoothFactor;
    private float angularSpeed;
    private Animator _animator;

    void Start()
    {
        _root = Vector3.zero;
        _visual = transform.Find("Visual");
        _radius = groundRadius;
        Vector3 pos = _root;
        pos.y = -_radius;
        _visual.localPosition = pos;
        _animator = _visual.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            targetAngle = Random.Range(0f, 360f);
            StartCoroutine(Fly());
        }

        // bee is on the ground
        if (groundRadius <= _radius + 0.2f)
        {
            Debug.Log("grounded");
            _animator.SetBool("Flying", false);
        }
        else
        {
            _animator.SetBool("Flying", true);
        }
        // bee is finishing it's angular movement	
        if (Mathf.Abs(angularSpeed) <= 0.02f)
        {
            Debug.Log("stopping");
        }
    }

    IEnumerator Fly()
    {
        // angle controls
        // float newAngle = Mathf.LerpAngle(_angle, targetAngle, Time.deltaTime * _smoothFactor);
        // transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
        // float angularSpeed = newAngle - _angle;
        // _angle = newAngle;
        float maxDelta = Mathf.Abs(Mathf.DeltaAngle(_angle, targetAngle));
        angularSpeed = 0f;
        while (Mathf.Abs(Mathf.DeltaAngle(_angle, targetAngle)) > 0.1f)
        {
            // angle controls
            float newAngle = Mathf.SmoothDampAngle(_angle, targetAngle, ref angularSpeed, Time.smoothDeltaTime * _smoothFactor, maxSpeed);
            transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            angularSpeed = newAngle - _angle;
            _angle = newAngle;
            // height controls
            float delta = Mathf.Abs(Mathf.DeltaAngle(_angle, targetAngle));
            progress = delta.Map(0f, maxDelta, 0f, 1f);
            _radius = groundRadius - _heightCurve.Evaluate(progress) * (groundRadius - minRadius);
            Vector3 pos = _root;
            pos.y = -_radius;
            _visual.localPosition = pos;
            // flip bee in counter-clockwise movement
            var scale = _visual.localScale;
            scale.x = (angularSpeed < 0f && _radius < groundRadius) ? -1f : 1f;
            _visual.localScale = scale;

            yield return null;
        }

        // bee is finishing it's angular movement
        // if (Mathf.Abs(angularSpeed) <= 0.02f)
        // {
        //     Debug.Log("stoping");
        // }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, groundRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
