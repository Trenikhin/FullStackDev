namespace Game.Maps
{
	using System;
	using UnityEngine;
	using Game.Utilities;


	[Serializable]
	public class Cells<T> : CellsBase<T>
	{
		public Cells( int width, int height, T cell )		: base( width, height, cell ) {}
		public Cells( int width, int height )				: base( width, height ) {}
		public Cells( Vector2Int size, T cell )			: base( size, cell ) {}
		public Cells( Vector2Int size = default )			: base( size ) {}
		public Cells( CellsBase<T> other )					: base( other ) {}
		public Cells( RectInt rect )						: base( rect ) {}


		public override T Get( int x, int y )						=> _cells[ x - _rect.min.x, y - _rect.min.y ];
		public override void Set( int x, int y, T cell )			=> _cells[ x - _rect.min.x, y - _rect.min.y ]		= cell;


		public void Extend( Vector2Int min, Vector2Int max, T cell = default, bool setAll = false )
		{
			Cells<T>old				= new Cells<T>( _rect ){ _cells = _cells };
			_rect					= ((RectInt)_rect).Grow( min, max );
			_cells					= new T[ Size.x, Size.y ];

			if (setAll)
				SetAll( cell );

			RectInt toCopy			= old.Rect.Intersection( _rect );
			Vector2Int dstMin		= toCopy.min;

			Set( old, toCopy, dstMin );
		}


		public void SetExtend( int x, int y, T cell )							=> SetExtend( new Vector2Int( x, y ), cell );
		public void SetExtend( Vector2Int pos, T cell )							=> SetExtend( new RectInt( pos, Vector2Int.one ), cell );
		public void SetExtend( RectInt rect, T cell, int border = 0 )			=> SetExtend( rect, cell, default, false, border );
		public void SetExtend( RectInt rect, T cell, T init, bool setAll, int border = 0 )
		{
			if (CheckExtendRequired( rect.Grow( border ), out Vector2Int extendMin, out Vector2Int extendMax ))
				Extend( extendMin, extendMax, init, setAll );

			Set( rect, cell );
		}


		bool CheckExtendRequired( RectInt rect, out Vector2Int extendMin, out Vector2Int extendMax )
		{
			extendMin		= Vector2Int.Max( Vector2Int.zero, Min - rect.min );
			extendMax		= Vector2Int.Max( Vector2Int.zero, rect.max - Max );

			return extendMin + extendMax != Vector2Int.zero;
		}


		public void SetMin( Vector2Int min )
		{
			_rect		= new SRectInt( min, Size );
		}
	}
}

