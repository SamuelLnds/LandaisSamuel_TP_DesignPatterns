# Decorator

## Explication

Un **décorateur** (*decorator*) est un **design pattern structurel** (*structural design pattern*) qui permet d'ajouter **dynamiquement** des comportements à un objet sans modifier sa classe. Il repose sur le principe de **composition** plutôt que d'**héritage** : un décorateur encapsule un objet cible et délègue les appels tout en ajoutant sa propre logique.

Contrairement à une sous-classe classique, les fonctionnalités peuvent être **empilées dynamiquement** à l’exécution.

```mermaid
classDiagram
	class IComponent {
		<<interface>>
		+operation()
	}

	class ConcreteComponent {
		+operation()
	}

	class Decorator {
		- component: IComponent
		+operation()
	}

	class ConcreteDecoratorA {
		+operation()
		+addedBehavior()
	}

	class ConcreteDecoratorB {
		+operation()
	}

	IComponent <|.. ConcreteComponent
	IComponent <|.. Decorator
	Decorator o--> IComponent
	Decorator <|-- ConcreteDecoratorA
	Decorator <|-- ConcreteDecoratorB

```

Le point clé est la **relation de composition** : le décorateur contient une référence vers un **Component**.

## Besoin

Le **Decorator** devient pertinent lorsque :

- on veut ajouter des responsabilités à un objet sans multiplier les sous-classes
- les combinaisons de comportements deviennent nombreuses
- certaines fonctionnalités doivent être activées dynamiquement

Sans mettre en place ce Design Pattern, on peut risquer une **explosion de sous-classes** :
```mermaid
graph LR
	Base --> Logging
	Base --> Caching
	Base --> Security
	Logging --> LoggingCaching
	Logging --> LoggingCachingSecurity
	Logging --> LoggingSecurity
	Caching --> CachingSecurity
```

## Implémentation

La solution consiste à introduire une classe **Décorateur** qui implémente la même interface que le **Composant**, tout en contenant une *référence* vers un objet de type composant.

Les *décorateurs concrets* encapsulent un composant existant et ajoutent des comportements avant ou après l’appel aux méthodes du composant encapsulé. Ainsi, le client manipule toujours une instance du composant, sans savoir s’il s’agit d’un objet décoré ou non.

Un décorateur permet de limiter l'explosion de sous-classes en combinant les comportements :
```mermaid
graph LR
	Client --> DecoratorA
	DecoratorA --> DecoratorB
	DecoratorB --> ConcreteComponent
```

Si on reprend l'exemple ci-dessus, il serait transformé en :

```mermaid
graph TD
    Client --> CacheDecorator
    CacheDecorator --> LoggingDecorator
    LoggingDecorator --> ConcreteComponent
```

## Limitations

> ⚠️ L'utilisation excessive de décorateurs peut rendre le code difficile à comprendre. Devoir suivre une longue chaîne de décorateurs est moins lisible.

> ⚠️ Il est difficile de retirer un comportement ajouté par un décorateur une fois qu'il a été appliqué, car les décorateurs sont généralement conçus pour être empilés. Ainsi, une fois qu'un décorateur est appliqué, il devient une partie intégrante de l'objet décoré.

## Démonstration

[Code de démonstration](./DecoratorDemo.cs)

## Sources

https://refactoring.guru/design-patterns/decorator