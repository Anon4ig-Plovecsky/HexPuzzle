using UnityEngine.ResourceManagement.AsyncOperations;
using LevelsController.TestedModules;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System;

namespace UnitTests
{
    /// <summary>
    /// Class responsible for image cropping tests
    /// </summary>
    public class CroppingTest
    {
        private const string TexturePath = "Paintings";

        private List<Sprite> _listSpriteImages = new();

        /// <summary>
        /// Obtaining resources for pruning. Executes an asynchronous method in which it
        /// dumps a list of all sprites into _listSpriteImages. In order to use _listSpriteImages
        /// you must first ensure that the asynchronous method was executed successfully
        /// </summary>
        [SetUp]
        public void CroppingSetUp()
        {
            var texturesLabelReference = new AssetLabelReference
            {
                labelString = TexturePath
            };
            var asyncOperationHandleListTextures = Addressables.LoadAssetsAsync<Sprite>(texturesLabelReference, _ => {});
            asyncOperationHandleListTextures.Completed += delegate
            {
                if (asyncOperationHandleListTextures.Status == AsyncOperationStatus.Succeeded)
                    _listSpriteImages = new List<Sprite>(asyncOperationHandleListTextures.Result);
            };
        }
    
        #nullable enable
        [UnityTest]
        [TestCase("Lovers - Rene Magritte", new[] { 3, 4 }, 398, ExpectedResult = null)]
        [TestCase("Moonlight Night on the Dnieper - Arkhip Ivanovich Kuindzhi", new[] { 4, 5 }, 217, ExpectedResult = null)]
        [TestCase("Still Life With Basket, Flowers And Fruit - Balthasar van der Ast", new[] { 6, 5 }, 152, ExpectedResult = null)]
        public IEnumerator CroppingUnityTest(string strSpriteName, int[] gridSize, int expectedPartSize)
        {
            if(gridSize.Length != 2)
                Assert.Fail("Invalid array length specified");
            // Continue executing the test after has been received all the necessary resources.
            // Also add a counter to avoid endless loop
            const int maxCount = 10000;
            var iCounter = 0;
            while (_listSpriteImages.Count == 0)
            {
                if (iCounter++ == maxCount)
                    Assert.Fail("Failed to get resource sheet");
                yield return null;
            }

            // Search for a specified image
            Sprite? sprite = null;
            foreach (var spriteImage in _listSpriteImages.Where(spriteImage => spriteImage.name.Equals(strSpriteName)))
                sprite = spriteImage;
            if(sprite == null)
                Assert.Fail();
            
            // Cropping a found image
            var imageCreator = new ImageCreator(sprite, new Tuple<int, int>(gridSize[0], gridSize[1]));
            imageCreator.CropPaintingByParts();
            var arrSpriteParts = imageCreator.GetPartsOfPainting();
            if(arrSpriteParts is null || arrSpriteParts.Length != gridSize[0] * gridSize[1])
                Assert.Fail("Not all puzzle pieces received");

            const float tolerance = 1e-3f;
            
            // Comparison of the expected size of the cropped part of the image with the actual one
            Assert.True(arrSpriteParts != null && arrSpriteParts.Count(spritePart => Math.Abs(expectedPartSize - spritePart.rect.width) < tolerance
                && Math.Abs(expectedPartSize - spritePart.rect.height) < tolerance) == arrSpriteParts.Length);
        }
        #nullable disable
    }
}
