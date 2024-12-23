using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using Attribute = System.Attribute;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public static class SerializerGenerator
{
	[MenuItem("Game/Run Serializers Code Generator")]
	public static void RunGenerator()
	{
		var classes = VariableFinder.FindVariables();
		
		string path = Path.Combine(Application.dataPath, "Game/Scripts/SaveSystem/Serializers");
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		else
		{
			//return;
		}

		foreach (var c in classes)
		{
			string generatedCode = 
@"namespace Game.SaveSystem
{
	using SampleGame.Common;
	using SampleGame.Gameplay;
	
	public class ExampleSerializer : BaseComponentSerializer<SomeComponent, SomeData >
	{
		protected override SomeData Get(SomeComponent component)
		{
			
		}
		
		protected override void Set(SomeComponent component, SomeData data)
		{
			
		}
	}
};";
		
			string newClassName = $"{c.Key.Name}Serializer";
			string pattern = @"\bExampleSerializer\b"; // Match "class ExampleSerializer"
			string replacement = $"{newClassName}";
		
			generatedCode = Regex.Replace(generatedCode, pattern, replacement);
		
			File.WriteAllText(Path.Combine(path, $"{newClassName}.cs"), generatedCode);
			AssetDatabase.Refresh();
		}
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class VariableAttribute : Attribute
{
	public VariableAttribute() { }
}

public static class VariableFinder
{
    public static Dictionary<Type, HashSet<PropertyInfo>> FindVariables()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var classes = new Dictionary<Type, HashSet<PropertyInfo>>();
        
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Where(prop => Attribute.IsDefined(prop, typeof(VariableAttribute)));
				
                foreach (var property in properties)
                {
                    var attribute = (VariableAttribute)Attribute.GetCustomAttribute(property, typeof(VariableAttribute));
                    if (!classes.ContainsKey(type))
	                    classes.Add(type, new HashSet<PropertyInfo>());
                   
					classes[type].Add(property);
                }
            }
        }
        
        return classes;
    }
}