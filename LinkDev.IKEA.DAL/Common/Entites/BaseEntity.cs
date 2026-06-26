namespace LinkDev.IKEA.DAL.Common.Entites
{
    public class BaseEntity<Tkey> where Tkey : IEquatable<Tkey>
    {
        public Tkey Id { get; set; }
    }
}
