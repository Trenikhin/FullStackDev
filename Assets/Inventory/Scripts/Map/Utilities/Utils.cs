namespace Game.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization.Formatters.Binary;
	using UnityEngine;
	using Random = UnityEngine.Random;


	public static partial class Utils
	{
#region Math

		public static int Dot( Vector2Int a, Vector2Int b )		=> a.x * b.x + a.y * b.y;

		public static double Clamp01( double value )
		{
			return
				value < 0.0 ? 0.0 :
				value > 1.0 ? 1.0 :
				value
			;
		}

		public static double Clamp( double value, double min, double max )
		{
			// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Mathf.cs

			return
				value < min ? min :
				value > max ? max :
				value
			;
		}

#endregion
#region Random

		public static Vector2Int RandomRange( Vector2Int minInclusive, Vector2Int maxExclusive )
		=>
			new Vector2Int(
				Random.Range( minInclusive.x, maxExclusive.x ),
				Random.Range( minInclusive.y, maxExclusive.y )
			);

		public static int RandomRange( Vector2Int range )			=> Random.Range( range.x, range.y + 1 );
		public static float RandomRange( Vector2 range )			=> Random.Range( range.x, range.y );
		public static float RandomPlusMinus1()						=> Random.value * 2 - 1;
		public static Vector2 RandomPlusMinus1v2()					=> new Vector2( RandomPlusMinus1(), RandomPlusMinus1() );
		public static Vector3 RandomPlusMinus1v3()					=> new Vector3( RandomPlusMinus1(), RandomPlusMinus1(), RandomPlusMinus1() );
		public static Vector2 RandomValue_v2()						=> new Vector2( Random.value, Random.value );
		public static Vector3 RandomValue_v3()						=> new Vector3( Random.value, Random.value, Random.value );
		public static bool RandomBool( float w0, float w1 )			=> Random.Range( 0, w0 + w1 ) > w0;
		public static bool RandomBool()								=> Random.Range( 0, 2 ) != 0;

		public static List< T > Shuffle_FisherYates< T >( this List< T > list )
		{
			int n		= list.Count;

			for (int i = 0; i < n - 1; i ++)
			{
				int j			= Random.Range( i, n );

				T tmp			= list[ i ];
				list[ i ]		= list[ j ];
				list[ j ]		= tmp;
			}

			return list;
		}

#endregion
#region Logic

		public static Vector3 Consume( this Vector3 total, Vector3 ability )
		=>
			new Vector3(
				total.x.Consume( ability.x ),
				total.y.Consume( ability.y ),
				total.z.Consume( ability.z )
		);

		public static Vector2 Consume( this Vector2 total, Vector2 ability )
		=>
			new Vector2(
				total.x.Consume( ability.x ),
				total.y.Consume( ability.y )
		);

		public static Vector2 Consumption( this Vector2 total, Vector2 ability )
		=>
			new Vector2(
				total.x.Consumption( ability.x ),
				total.y.Consumption( ability.y )
		);


		public static float Consume( this float total, float ability )
		{
			float consumption		= Consumption( total, ability );

			return total - consumption;
		}

		public static float Consumption( this float total, float ability )
		{
			float totalSign			= Mathf.Sign( total );
			float totalAbs			= Mathf.Abs( total );
			float consumptionAbs	= Mathf.Min( ability, totalAbs );
			float consumption		= totalSign * consumptionAbs;

			return consumption;
		}

#endregion


#region Collections

		public static void Remove<T>( List<T> list, int index )
		{
			int lastIndex		= list.Count - 1;
			list[ index ]		= list[ lastIndex ];

			list.RemoveAt( lastIndex );
		}


		public static int LastIndexOf<T>( this Collection<T> collection, T value ) where T : class
		=>
			collection
				.Select( (e, i) => ((T Elem, int Index)?)(e, i) )
				.LastOrDefault( x => x.Value.Elem == value )?.Index ?? -1;

		public static void ForEach<TKey, TVal>( this Dictionary<TKey, TVal> dict, System.Action<TKey, TVal> action )
		{
			foreach (var vals in dict)
				action( vals.Key, vals.Value );
		}

		public static KeyValuePair<TKey, TVal> GetByValueOrDefault<TKey, TVal>( this Dictionary<TKey, TVal> dict, TVal value )
		=>
			dict
				.FirstOrDefault( ( x ) => x.Value.Equals( value ));

		#endregion

		public static Vector2 CamHalfSize()		=> CamSize() / 2;
		public static Vector2 CamSize()			=> Camera.main.orthographicSize * new Vector2( Camera.main.aspect, 1 ) * 2;


		public static T Nullify<T>( ref T value ) where T : class
		{
			T copy		= value;
			value		= null;
			return copy;
		}


		public static T DeepClone<T>( this T obj )
		{
			// https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net
			using (var ms = new MemoryStream())
			{
				BinaryFormatter formatter		= new BinaryFormatter();
				formatter.Serialize( ms, obj );
				ms.Position		= 0;
				return (T) formatter.Deserialize(ms);
			}
		}


		public static void DestroyChildren( Transform transform )
		{
			for (int i = transform.childCount - 1; i >= 0; i --)
				GameObject.Destroy( transform.GetChild( i ).gameObject );
		}


		public static void DisableAndDestroy( GameObject go )
		{
			go.SetActive( false );
			GameObject.Destroy( go );
		}
		
		public static int ToMilliseconds(this float seconds)
		=>
			(int)(seconds * 1000);

		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (action == null) throw new ArgumentNullException(nameof(action));

			foreach (var item in source)
			{
				action(item);
			}
		}

		public static RectInt Intersection( this RectInt rect, RectInt other )
		{
			Vector2Int min		= Vector2Int.Max( rect.min, other.min );
			Vector2Int max		= Vector2Int.Min( rect.max, other.max );

			return new RectInt( min, max - min );
		}
		
		public static bool ScreenTouch() => Input.touchCount > 0 || Input.GetMouseButton( 0 );
		
		public static RectInt Shrink( this RectInt r, int shrink = 1 )					=> r.Grow( shrink * (-1) );
		public static RectInt Grow( this RectInt r, int extent = 1 )					=> r.Grow( Vector2Int.one * extent );
		public static RectInt Grow( this RectInt r, int extentX, int extentY )			=> r.Grow( new Vector2Int( extentX, extentY ) );
		public static RectInt Grow( this RectInt r, Vector2Int extent )					=> r.Grow( extent, extent );
		public static RectInt Grow( this RectInt r, Vector2Int extentMin, Vector2Int extentMax )
		=>
			new RectInt(
				r.min	- extentMin,
				r.size	+ extentMin + extentMax
			);
		
		public static Vector2Int Rotated_cw_90( this Vector2Int v )							=> Rotated_90( v, true );
		public static Vector2Int Rotated_ccw_90( this Vector2Int v )						=> Rotated_90( v, false );
		public static Vector2Int Rotated_90( this Vector2Int v, bool clockWise )			=> new Vector2Int( v.y, -v.x ) * (clockWise ? 1 : -1);
		public static Vector2Int Rotate_90( ref this Vector2Int v, bool clockWise )			=> v = v.Rotated_90( clockWise );

		public static Vector2 Rotated_cw_90( this Vector2 v )								=> Rotated_90( v, true );
		public static Vector2 Rotated_ccw_90( this Vector2 v )								=> Rotated_90( v, false );
		public static Vector2 Rotated_90( this Vector2 v, bool clockWise )					=> new Vector2( v.y, -v.x ) * (clockWise ? 1 : -1);
		public static Vector2 Rotate_90( ref this Vector2 v, bool clockWise )				=> v = v.Rotated_90( clockWise );
		public static Vector2Int Swap( this Vector2Int v )									=> new Vector2Int( v.y, v.x );
		public static Vector2Int Abs( this Vector2Int v )									=> new Vector2Int( Mathf.Abs( v.x ), Mathf.Abs( v.y ) );
		public static Vector2 Abs( this Vector2 v )											=> new Vector2( Mathf.Abs( v.x ), Mathf.Abs( v.y ) );
		public static RectInt AsRect(this Vector2Int size)									=> new RectInt(0, 0, size.x, size.y);
		
		public static IEnumerable<Vector2Int> Iterate(this Vector2Int size)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					yield return new Vector2Int(x, y);
				}
			}
		}
		
		public static RectInt ToRectInt(this Vector2Int[] points)
		{
			if (points == null || points.Length == 0)
			{
				throw new System.ArgumentException("points cannot be null or empty");
			}
			
			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;
			
			foreach (var point in points)
			{
				if (point.x < minX) minX = point.x;
				if (point.y < minY) minY = point.y;
				if (point.x > maxX) maxX = point.x;
				if (point.y > maxY) maxY = point.y;
			}
			
			return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
		}
		
		public static bool LessThan(this Vector2Int vector, int value)
		{
			return vector.x < value || vector.y < value;
		}
	}
}

