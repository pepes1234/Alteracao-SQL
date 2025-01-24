
public class Igrejas
{
    public string Id { get; set; }
    public int TrueId { get; set; }

    public static implicit operator List<object>(Igrejas v)
    {
        throw new NotImplementedException();
    }
}