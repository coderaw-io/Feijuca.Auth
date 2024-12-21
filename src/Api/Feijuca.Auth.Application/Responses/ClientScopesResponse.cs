namespace Feijuca.Auth.Application.Responses
{
    public record ClientScopesResponse(string Id, string Name, string Description, string Protocol, Dictionary<string, string> Attributes, IEnumerable<ProtocolMapperResponse> ProtocolMappers);

    public record ProtocolMapperResponse(string Id, string Name, string Protocol, string ProtocolMapperType, bool ConsentRequired, Dictionary<string, string> Config);
}
