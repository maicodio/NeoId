namespace NeoId;

public interface INeoIdProvider
{
    Guid CreateNew();
    Guid Convert(NeoIdGuid data);
    NeoIdGuid Parse(Guid data);
}