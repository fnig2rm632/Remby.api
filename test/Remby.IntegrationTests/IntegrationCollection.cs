using Xunit;

namespace Remby.IntegrationTests;

[CollectionDefinition("Integration Tests")]
public class IntegrationCollection : ICollectionFixture<TestInfrastructureFixture>;