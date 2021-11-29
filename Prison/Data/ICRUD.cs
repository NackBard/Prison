namespace Prison.Data
{
    internal interface ICRUD
    {
        bool CanUpdate { get; }
        bool CanDelete { get; }
        bool CanCreate { get; }
        bool CanClear { get; }
        void CreateAsync();
        void ReadAsync();
        void UpdateAsync();
        void DeleteAsync();
    }
}
