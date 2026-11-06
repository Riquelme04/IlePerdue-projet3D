# Contrôleur de robot 3D – Unity
 
Petit projet Unity (URP) où un personnage en forme de capsule se déplace sur un terrain montagneux grâce à la physique (Rigidbody). Une caméra suit le joueur.
 
---
 
## Aperçu
 
### Vue de la scène (éditeur)
<img width="1536" height="784" alt="image" src="https://github.com/user-attachments/assets/3140e9d6-c181-42bb-a193-716ea905b809" />

 
### Vue en jeu
<img width="1221" height="688" alt="image" src="https://github.com/user-attachments/assets/41dfaefb-5cc5-47a2-9dc1-01be6252fe03" />

 
---
 
## Ce qui a été fait
 
### 1. Le terrain
- Création d'un **Terrain** sculpté avec des montagnes et des reliefs sur les bords et une zone plus plate au centre.
- Ajout d'une texture en damier pour bien voir le relief et les déplacements.
- Ajout d'un objet **Sol** sous le terrain.
### 2. Le joueur (`Player`)
Capsule avec un matériau brun (`New Material`, shader *Universal Render Pipeline/Lit*) et les composants suivants :
 
| Composant | Configuration |
|---|---|
| **Capsule Collider** | Rayon `0.5`, hauteur `2`, direction Y |
| **Rigidbody** | Masse `1`, Angular Damping `0.05`, gravité activée, collisions *Discrete* |
| **Contraintes** | Rotation bloquée sur **X** et **Z** pour que la capsule ne tombe pas |
| **ControleurRobot3D** (script) | `Vitesse = 5`, `Vitesse Rotation = 10` |
 
### 3. Les scripts
Dans `Assets/Scripts` :
- **`ControleurRobot3D.cs`** : déplace et fait tourner le joueur selon les entrées du joueur, avec une vitesse et une vitesse de rotation réglables dans l'Inspector.
- **`CameraSuivi.cs`** : fait suivre le joueur par la caméra.
### 4. La scène (`Scene1`)
```
Scene1
├── Main Camera        (suit le joueur)
├── Directional Light  (éclairage)
├── Player             (capsule contrôlée)
├── Sol
└── Terrain
```
 
---
 
## Structure du projet
```
Assets/
├── Materials/
├── Scenes/
├── Scripts/
│   ├── CameraSuivi.cs
│   └── ControleurRobot3D.cs
├── Settings/
├── Textures/
└── TutorialInfo/
```
 
---
 
## Lancer le projet
1. Ouvrir le projet dans **Unity** (avec le pipeline URP).
2. Ouvrir la scène `Assets/Scenes/Scene1`.
3. Appuyer sur **Play** et déplacer le joueur avec les contrôles prévus dans `ControleurRobot3D`.
---
 
## Auteur
Diego Fanara Alvarez – Cégep Marie-Victorin
