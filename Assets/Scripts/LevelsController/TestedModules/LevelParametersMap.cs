using System.Collections.Generic;
using System;

namespace LevelsController.TestedModules
{
    public abstract record LevelParametersMap
    {
        public static readonly Dictionary<int, LevelInfoTransfer> LevelInfo = new()
        {
            { 1, new LevelInfoTransfer(1, new Tuple<int, int>(2, 3), new List<string>
                {
                    new("The Persistence of Memory - Salvador Dali")
                })},
            
            { 2, new LevelInfoTransfer(2, new Tuple<int, int>(2, 3), new List<string>
                {
                    new("Lovers - Rene Magritte"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast")
                })},
            
            { 3, new LevelInfoTransfer(3, new Tuple<int, int>(2, 3), new List<string>
                {
                    new("The Last Day of Pompeii - Karl Bryullov"),
                    new("The Last Supper - Leonardo da Vinci"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast")
                })},
            
            { 4, new LevelInfoTransfer(4, new Tuple<int, int>(3, 4), new List<string>
                {
                    new("The Last Supper - Leonardo da Vinci"),
                    new("The Persistence of Memory - Salvador Dali"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast"),
                    new("Lovers - Rene Magritte")
                })},
            { 5, new LevelInfoTransfer(5, new Tuple<int, int>(3, 4), new List<string>
                {
                    new("Morning in a pine forest - Ivan Shishkin and Konstantin Konstantin Savitsky 1889"),
                    new("The Last Supper - Leonardo da Vinci"),
                    new("The Last Day of Pompeii - Karl Bryullov"),
                    new("The Persistence of Memory - Salvador Dali"),
                    new("Lovers - Rene Magritte")
                })},
            
            { 6, new LevelInfoTransfer(6, new Tuple<int, int>(4, 5), new List<string>
                {
                    new("Rainbow - Ivan Aivazovsky"),
                    new("Morning in a pine forest - Ivan Shishkin and Konstantin Konstantin Savitsky 1889"),
                    new("Golden Autumn - Isaac Levitan"),
                    new("The Last Day of Pompeii - Karl Bryullov"),
                    new("The Persistence of Memory - Salvador Dali"),
                    new("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast")
                })},
        };
    }
}