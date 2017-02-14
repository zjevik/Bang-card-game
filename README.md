# Bang-card-game

This program was created by Ondrej Zjevik and Lukas Maly in 2010 as a final class project at VSB-Technical University of Ostrava. The program is an implementation of the card game Bang! (https://en.wikipedia.org/wiki/Bang!_(card_game)). The game is written in C# and it is using TCP/IP for communication between players and .NET libraries.

To start a game, you must setup a server first and then connect at least 3 clients because the game requires at least four players. Players then select their role card and play one after another.

To satisfy the project requirements we implemented a music player that saves the playlist in a XML file and it loads the saved file at the start of each game. The game can also save the current state or revert players moves.
