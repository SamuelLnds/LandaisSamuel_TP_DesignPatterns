# Mediator

## Explication

**Mediator** est un **design pattern comportemental** (*behavioral design pattern*). Le **médiateur** agit comme *chef d'orchestre* entre des **composants** (*components*) : il centralise la logique de communication et supprime les dépendances directes entre composants. Chaque composant connaît uniquement le médiateur.

```mermaid
classDiagram
    class IMediator {
        <<interface>>
        +notify(sender: object, event: string)
    }

    class ConcreteMediator {
        +notify(sender: object, event: string)
    }

    class BaseComponent {
        - mediator: IMediator
    }

    class ComponentA {
        +do1()
    }

    class ComponentB {
        +do2()
    }

    class ComponentC {
        +do3()
    }

    IMediator <|.. ConcreteMediator
    BaseComponent --> IMediator
    BaseComponent <|-- ComponentA
    BaseComponent <|-- ComponentB
    BaseComponent <|-- ComponentC
    ConcreteMediator --> ComponentA
    ConcreteMediator --> ComponentB
    ConcreteMediator --> ComponentC
```

## Besoin

Dans un système complexe, les composants peuvent être fortement **couplés**, ce qui rend le code difficile à maintenir, à faire évoluer, et les composants sont difficilement réutilisables. Ces systèmes prennent une forme *spaghetti* :

```mermaid
graph LR
    A --> B
    B --> C
    C --> A
    D --> A
    D --> B
    C --> D
    E --> A
    E --> C
    E --> B
```

Le pattern **Mediator** est particulièrement pertinent lorsque :

- des classes sont fortement couplées à d'autres classes qui interagissent entre elles
- des classes ne sont pas ou peu réutilisables à cause de leur couplage, forçant la création de classes proches

## Implémentation

Chaque composant communique uniquement avec le médiateur via `notify()`. Le médiateur décide seul quels composants réagissent à quel événement :

```mermaid
sequenceDiagram
    ComponentA->>Mediator: notify(this, "event1")
    Mediator->>ComponentB: do2()
    Mediator->>ComponentC: do3()
```

La communication centralisée donne au système une structure claire :

```mermaid
graph TD
    M --> A & B & C & D & E
    A & B & C & D & E --> M
```

## Limitations

> ⚠️ Le médiateur peut devenir un **God object**, c'est-à-dire une classe qui connaît et gère tout. Ces classes deviennent difficiles à comprendre, même si elles respectent le principe de responsabilité unique sur papier. Une classe trop volumineuse doit malgré tout être fragmentée.

> ⚠️ Si les composants dépendent d'une implémentation concrète du médiateur plutôt que d'une abstraction (`IMediator`), leur testabilité en isolation est compromise.

## Démonstration

[Code de démonstration](./MediatorDemo.cs)

## Sources

https://refactoring.guru/design-patterns/mediator
https://en.wikipedia.org/wiki/God_object