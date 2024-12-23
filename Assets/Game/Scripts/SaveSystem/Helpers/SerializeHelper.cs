namespace Game.SaveSystem
{
	using System;
	using Sirenix.Serialization;

	public interface ISerializeHelper
 	{
	    string Serialize<T>( T value );
	    T Deserialize<T>(string data);
    }
	
	public class SerializeHelper : ISerializeHelper
	{
		public string Serialize<T>( T value )
		{
			byte[] bytes = SerializationUtility.SerializeValue(value, DataFormat.Binary);
			
			return Convert.ToBase64String(bytes);
		}

		public T Deserialize<T>( string data )
		{
			byte[] bytes = Convert.FromBase64String(data);
			
			return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
		}
	}
}