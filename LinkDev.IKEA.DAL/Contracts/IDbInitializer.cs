namespace LinkDev.IKEA.DAL.Contracts
{
    public interface IDbInitializer
    {
        void Initialize();
        Task Seed();
    }
}
