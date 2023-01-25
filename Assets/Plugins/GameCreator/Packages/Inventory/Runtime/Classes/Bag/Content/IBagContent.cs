using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    public interface IBagContent
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        List<Cell> List { get; }
        int CurrentWeight { get; }
        
        int CountWithStack { get; }
        int CountWithoutStack { get; }

        // EVENTS: --------------------------------------------------------------------------------

        event Action EventChange;
        
        public event Action<RuntimeItem> EventAdd;
        public event Action<RuntimeItem> EventRemove;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        void OnAwake(Bag bag);
        void OnLoad(TokenBagItems tokenBagItems);

        // CHECKERS: ------------------------------------------------------------------------------
        
        bool IsAvailable(Vector2Int position, Vector2Int size);

        bool CanAdd(RuntimeItem runtimeItem, bool canStack);
        bool CanAddType(Item item, bool canStack);
        bool CanMove(Vector2Int positionA, Vector2Int positionB, bool canStack);

        bool Contains(IdString runtimeItemID);
        bool Contains(RuntimeItem runtimeItem);
        bool ContainsType(Item item, int amount);

        // GETTERS: -------------------------------------------------------------------------------

        RuntimeItem FindRuntimeItem(Item item);
        RuntimeItem GetRuntimeItem(IdString runtimeItemID);
        Cell GetContent(Vector2Int position);

        Vector2Int FindPosition(IdString runtimeItemID);
        Vector2Int FindStartPosition(Vector2Int position);

        Cell[] GetTypes(Item item);
        int CountType(Item item);

        // SETTERS: -------------------------------------------------------------------------------

        bool Move(Vector2Int positionA, Vector2Int positionB, bool canStack);

        bool Add(RuntimeItem runtimeItem, Vector2Int position, bool canStack);
        bool Add(RuntimeItem runtimeItem, bool canStack);
        
        RuntimeItem AddType(Item item, Vector2Int position, bool canStack);
        RuntimeItem AddType(Item item, bool canStack);

        RuntimeItem Remove(Vector2Int position);
        RuntimeItem Remove(RuntimeItem runtimeItem);
        RuntimeItem RemoveType(Item item);

        void Sort(Func<int, int, int> comparison);

        bool Use(RuntimeItem runtimeItem);
        bool Use(Vector2Int position);
        
        GameObject Drop(RuntimeItem runtimeItem, Vector3 point);
        GameObject[] Drop(Vector2Int position, int maxAmount, Vector3 point);
    }
}