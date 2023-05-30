using Api.Data;
using HotChocolate.Data.Filters;

namespace Api.Graph.Todos
{
    public class TodoFilterType : FilterInputType<Todo>
    {
        protected override void Configure(
            IFilterInputTypeDescriptor<Todo> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(f => f.IsDone).Type<CustomStringOperationFilterInputType>();
        }
    }

    public class CustomStringOperationFilterInputType : StringOperationFilterInputType
    {
        protected override void Configure(IFilterInputTypeDescriptor descriptor)
        {
            descriptor.Operation(DefaultFilterOperations.Equals).Type<StringType>();
            descriptor.Operation(DefaultFilterOperations.NotEquals).Type<StringType>();
        }
    }

}
