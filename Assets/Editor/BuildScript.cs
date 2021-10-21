/******************************************************************
*    Author: Doug Guzman
*    Contributors: 
*    Date Created: 9/3/2021
*******************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DisasterGolf.Editor
{
    /// <summary>
    ///  Builds the game. Used by TeamCity for autobuilding
    /// </summary>
    public class BuildScript
    {

        /// <summary>
        /// More general build function. Specify the platform and the game will be built for it
        /// </summary>
        /// <param name="target">Platform to build to</param>
        public static void BuildGame(BuildTarget target)
        {
            // Output location for the built game
            var outDir = System.Environment.CurrentDirectory + "/BuildOutput/";
            
            string outputPath = "";
            switch (target)
            {
                case BuildTarget.StandaloneWindows64:
                    outDir += "Windows";
                    outputPath = Path.Combine(outDir, Application.productName + ".exe");
                    break;
                case BuildTarget.StandaloneOSX:
                    outDir += "Mac";
                    outputPath = Path.Combine(outDir, Application.productName);
                    break;
            }

            // If the above directory does not exist, create it
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            
            // If a previous build exists, delete it
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            
            // TODO: Addressables go here
            
            // Do the build
            UnityEditor.BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outputPath, target,
                BuildOptions.None);
            
            // Ensure the build was successful
            if (File.Exists(outputPath))
            {
                Debug.Log($"Build success : {outputPath}");
            }
            else
            {
                Debug.LogException(new Exception("Build failed! Please check the log!"));
            }
        }
        
        /// <summary>
        ///  Builds the game for Windows
        /// </summary>
        public static void BuildWindows()
        {
            BuildGame(BuildTarget.StandaloneWindows64);
        }

        /// <summary>
        /// Builds the game for MacOS
        /// </summary>
        public static void BuildMac()
        {
            BuildGame(BuildTarget.StandaloneOSX);
        }
    }
}
