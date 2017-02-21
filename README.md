# Bang-card-game

This program was created by Ondrej Zjevik and Lukas Maly in 2010 as a final class project at VSB-Technical University of Ostrava. The program is an implementation of the card game Bang! (https://en.wikipedia.org/wiki/Bang!_(card_game)). The game is written in C# and uses TCP/IP for communication between players and .NET libraries.

To start a new game, you must first set up a server and then connect at least 3 clients. The card game Bang! requires at least four players. The players then select their role card and take turns.

To satisfy the project requirements, we implemented a music player that saves the playlist in an XML file which loads the saved file at the start of each game. The game can also save the current state or revert player's moves.
