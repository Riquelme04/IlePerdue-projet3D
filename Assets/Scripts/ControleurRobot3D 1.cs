using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControleurRobot3D : MonoBehaviour
{
    [SerializeField] private float vitesse = 5f;
    [SerializeField] private float vitesseRotation = 10f;
    private Rigidbody corps;
    private Vector3 direction;

    private void Awake() => corps = GetComponent<Rigidbody>();

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = Vector3.ClampMagnitude(new Vector3(horizontal, 0f, vertical), 1f);
    }

    private void FixedUpdate()
    {
        corps.MovePosition(corps.position + direction * vitesse * Time.fixedDeltaTime);
        if (direction.sqrMagnitude <= 0.001f) return;
        Quaternion cible = Quaternion.LookRotation(direction, Vector3.up);
        corps.MoveRotation(Quaternion.Slerp(corps.rotation, cible, vitesseRotation * Time.fixedDeltaTime));
    }
}
