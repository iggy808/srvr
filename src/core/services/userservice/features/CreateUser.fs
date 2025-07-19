namespace core.Services.UserService
open System
open core.Records

module CreateUser =
  module Validation =
    let private UserIdIsNotEmpty userId = userId <> Guid.Empty
    let private HandleIsNotEmpty handle = handle <> String.Empty
    let private HandleIsNotWhitespace handle = not (handle |> String.forall Char.IsWhiteSpace)

    let IsRequestValid (request : User) =
      UserIdIsNotEmpty request.Id &&
      HandleIsNotEmpty request.Handle &&
      HandleIsNotWhitespace request.Handle
