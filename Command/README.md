# Command

## Explication

**Command** correspond à un **design pattern comportemental** (*behavioral design pattern*).

L'idée est d'**encapsuler une requête** (une action) dans un objet appelé **commande** (*command*). Une commande contient toutes les informations nécessaires pour exécuter l'action plus tard : **quoi faire**, **sur quel objet**, et éventuellement **avec quels paramètres**.

Ce pattern introduit généralement :
- un **Invoker** (ou *sender*) : déclenche l'exécution (bouton, queue...)
- une **Command** : interface commune avec une méthode d'exécution
- une ou plusieurs **ConcreteCommand** : implémentations des actions
- un **Receiver** : l'objet qui sait réellement effectuer le travail
- un **Client** : configure le tout (associe une commande à un invoker)

```mermaid
classDiagram
    class ICommand {
        <<interface>>
        +execute()
    }

    class IUndoableCommand {
        <<interface>>
        +undo()
    }

    class ConcreteCommandA {
        - receiver: Receiver
        +execute()
        +undo()
    }

    class ConcreteCommandB {
        - receiver: Receiver
        +execute()
    }

    class Receiver {
        +actionA()
        +actionB()
    }

    class Invoker {
        - command: ICommand
        +setCommand(command: ICommand)
        +invoke()
    }

    class Client

    ICommand <|-- IUndoableCommand
    IUndoableCommand <|.. ConcreteCommandA
    ICommand <|.. ConcreteCommandB
    ConcreteCommandA --> Receiver
    ConcreteCommandB --> Receiver
    Invoker --> ICommand
    Client --> Invoker
    Client --> ConcreteCommandA
    Client --> ConcreteCommandB
    Client --> Receiver
```

Lorsque le support de l'annulation est nécessaire, une interface `IUndoableCommand` étend `ICommand` en ajoutant une méthode `undo()`. Seules les commandes qui doivent supporter l'annulation implémentent cette interface, les autres implémentent simplement `ICommand`, sans être forcées à fournir une implémentation vide.

En pratique, le **client** ne déclenche pas directement le *receiver* : il passe par une commande, ce qui permet d'ajouter des fonctionnalités autour de l'action (historique, annulation, logs, retry, exécution différée, etc.).

## Besoin

Dans un système, on a souvent des actions qui doivent être :
- déclenchées depuis différents endroits (bouton, raccourci clavier, API...)
- mises en file (queue), planifiées, rejouées
- annulables (**undo/redo**)

Sans Command, l'Invoker (ex : un bouton) finit par appeler directement des méthodes métier, ce qui crée un **couplage fort**. Dès que l'on ajoute des responsabilités transverses comme la journalisation ou l'annulation, chaque point d'entrée doit les gérer individuellement :

```mermaid
graph LR
    ButtonSave -->|"appel direct"| DocumentService
    ButtonSave -->|"log manuel"| Logger
    ButtonSave -->|"undo manuel"| UndoManager
    ButtonPrint -->|"appel direct"| PrinterService
    ButtonPrint -->|"log manuel"| Logger
```

Le système devient difficile à faire évoluer :
- changer la logique d'exécution implique de toucher l'UI
- ajouter undo/redo impose de dupliquer de la logique partout
- impossible de mettre des actions en queue proprement

Les services appelés directement (`DocumentService`, `PrinterService`...) correspondent à ce que le pattern appelle les **Receivers** : les objets qui savent réellement effectuer le travail.

## Implémentation

La solution consiste à introduire des objets **Command** qui représentent des actions, et à faire en sorte que les **Invokers** ne connaissent que l'interface `ICommand`, pas le métier.

```mermaid
sequenceDiagram
    Client->>ConcreteCommand: new(receiver)
    Client->>Invoker: setCommand(command)
    Invoker->>ConcreteCommand: execute()
    ConcreteCommand->>Receiver: action()
    Receiver-->>ConcreteCommand: résultat
```

Ce diagramme met en évidence les deux phases distinctes du pattern : la phase de **configuration**, où le Client assemble les commandes et les injecte dans l'Invoker, et la phase d'**exécution**, où l'Invoker déclenche la commande sans rien savoir du Receiver ni de la logique métier.

- **Invoker** : ne sait qu'exécuter une commande
- **Command** : encapsule l'**intention** et la **délégation**
- **Receiver** : contient la logique métier réelle
- **Client** : instancie les commandes, injecte les receivers, et configure les invokers

Le pattern Command est surtout utile pour les systèmes qui :
- ont des actions déclenchées par une UI (boutons, menus) ou par un *orchestrateur*
- doivent tracer/auditer l'exécution des actions
- doivent proposer **undo/redo**
- doivent exécuter des actions **plus tard** (scheduler/queue) ou **à distance** (RPC/message bus)
- doivent composer des actions (macros, transactions applicatives)

## Limitations

> ⚠️ L'introduction de commandes peut ajouter de la complexité, surtout si les actions sont simples et ne nécessitent pas de fonctionnalités avancées. Il y a un risque d'**over-engineering**.

> ⚠️ Chaque action du système nécessite sa propre classe ConcreteCommand, ce qui peut mener à une **prolifération de classes** dans les systèmes comportant de nombreuses actions. Des approches comme les commandes paramétrées ou les lambdas (dans les langages qui le permettent) peuvent atténuer ce problème.

## Démonstration

[Code de démonstration](./CommandDemo.cs)

## Sources

https://refactoring.guru/design-patterns/command