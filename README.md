# TP Design Patterns

## Langage

C# 14 (.NET 10)

## Contenu

Chaque Design Pattern dispose de son propre dossier contenant :

- Un fichier `README.md` avec la description du pattern, le besoin auquel il répond, la façon dont il peut être implémenté, mais aussi les inconvénients qui peuvent en découler. On y retrouve le lien vers le code de démonstration ainsi que la/les source(s) utilisée(s).
- Le code source de l'implémentation avec un exemple original.

Les diagrammes Mermaid sont lisibles directement sur GitHub. Pour une lecture locale, vous pouvez utiliser :

- **Visual Studio** : extension Markdown Editor v2 de Mads Kristensen
- **Visual Studio Code** : extension Markdown Preview Enhanced de Yiyi Wang

## Lancer les démos

Avec la CLI .NET :

```bash
dotnet run
```

Ou directement depuis Visual Studio / Visual Studio Code (F5 ou bouton Run).

Un menu interactif s'affiche dans la console :

```
====================================================================
Menu des Design Patterns
====================================================================
Utilisez ↑/↓ pour naviguer, Entrée pour valider, ou tapez un numéro.

⦿ 0 - Exécuter toutes les démos
○ 1 - Mediator
○ 2 - Singleton
○ 3 - Observer
○ 4 - Decorator
○ 5 - Command
○ 6 - Adapter
○ 7 - Strategy
○ 8 - Composite
○ 9 - Facade
○ 10 - Builder
○ 11 - State
○ 12 - Abstract Factory
○ 13 - Factory Method
```

Naviguer avec les flèches directionnelles et valider avec Entrée, ou taper directement le numéro correspondant.

Chaque démo affiche le déroulement de l'exemple. Il est recommandé d'aller voir le fichier de code de la démo pour comprendre les détails de l'implémentation.