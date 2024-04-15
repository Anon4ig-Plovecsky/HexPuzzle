using System.Collections.Generic;
using System;

namespace LevelsController
{
    public abstract record LevelParametersMap
    {
        public static readonly Dictionary<int, LevelInfoTransfer> LevelInfo = new()
        {
            { 1, new LevelInfoTransfer(1, new Tuple<int, int>(3, 4), new List<string>
                {
                    new("The Persistence of Memory - Salvador Dali.jpg")
                })},
            
            { 2, new LevelInfoTransfer(2, new Tuple<int, int>(3, 4), new List<string>
                {
                    new("Lovers - Rene Magritte.jpg"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast.jpg")
                })},
            
            { 3, new LevelInfoTransfer(3, new Tuple<int, int>(3, 4), new List<string>
                {
                    new("The Last Day of Pompeii - Karl Bryullov.jpg"),
                    new("The Last Supper - Leonardo da Vinci.jpg"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast.jpg")
                })},
            
            { 4, new LevelInfoTransfer(4, new Tuple<int, int>(3, 4), new List<string>
                {
                    new("The Last Supper - Leonardo da Vinci.jpg"),
                    new("The Persistence of Memory - Salvador Dali.jpg"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast.jpg"),
                    new("Lovers - Rene Magritte.jpg")
                })},
            { 5, new LevelInfoTransfer(5, new Tuple<int, int>(3, 4), new List<string>
                {
                    new("Morning in a pine forest - Ivan Shishkin and Konstantin Konstantin Savitsky 1889.jpg"),
                    new("The Last Supper - Leonardo da Vinci.jpg"),
                    new("The Last Day of Pompeii - Karl Bryullov.jpg"),
                    new("The Persistence of Memory - Salvador Dali.jpg"),
                    new("Lovers - Rene Magritte.jpg")
                })},
            
            { 6, new LevelInfoTransfer(6, new Tuple<int, int>(4, 5), new List<string>
                {
                    new("Rainbow - Ivan Aivazovsky.jpg"),
                    new("Morning in a pine forest - Ivan Shishkin and Konstantin Konstantin Savitsky 1889.jpg"),
                    new("Golden Autumn - Isaac Levitan.jpg"),
                    new("The Last Day of Pompeii - Karl Bryullov.jpg"),
                    new("The Persistence of Memory - Salvador Dali.jpg"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast.jpg")
                })},
        };
    }
}