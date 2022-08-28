namespace RandomUtilities
{
    public interface ICustomDllClass
    {
        public string GetName();
        public ICustomDllClass Build(object instance);
    }
}
