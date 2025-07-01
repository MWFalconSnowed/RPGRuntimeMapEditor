# RPG Runtime Map Editor 🧩✨(WIP)

Un éditeur de carte **intégré au runtime** dans Unity, inspiré des anciens éditeurs à la RPG Maker — mais avec une touche moderne, sombre et immersive.

---

## ✨ Fonctionnalités

- 🎮 **Éditeur intégré dans le build** `.exe`, pas besoin de rester dans l'éditeur Unity.
- 🧱 **Dessin de colliders polygonaux** avec clic gauche/droit.
- 🧙 **Spawn de joueur** avec stats RPG (attaque, défense, mana, etc).
- 🎨 **GUI Dark Mode Glass** immersif avec HUD stylisé.
- 🗺️ Compatible avec des maps 1980x1080 en pixel art.
- 🖱️ Contrôles simples et accessibles :
  - Clic gauche : dessiner un point de polygone
  - Clic droit : fermer le polygone
  - Bouton "Spawn Player" : faire apparaître un joueur dans la scène

---
![{E9205E5F-01D4-461E-B9F6-C71AC0E3C9FF}](https://github.com/user-attachments/assets/d6adc740-7348-41f7-b831-711d4db90358)


## 🏁 Démarrage rapide

1. Ouvrir le projet Unity (`2019.4.40f1` recommandé)
2. Ouvrir `SampleScene` et cliquer sur ▶️ **Play**
3. Dans la scène :
   - Utilisez l’outil GUI pour créer des colliders.
   - Cliquez sur "Spawn Player" pour tester l’apparition du joueur.

---

## 📁 Structure des scripts

- `PolygonEditorRuntimeGUI.cs` – gestion des colliders et de l’éditeur visuel
- `PlayerPanelRuntime.cs` – HUD RPG avec bouton de spawn
- `RPGPlayer.cs` – gestion du joueur et de ses stats
- `PolygonColliderGizmos.cs` – dessin des colliders avec Gizmos

---

## 🔒 Licence

Ce projet est open-source sous licence MIT.

---

## 👤 Auteur

**MWFalconSnowed** – Développeur solo Unity | Projet RPG immersif
