
using UnityEngine;

public class RotationPieceRadio : MonoBehaviour
{
    [SerializeField] private float vitesseRotation = 30f;
    [SerializeField] private float amplitudeFlottement = 0.15f;
    [SerializeField] private float vitesseFlottement = 2f;

    private Vector3 positionInitiale;

    void Start()
    {
        // Enregistre la position initiale de la pièce.
        positionInitiale = transform.localPosition;
    }

    void Update()
    {
        // Rotation lente et continue.
        transform.Rotate(0f, vitesseRotation * Time.deltaTime, 0f);

        // Mouvement vertical de haut en bas.
        float flottement = Mathf.Sin(Time.time * vitesseFlottement)
                          * amplitudeFlottement;

        transform.localPosition = positionInitiale
            + Vector3.up * flottement;
    }
}
