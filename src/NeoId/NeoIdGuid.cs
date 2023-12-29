namespace NeoId;

public record NeoIdGuid
{
    public DateTimeOffset CreatedAt { get; private set; }
    public ushort SequentialNumber { get; private set; }
    public byte[] WorkerId { get; private set; }
    public ushort RandomData { get; private set; }

    internal NeoIdGuid(DateTimeOffset dateTime, ushort sequentialNumber, byte[] workerId, ushort randomData) 
    {
        this.CreatedAt = dateTime;
        this.SequentialNumber = sequentialNumber;
        this.WorkerId = workerId;
        this.RandomData = randomData;
    }
}