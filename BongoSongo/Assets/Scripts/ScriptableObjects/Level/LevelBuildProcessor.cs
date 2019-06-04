//#if UNITY_EDITOR
//using System.IO;
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.Build;
//using UnityEditor.Build.Reporting;

//class MyCustomBuildProcessor : IPreprocessBuildWithReport {
//    public int callbackOrder { get { return 0; } }

//    public void OnPreprocessBuild(BuildReport report) {

//        Debug.Log("Preprocessing levels...");

//        // To copy a folder's contents to a new location:
//        // Create a new target folder, if necessary.
//        if (!System.IO.Directory.Exists(Level.FolderRelease)) {
//            System.IO.Directory.CreateDirectory(Level.FolderRelease);
//        }


//        Debug.Log($"Copying level files to {Level.FolderRelease}");

//        // To copy all the files in one directory to another directory.
//        // Get the files in the source folder. (To recursively iterate through
//        // all subfolders under the current directory, see
//        // "How to: Iterate Through a Directory Tree.")
//        // Note: Check for target path was performed previously
//        //       in this code example.
//        if (System.IO.Directory.Exists(Level.FolderEditor)) {
//            string[] files = System.IO.Directory.GetFiles(Level.FolderEditor);

//            // Copy the files and overwrite destination files if they already exist.
//            foreach (string s in files) {
//                // Use static Path methods to extract only the file name from the path.
//                var fileName = System.IO.Path.GetFileName(s);
//                var destFile = System.IO.Path.Combine(Level.FolderRelease, fileName);
//                System.IO.File.Copy(s, destFile, true);
//            }
//        }
//        else {
//            Debug.LogError("Source path does not exist!");
//        }
//    }
//}
//#endif