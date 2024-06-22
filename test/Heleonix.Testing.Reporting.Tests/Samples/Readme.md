# Generate TRX sample

+ Comment the `[Ignore("Dev time")]` attributes in the `*Tests.cs` files
1. Uncomment the `Component*` attributes
1. Build the solution
1. Run `dotnet test --no-build --logger "trx;LogFileName=./../Samples/Sample.trx" --filter "FullyQualifiedName~Heleonix.Testing.Reporting.Tests.Samples."`
1. Uncomment the `[Ignore("Dev time")]` attributes
1. Comment the `Component*` attributes