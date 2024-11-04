namespace Game.Maps
{
	using System;
	using UnityEngine;

	[Serializable]
	public struct SRectInt
	{
		public SVector2Int	min;
		public SVector2Int	size;


		public SRectInt( Vector2Int min, Vector2Int size )
		{
			this.min		= min;
			this.size		= size;
		}


		public static implicit operator RectInt( SRectInt r )		=> new RectInt( r.min, r.size );
		public static implicit operator SRectInt( RectInt r )		=> new SRectInt( r.min, r.size );
	}
	
	[Serializable]
	public struct SVector2Int
	{
		public int x;
		public int y;


		public SVector2Int( int x, int y )
		{
			this.x		= x;
			this.y		= y;
		}


		public static implicit operator Vector2( SVector2Int v )		=> new Vector2		( v.x, v.y );
		public static implicit operator Vector2Int( SVector2Int v )		=> new Vector2Int	( v.x, v.y );
		public static implicit operator SVector2Int( Vector2Int v )		=> new SVector2Int	( v.x, v.y );
	}
}