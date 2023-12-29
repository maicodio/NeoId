namespace NeoId;

public sealed class NeoIdConfiguration
{
    public static NeoIdConfiguration DefaultConfiguration { get; set; } = new NeoIdConfiguration();
    public INeoIdProvider Provider { get; private set; }

    public NeoIdConfiguration(INeoIdProvider? provider = null, byte[]? workerId = null)
    {
        this.Provider = provider ?? new NeoIdTimeSeqWorkerRandomProvider(workerId);
    }
}