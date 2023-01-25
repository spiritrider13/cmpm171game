using System.Threading.Tasks;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    public interface ISequence : ICancellable
    {
        TimeMode.UpdateMode UpdateMode { get; }

        float T { get; }
        float Time { get; }
        float Duration { get; }
        
        bool IsRunning { get; }

        // METHODS: -------------------------------------------------------------------------------

        // Task Run(Args args);
        // void Cancel(Args args);
    }
}