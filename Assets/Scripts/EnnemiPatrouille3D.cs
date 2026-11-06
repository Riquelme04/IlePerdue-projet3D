// Importe les classes principales de Unity.
using UnityEngine;

// Oblige l’objet à posséder un composant Rigidbody.
// Si le composant est absent, Unity l’ajoute automatiquement.
[RequireComponent(typeof(Rigidbody))]
public class EnnemiPatrouille3D : MonoBehaviour
{
    // Liste des différents comportements possibles de l’ennemi.
    private enum EtatEnnemi
    {
        Patrouille, // L’ennemi se déplace entre les points de patrouille.
        Poursuite,  // L’ennemi poursuit le joueur.
        Retour      // L’ennemi retourne vers son parcours de patrouille.
    }

    // Affiche une section « Patrouille » dans l’Inspector.
    [Header("Patrouille")]

    // Tableau contenant les points que l’ennemi doit parcourir.
    [SerializeField] private Transform[] pointsPatrouille;

    // Vitesse utilisée pendant la patrouille.
    [SerializeField] private float vitessePatrouille = 2f;

    // Distance minimale permettant de considérer qu’un point est atteint.
    [SerializeField] private float distanceChangementPoint = 0.25f;

    // Affiche une section « Poursuite » dans l’Inspector.
    [Header("Poursuite")]

    // Référence vers le Transform du joueur.
    [SerializeField] private Transform joueur;

    // Vitesse utilisée lorsque l’ennemi poursuit le joueur.
    [SerializeField] private float vitessePoursuite = 3.5f;

    // Distance à partir de laquelle l’ennemi détecte le joueur.
    [SerializeField] private float distanceDetection = 6f;

    // Distance à partir de laquelle l’ennemi abandonne la poursuite.
    [SerializeField] private float distanceAbandon = 9f;

    // Affiche une section « Rotation » dans l’Inspector.
    [Header("Rotation")]

    // Contrôle la rapidité avec laquelle l’ennemi tourne.
    [SerializeField] private float vitesseRotation = 8f;

    // Référence vers le Rigidbody de l’ennemi.
    private Rigidbody corps;

    // État actuel de l’ennemi.
    // Au démarrage, l’ennemi est en mode Patrouille.
    private EtatEnnemi etat = EtatEnnemi.Patrouille;

    // Position actuelle dans le tableau des points de patrouille.
    private int indexPoint;

    // Permet de configurer l’ennemi à partir d’un autre script.
    public void Configurer(Transform cible, Transform[] points)
    {
        // Enregistre la cible que l’ennemi devra poursuivre.
        joueur = cible;

        // Enregistre les points que l’ennemi devra parcourir.
        pointsPatrouille = points;
    }

    // Awake est exécutée lorsque l’objet est chargé.
    private void Awake()
    {
        // Récupère le composant Rigidbody attaché à l’ennemi.
        corps = GetComponent<Rigidbody>();

        // Rend le Rigidbody cinématique.
        // Le déplacement sera contrôlé par le script.
        corps.isKinematic = false;

        // Rend les déplacements visuellement plus fluides.
        corps.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // FixedUpdate est exécutée à intervalles réguliers.
    // Elle est recommandée pour les déplacements utilisant un Rigidbody.
    private void FixedUpdate()
    {
        // Vérifie que le joueur et les points de patrouille sont configurés.
        if (joueur == null ||
            pointsPatrouille == null ||
            pointsPatrouille.Length == 0)
        {
            // Interrompt cette exécution si une référence est manquante.
            return;
        }

        // Calcule la distance entre l’ennemi et le joueur.
        float distanceJoueur = Vector3.Distance(
            transform.position,
            joueur.position
        );

        // Si le joueur entre dans la zone de détection,
        // l’ennemi commence à le poursuivre.
        if (distanceJoueur <= distanceDetection)
        {
            etat = EtatEnnemi.Poursuite;
        }
        // Si l’ennemi poursuivait le joueur et que celui-ci est trop loin,
        // l’ennemi retourne vers son parcours.
        else if (
            etat == EtatEnnemi.Poursuite &&
            distanceJoueur >= distanceAbandon
        )
        {
            etat = EtatEnnemi.Retour;
        }

        // Exécute le comportement correspondant à l’état actuel.
        if (etat == EtatEnnemi.Poursuite)
        {
            // Déplace l’ennemi vers la position du joueur.
            DeplacerVers(joueur.position, vitessePoursuite);
        }
        else
        {
            // Continue ou reprend le parcours de patrouille.
            Patrouiller();
        }
    }

    // Contrôle le déplacement entre les points de patrouille.
    private void Patrouiller()
    {
        // Récupère le point actuellement visé.
        Transform pointCible = pointsPatrouille[indexPoint];

        // Déplace l’ennemi vers ce point.
        DeplacerVers(pointCible.position, vitessePatrouille);

        // Calcule le vecteur entre l’ennemi et le point cible.
        Vector3 ecartPoint =
            pointCible.position - transform.position;

        // Ignore la différence de hauteur.
        // L’ennemi se déplace uniquement sur le plan horizontal.
        ecartPoint.y = 0f;

        // Calcule la distance restante jusqu’au point.
        float distancePoint = ecartPoint.magnitude;

        // Si le point n’est pas encore atteint,
        // l’ennemi continue son déplacement.
        if (distancePoint > distanceChangementPoint)
        {
            return;
        }

        // Passe au prochain point du tableau.
        // L’opérateur % permet de revenir au premier point
        // après avoir atteint le dernier.
        indexPoint =
            (indexPoint + 1) % pointsPatrouille.Length;

        // Replace l’ennemi dans l’état Patrouille.
        etat = EtatEnnemi.Patrouille;
    }

    // Déplace l’ennemi vers une destination donnée.
    private void DeplacerVers(
        Vector3 destination,
        float vitesse
    )
    {
        // Calcule la direction entre l’ennemi et la destination.
        Vector3 direction = destination - corps.position;

        // Supprime le déplacement vertical.
        direction.y = 0f;

        // Vérifie que la destination n’est pas déjà atteinte.
        // sqrMagnitude est utilisée pour éviter un calcul de racine carrée.
        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        // Calcule la prochaine position du Rigidbody.
        Vector3 nouvellePosition =
            corps.position
            + direction.normalized
            * vitesse
            * Time.fixedDeltaTime;

        // Déplace le Rigidbody vers la nouvelle position.
        corps.MovePosition(nouvellePosition);

        // Calcule la rotation qui regarde dans la direction du déplacement.
        Quaternion rotationCible = Quaternion.LookRotation(
            direction.normalized,
            Vector3.up
        );

        // Crée une transition progressive entre la rotation actuelle
        // et la rotation souhaitée.
        Quaternion nouvelleRotation = Quaternion.Slerp(
            corps.rotation,
            rotationCible,
            vitesseRotation * Time.fixedDeltaTime
        );

        // Applique la nouvelle rotation au Rigidbody.
        corps.MoveRotation(nouvelleRotation);
    }

    // Affiche des repères visuels lorsque l’ennemi
    // est sélectionné dans la fenêtre Scene.
    private void OnDrawGizmosSelected()
    {
        // Sélectionne la couleur jaune.
        Gizmos.color = Color.yellow;

        // Dessine la zone de détection du joueur.
        Gizmos.DrawWireSphere(
            transform.position,
            distanceDetection
        );

        // Sélectionne la couleur rouge.
        Gizmos.color = Color.red;

        // Dessine la zone au-delà de laquelle
        // l’ennemi abandonne la poursuite.
        Gizmos.DrawWireSphere(
            transform.position,
            distanceAbandon
        );
    }
}