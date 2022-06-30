namespace System.Runtime.CompilerServices
{
#pragma warning disable CS0436
    public sealed class AsyncMethodBuilderAttribute: Attribute
    {
        public Type BuilderType
        {
            get;
        }

        public AsyncMethodBuilderAttribute(Type builderType)
        {
            BuilderType = builderType;
        }
    }
}