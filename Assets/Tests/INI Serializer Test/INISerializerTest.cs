using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class INISerializerTest : MonoBehaviour
{
    [System.Serializable]
    public struct TestData
    {
        public KeyCode KeyEnumeration;
        public PrimitiveData PrimitiveStruct;
        public ComplexData ComplexStruct;
    }

    [System.Serializable]
    public struct PrimitiveData
    {
        public bool Boolean;
        public char Char;
        public byte Byte;
        public short Int16;
        public ushort UInt16;
        public uint UInt32;
        public int Int32;
        public ulong ULong;
        public long Long;
        public float Float;
        public double Double;
        public string String;
        public int[] IntegerArray;
        public List<int> IntegerList;
        public KeyCode Enumeration;
        public UnityData Data;
    }

    [System.Serializable]
    public struct UnityData
    {
        public Vector3 Vector;
        public Rect Rectangle;
        public Color Color;
    }

    [System.Serializable]
    public struct ComplexData
    {
        public PrimitiveData Struct1;
        public UnityData Struct2;
    }

    int indent = 0;

    public PrimitiveData Test1;

    private void Awake()
    {
        this.Test1.Byte = byte.MaxValue;
        this.Test1.Char = 'c';
        this.Test1.Boolean = true;
        this.Test1.Int32 = int.MinValue;
        this.Test1.Long = long.MaxValue;
        this.Test1.Float = float.MinValue;
        this.Test1.Double = double.MaxValue;
        this.Test1.String = "abcdefghijklmnñopqrstuvwxyz";
        this.Test1.IntegerArray = new int[] { 0, 1, 2, 3 };
        //this.Test1.

        //StartCoroutine(this.GetFields(this.Test1.GetType()));
        this.Serialize(this.Test1);
    }

    void Serialize(object data)
    {
        try
        {
            var bindingFlags = BindingFlags.Instance |
                       BindingFlags.NonPublic |
                       BindingFlags.Public;
            var foo = data;// this.Test1;
                           //var fieldValues = foo.GetType()
                           //                     .GetFields(bindingFlags)
                           //                     .Select(field => field.GetValue(foo))
                           //                     .ToList();
            var fieldValues = foo.GetType()
                                 .GetFields(bindingFlags)
                                 .ToList();

            foreach (var item in fieldValues)
            {
                print($"Name: {item.Name}");
                if (item.FieldType.IsArray)
                {
                    print($"- Array of {item.FieldType} type and {item.FieldType.GetArrayRank()} ranks.");
                }
                else if (item.FieldType.IsEnum)
                {
                    print($"- Enumeration: {item.FieldType}");
                }
                else if (item.FieldType.IsPrimitive)
                {
                    if (item.FieldType == typeof(bool))
                    {
                        print("- Boolean.");
                    }
                    else if (item.FieldType == typeof(char))
                    {
                        print("- Character.");
                    }
                    else if (item.FieldType == typeof(byte))
                    {
                        print("- Byte.");
                    }
                    else if (item.FieldType == typeof(ushort))
                    {
                        print("- Unsigned 16 bits integer.");
                    }
                    else if (item.FieldType == typeof(short))
                    {
                        print("- 16 bits integer.");
                    }
                    else if (item.FieldType == typeof(uint))
                    {
                        print("- Unsigned 32 bits integer.");
                    }
                    else if (item.FieldType == typeof(int))
                    {
                        print("- 32 bits integer.");
                    }
                    else if (item.FieldType == typeof(ulong))
                    {
                        print("- Unsigned 64 bits integer.");
                    }
                    else if (item.FieldType == typeof(long))
                    {
                        print("- 64 bits integer.");
                    }
                    else if (item.FieldType == typeof(float))
                    {
                        print("- Float.");
                    }
                    else if (item.FieldType == typeof(double))
                    {
                        print("- Double.");
                    }
                    else if (item.FieldType == typeof(string))
                    {
                        print("- String.");
                    }
                }
                else
                {
                    print($"- Unknown: {item.FieldType}");
                    this.Serialize(item.GetValue(data));
                }

                //print($"Var Name: {item.Name}, Type: {item.FieldType}, Value: {item.GetValue(foo)}, Is array?: {item.GetValue(foo)}");
            }
        }
        catch
        {
        }
    }
}
