namespace srvr.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

open core.records
open core.features

[<ApiController>]
[<Route("user")>]
type UserController(logger : ILogger<UserController>, userFeatures : User.Features) = 
  inherit ControllerBase()

  let _logger = logger
  let _userFeatures = userFeatures

  [<HttpGet>]
  [<Route("get/{userId}")>]
  member _.GetUser (userId : int) = task {
    try
      match User.Validation.getUserRequestIsValid userId with
      | false ->
          return Results.BadRequest("GetUser request is invalid.")
      | true ->
          let! user = _userFeatures.GetUser userId
          return Results.Ok(user)        
    with ex ->
      _logger.LogError("An unexpected error occurred while fetching the user record. [UserController] -> [GetUser]", ex)
      return Results.Problem("Internal server error occurred.", statusCode = 500)
  }

  [<HttpPost>]
  [<Route("create")>]
  member _.CreateUser (user : User) = task {
    try
      match User.Validation.createUserRequestIsValid user with
      | false ->
          return Results.BadRequest("CreateUser request is invalid.")
      | true ->
          do! _userFeatures.CreateUser user
          return Results.Ok("User successfully created.")
    with ex ->
      _logger.LogError("An unexpected error occurred while creating the user record. [UserController] -> [CreateUser]", ex)
      return Results.Problem("Internal server error occurred.", statusCode = 500)
  }

  [<HttpPut>]
  [<Route("update")>]
  member _.UpdateUser (user : User) = task {
    try
      match User.Validation.updateUserRequestIsValid user with
      | false ->
          return Results.BadRequest("UpdateUser request is invalid.")
      | true ->
          do! _userFeatures.UpdateUser user
          return Results.Ok("User successfully updated.")
    with ex ->
      _logger.LogError("An unexpected error occurred while updating the user record. [UserController] -> [UpdateUser]", ex)
      return Results.Problem("Internal server error occurred.", statusCode = 500)
  }

  [<HttpDelete>]
  [<Route("delete/{userId}")>]
  member _.DeleteUser (userId : int) = task {
    try
      match User.Validation.deleteUserRequestIsValid userId with
      | false ->
          return Results.BadRequest("DeleteUser request is invalid.")
      | true ->
          do! _userFeatures.DeleteUser userId
          return Results.Ok("User successfully deleted.")
    with ex ->
      _logger.LogError("An unexpected error occurred while deleting the user record. [UserController] -> [DeleteUser]", ex)
      return Results.Problem("Internal server error occurred.", statusCode = 500)
  }