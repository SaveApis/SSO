using HotChocolate.Types;

namespace Backend.Types;

[QueryType]
public static class Query
{
    public static Book GetBook()
    {
        return new Book("C# in depth.", new Author("Jon Skeet"));
    }
}
