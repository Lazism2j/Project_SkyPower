using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IO
{
    /// <summary>
    /// # Summary
    /// Abstract base class for handling data saving and loading operations in Unity.
    /// Provides a common interface and utility methods for derived save handler classes.
    /// ����Ƽ���� ������ ���� �� �ε� �۾��� ó���ϱ� ���� �߻� �⺻ Ŭ����.
    /// �Ļ��� ���� �ڵ鷯 Ŭ�������� ���� ���� �������̽��� ��ƿ��Ƽ �޼��带 �����Ѵ�.
    ///
    /// ## Usage:
    /// - Inherit from this class to create custom save handlers for different save types (e.g., JSON, Binary).
    /// - Implement the `Save` and `Load` methods to handle specific save logic for different formats.
    /// ## ��� ���:
    /// - �� Ŭ������ ����Ͽ� �ٸ� ���� ����(JSON, Binary ��)�� ���� ����� ���� ���� �ڵ鷯�� �����Ѵ�.
    /// - `Save` �� `Load` �޼��带 �����Ͽ� �پ��� ���Ŀ� ���� Ư�� ���� ������ ó���Ѵ�.
    ///
    /// ## Features:
    /// - Provides base path determination for saving files based on the environment (Editor or Build).
    /// - Includes utility methods to check file accessibility and validity.
    /// - Offers an interface for derived classes to implement their own save/load logic.
    /// ## ���:
    /// - ȯ��(Editor �Ǵ� Build)�� ���� ������ ������ �⺻ ��θ� �����Ѵ�.
    /// - ���� ���ټ��� ��ȿ���� Ȯ���ϱ� ���� ��ƿ��Ƽ �޼��带 �����Ѵ�.
    /// - �Ļ� Ŭ������ ��ü ����/�ε� ������ ������ �� �ִ� �������̽��� �����Ѵ�.
    /// </summary>
    public class SaveHandle
    {
        /// <summary>
        /// Gets the base path for saving files depending on the build environment.
        /// Returns the 'SaveData' directory path within the project's assets directory in the editor,
        /// and the persistent data path in a build.
        /// ���� ȯ�濡 ���� ���� ���� ��θ� �����´�.
        /// �����Ϳ����� ������Ʈ�� 'SaveData' ���͸� ��θ� ��ȯ�ϸ�, ����� ȯ�濡���� ���� ������ ��θ� ��ȯ�Ѵ�.
        /// </summary>
        public static string BasePath
        {
            get
            {
#if UNITY_EDITOR
                return Path.Combine(Application.dataPath, "SaveData");
#else
                return Path.Combine(Application.persistentDataPath, "SaveData");
#endif
            }
        }


        /// <summary>
        /// Checks if the given JSON string is empty or null.
        /// If empty, logs an error message and returns true.
        /// �־��� JSON ���ڿ��� ��� �ְų� null���� Ȯ���Ѵ�.
        /// ��� ������ ���� �޽����� ����ϰ� true�� ��ȯ�Ѵ�.
        /// </summary>
        /// <param name="jsonString">The JSON string to check. Ȯ���� JSON ���ڿ�.</param>
        /// <param name="type">The save type being used. ��� ���� ���� Ÿ��.</param>
        /// <returns>Returns true if the string is empty or null; otherwise false. ���ڿ��� ����ְų� null�̸� true, �׷��� ������ false�� ��ȯ.</returns>
        private bool IsFileEmpty(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                PrintErrorMessage($"Failed to convert object : File Empty");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the file at the given path is accessible.
        /// If accessible, logs a success message; otherwise, logs an error message.
        /// �־��� ��ο� �ִ� ���Ͽ� ������ �� �ִ��� Ȯ���Ѵ�.
        /// ������ �� ������ ���� �޽����� ����ϰ�, �׷��� ������ ���� �޽����� ����Ѵ�.
        /// </summary>
        /// <param name="filePath">The path of the file to check. Ȯ���� ������ ���.</param>
        /// <returns>Returns true if the file is accessible; otherwise false. ���Ͽ� ������ �� ������ true, �׷��� ������ false�� ��ȯ.</returns>
        private bool IsFileAccessible(string filePath)
        {
            if (File.Exists(filePath))
            {
                PrintSuccessMessage($"Successfully accessed the file at path: {filePath}.");
                return true;
            }
            else
            {
                PrintErrorMessage($"Failed to access the file at path: {filePath}.");
                return false;
            }
        }

        /// <summary>
        /// Logs a success message. Only logs in the Unity Editor.
        /// ���� �޽����� ����Ѵ�. ����Ƽ �����Ϳ����� ��ϵȴ�.
        /// ���� �α� ���� ����� �߰��� �� �� �κ��� �����Ͽ� �ٸ� �α� ���� �ý��۰� ������ �� �ִ�.
        /// </summary>
        /// <param name="message">The success message to log. ����� ���� �޽���.</param>
        private void PrintSuccessMessage(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
            // Add Error Log - To be replaced with a centralized logging system in the future
        }

        /// <summary>
        /// Logs an error message. Only logs in the Unity Editor.
        /// ���� �޽����� ����Ѵ�. ����Ƽ �����Ϳ����� ��ϵȴ�.
        /// ���� �α� ���� ����� �߰��� �� �� �κ��� �����Ͽ� �ٸ� �α� ���� �ý��۰� ������ �� �ִ�.
        /// </summary>
        /// <param name="message">The error message to log. ����� ���� �޽���.</param>
        private void PrintErrorMessage(string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
            // Add Error Log - To be replaced with a centralized logging system in the future
        }

        /// <summary>
        /// Generates the full file path using the base path and the given file name.
        /// �־��� ���� �̸��� ����Ͽ� �⺻ ��ο� ������ ��ü ���� ��θ� �����Ѵ�.
        /// </summary>
        /// <param name="fileName">The name of the file. ���� �̸�.</param>
        /// <returns>Returns the full file path. ��ü ���� ��θ� ��ȯ�Ѵ�.</returns>
        private static string GetFilePath(string fileName, int index)
        {
            return Path.Combine(BasePath, $"{fileName}_{index}.json");
        }

        /// <summary>
        /// Abstract method for saving data. Must be implemented by derived classes.
        /// �����͸� �����ϴ� �߻� �޼���. �Ļ� Ŭ�������� �����ؾ� �Ѵ�.
        /// </summary>
        /// <typeparam name="T">The type of data to save. ������ �������� Ÿ��.</typeparam>
        /// <param name="target">The data object to save. ������ ������ ��ü.</param>
        public void Save<T>(T target, int index) where T : SaveData
        {
            Directory.CreateDirectory(BasePath);

            string filePath = GetFilePath(target.FileName, index);
            string jsonString = JsonUtility.ToJson(target);

            if (IsFileEmpty(jsonString)) return;

            File.WriteAllText(filePath, jsonString);

            IsFileAccessible(filePath);
        }

        /// <summary>
        /// Abstract method for loading data. Must be implemented by derived classes.
        /// �����͸� �ҷ����� �߻� �޼���. �Ļ� Ŭ�������� �����ؾ� �Ѵ�.
        /// </summary>
        /// <typeparam name="T">The type of data to load. �ҷ��� �������� Ÿ��.</typeparam>
        /// <param name="target">The data object to load into. �����͸� �ҷ��� ��ü.</param>
        public void Load<T>(ref T target, int index) where T : SaveData, new()
        {
            string filePath = GetFilePath(target.GetType().ToString(), index);
            string jsonString = File.ReadAllText(filePath);

            if (!IsFileAccessible(filePath)) return;

            if (IsFileEmpty(jsonString)) return;

            target = JsonUtility.FromJson<T>(jsonString);
        }

        public void Delete<T>(T target, int index) where T : SaveData
        {
            string filePath = GetFilePath(target.GetType().ToString(), index);
            string jsonString = File.ReadAllText(filePath);

            if (!IsFileAccessible(filePath)) return;

            if (IsFileEmpty(jsonString)) return;

            File.Delete(filePath);
        }
        
    } 
}
