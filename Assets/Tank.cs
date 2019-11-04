using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Transform canon, radar;
    public GameObject missile;

    Rigidbody2D rb;
    public float angle;
    public bool clockwise;
    float dps = 90;     //degrees per second
    float ups = 1;      //units   per second

    public IEnumerator bodyRoutine, canonRoutine, radarRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        //rb.velocity = Vector3.up;
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //StartCoroutine(LookAt(mouseScreenPosition));
            StartCoroutine(Curve(angle, clockwise, true));
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            Rigidbody2D missileRb = Instantiate(missile, transform.position + transform.up / 2, transform.rotation).GetComponent<Rigidbody2D>();
            missileRb.velocity = transform.up * 4;
            Destroy(missileRb.gameObject, 5);
        }

        Action act = () => { };
        /*
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        canon.up = direction;*/
    }

    public void Idle() {
        //move
        //rotate
        //lookAt
        //shoot
        //wait
    }

    Vector2 Rotate(Vector2 v, float angle) {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }

    Vector2 RotateAround(Vector2 v, Vector2 pivot, float angle) {
        angle *= Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * (v.x - pivot.x) - Mathf.Sin(angle) * (v.y - pivot.y) + pivot.x;
        float y = Mathf.Sin(angle) * (v.x - pivot.x) + Mathf.Cos(angle) * (v.y - pivot.y) + pivot.y;
        return new Vector2(x, y);
    }


    public IEnumerator Curve(float angle, bool clockwise, bool ahead) {
        //calcular posicao final previamente
        float radius = 2f / Mathf.PI;
        Vector2 pivot = transform.position + radius * (clockwise ? transform.right : -transform.right);
        Vector2 endPosition = RotateAround(transform.position, pivot, clockwise ? -angle : angle);

        //movimento de curva
        float startAngle = transform.eulerAngles.z;
        float finalAngle = this.angle + (clockwise? -angle : angle);
        for(float i = 0, mult = (dps / angle); i <= 1; i += Time.deltaTime * mult) {
            rb.velocity = transform.up;
            transform.eulerAngles = new Vector3(0, 0, startAngle + i * (clockwise ? -angle : angle));
            yield return null;
        }

        //definicao final
        rb.velocity = Vector3.zero;
        transform.position = endPosition;
        transform.eulerAngles = new Vector3(0, 0, startAngle + (clockwise ? -angle : angle));        
    }

    public IEnumerator LookAt(Vector2 position) {
        Vector2 direction = (position - (Vector2) transform.position).normalized;
        float angle = Vector2.SignedAngle(direction, transform.up);
        print("ANGLE: " + angle);

        float startAngle = transform.eulerAngles.z;


        for(float t = 0; t <= 1; t += Time.deltaTime * (180 / Mathf.Max(1, Mathf.Abs(angle)))) {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(startAngle, startAngle - angle, t));
            yield return null;
        }
        //180 - 1
        //0 - 0   f(x) = x / 180
    }


}

