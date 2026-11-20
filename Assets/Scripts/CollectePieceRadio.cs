
using UnityEngine;

public class CollectePieceRadio : MonoBehaviour
{
    private bool estCollectee = false;

    private void OnTriggerEnter(Collider autre)
    {
        // Vérifie que l'objet détecté est le joueur.
        if (!autre.CompareTag("Player"))
        {
            return;
        }

        // Évite de collecter la même pièce plusieurs fois.
        if (estCollectee)
        {
            return;
        }

        estCollectee = true;

        // Affiche un message pour confirmer la collecte.
        Debug.Log("Pièce de radio récupérée !");

        // Fait disparaître la pièce de la scène.
        gameObject.SetActive(false);
    }
}
