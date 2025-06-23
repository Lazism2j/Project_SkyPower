using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IO
{
    /// <summary>
    /// # Summary
    /// Base class for data objects that can be saved and loaded using the data save system.
    /// This class provides a common structure for all saveable data types, including a file name property.
    /// ������ ���� �ý����� ����Ͽ� �����ϰ� �ҷ��� �� �ִ� ������ ��ü�� �⺻ Ŭ����.
    /// �� Ŭ������ ��� ���� ������ ������ ������ ���� ���� ����(���� �̸� �Ӽ� ����)�� �����Ѵ�.
    ///
    /// ## Usage:
    /// - Inherit from `SaveData` to create a custom data class that can be saved and loaded.
    /// - Use the provided `FileName` property or override it to specify a custom file name.
    /// ## ��� ���:
    /// - `SaveData`�� ����Ͽ� ���� �� �ε��� �� �ִ� ����� ���� ������ Ŭ������ �����Ѵ�.
    /// - ������ `FileName` �Ӽ��� ����ϰų� �̸� �������Ͽ� ����� ���� ���� �̸��� �����Ѵ�.
    ///
    /// ## Features:
    /// - Automatically sets the file name based on the class type name.
    /// - Provides a protected constructor to initialize the file name property.
    /// - Supports the serialization of properties for JSON save operations.
    /// ## ���:
    /// - Ŭ���� ���� �̸��� ������� ���� �̸��� �ڵ����� �����Ѵ�.
    /// - ���� �̸� �Ӽ��� �ʱ�ȭ�ϱ� ���� ��ȣ�� �����ڸ� �����Ѵ�.
    /// - JSON ���� �۾��� ���� �Ӽ��� ����ȭ�� �����Ѵ�.
    ///
    /// ## Notes for Inheriting Classes:
    /// - All fields or properties that need to be saved must be marked as serializable.
    ///   Use `[SerializeField]` for private fields or `[field: SerializeField]` for auto-properties.
    /// - Ensure that all serializable fields or properties have data types supported by Unity's `JsonUtility`.
    ///   Custom types should either be serializable or provide explicit conversion to/from supported types.
    /// - Avoid using complex types like dictionaries, and consider using lists or arrays instead, as `JsonUtility` does not support them.
    /// - Remember to initialize all properties or fields properly in the constructor to avoid null reference issues during serialization.
    /// - You can define custom constructors with parameters in derived classes, but you must also explicitly declare a parameterless constructor.
    ///   This ensures compatibility with Unity's serialization system, which requires a parameterless constructor for deserialization.
    /// ## �Ļ� Ŭ���������� ������:
    /// - �����ؾ� �ϴ� ��� �ʵ峪 �Ӽ��� ����ȭ �����ϵ��� �����ؾ� �Ѵ�.
    ///   �����̺� �ʵ�� `[SerializeField]`, �ڵ� ������Ƽ�� `[field: SerializeField]`�� ����Ѵ�.
    /// - ����ȭ�� ��� �ʵ峪 �Ӽ��� Unity�� `JsonUtility`���� �����ϴ� ������ Ÿ���̾�� �Ѵ�.
    ///   ����� ���� Ÿ���� ����ȭ �����ϰų� �����Ǵ� Ÿ������ ����� ��ȯ�� �����ؾ� �Ѵ�.
    /// - ��ųʸ� ���� ������ Ÿ�� ����� ���ϰ�, ��� ����Ʈ�� �迭�� ����ϴ� ���� ����. `JsonUtility`�� �̸� �������� �ʴ´�.
    /// - ����ȭ �������� �� ���� ������ ���ϱ� ���� ��� �Ӽ��̳� �ʵ带 �����ڿ��� �ùٸ��� �ʱ�ȭ�ؾ� �Ѵ�.
    /// - �Ļ� Ŭ�������� ���ڸ� �޴� �����ڸ� ������ �� ������, �ݵ�� ���ڸ� ���� �ʴ� �����ڵ� ��������� �����ؾ� �Ѵ�.
    ///   �̴� Unity�� ����ȭ �ý����� ������ȭ�� �� �Ű������� ���� �����ڸ� �ʿ�� �ϱ� �����̴�.
    /// </summary>
    public class SaveData
    {
        /// <summary>
        /// Gets the name of the file to be used for saving data.
        /// The file name is automatically set to the type name of the derived class.
        /// �����͸� �����ϴ� �� ����� ���� �̸��� �����´�.
        /// ���� �̸��� �Ļ� Ŭ������ ���� �̸����� �ڵ� �����ȴ�.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Protected constructor that initializes the file name property.
        /// �Ļ� Ŭ������ �� �����ڸ� ȣ���Ͽ� ���� �̸� �Ӽ��� �ʱ�ȭ�� �� �ִ�.
        /// </summary>
        protected SaveData()
        {
            FileName = this.GetType().ToString();
        }

    }
}
