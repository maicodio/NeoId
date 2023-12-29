
namespace NeoId;

public class NeoIdTimeSeqWorkerRandomProvider : NeoIdBaseProvider
{
    public NeoIdTimeSeqWorkerRandomProvider(byte[]? workerId = null): base(workerId)
    {

    }

    public override Guid Convert(NeoIdGuid data)
    {
        var ticks = data.CreatedAt.Ticks;
        var ticksMinor = (int)(ticks & 0xFFFFFFFF); 
        var a = (int)(ticks >> 32);
        var b = (short)(ticksMinor >> 16);
        var c = (short)(ticksMinor & 0xFFFF);

        var d1 = data.WorkerId[..4];
        var d2 = BitConverter.GetBytes(data.SequentialNumber).Reverse().ToArray();
        var d3 = BitConverter.GetBytes(data.RandomData)[..2];

        var d = Combine(d1, d2, d3);

        return new Guid(a, b, c, d);
    }

    public override NeoIdGuid Parse(Guid data)
    {
        var bytes = data.ToByteArray().AsSpan();
        
        var a = BitConverter.ToInt32(bytes[..4]);
        var b = BitConverter.ToInt16(bytes[4..6]);
        var c = BitConverter.ToInt16(bytes[6..8]);
        var d = bytes[8..16];

        var ticksMinor = ((int)b << 16) | (c & 0xFFFF);
        var ticks = ((long)a << 32) | ticksMinor & 0xFFFFFFFF;

        DateTimeOffset dateTime = new DateTimeOffset(ticks, TimeSpan.Zero);
        byte[] workerId = d[..4].ToArray();
        ushort sequentialNumber = BitConverter.ToUInt16(d[4..6].ToArray().Reverse().ToArray());
        ushort randomData = BitConverter.ToUInt16(d[6..8]);
        
        return new NeoIdGuid(dateTime, sequentialNumber, workerId, randomData);
    }

    public static byte[] Combine(params byte[][] arrays)
	{
		byte[] ret = new byte[arrays.Sum(x => x.Length)];
		int offset = 0;
		foreach (byte[] data in arrays)
		{
			Buffer.BlockCopy(data, 0, ret, offset, data.Length);
			offset += data.Length;
		}
		return ret;
	}
}