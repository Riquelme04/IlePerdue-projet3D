// Importe les classes principales de Unity.
using UnityEngine;

// Déclare le composant responsable des dégâts causés par un ennemi.
public class EnnemiDegats3D : MonoBehaviour
{
    // Nombre de points de vie retirés au joueur lors d’une collision.
    //
    // SerializeField permet de modifier cette valeur dans l’Inspector
    // tout en conservant la variable privée.
    [SerializeField] private int dommages = 1;

    // Cette méthode est appelée automatiquement par Unity
    // lorsque l’ennemi entre en collision avec un autre objet.
    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si l’objet touché possède le tag « Player ».
        //
        // Si l’objet ne possède pas ce tag, la méthode est interrompue.
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // Recherche le composant VieRobot3D sur l’objet touché.
        //
        // Ce composant contient les informations concernant
        // les points de vie du robot.
        VieRobot3D vieRobot =
            collision.gameObject.GetComponent<VieRobot3D>();

        // Vérifie que le composant VieRobot3D a bien été trouvé.
        //
        // Cette vérification évite une erreur si l’objet portant
        // le tag Player ne possède pas le script VieRobot3D.
        if (vieRobot != null)
        {
            // Demande au robot de perdre le nombre de points de vie
            // indiqué par la variable dommages.
            vieRobot.SubirDegats(dommages);
        }
    }
}