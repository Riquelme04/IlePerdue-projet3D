// Importe les outils nécessaires aux coroutines,
// notamment IEnumerator.
using System.Collections;

// Importe les classes principales de Unity.
using UnityEngine;

// Importe les composants d’interface utilisateur,
// notamment le composant Text.
using UnityEngine.UI;

// Oblige l’objet à posséder un composant Rigidbody.
// Unity ajoute automatiquement ce composant s’il est absent.
[RequireComponent(typeof(Rigidbody))]
public class VieRobot3D : MonoBehaviour
{
    // Nombre de vies disponibles au début de la partie.
    [SerializeField] private int viesMaximales = 3;

    // Point vers lequel le robot est déplacé après avoir subi des dégâts.
    [SerializeField] private Transform pointReapparition;

    // Durée pendant laquelle le robot ne peut plus subir de dégâts.
    [SerializeField] private float dureeInvincibilite = 1.5f;

    // Élément d’interface utilisé pour afficher le nombre de vies.
    [SerializeField] private Text texteVies;

    // Référence vers le Rigidbody du robot.
    private Rigidbody corps;

    // Nombre de vies restantes pendant la partie.
    private int viesActuelles;

    // Indique si le robot est temporairement invincible.
    private bool invincible;

    // Permet de configurer le point de réapparition et le texte
    // à partir d’un autre script.
    public void Configurer(Transform point, Text affichage)
    {
        // Enregistre le point de réapparition.
        pointReapparition = point;

        // Enregistre le composant Text utilisé pour afficher les vies.
        texteVies = affichage;
    }

    // Awake est exécutée lorsque l’objet est chargé.
    private void Awake()
    {
        // Récupère le composant Rigidbody du robot.
        corps = GetComponent<Rigidbody>();

        // Initialise les vies actuelles avec le nombre maximal de vies.
        viesActuelles = viesMaximales;
    }

    // Start est exécutée avant la première image du jeu.
    private void Start()
    {
        // Affiche le nombre initial de vies.
        MettreAJourAffichage();
    }

    // Méthode publique appelée lorsqu’un ennemi touche le robot.
    public void SubirDegats(int dommages)
    {
        // Vérifie si le robot est invincible
        // ou s’il ne possède déjà plus de vies.
        if (invincible || viesActuelles <= 0)
        {
            // Interrompt la méthode sans retirer de vie.
            return;
        }

        // Retire les dégâts du nombre de vies actuelles.
        //
        // Mathf.Max empêche le nombre de vies de descendre
        // sous la valeur zéro.
        viesActuelles = Mathf.Max(
            0,
            viesActuelles - dommages
        );

        // Actualise le texte affiché à l’écran.
        MettreAJourAffichage();

        // Vérifie qu’un point de réapparition a été configuré.
        if (pointReapparition != null)
        {
            // Replace le robot au point de réapparition.
            Reapparaitre();
        }

        // Vérifie si le robot ne possède plus aucune vie.
        if (viesActuelles == 0)
        {
            // Recherche le script responsable du déplacement du robot.
            ControleurRobot3D controleur =
                GetComponent<ControleurRobot3D>();

            // Vérifie que le contrôleur a été trouvé.
            if (controleur != null)
            {
                // Désactive le déplacement du robot.
                controleur.enabled = false;
            }

            // Affiche un message de fin de partie dans la Console.
            Debug.Log(
                "Partie terminee : le robot n'a plus de vies."
            );

            // Termine immédiatement la méthode.
            return;
        }

        // Lance la période d’invincibilité temporaire.
        StartCoroutine(InvincibiliteTemporaire());
    }

    // Replace le robot à son point de réapparition.
    private void Reapparaitre()
    {
        // Modifie directement la position du Rigidbody.
        corps.position = pointReapparition.position;

        // Annule la vitesse de déplacement du robot.
        corps.linearVelocity = Vector3.zero;

        // Annule sa vitesse de rotation.
        corps.angularVelocity = Vector3.zero;
    }

    // Coroutine responsable de l’invincibilité temporaire.
    private IEnumerator InvincibiliteTemporaire()
    {
        // Active l’invincibilité.
        invincible = true;

        // Attend le nombre de secondes configuré dans l’Inspector.
        yield return new WaitForSeconds(dureeInvincibilite);

        // Désactive l’invincibilité après l’attente.
        invincible = false;
    }

    // Actualise le nombre de vies affiché à l’écran.
    private void MettreAJourAffichage()
    {
        // Vérifie que le composant Text a été assigné.
        if (texteVies != null)
        {
            // Modifie le contenu du texte.
            texteVies.text = $"Vies : {viesActuelles}";
        }
    }
}