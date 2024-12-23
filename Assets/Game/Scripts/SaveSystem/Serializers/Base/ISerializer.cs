namespace Game.SaveSystem
{
	using System.Collections.Generic;

	public interface ISerializer
	{
		void Serialize(Dictionary<string, string> data);
		void Deserialize(Dictionary<string, string> data);
	}
}