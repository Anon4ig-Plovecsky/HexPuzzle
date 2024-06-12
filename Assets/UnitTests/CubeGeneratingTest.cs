using LevelsController.TestedModules;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using UI.TestedModules;
using NUnit.Framework;
using UnityEngine;
using System;

namespace UnitTests
{
    /// <summary>
    /// Class responsible for tests that generate puzzle pieces
    /// </summary>
    public class CubeGeneratingTest
    {
        private string _currentScene;

        public static IEnumerable<TestCaseData> CubeGeneratingUnityTestCases
        {
            get
            {
                yield return new TestCaseData(new LevelInfoTransfer(4, new Tuple<int, int>(2, 2), Array.Empty<string>(), CommonKeys.Names.SceneNature)).SetName("StartEmptyLevel").Returns(null);
                yield return new TestCaseData(new LevelInfoTransfer(8, new Tuple<int, int>(4, 5), new []{ "The Last Day of Pompeii - Karl Bryullov" }, CommonKeys.Names.SceneNature)).SetName("StartMainLevel").Returns(null);
                yield return new TestCaseData(new LevelInfoTransfer(8, new Tuple<int, int>(2, 3), new []{ "Error file name" }, CommonKeys.Names.SceneNature)).SetName("StartLevelWithError").Returns(null);
            }
        }
        
        /// <summary>
        /// Opens a game location with the specified level parameters,
        /// checks the success of creating cubes and textures on their sides
        /// </summary>
        /// <param name="levelInfo">Set of parameters for the level</param>
        /// <returns></returns>
        [UnityTest]
        [TestCaseSource(nameof(CubeGeneratingUnityTestCases))]
        public IEnumerator CubeGeneratingUnityTest(LevelInfoTransfer levelInfo)
        {
            _currentScene = SceneManager.GetActiveScene().name;
            var levelInfoTransfer = LevelInfoTransfer.SetInstance(levelInfo);
            levelInfoTransfer.IsTestMode = true;
            
            // Change scene
            Time.timeScale = 1;
            if (CanvasController.ClassCanvasController != null)
                CanvasController.DestroyClass();
            SceneManager.LoadScene(levelInfo.StrSceneName);

            // Waiting for the scene switch to complete
            const int maxCount = 10000;
            var iCounter = 0;
            while (CanvasController.ClassCanvasController == null || Time.timeScale == 0
                   || !SceneManager.GetActiveScene().name.Equals(CommonKeys.Names.SceneNature))
            {
                if(iCounter++ == maxCount)
                    Assert.Fail("Scene change error");
                yield return null;
            }

            // Check for cube creation
            var objCubesParent = GameObject.Find(CommonKeys.Names.Cubes);
            if (objCubesParent is null)
                Assert.Fail("Failed to get Cubes");
            if(objCubesParent.transform.childCount != levelInfo.GridSize.Item1 * levelInfo.GridSize.Item2)
                Assert.Fail("Incorrect number of cubes created");

            // Checking that there is an image on each side of the cube
            for (var i = 0; i < objCubesParent.transform.childCount; i++)
            {
                var transformCube = objCubesParent.transform.GetChild(i);
                if(transformCube is null)
                    Assert.Fail("Failed to get cube prefab");

                for(var side = 0; side < 6; side++)
                {
                    var objCubeSide = transformCube.GetChild(0).GetChild(side);
                    if (objCubeSide is null)
                        Assert.Fail($"Failed to get cube side {side} of {i} cube");

                    var rendererSide = objCubeSide.GetComponent<Renderer>();
                    if(rendererSide is null)
                        Assert.Fail($"Failed to get renderer cube side {side} of {i} cube");
                    
                    if(rendererSide.material is null)
                        Assert.Fail($"The {side}-th side of the {i}-th cube has no material");
                }                
            }
            
            Assert.Pass();
        }

        /// <summary>
        /// Switches the scene to the original after the end of each test
        /// </summary>
        [TearDown]
        public void ChangeBackScene()
        {
            // Change back scene
            Time.timeScale = 1;
            if (CanvasController.ClassCanvasController != null)
                CanvasController.DestroyClass();
            SceneManager.LoadScene(_currentScene);
        }
    }
}