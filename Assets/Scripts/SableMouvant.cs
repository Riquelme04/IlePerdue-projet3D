
using UnityEngine;

public class SableMouvant : MonoBehaviour
{
    [SerializeField] private float vitesseX = 0.05f;
    [SerializeField] private float vitesseY = 0.03f;

    private Renderer rendu;
    private Material materiau;
    private Vector2 decalage;

    void Start()
    {
        rendu = GetComponent<Renderer>();
        materiau = rendu.material;
    }

    void Update()
    {
        decalage.x += vitesseX * Time.deltaTime;
        decalage.y += vitesseY * Time.deltaTime;

        materiau.mainTextureOffset = decalage;
    }
}
