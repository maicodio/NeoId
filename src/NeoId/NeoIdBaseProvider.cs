using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("NeoId.Tests")]

namespace NeoId;

public abstract class NeoIdBaseProvider: INeoIdProvider
{
    private static readonly object _SequentialLock = new();
    private static ushort _CurrentSequential = 0;

    internal static ushort  CurrentSequential 
    {
        get
        {
            return _CurrentSequential;
        }
    }
    
    public byte[] WorkerId { get; private set; }

    protected NeoIdBaseProvider(byte[]? workerId)
    {
        this.WorkerId = (workerId ?? System.Security.Cryptography.RandomNumberGenerator.GetBytes(4))
            .Union(Enumerable.Range(0, 4).Select(x => (byte)0)).Take(4).ToArray();
    }

    protected static ushort GetAnUpdateNextSequential()
    {
        lock(_SequentialLock)
        {
            return ++_CurrentSequential;
        }
    }

    protected virtual ushort GetNextSequential()
    {
        return GetAnUpdateNextSequential();
    }


    public virtual Guid CreateNew()
    {
        var sequentialNumber = GetNextSequential();
        
        NeoIdGuid data = new NeoIdGuid(
            dateTime: DateTimeOffset.Now,
            sequentialNumber: sequentialNumber,
            workerId: WorkerId, 
            randomData: BitConverter.ToUInt16(System.Security.Cryptography.RandomNumberGenerator.GetBytes(2)));

        return Convert(data);

    }

    public abstract Guid Convert(NeoIdGuid data);

    public abstract NeoIdGuid Parse(Guid data);
}