namespace Inventories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public sealed class Inventory : IEnumerable<Item>
    {
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        public int Width => _cells.GetLength(0);
        public int Height => _cells.GetLength(1);
        public int Count => _items.Count;

        Dictionary<Item, Vector2Int> _items = new ();
        Item[,] _cells;
         
        public Inventory(in int width, in int height)
        {
            if ((width < 0 || height < 0) || (width < 1 && height < 1))
                throw new ArgumentException();
            
            _cells = new Item[width, height];
        }

        public Inventory(
            in int width,
            in int height,
            params KeyValuePair<Item, Vector2Int>[] items
        ) : this(width, height)
        {
            AddItems(items.Select(i => (i.Key, i.Value)));
        }

        public Inventory(
            in int width,
            in int height,
            params Item[] items
        ) : this(width, height)
        {
            AddItems(items);
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<KeyValuePair<Item, Vector2Int>> items
        ) : this(width, height)
        {
            AddItems(items.Select(i => (i.Key, i.Value)));
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<Item> items
        ) : this(width, height)
        {
            AddItems(items);
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(in Item item, in Vector2Int position)
        {
            return CanAddItem(item) && IsFree(position, item.Size);
        }

        public bool CanAddItem(in Item item, in int posX, in int posY)
        {
           return CanAddItem(item, new Vector2Int(posX, posY));
        }

        /// <summary>
        /// Adds an item on a specified position if not exists
        /// </summary>
        public bool AddItem(in Item item, in Vector2Int position)
        {
            if (!CanAddItem(item, position))
                return false;
            
            Add(position, item);
            
            return true;
        }

        public bool AddItem(in Item item, in int posX, in int posY)
        {
            return AddItem( item, new Vector2Int(posX, posY) );
        }

        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(in Item item)
        {
            return CanPlace( item,  out _ );
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(in Item item)
        {
            if (!CanPlace(item, out var position))
                return false;
            
            Add(position, item);
            
            return true;
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(in Item item, out Vector2Int freePosition)
        {
            return FindFreePosition(item.Size, out freePosition);
        }

        public bool FindFreePosition(in int sizeX, int sizeY, out Vector2Int freePosition)
        {
            return FindFreePosition(new Vector2Int(sizeX, sizeY), out freePosition);
        }

        public bool FindFreePosition(in Vector2Int size, out Vector2Int freePosition)
        {
            if (LessThan(size, 1 ))
                throw new ArgumentException("Size cannot be less than 1");
            
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector2Int p = new Vector2Int(x, y);
                   
                    if (IsFree(p, size))
                    {
                        freePosition = p;
                        return true;
                    }
                }
            }
            
            freePosition    = default;
            return false;
        }

        /// <summary>
        /// Checks if a specified item exists
        /// </summary>
        public bool Contains(in Item item)
        {
            return item != null && _items.ContainsKey(item);
        }

        /// <summary>
        /// Checks if a specified position is occupied
        /// </summary>
        public bool IsOccupied(in Vector2Int position)
        {
            return !IsFree(position);
        }

        public bool IsOccupied(in int x, in int y)
        {
            return !IsFree(x, y);
        }

        /// <summary>
        /// Checks if the a position is free
        /// </summary>
        public bool IsFree(in Vector2Int position)
        {
            return IsFree( position, Vector2Int.one );
        }

        public bool IsFree(in int x, in int y)
        {
            return IsFree( new Vector2Int(x,y), Vector2Int.one );
        }

        /// <summary>
        /// Removes a specified item if exists
        /// </summary>
        public bool RemoveItem(in Item item)
        {
            if (!Contains(item))
                return false;
            
            Remove(item);
            return true;
        }

        public bool RemoveItem(in Item item, out Vector2Int position)
        {
            if (!Contains(item))
            {
                position = default;
                return false;   
            }
            
            position = GetPos(item);
            Remove( item);
            
            return true;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(in Vector2Int position)
        {
            var item = _cells[ position.x, position.y ];

            if (item == null)
                throw new NullReferenceException();
            
            return item;
        }

        public Item GetItem(in int x, in int y)
        {
            return GetItem(new Vector2Int(x, y));
        }

        public bool TryGetItem(in Vector2Int position, out Item item)
        {
            if (!InBounds(position, Vector2Int.one) || IsFree(position))
            {
                item = default;
                return false;   
            }
            
            item = GetItem(position);
            
            return true;
        }

        public bool TryGetItem(in int x, in int y, out Item item)
        {
            return TryGetItem(new Vector2Int(x, y), out item);
        }

        /// <summary>
        /// Returns matrix positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(in Item item)
        {
            if (item == null)
                throw new NullReferenceException(nameof(item));
            
            return item
                .GetRect(GetPos(item))
                .ToArray();
        }

        public bool TryGetPositions(in Item item, out Vector2Int[] positions)
        {
            if (!Contains(item))
            {
                positions = null;
                return false;
            }
            
            positions = GetPositions(item);
            return true;
        }

        /// <summary>
        /// Clears all inventory items
        /// </summary>
        public void Clear()
        {
            var inventorySize = Count;

            ClearInventory();
            
            if ( inventorySize > 0 )
                OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns a count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            return _items.Keys.Count( i => i.Name == name );
        }

        /// <summary>
        /// Moves a specified item at target position if exists
        /// </summary>
        public bool MoveItem(in Item item, in Vector2Int position)
        {
            if (item == null)
                throw new ArgumentNullException();
            
            bool canMove  = Contains(item) &&
                            IsFree(position, item.Size) || 
                            (TryGetItem(position, out Item i) && i.Equals(item));

            if (!canMove)
                return false;
            
            Remove( item, false );
            Add( position, item, false );
            OnMoved?.Invoke( item, position );
                
            return true;
        }

        /// <summary>
        /// Reorganizes a inventory space so that the free area is uniform
        /// </summary>
        public void ReorganizeSpace()
        {
            // Copy and clear
            var copy = _items.ToArray();
            ClearInventory();
            
            // Sort items by size
            for (int i = 0; i < copy.Length - 1; i++)
            {
                for (int j = 0; j < copy.Length - 1 - i; j++)
                {
                    int sizeA = copy[j].Key.Size.x * copy[j].Key.Size.y;
                    int sizeB = copy[j + 1].Key.Size.x * copy[j + 1].Key.Size.y;
                    
                    if (sizeA < sizeB)
                        (copy[j], copy[j + 1]) = (copy[j + 1], copy[j]);
                }
            }
            
            // Set items to new positions
            for (int i = 0; i < copy.Length; i++)
            {
                FindFreePosition(copy[i].Key, out Vector2Int position);
                Add(position, copy[i].Key, false);
            }
        }

        /// <summary>
        /// Copies inventory items to a specified matrix
        /// </summary>
        public void CopyTo(in Item[,] matrix)
        {
            if (matrix.GetLength(0) != Width || matrix.GetLength(1) != Height)
                throw new ArgumentException("The provided matrix must have the same dimensions as the inventory.");
            
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    matrix[x, y] = _cells[x, y];
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _items.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        bool IsFree( in Vector2Int pos, in Vector2Int size )
        {
            RectInt rectInt = new RectInt( pos, size );
            
            return InBounds(pos, size) && 
                   Is(_cells, rectInt, null);
        }

        bool InBounds(in Vector2Int pos, in Vector2Int size)
        {
            RectInt rectInt = new(pos, size);
            
            return !(rectInt.x < 0 ||
                    rectInt.y < 0 ||
                    rectInt.xMax > Width ||
                    rectInt.yMax > Height);
        }

        void Add(Vector2Int p, Item i, bool callEvent = true)
        {
            RectInt rectInt     = i.GetRect( p );

            _items.Add(i, p);
            Set(_cells, rectInt, i);
            
            if (callEvent)
                OnAdded?.Invoke( i, p );
        }

        void Remove( Item i, bool callEvent = true )
        {
            Vector2Int p      = GetPos(i);
            RectInt r         = i.GetRect( p );
            
            _items.Remove(i);
            Set(_cells, r, null);
            
            if (callEvent)
                OnRemoved?.Invoke( i, p );
        }

        Vector2Int GetPos(Item item)
        {
            if ( !_items.TryGetValue( item, out Vector2Int p ) )
                throw new KeyNotFoundException();

            return p;
        }

        bool CanPlace( Item item, out Vector2Int p )
        {
            if (item == null || Contains(item))
            {
                p = default;
                return false;   
            }

            if (LessThan(item.Size, 1))
                throw new ArgumentException();
            
            return FindFreePosition(item, out p);
        }

        void AddItems(IEnumerable<Item> items)
        {
            if (items == null)
                throw new ArgumentException(nameof(items));
            
            foreach (var i in items)
            {
                if (i == null)
                    throw new ArgumentException(nameof(items));
                
                AddItem( i );
            }
        }

        void AddItems( IEnumerable< (Item, Vector2Int) > items )
        {
            if (items == null)
                throw new ArgumentException(nameof(items));
            
            foreach (var i in items)
            {
                if (i.Item1 == null)
                    throw new ArgumentException(nameof(items));
                
                AddItem( i.Item1, i.Item2 );
            }
        }

        void ClearInventory()
        {
            _items.Clear();
            _cells = new Item[Width, Height];
        }
        
        void Set( Item[,] matrix, RectInt r, Item item )
        {
            for (int x = r.x; x < r.xMax; x++)
            for (int y = r.y; y < r.yMax; y++)
                matrix[x, y] = item;
        }
        
        bool Is<T>( T[,] matrix, RectInt r, T item ) where T : class
        {
            for (int x = r.x; x < r.xMax; x++)
            {
                for (int y = r.y; y < r.yMax; y++)
                {
                    var isMap = x >= 0 && x < matrix.GetLength(0) && y >= 0 && y < matrix.GetLength(1);
                    var isSame = matrix[x, y] != item;
                    
                    if (!isMap || isSame)
                        return false;
                }
            }

            return true;
        }
        
        bool LessThan( Vector2Int vector, int value)
        {
            return vector.x < value || vector.y < value;
        }
    }
    
    public static class ItemExt
    {
        public static RectInt GetRect( this Item i, Vector2Int p ) => new ( p, i.Size );

        public static Vector2Int[] ToArray(this RectInt r)
        {
            Vector2Int[] points = new Vector2Int[r.width * r.height];
            
            int index = 0;
            
            for (int x = r.x; x < r.xMax; x++)
                for (int y = r.y; y < r.yMax; y++)
                    points[index++] = new Vector2Int(x, y);

            return points;
        }
    }
}