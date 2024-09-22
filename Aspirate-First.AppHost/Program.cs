var builder = DistributedApplication.CreateBuilder(args);

//var apiService = builder.AddProject<Projects.Aspirate_First_ApiService>("apiservice");

//builder.AddProject<Projects.Aspirate_First_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(apiService);

builder.AddProject<Projects.Handle_Sharepoint>("handle-sharepoint");

//var apiService = builder.AddProject<Projects.Aspirate_First_ApiService>("apiservice");

//builder.AddProject<Projects.Aspirate_First_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(apiService);

builder.AddProject<Projects.WebApi2>("webapi2");

//var apiService = builder.AddProject<Projects.Aspirate_First_ApiService>("apiservice");

//builder.AddProject<Projects.Aspirate_First_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(apiService);

builder.Build().Run();
