namespace Prison.Data
{
    public interface ITableModel
    {
        bool CanRecover { get; }
        void Recover();
        void Drop();
        void Export();
    }
}
