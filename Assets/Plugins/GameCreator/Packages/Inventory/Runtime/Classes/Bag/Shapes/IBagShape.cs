using System;

namespace GameCreator.Runtime.Inventory
{
    public interface IBagShape
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        int MaxWidth { get; }
        int MaxHeight { get; }
        int MaxWeight { get; }
        
        bool HasInfiniteWidth { get; }
        bool HasInfiniteHeight { get; }
        
        // EVENTS: --------------------------------------------------------------------------------

        event Action EventChange; 

        // INITIALIZERS: --------------------------------------------------------------------------

        void OnAwake(Bag bag);
        void OnLoad(TokenBagShape tokenBagShape);
    }
}