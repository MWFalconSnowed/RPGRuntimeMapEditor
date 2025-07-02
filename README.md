# 🧩 Ultimate Runtime RPG Editor

Un éditeur **Unity 2D** complet intégré **dans le runtime** du jeu, permettant la création de **PolygonCollider2D**, le **spawn de joueur RPG**, l'**exportation JSON**, et l’analyse visuelle des zones de déplacement à partir d’une **image de map**.
![image](https://github.com/user-attachments/assets/f0841dae-01d5-45a4-b13c-8478ccc35285)

## 🎮 Fonctionnalités

### ✅ Éditeur de polygones en jeu
- Ajout de points avec la souris (zoom, pan, undo).
- Génération de colliders `PolygonCollider2D` dynamiquement.
- Détection automatique des zones jouables à partir d’une texture (auto-polygone).
- Visualisation avancée des colliders avec `PolygonColliderVisualizer`.

### 🧠 Analyseur et simplificateur
- Simplification via l’algorithme de **Douglas-Peucker**.
- Génération multiple à partir d’une texture (zones logiques).

### 👤 Système RPG intégré
- Composant `RPGPlayer` mobile avec Rigidbody2D.
- `PlayerSpawnerManager` pour instancier dynamiquement le joueur.
- `PlayerAnimator` pour gérer les frames animées (spritesheet).
- `MapBackground` pour charger une map PNG à l’échelle avec caméra auto-alignée.

### 🔧 Console en jeu
Tapez les commandes dans le `Console GUI` :
```
>spawn player
>export json
>clear poly
>zoom 3.5
>color red
```

### 🗂️ Structure des fichiers
```
Assets/
├── Scripts/
│   ├── UltimateRuntimeEditor.cs       # Éditeur principal avec auto génération
│   ├── PolygonAnalyzer.cs             # Simplification et analyse
│   ├── PolygonColliderVisualizer.cs   # Visualisation des colliders
│   ├── PolygonExtendedFeatures.cs     # Outils avancés (jitter, rotation, etc.)
│   ├── PolygonMetaTagger.cs           # Marqueurs logiques (ex: zone piège)
│   ├── RPGPlayer.cs                   # Contrôleur RPG simple
│   ├── PlayerSpawner.cs               # Script de spawn
│   ├── PlayerSpawnerManager.cs        # Gestion centralisée
│   ├── PlayerAnimator.cs              # Animation simple (spritesheet)
│   └── MapBackground.cs               # Charge une image de map 1980x1080
```

## 🔁 Exportation & JSON
- L’éditeur permet d’exporter la carte et ses polygones dans un fichier `polygon_data.json`.
- Les données peuvent être utilisées pour le **pathfinding, la collision, ou le level design**.

## 📸 Exemple d'utilisation
1. Chargez une map `.png` (MAP001).
2. Cliquez pour créer un polygone autour d’un obstacle.
3. Tapez `>spawn player` pour tester la navigation.
4. Cliquez sur `export` pour enregistrer les données.

## 🧪 Extensions prévues
- Système de couches logiques (`Sol`, `Obstacle`, `Trigger`).
- Importation de données JSON dans le build principal.
- Génération de pathfinding walkable grid.

---

## 🚀 Dépendances
- Unity 2020+ (testé sur 2022.3)
- Aucun package externe requis
- Compatible build `.exe` (standalone Windows)

## 🧙‍♂️ Auteur
Projet imaginé par **Maëlik**, un dev solo 🔥 passionné par les jeux évolutifs, la simulation atomique, et la magie interactive.
