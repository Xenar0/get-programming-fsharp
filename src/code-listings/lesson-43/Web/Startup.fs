namespace Web

open Owin
open System.Web.Http
open Newtonsoft.Json.Serialization
open Swashbuckle.Application

type Startup() =
    static member RegisterWebApi(config: HttpConfiguration) =
        // Configure routing
        config.MapHttpAttributeRoutes()

        // Configure serialization
        config.Formatters.Remove(config.Formatters.XmlFormatter) |> ignore
        config.Formatters.JsonFormatter.SerializerSettings.ContractResolver <- DefaultContractResolver()

    member __.Configuration(builder: IAppBuilder) =
        let config = new HttpConfiguration(DependencyResolver = new Capstone8.Controllers.DependencyResolver())
        Startup.RegisterWebApi(config)
        config
            .EnableSwagger(fun config ->
                let path = sprintf @"%s\bin\Web.XML" System.AppDomain.CurrentDomain.BaseDirectory
                config.IncludeXmlComments path
                config.SingleApiVersion("v1", "Bank Accounts") |> ignore)
            .EnableSwaggerUi() |> ignore
        builder.UseWebApi(config) |> ignore